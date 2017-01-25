using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    [Range(2, 4)][SerializeField] int playerCount = 4;

    static GameObject[] playerObjects = new GameObject[4];

    private int currentPlayer;
    private int[] turnOrder;

    //Variables used to control the game state
    //IsNextTurn; go to next turn
    private bool isNextTurn = false;
    //isStarted; game has started
    private bool isStarted = false;
    private bool isPaused = false;

    //used for calculating tiles
    private float plane_length_x;
    private float plane_length_z;

    //width/length of the arena, tile-wise
    private long num_tiles = 5;

    //2d array to manage which tiles have booty
    private bool[,] board;

    //for combat code
    private bool first_attack_received;
    private int combat_player_1, combat_player_2;

    //used for various random assignments
    private System.Random rnd;

    // Use this for initialization
    void Start () {

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

        foreach (GameObject playerObject in playerObjects)
        {
            playerObject.SendMessage("setBoundary", plane_length_x);
        }

        currentPlayer = 0;

       // Debug.Log("Start log");
        
    }

    //<summary
    //Starts the game
    //</summary>
    void StartGame()
    {
        //Start the game with a certain amount of players. playerCount defines the amount of players.
        //A number of devices corresponding to playerCount is each designated a player number from 0 to playerCount-1

        playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        AirConsole.instance.SetActivePlayers(playerCount);
        isStarted = true;

        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(0), "player_color", "color", "red");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(1), "player_color", "color", "blue");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(2), "player_color", "color", "green");
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(3), "player_color", "color", "yellow");
        
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
        isNextTurn = false;

    }

    /// <summary>
    /// Add booty to a random tile
    /// </summary>
    void RandomiseTiles()
    {
        int i = 0;

        //number of tiles with a booty
        int true_pos = 9;

        //find interactable objects on the board
        GameObject[] interactable_objects = GameObject.FindGameObjectsWithTag("interactable");

        //checks if there is not enough tiles with objects to assign booty to
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

            //the if condition makes sure not to assign a booty twice
            // and iterates only through those tiles that contain objects
            if (!board[pos_x, pos_z] && IsObject(pos_x, pos_z, interactable_objects))
            {
                Debug.Log("Assigning booty piece to " + pos_x + ", " + pos_z);
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

        return tiles;
    }

    int CalculateStepNum(float pos, float length, long steps)
    {
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
        currentPlayer++;

        if (currentPlayer == playerCount)
            currentPlayer = 0;
        
        //send message to controller of next player
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
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
                playerObjects[currentPlayer].SendMessage("Action", actions);

            }
            
        }
        /****COMBAT HANDLING****/
        //A player successfully pushed the correct buttons and is the first to do so
        else if ((string)data["action"] == "attack_response_success" && !first_attack_received)
        {
            first_attack_received = true;
            

            //Send loss message to appropriate player
            if (device_id != combat_player_1)
            {
                SendAirConsoleMessage(combat_player_2, "combat_result_won");
                SendAirConsoleMessage(combat_player_1, "combat_result_loss");
            }
            else
            {
                SendAirConsoleMessage(combat_player_1, "combat_result_won");
                SendAirConsoleMessage(combat_player_2, "combat_result_loss");
            }
                

        }
        //A player messed up and is the first to do so
        else if((string)data["action"] == "attack_response_failure" && !first_attack_received)
        {
            first_attack_received = true;

            //Send win message
            if (device_id != combat_player_1)
            {
                SendAirConsoleMessage(combat_player_1, "combat_result_won");
                SendAirConsoleMessage(combat_player_2, "combat_result_loss");
            }
            else
            {
                SendAirConsoleMessage(combat_player_2, "combat_result_won");
                SendAirConsoleMessage(combat_player_1, "combat_result_loss");
            }

        }
        //Final response. Resets combat variable and starts the next turn.
        else if ((string)data["action"] == "combat_result_acknowledged")
        {
            first_attack_received = false;
            isNextTurn = true;
        }
        //if a player wins the game
        else if ((string)data["action"] == "overall_win")
        {
            //TODO:show results table
        }
        else if ((string)data["action"] == "no_booty_to_steal")
        {
            if (device_id == combat_player_1)
            {
                SendAirConsoleMessage(combat_player_2, "no_booty");
            }
            else 
            {
                SendAirConsoleMessage(combat_player_1, "no_booty");
            }
        }
    }

    /// <summary>
    /// Playerobject calls this method when they have finished moving.
    /// </summary>
    void OnPlayerFinishedMoving()
    {
        //Check whether a combat is initiated
        if (!checkAttackAction(currentPlayer))
            isNextTurn = true;   
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
        int[] tile = CalculateTile(playerObjects[currentPlayer]);
        if (HasBooty(tile[0], tile[1]))
        {
            //If the tile has a booty, send it to the phone.
            SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "booty_found");
            board[tile[0], tile[1]] = false;
        }
    }

    /// <summary>
    /// Check whether given tile has a booty
    /// </summary>
    /// <param name="tile_x">x-coordinate of tile</param>
    /// <param name="tile_z">z-coordinate of tile</param>
    /// <returns>boolean value of 2D position of tile in board array</returns>
    bool HasBooty(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a booty 
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
        //TODO: Replace with start menu stuff
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
        //TODO:Stuff to let players reconnect properly
        //Debug.Log("Device no " + device_id + " disconnected");
    }

    // Update is called once per frame
    void Update()
    {

        if (isNextTurn && !isPaused && isStarted)
        {
            nextTurn();
            isNextTurn = false;
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
  
}
