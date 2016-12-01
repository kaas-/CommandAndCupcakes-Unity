using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System;

public class GameManager : MonoBehaviour {

    [Range(2, 4)][SerializeField] int playerCount = 4;

    static GameObject[] playerObjects = new GameObject[4];

    private int currentPlayer;
    public Camera[] cameras = new Camera[5]; //an array with all the cameras in the game, [red, blue, green, yellow, game]
    private Camera[] cameraTemp = new Camera[5];
    private float t;
    private int lastPlayer;
    private int count = 0;
    private int[] turnOrder;

    //Variables used to control the game state
    //IsWaiting; waiting for player to make their turn
    private bool isWaiting = false;
    //isPaused; game is paused because of disconnect
    private bool isPaused = false;
    //isStarted; game has started
    private bool isStarted = false;
    //waiting for turn to execute
    private bool isMoving = false;


    private float plane_length_x;
    private float plane_length_z;

    private long num_tiles = 5;
    private bool[,] board;

    private bool first_attack_received;
    private int first_attack_player;
    private int combat_player_1, combat_player_2;

    private int map_no;
    private int map_piece_no;

    private System.Random rnd;

    // Use this for initialization
    void Start () {

        //This is important for reasons. I guess we don't receive messages unless we do this.
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;


        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;

        //Debug.Log("bound.size.x: " + plane_length_x);
        //Debug.Log("bound.size.z: " + plane_length_z);

        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();

        //Debug.Log(GameObject.FindGameObjectsWithTag("Player") + " Player objects");
        playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to an array
        currentPlayer = 0;
        turnOrder = new int[] { 0, 1, 2, 4 };

        Debug.Log("Start log");

        //Array.Copy(cameras, cameraTemp, 4);

    }

