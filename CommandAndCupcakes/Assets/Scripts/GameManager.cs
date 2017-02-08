using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Timers;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {


    [Range(2, 4)][SerializeField] int playerCount = 4;

    private GameObject[] playerObjects;

    //UI text for turn change and combat
    public Text notifications;
    //UI text for timer
    public Text text_timer;
    //text for score board
    public Text final_score;

    //background image for score board
    public Image final_score_image;

    private int currentPlayer;
    private int[] turnOrder;

    //Variables used to control the game state
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
    private int combat_player_1_local, combat_player_2_local;

    //used for various random assignments
    private System.Random rnd;

    //initialize the timer
    private Timer timer;

    //set how much time is left (in seconds)
    private int timeLeft = 300;

    //when time for the overall game runs out it sets to false
    private bool getFinalBooty = true;

    //stores the color of the player (key) and the total amount of booty that player has
    //is used for scoreboard
    private List<KeyValuePair<string, int>> final_score_list = new List<KeyValuePair<string, int>>();

    //counts how many players have send their final amount of booty
    private int finalCount = 0;

    //stores the positions of other players on board that are not participating in combat
    public class MultiDimList : List<List<int>> { }
    MultiDimList otherPlayerPositions = new MultiDimList();

    //states whether the pirate has finished knockback actions
    private bool isInKnockback = false;

    //activates if a player disconnected
    private bool isDisconnected = false;

    //number (local) of the disconnected player
    private int disconnected_player;

    // Use this for initialization
    void Start () {
        //This is important for reasons. I guess we don't receive messages unless we do this.
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        //length and width of arena
        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;

        //define length and width of board array
        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();


        playerObjects = new GameObject[playerCount];
        //Add all players to an array
        playerObjects = GameObject.FindGameObjectsWithTag("Player"); 

        foreach (GameObject playerObject in playerObjects)
        {
            playerObject.SendMessage("setBoundary", plane_length_x);
        }

        currentPlayer = 0;

        //sets the timer with an interval of 1 second
        timer = new Timer(1000);
        

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

        //Debug.Log("Tiles: " + tiles[0] + ", " + tiles[1] + ". Object " + g.ToString());

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
            Debug.Log("Object tiles: " + obj_tiles[0] + ", " + obj_tiles[1]);

            if (obj_tiles[0] == tile_x && obj_tiles[1] == tile_z)
            {
                return true;
            }
        }
        return false;
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
            else if (AirConsole.instance.GetControllerDeviceIds().Count == 4)
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
        //TODO:Stuff to let players reconnect properly
        disconnected_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        isDisconnected = true;
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
        SetUITextTurn(currentPlayer);
        isNextTurn = false;


        // Hook up the Elapsed event for the timer. 
        timer.Elapsed += OnTimedEvent;

        // Have the timer fire repeated events (true is the default)
        timer.AutoReset = true;

    }

    /// <summary>
    /// handles messages from controllers, gets called whenever a controller does something
    /// </summary>
    /// <param name="device_id">the controller that did the thing</param>
    /// <param name="data">whatever it did</param>
    void OnMessage(int device_id, JToken data)
    {
        //has game started? if no, and the message says start game, start the game
        if (!isStarted)
        {
            if ((string)data["action"] == "start_game")
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
                ExcuteTurnActions((string)data["action_1"], (string)data["action_2"], currentPlayer, "Action");

            }

        }
        /****COMBAT HANDLING****/
        //A player successfully pushed the correct buttons and is the first to do so
        else if ((string)data["action"] == "attack_response_success" && !first_attack_received)
        {
            first_attack_received = true;
            OtherPlayerPositionsAfterCombat(combat_player_1_local, combat_player_2_local);

            //Send win and loss message to appropriate player
            if (device_id == combat_player_1)
            {
                HandleCombatResult(combat_player_1, combat_player_2);
            }
            else
            {
                HandleCombatResult(combat_player_2, combat_player_1);
            }


        }
        //A player messed up and is the first to do so
        else if ((string)data["action"] == "attack_response_failure" && !first_attack_received)
        {
            first_attack_received = true;
            OtherPlayerPositionsAfterCombat(combat_player_1_local, combat_player_2_local);

            //Send win and loss message to appropriate player
            if (device_id == combat_player_1)
            {
                HandleCombatResult(combat_player_2, combat_player_1);
            }
            else
            {
                HandleCombatResult(combat_player_1, combat_player_2);
            }

        }
        //Final response. Resets combat variable and starts the next turn.
        else if ((string)data["action"] == "combat_result_acknowledged")
        {
            first_attack_received = false;
            isNextTurn = true;
        }
        //if a player wins the game, has 9 out of 9 booty
        else if ((string)data["action"] == "overall_win")
        {
            timeLeft = 0;
            getFinalScore();
        }
        //if the loosing side (in combat) has no booty to steal
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
        //if a player has send their final booty amount
        else if ((string)data["action"] == "final_score")
        {
            finalCount++;
            string s = (string)data["color"];
            s = s.ToUpper();

            //store each players color and their final amount of booty
            final_score_list.Add(new KeyValuePair<string, int>(s, (int)data["total_booty"]));

            //if all players have send their final booty amount
            if (finalCount == AirConsole.instance.GetControllerDeviceIds().Count)
            {
                //sort the values from the highest to the lowest value
                final_score_list.Sort((x, y) => y.Value.CompareTo(x.Value));

                //show scoreboard on main screen
                ShowResolutionScreen();
            }
        }
    }

    private void HandleCombatResult(int winner, int looser)
    {

        int looser_local_ID;

        if (looser == combat_player_1)
        {
            looser_local_ID = combat_player_1_local;
        }
        else
        {
            looser_local_ID = combat_player_2_local;
        }

        SendAirConsoleMessage(winner, "combat_result_won");
        SendAirConsoleMessage(looser, "combat_result_loss");
        SearchAvailableTile(looser_local_ID);
    }

    // Update is called once per frame
    void Update()
    {
        //updates the timer
        SetTimer();

        //a player disconnected
        if (isDisconnected)
        {
            //erases the pirate from the field
            Destroy(playerObjects[disconnected_player]);

            if (disconnected_player == currentPlayer)
            {
                isNextTurn = true;
            }

            //get final result if there are less than 2 players on the field
            if (AirConsole.instance.GetControllerDeviceIds().Count < 2)
            {
                timeLeft = 0;
                getFinalScore();
            }

            isDisconnected = false;
        }

        if (isNextTurn && !isPaused && isStarted && !isInKnockback)
        {
            nextTurn();
            isNextTurn = false;
        }
    }

    /// <summary>
    /// Switches turn to next player. Update turn order if needed
    /// </summary>
    private void nextTurn()
    {
        currentPlayer++;
        if (currentPlayer == playerCount)
            currentPlayer = 0;

        //if player has disconnected
        while (playerObjects[currentPlayer] == null)
        {
            //Debug.Log("Skipping over disconnected player");
            currentPlayer++;
            if (currentPlayer == playerCount)
                currentPlayer = 0;
        }

        SetUITextTurn(currentPlayer);

        //send message to controller of next player
        SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    
    /// <summary>
    /// Sends actions to player object (pirate)
    /// </summary>
    /// <param name="action_one">first action</param>
    /// <param name="action_two">second action</param>
    /// <param name="p">index of the player, who receives the message</param>
    /// <param name="actionType">can be either a regular action, or a concequence of a knockback (when loosing combat)</param>
    private void ExcuteTurnActions(string action_one, string action_two, int p, string actionType)
    {
        //execute turn
        string[] actions = new string[2];
        actions[0] = action_one;
        actions[1] = action_two;

        //Debug.Log("TEST executing " + actionType + " actions of player " + p + ": " + action_one + " " + action_two);
        //send actions to the player object
        playerObjects[p].SendMessage(actionType, actions);
    }

    /// <summary>
    /// Playerobject calls this method when they have finished moving.
    /// </summary>
    void OnPlayerFinishedMoving()
    {
        //wait for the knockback to be over, them continue
        Debug.Log("Player " + currentPlayer + " has finished moving");
        //Check whether a combat is initiated
        if (!checkAttackAction(currentPlayer))
        {
            Debug.Log("Assigning next turn");
            isNextTurn = true;
        }
        else
        {
            isInKnockback = true;
        }
             
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

        //for each player
        for (int i = 0; i < playerCount; i++)
        {
            if (playerObjects[i] == null)
            {
                //Debug.Log("Disconnected player is null");
                continue;
            }

            //Debug.Log("Checking attack action for player number " + i);

            int[] otherPlayerPosition = CalculateTile(playerObjects[i]);

            //Compare player positions on grid. If they match, initiate combat.
            if (currentPlayerPosition[0] == otherPlayerPosition[0] && currentPlayerPosition[1] == otherPlayerPosition[1] && i != player)
            {
                combat_player_1_local = i;
                combat_player_2_local = player;

                //Debug.Log("Combat!");
                combat_player_1 = AirConsole.instance.ConvertPlayerNumberToDeviceId(i);
                combat_player_2 = AirConsole.instance.ConvertPlayerNumberToDeviceId(player);

                //changes the UI text to who is in combat
                SetUITextCombat(player, i);

                // Debug.Log("Combat action to: " + combat_player_1 + " and " + combat_player_2); 
                SendAirConsoleMessage(combat_player_1, "attack");
                SendAirConsoleMessage(combat_player_2, "attack");

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Player object calls this method when a player is finished executing actions after knockback
    /// </summary>
    void OnPlayerFinishedMovingKnockback()
    {
       // Debug.Log("Finished knockback of pirate");
        isInKnockback = false;
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
            Debug.Log("Booty gained at " + tile[0] +  tile[1]);
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
    /// gets the colour of a player to later show in UI text
    /// </summary>
    /// <param name="dev_id">device ID</param>
    /// <returns></returns>
    string ConvertDeviceIDtoColour(int dev_id)
    {
        switch (dev_id)
        {
            case 0:
                return "RED";
            case 1:
                return "BLUE";
            case 2:
                return "GREEN";
            case 3:
                return "YELLOW";
            default:
                return "GRAY";
        }
    }


    void ClearUIText()
    {
        notifications.text = "";
    }

    /// <summary>
    /// Changes the UI text to indicate whose turn it is
    /// </summary>
    /// <param name="dev_id">pirate ID</param>
    void SetUITextTurn(int dev_id)
    {
        notifications.text = "It is " + ConvertDeviceIDtoColour(dev_id) + "\npirate's turn";
    }

    /// <summary>
    /// Changes UI text to indicate which pirates are in combat
    /// </summary>
    /// <param name="pl_1">ID of attacking pirate in combat</param>
    /// <param name="pl_2">ID of second pirate in combat</param>
    void SetUITextCombat(int pl_1, int pl_2)
    {
        notifications.text = ConvertDeviceIDtoColour(pl_1) + " and " + ConvertDeviceIDtoColour(pl_2) + "\npirates are in combat";
    }

    private void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        if (timeLeft > 0)
        {
            // Display the new time left by updating the Time Left label.
            timeLeft = timeLeft - 1;
        }
        else
        {
            // If the user ran out of time, stop the timer
            timer.Stop();
        }
    }

    /// <summary>
    /// Sets the UI text for the timer when the timer is running
    /// </summary>
    private void SetTimer()
    {
        text_timer.text = "Time left: " + timeLeft.ToString();
        
        // "Resets" the timer
        timer.Enabled = true;

        if (timeLeft == 0)
        {
            text_timer.text = "Time's up!";
            ClearUIText();
            if (getFinalBooty)
            {
                getFinalScore();
                getFinalBooty = false;
                isPaused = true;
            }
        }
    }

    /// <summary>
    /// sends out a message to all controllers to reveal their final booty amount
    /// </summary>
    private void getFinalScore()
    {
        for (int i = 0; i < playerCount; i++)
        {
            SendAirConsoleMessage(AirConsole.instance.ConvertPlayerNumberToDeviceId(i), "game_over");
        }
    }

    

    /// <summary>
    /// shows scoreboard on main screen
    /// </summary>
    private void ShowResolutionScreen()
    {
        string final_message = "";

        //reveal the winner, if any
        if (final_score_list.Count > 1 && final_score_list[0].Value == final_score_list[1].Value)
        {
            final_message += "IT'S A DRAW!\n\n";
        }
        else
        {
            final_message += final_score_list[0].Key + " PIRATE WINS!\n\n";
        }

        //show scoreboard
        final_message += "FINAL SCORE\n";
        foreach (KeyValuePair<string, int> kv in final_score_list)
        {
            string col = kv.Key;
            string p = "\t\tpirate:\t\t";
            if (col == "RED")
            {
                p = "\t\t\tpirate:\t\t";
            }
            else if (col == "YELLOW")
            {
                p = "\tpirate:\t\t";
            }

            string b = " booty items!\n";
            if (kv.Value == 1)
            {
                b = " booty item!\n";
            }

            final_message += col + p + kv.Value.ToString() + b;
        }

        //makes the background image for the scoreboard visible
        Color c = final_score_image.color;
        c.a = 1;
        final_score_image.color = c;

        //put the scoreboard into UI text
        final_score.text = final_message;
    }



    /// <summary>
    /// Goes through all the player objects and stores positions of players that were NOT participating in combat
    /// </summary>
    /// <param name="player_1">player participating in combat</param>
    /// <param name="player_2">player participating in combat</param>
    private void OtherPlayerPositionsAfterCombat(int player_1, int player_2)
    {
        for (int pl = 0; pl < playerCount; pl++)
        {
            if (pl != player_1 && pl != player_2)
            {
                if (playerObjects[pl] == null)
                {
                    continue;
                }

                int[] pl_pos = CalculateTile(playerObjects[pl]);
                List<int> lst = new List<int>();
                lst.Add(pl_pos[0]);
                lst.Add(pl_pos[1]);
                otherPlayerPositions.Add(lst);
                //Debug.Log("TEST Added another player position to list: " + pl_pos[0] + ", " + pl_pos[1]);
            }
        }
    }


    /// <summary>
    /// Checks whether there are player objects on a tile that were NOT participating in combat
    /// </summary>
    /// <param name="x">tile position in the x direction</param>
    /// <param name="z">tile position in the z direction</param>
    /// <returns></returns>
    private bool IsAvailableTile(int x, int z)
    {
        foreach (List<int> pl_pos in otherPlayerPositions)
        {
            if (pl_pos[0] == x && pl_pos[1] == z)
            {
                //there is a player at that tile
                return false;
            }
        }
        //there isn't a player at that tile
        return true;
    }
   

    
    /// <summary>
    /// Goes through the nearby tiles to see where to relocate the player that had lost in combat
    /// </summary>
    /// <param name="loosing_player">the local ID of the loosing player</param>
    private void SearchAvailableTile(int loosing_player)
    {
        //location of the player that had lost
        int[] lost_pl_pos = CalculateTile(playerObjects[loosing_player]);
        int x_position = lost_pl_pos[0];
        int z_position = lost_pl_pos[1];

        //possible offsets from the original position of the player that had lost
        int positive_x = lost_pl_pos[0] + 1;
        int negative_x = lost_pl_pos[0] - 1;
        int positive_z = lost_pl_pos[1] + 1;
        int negative_z = lost_pl_pos[1] - 1;

        //the if-else statement determines how to offset the player that had lost
        //in the positive x direction
        if (positive_x < num_tiles && IsAvailableTile(positive_x, z_position))
        {
            ExcuteTurnActions("right_down", "", loosing_player, "Knockback");
        }
        //in the negative x direction
        else if (negative_x >= 0 && IsAvailableTile(negative_x, z_position))
        {
            ExcuteTurnActions("left_up", "", loosing_player, "Knockback");
        }
        //in the positive z direction
        else if (positive_z < num_tiles && IsAvailableTile(x_position, positive_z))
        {
            ExcuteTurnActions("right_up", "", loosing_player, "Knockback");
        }
        //in the negative z direction
        else if (negative_z >= 0 && IsAvailableTile(x_position, negative_z))
        {
            ExcuteTurnActions("left_down", "", loosing_player, "Knockback");
        }
    
 
        //if stuck in a corner and surrounded by other pirates, move diagonally
        //positive x, positive z direction
        else if (positive_x < num_tiles && positive_z < num_tiles)
        {
            ExcuteTurnActions("right_down", "right_up", loosing_player, "Knockback");
        }
        
        //positive x, negative z direction
        else if (positive_x < num_tiles && negative_z >= 0)
        {
            ExcuteTurnActions("right_down", "left_down", loosing_player, "Knockback");
        }

        //negative x, negative z direction
        else if (negative_x >= 0 && negative_z >= 0)
        {
            ExcuteTurnActions("left_up", "left_down", loosing_player, "Knockback");
        }

        //negative x, positive z direction
        else if (negative_x >= 0 && positive_z < num_tiles)
        {
            ExcuteTurnActions("left_up", "right_up", loosing_player, "Knockback");
        }

        //something is wrong with knockback
        else 
        {
            Debug.Log("KNOCKBACK Something is wrong");
        }

        //clear the positions of players that had not participated in combat
        otherPlayerPositions.Clear(); 
    }

}
