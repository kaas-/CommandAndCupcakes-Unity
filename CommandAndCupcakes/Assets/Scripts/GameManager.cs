using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //path to the text file that logs information for evaluation testing
    string log_path;

    [Range(2, 4)][SerializeField] int playerCount = 4;

    static GameObject[] playerObjects = new GameObject[4];

    private int currentPlayer;
    public Image imageToChange;
    public Sprite[] players = new Sprite[4];
    public Sprite[] combat = new Sprite[6];
    public Sprite[] endGame = new Sprite[4];
    public Camera mainCamera;
    public Camera splashCamera;
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

    //used for calculating tiles
    private float plane_length_x;
    private float plane_length_z;

    //width/length of the arena, tile-wise
    private long num_tiles = 5;

    //2d array to manage which tiles have map pieces
    private bool[,] board;

    //for combat code
    private bool first_attack_received;
    private int first_attack_player;
    private int combat_player_1, combat_player_2;

    private enum splashType
    {
        turn,
        battle,
        end
    };
    private int map_piece_no = 0;

    //used for various random assignments
    private System.Random rnd;

    // Use this for initialization
    void Start () {

        //get current time
        DateTime localDate = DateTime.Now;
        //store current time in ceparate string
        string time = localDate.ToString("dd.hh.mm");
        //create a path where the logged information will be stored in
        log_path = @".\log\" + time + ".txt";
        Debug.Log("log_path: " + log_path);

        //This is important for reasons. I guess we don't receive messages unless we do this.
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        //length and width of arena
        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;

        //Debug.Log("bound.size.x: " + plane_length_x);
        //Debug.Log("bound.size.z: " + plane_length_z);

        //define length and width of board array
        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();

        //Debug.Log(GameObject.FindGameObjectsWithTag("Player") + " Player objects");

        //Add all players to an array
        playerObjects = GameObject.FindGameObjectsWithTag("Player"); 
        currentPlayer = 0;

        mainCamera.enabled = true;
        splashCamera.enabled = false;

       // Debug.Log("Start log");
        
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


        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(0), "player_color", "color", "red");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(1), "player_color", "color", "blue");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(2), "player_color", "color", "green");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(3), "player_color", "color", "yellow");
        
        //Debug.Log("Player count: " + playerCount);


        //Debug.Log("Starting game for device no. " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
        SetSplashScreen(currentPlayer, splashType.turn);
        StartCoroutine("ChangeCamera");
        isWaiting = true;

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

        //find interactable objects on the board
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
                Debug.Log("Assigning map piece to " + pos_x + ", " + pos_z);
                board[pos_x, pos_z] = true;
                i++;
            }
            
        }
    }

    /// <summary>
    /// Determines on which tile a gameobject is. Returns a 2D array.
    /// </summary>
    /// <param name="g">GameObject we find the position of.</param>
    int[] CalculateTile(GameObject g)
    {
        //Debug.Log("Calculating tile: " + g);
        float pirate_x = g.transform.position.x;
        float pirate_z = g.transform.position.z;

        int[] tiles = new int[2];

        tiles[0] = CalculateStepNum(pirate_x, plane_length_x, num_tiles);
        tiles[1] = CalculateStepNum(pirate_z, plane_length_z, num_tiles);

        //Debug.Log("Tiles: " + tiles[0] + ", " + tiles[1]);

        return tiles;
    }

    int CalculateStepNum(float pos, float length, long steps)
    {
        //Debug.Log("Calculating step number for " + pos + ", " + length + ", " + ",  " + steps);
        //Debug.Log("Step number is " + (int)Mathf.Floor((pos / length) * steps));
        return (int)Mathf.Floor((pos / length) * steps);
    }

    /// <summary>
    /// Finds whether a tile has an interactable object
    /// </summary>
    /// <param name="tile_x">x-coodinate of tile</param>
    /// <param name="tile_z">z-coordinate of tile</param>
    /// <param name="int_objects">All interactable objects to check against</param>
    /// <returns></returns>
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
    /// <summary>
    /// Switches turn to next player. Update turn order if needed
    /// </summary>
    private void nextTurn()
    {
        lastPlayer = currentPlayer; //update last player

        //check if the turn order is depleted
        if (count < playerCount-1) //turn order is not depleted
        {
            count++; //increase turn counter
            currentPlayer = turnOrder[count]; //update current player
            StartCoroutine("wait");
            print(currentPlayer);
            StartCoroutine("ChangeCamera");
            SetSplashScreen(currentPlayer, splashType.turn);
        }
        else //turn order is depleted
        {
            print("Running else in the nextturn");
            UpdateOrder(); //scramble order
            currentPlayer = turnOrder[0]; //update current player
            //Debug.LogWarning(currentPlayer);
            count = 0; //reset turn counter
            StartCoroutine("wait");
        }

        SendLogMessageToFile(0, currentPlayer.ToString());
        //send message to controller of next player
        //Debug.Log("Sending message to player: " + currentPlayer + " at device ID " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));

        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    /// <summary>
    /// Scramble turnorder
    /// </summary>
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

    /// <summary>
    /// Send Action to playerobject
    /// </summary>
    /// <param name="player">Index of the playerObject array</param>
    /// <param name="actions">Actions to send</param>
    private void Action(int player, string[] actions)
    {
        //Actions to be executed are sent to the appropriate player object.
        Debug.Log("Current player: " + currentPlayer);
        Debug.Log("Actions: " + actions[0] + ", " + actions[1]);
        playerObjects[currentPlayer].SendMessage("Action", actions);
    }

    /// <summary>
    /// handles messages from controllers, gets called whenever a controller does something
    /// </summary>
    /// <param name="device_id">the controller that did the thing</param>
    /// <param name="data">whatever it did</param>
    void OnMessage(int device_id, JToken data)
    {
        //data gets logged to console for dev reasons
       // Debug.Log("Received data: " + data + " from device: " + device_id);
        //Debug.Log("Message type is: " + data["action"]);

        //has game started? if no, and the message says start game, start the game
        if (!isStarted)
        {
            if ((string)data["action"] =="start_game")
            {
                StartGame();
            }
        }
        //if the game has started, the message will be actions for turns
        else if ((string)data["action"] == "turn_action") 
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
                isWaiting = false;
                isMoving = true;
                //Debug.Log("nextturn started");
            }
            else
            {
                //Debug.Log("Else statement run");
                //wtf how did you do that!?
            }
        }
        /****COMBAT HANDLING****/
        //A player successfully pushed the correct buttons and is the first to do so
        else if ((string)data["action"] == "attack_response_success" && !first_attack_received)
        {
            first_attack_received = true;
            first_attack_player = device_id;

            //Send loss message to appropriate player
            if (device_id != combat_player_1)
                SendAirConsoleMessage(combat_player_1, "combat_result_loss");
            else
                SendAirConsoleMessage(combat_player_2, "combat_result_loss");

        }
        //A player messed up and is the first to do so. This message includes that player's map
        else if((string)data["action"] == "attack_response_failure" && !first_attack_received)
        {
            first_attack_received = true;

            //Send win message and map to appropriate player
            if (device_id != combat_player_1)
            {
                SendAirConsoleMessage(combat_player_1, "combat_result_won", "map", data["map"]);
                SetSplashScreen(AirConsole.instance.ConvertDeviceIdToPlayerNumber(combat_player_1), splashType.battle);
            }
            else
            {
                SendAirConsoleMessage(combat_player_2, "combat_result_won", "map", data["map"]);
                SetSplashScreen(AirConsole.instance.ConvertDeviceIdToPlayerNumber(combat_player_2), splashType.battle);
            }

        }
        //Response to "combat_result_loss" message. Losing player sends their map
        else if ((string)data["action"] == "map_piece_loss")
        {
            first_attack_received = false;

            SendAirConsoleMessage(first_attack_player, "combat_result_won", "map", data["map"]);
        }
        //Final response. Resets combat variable and starts the next turn.
        else if ((string)data["action"] == "combat_result_acknowledged")
        {
            first_attack_received = false;
            isMoving = false;
        }
        //if a player wins the game
        else if ((string)data["action"] == "overall_win")
        {
            Debug.Log("PLAYER NUMBER " + currentPlayer + " WON");
            //TODO needs splash screen
        }
    }

    /// <summary>
    /// Playerobject calls this method when they have finished moving.
    /// </summary>
    void OnPlayerFinishedMoving()
    {

        //Debug.Log("Player" + currentPlayer + " finished moving");
        //Check whether a combat is initiated
        if (!checkAttackAction(currentPlayer))
            isMoving = false;
    }

    /// <summary>
    /// Check for combat. Sends combat message to each player.
    /// </summary>
    /// <param name="player">playerObjects index of player</param>
    /// <returns>Whether combat is initiated</returns>
    bool checkAttackAction(int player)
    {
        //Debug.Log("Checking for combat");
        
        //Get the position of the current player
        int[] currentPlayerPosition = CalculateTile(playerObjects[player]);
        //Debug.Log("Current player position: " + currentPlayerPosition);

        //for each player
        for (int i = 0; i < playerCount; i++)
        {
            //Debug.Log("Other player position: " + CalculateTile(playerObjects[i]));
            int[] otherPlayerPosition = CalculateTile(playerObjects[i]);

            //Compare player positions on grid. If they match, initiate combat.
            if (currentPlayerPosition[0] == otherPlayerPosition[0] && currentPlayerPosition[1] == otherPlayerPosition[1]  && i != player)
            {
                string combat_log = currentPlayer.ToString() + " " + i.ToString();
                SendLogMessageToFile(1, combat_log);

                //Debug.Log("Combat!");
                combat_player_1 = AirConsole.instance.ConvertPlayerNumberToDeviceId(i);
                combat_player_2 = AirConsole.instance.ConvertPlayerNumberToDeviceId(player);

               // Debug.Log("Combat action to: " + combat_player_1 + " and " + combat_player_2); 
                SendAirConsoleMessage(combat_player_1, "attack");
                SendAirConsoleMessage(combat_player_2, "attack");

                return true;
            }
        }
        return false;
    }


    /// <summary>
    /// Gets called by playerobject when they interact with a tile. Compares players current position against the board array.
    /// </summary>
    void OnPlayerInteractWithTile()
    {
        Debug.Log("Player checking for map piece");
        int[] tile = CalculateTile(playerObjects[currentPlayer]);
        Debug.Log("PLAYER IS ON " + tile[0] + " " + tile[1]);
        if (HasMapPiece(tile[0], tile[1]))
        {
            //If the tile has a map piece, send it to the phone.
            SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "map_piece_found", "map_piece", map_piece_no);
            Debug.Log("MAP PIECE FOUND AND SENT " + map_piece_no);
            board[tile[0], tile[1]] = false;
            map_piece_no++;
        }
        else
        {
            //Debug.Log("No map piece found at " + tile);
        }
    }

    /// <summary>
    /// Check whether given tile has a map piece
    /// </summary>
    /// <param name="tile_x">x-coordinate of tile</param>
    /// <param name="tile_z">z-coordinate of tile</param>
    /// <returns>boolean value of 2D position of tile in board array</returns>
    bool HasMapPiece(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a map piece
        return board[tile_x, tile_z];
    }

    /// <summary>
    /// When a device connects to the game
    /// </summary>
    /// <param name="device_id"></param>
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
            //Start the game at 4 players
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

        //Debug.Log("Device no " + device_id + " disconnected");
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

       // Debug.Log(isWaiting + ", " + isMoving);

        if (!isMoving && !isWaiting && !isPaused && isStarted)
        {
            nextTurn();
            isWaiting = true;
        }
    }

    /// <summary>
    /// Send generic message to airconsole device
    /// </summary>
    /// <param name="device_id"></param>
    /// <param name="m_action"></param>
    void SendAirConsoleMessage(int device_id, string m_action)
    {

        var message = new
        {
            action = m_action
        };

        AirConsole.instance.Message(device_id, message);
    }

    /// <summary>
    /// Send generic message to airconsole device with one custom parameter.
    /// </summary>
    /// <param name="device_id"></param>
    /// <param name="m_action"></param>
    /// <param name="param_name_1">Name of the first custom parameter</param>
    /// <param name="param_1">Content of the first custom parameter</param>
    void SendAirConsoleMessage(int device_id, string m_action, string param_name_1, object param_1)
    {

        JProperty action_param = new JProperty("action", m_action);
        JProperty m_param_1 = new JProperty(param_name_1, param_1);

        JObject message = new JObject();
        message.Add(action_param);
        message.Add(m_param_1);

        AirConsole.instance.Message(device_id, message);
    }

    /// <summary>
    /// Send generic message to airconsole device with two custom parameters
    /// </summary>
    /// <param name="device_id"></param>
    /// <param name="m_action"></param>
    /// <param name="param_name_1">Name of the first custom parameter</param>
    /// <param name="param_1">Content of the first custom parameter</param>
    /// <param name="param_name_2">Name of the second custom parameter</param>
    /// <param name="param_2">Content of the second custom parameter</param>
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

   
    /// <summary>
    /// Sends log message to text file
    /// </summary>
    /// <param name="ind">determines what message to send, either turn_changed, or combat_started</param>
    /// <param name="add_message">if turnorder changed, then insert current player index, if combat started, then write who participates in it</param>
    void SendLogMessageToFile(int ind, string add_message)
    {
        //get current time
        DateTime currDate = DateTime.Now;
        //store current time in ceparate string
        string time_log = currDate.ToString("hh:mm:ss");
        string ev;
        switch (ind){
            case 0:
                ev = "turn_changed";
                break;
            case 1:
                ev = "combat_started";
                break;
            default:
                ev = "case_not_specified";
                break;
        }
        string message = time_log + "\t" + ev + "\t" + add_message;
        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = File.AppendText(log_path))
        {
            sw.WriteLine(message);
        }
    }

    void SetSplashScreen(int player, splashType splash)
    {
        switch (splash)
        {
            case splashType.battle:
                imageToChange.sprite = combat[player];
                break;
            case splashType.turn:
                imageToChange.sprite = players[player];
                break;
            case splashType.end:
                imageToChange.sprite = endGame[player];
                break;
        }
   }

    IEnumerator ChangeCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        splashCamera.enabled = !splashCamera.enabled;
        yield return new WaitForSecondsRealtime(3);
        mainCamera.enabled = !mainCamera.enabled;
        splashCamera.enabled = !splashCamera.enabled;
    }
    
}