    //<summary
    //Starts the game
    //</summary>
    void StartGame()
    {
        //Start the game with a certain amount of players. playerCount defines the amount of players.
        //A number of devices corresponding to playerCount is each designated a player number from 0 to playerCount-1
        //Debug.Log("Game started");
        //Debug.Log("Player 1: " + playerObjects[0]);
        //Debug.Log("Player 2: " + playerObjects[1]);
        playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        AirConsole.instance.SetActivePlayers(playerCount);
        isStarted = true;

        //initialise turn order
        turnOrder = new int[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            turnOrder[i] = i; //fill array with players
        }
        UpdateOrder(); //scramble order
        currentPlayer = turnOrder[0];


        /*for (int i = 0; i < 4; i++)
        {
            cameraTemp[i].enabled = false;
        }
        cameraTemp[0] = cameras[currentPlayer];
        for (int i = 0; i < 3; i++)
        {
            cameraTemp[count].enabled = true;
        }*/
        StartCoroutine("wait");
        //Debug.LogWarning(cameraTemp[currentPlayer]);
        
        //Debug.Log("Player count: " + playerCount);


        //Debug.Log("Starting game for device no. " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    /// <summary>
    /// Randomly add some tile map pieces
    /// </summary>
    void RandomiseTiles()
    {
        int i = 0;
        //defines what percent of tiles contains a map piece
        float percentage = 0.25f;

        //number of tiles with a map piece/pieces 
        int true_pos = (int)((board.GetLength(0) * board.GetLength(1)) * percentage);

        GameObject[] interactable_objects = GameObject.FindGameObjectsWithTag("interactable");

        //checks if there is not enough tiles with objects to assign map pieces to
        if (true_pos > interactable_objects.Length)
        {
            true_pos = interactable_objects.Length;
        }

        while (i < true_pos)
        {
            //gets the random position for the x direction
            int pos_x = rnd.Next(0, board.GetLength(0));

            //gets the random position for the z direction
            int pos_z = rnd.Next(0, board.GetLength(1));

            //Debug.Log("Checking tile [" + pos_x + "," + pos_z + "]. Board has been assigned: " + board[pos_x, pos_z] + ". Has object: " + IsObject(pos_x, pos_z, interactable_objects));
            //the if condition makes sure not to assign a map piece twice
            // and iterates only through those tiles that contain objects
            if (!board[pos_x, pos_z] && IsObject(pos_x, pos_z, interactable_objects))
            {
                //Debug.Log("Assigning map piece to " + pos_x + ", " + pos_z);
                board[pos_x, pos_z] = true;
                i++;
            }
            
        }
    }

    /// <summary>
    /// Determines on which tile the player is
    /// </summary>
    int[] CalculateTile(GameObject g)
    {
        //Debug.Log("Calculating tile: " + g);
        float pirate_x = g.transform.position.x;
        float pirate_z = g.transform.position.z;

        int[] tiles = new int[2];
        tiles[0] = CalculateStepNum(pirate_x, plane_length_x, num_tiles);
        tiles[1] = CalculateStepNum(pirate_z, plane_length_z, num_tiles);

        return tiles;
    }

    int CalculateStepNum(float pos, float length, long steps)
    {
        //Debug.Log("Calculating step number for " + pos + ", " + length + ", " + ",  " + steps);
        //Debug.Log("Step number is " + (int)Mathf.Floor((pos / length) * steps));
        return (int)Mathf.Floor((pos / length) * steps);
    }


    bool IsObject(int tile_x, int tile_z, GameObject[] int_objects)
    {
        foreach (GameObject inter_obj in int_objects)
        {
            int[] obj_tiles = CalculateTile(inter_obj);

            if (obj_tiles[0] == tile_x && obj_tiles[1] == tile_z)
            {
                return true;
            }
        }
        return false;
    }

    private void nextTurn()
    {
        lastPlayer = currentPlayer; //update last player

        //check if the turn order is depleted
        if (count < playerCount-1) //turn order is not depleted
        {
            count++; //increase turn counter
            currentPlayer = turnOrder[count]; //update current player
            //cameraTemp[count] = cameras[currentPlayer];
            /*for (int i = 0; i < 4; i++)
            {
                        cameraTemp[i].enabled = false;
            }*/
            StartCoroutine("wait");
            print(currentPlayer);
            //Debug.LogWarning(cameras[currentPlayer]);

        }
        else //turn order is depleted
        {
            print("Running else in the nextturn");
            UpdateOrder(); //scramble order
            currentPlayer = turnOrder[0]; //update current player
            Debug.LogWarning(currentPlayer);
            count = 0; //reset turn counter
            //cameraTemp[count] = cameras[currentPlayer];
            StartCoroutine("wait");
            //Debug.LogWarning(cameraTemp[currentPlayer]);
        }

        //send message to controller of next player
        //Debug.Log("Sending message to player: " + currentPlayer + " at device ID " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));

        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    //method to scramble the turn order
    void UpdateOrder()
    {
        System.Random random = new System.Random();
        for (int i = playerCount -1; i>=0; i--)
        {
            int rnd = random.Next(i + 1);
            int temp = turnOrder[i];
            turnOrder[i] = turnOrder[rnd];
            turnOrder[rnd] = temp;
        }
        if (lastPlayer == turnOrder[0])
        {
            int temp = turnOrder[0];
            turnOrder[0] = turnOrder[1];
            turnOrder[1] = temp;
        }
        
    }

    IEnumerator wait()
    {
        //cameraTemp[count].enabled = true;
        Debug.LogWarning("wait started");
        yield return new WaitForSecondsRealtime(3);
        //cameraTemp[count].enabled = false;
        Debug.LogWarning("wait done");
    }

    private void Action(int player, string[] actions)
    {
        //Actions to be executed are sent to the appropriate player object.
        //Debug.Log("Current player: " + currentPlayer);
        //Debug.Log("Actions: " + actions[0] + ", " + actions[1]);
        playerObjects[currentPlayer].SendMessage("Action", actions);
    }

    //handles messages from controllers, gets called whenever a controller does something
    //device_id; the controller that did the thing, data; whatever it did
    void OnMessage(int device_id, JToken data)
    {
        //data gets logged to console for dev reasons
        Debug.Log("Received data: " + data + " from device: " + device_id);
        Debug.Log("Message type is: " + data["action"]);

        //has game started? if no, and the message says start game, start the game
        if (!isStarted)
        {
            if ((string)data["action"] =="start_game")
            {
                StartGame();
            }
        }
        else if ((string)data["action"] == "turn_action") //if the game has started, the message will be actions for turns
        {
            //Debug.Log("Turn action received from " + device_id + ". Current player is " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
            //If the controller that sent the message corresponds to active player
            if (device_id == AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer))
            {

                //execute turn
                string[] actions = new string[2];
                actions[0] = (string)data["action_1"];
                actions[1] = (string)data["action_2"];

                //send actions to the player object
                Action(currentPlayer, actions);

                //so that the game knows the player has executed their turn
                //isWaiting = false;
                nextTurn();
                //Debug.Log("nextturn started");
            }
            else
            {
                //Debug.Log("Else statement run");
                //wtf how did you do that!?
            }
        }
        else if ((string)data["action"] == "attack_response_success" && !first_attack_received)
        {
            first_attack_received = true;
            first_attack_player = device_id;

            if (device_id != combat_player_1)
                SendAirConsoleMessage(combat_player_1, "combat_result_loss");
            else
                SendAirConsoleMessage(combat_player_2, "combat_result_loss");

        }
        else if((string)data["action"] == "attack_response_failure" && !first_attack_received)
        {
            first_attack_received = true;
            var message_winner = new
            {
                action = "combat_result_won",
                map = data["map"]
            };
            var message_loser = new
            {
                action = "action"
            };

            if (device_id != combat_player_1)
                SendAirConsoleMessage(combat_player_1, "combat_result_won", "map", data["map"]);
            else
                SendAirConsoleMessage(combat_player_2, "combat_result_won", "map", data["map"]);
        }
        else if ((string)data["action"] == "map_piece_loss")
        {
            first_attack_received = false;

            SendAirConsoleMessage(first_attack_player, "combat_result_won", "map", data["map"]);
            isMoving = false;
        }
        else if ((string)data["action"] == "combat_result_acknowledged")
        {
            first_attack_received = false;
            isMoving = false;
        }
    }

    void OnPlayerFinishedMoving()
    {
        //gets called by the player object - ends the turn
        

        if(!checkAttackAction(currentPlayer))
            isMoving = false;
    }

    bool checkAttackAction(int player)
    {
        int[] currentPlayerPosition = CalculateTile(playerObjects[player]);
        for(int i = 0; i < playerCount; i++)
        {
            if(currentPlayerPosition == CalculateTile(playerObjects[i]) && i != player)
            {


                combat_player_1 = AirConsole.instance.ConvertPlayerNumberToDeviceId(i);
                combat_player_2 = AirConsole.instance.ConvertPlayerNumberToDeviceId(player);

                SendAirConsoleMessage(combat_player_1, "attack");
                SendAirConsoleMessage(combat_player_2, "attack");

                return true;
            }
        }
        return false;
    }

    void OnPlayerInteractWithTile()
    {
        //Debug.Log("Player checking for map piece");
        int[] tile = CalculateTile(playerObjects[currentPlayer]);
        if (HasMapPiece(tile[0], tile[1]))
        {
            SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "map_piece_found", "map_piece", map_piece_no);
            board[tile[0], tile[1]] = false;
            map_piece_no++;
        }
        else
        {
            //Debug.Log("No map piece found at " + tile);
        }
    }

    //<summary>
    //Check whether tile has map piece
    //</summary>
    //
    bool HasMapPiece(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a map piece
        return board[tile_x, tile_z];
    }

    //When a device connects to the game
    void OnConnect(int device_id)
    {
        //Debug.Log("Device no. " + device_id + " connected");
        //If the game has started (SetActivePlayers(int) sets this number to a non-zero value. Thus, if it is 0, the game has not started
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            //max players is 4
            if (AirConsole.instance.GetControllerDeviceIds().Count > 4)
            {
                //too many players
            }
            else if(AirConsole.instance.GetControllerDeviceIds().Count == 4)
            {
                StartGame();
            }
            else
            {
                //set the playerCount to the amount of devices connected
                playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
            }
        }
        else if (AirConsole.instance.GetControllerDeviceIds().Count == playerCount)
        {
            isPaused = false;
        }
    }

    //device disconnects
    void OnDisconnect(int device_id)
    {
        //pause
        isPaused = true;
        Debug.Log("Device no " + device_id + " disconnected");
    }

    //do we need this? probably not. Not sure under what circumstances this would be called.
    void onActivePlayersChange(int device_id)
    {

    }


    // Update is called once per frame
    void Update()
    {

        //isMoving == false, means active player is not moving,
        //isWaiting == false, means active player has executed their turn
        //isPaused == false, because we don't want to continue if paused
        if (Input.GetKeyDown(KeyCode.E))
        {
            nextTurn();
        }
        if (!isMoving && !isWaiting && !isPaused && isStarted)
        {
            //nextTurn();
            isWaiting = true;
        }
    }

    void SendAirConsoleMessage(int device_id, string m_action)
    {

        var message = new
        {
            action = m_action
        };

        AirConsole.instance.Message(device_id, message);
    }

    void SendAirConsoleMessage(int device_id, string m_action, string param_name_1, object param_1)
    {

        JProperty action_param = new JProperty("action", m_action);
        JProperty m_param_1 = new JProperty(param_name_1, param_1);

        JObject message = new JObject();
        message.Add(action_param);
        message.Add(m_param_1);

        AirConsole.instance.Message(device_id, message);
    }

    void SendAirConsoleMessage(int device_id, string m_action, string param_name_1, object param_1, string param_name_2, object param_2)
    {
        JProperty action_param = new JProperty("action", m_action);
        JProperty m_param_1 = new JProperty(param_name_1, param_1);
        JProperty m_param_2 = new JProperty(param_name_2, param_2);

        JObject message = new JObject();
        message.Add(action_param);
        message.Add(m_param_1);
        message.Add(m_param_2);

        AirConsole.instance.Message(device_id, message);
    }

    

}
