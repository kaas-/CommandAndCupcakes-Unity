using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour {

    [Range(2, 4)][SerializeField] int playerCount = 4;

    static GameObject[] playerObjects = new GameObject[4];

    private int currentPlayer;

    private int lastPlayer;
    private int count;
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

    private System.Random rnd;

    // Use this for initialization
    void Start () {

        //This is important for reasons. I guess we don't receive messages unless we do this.
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;


        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;

        Debug.Log("bound.size.x: " + plane_length_x);
        Debug.Log("bound.size.z: " + plane_length_z);

        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();

        Debug.Log(GameObject.FindGameObjectsWithTag("Player") + " Player objects");
        playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to an array
        currentPlayer = 0;
        turnOrder = new int[] { 0, 1, 2, 4 };

        Debug.Log("Start log");

    }


    //<summary
    //Starts the game
    //</summary>
    void StartGame()
    {
        //Start the game with a certain amount of players. playerCount defines the amount of players.
        //A number of devices corresponding to playerCount is each designated a player number from 0 to playerCount-1
        Debug.Log("Game started");
        Debug.Log("Player 1: " + playerObjects[0]);
        Debug.Log("Player 2: " + playerObjects[1]);
        playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        AirConsole.instance.SetActivePlayers(playerCount);
        isStarted = true;

        Debug.Log("Player count: " + playerCount);

        Debug.Log("Starting game for device no. " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
        AirConsole.instance.Message(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
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

            Debug.Log("Checking tile [" + pos_x + "," + pos_z + "]. Board has been assigned: " + board[pos_x, pos_z] + ". Has object: " + IsObject(pos_x, pos_z, interactable_objects));
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
    /// Determines on which tile the player is
    /// </summary>
    int[] CalculateTile(GameObject g)
    {
        Debug.Log("Calculating tile: " + g);
        float pirate_x = g.transform.position.x;
        float pirate_z = g.transform.position.z;

        int[] tiles = new int[2];
        tiles[0] = CalculateStepNum(pirate_x, plane_length_x, num_tiles);
        tiles[1] = CalculateStepNum(pirate_z, plane_length_z, num_tiles);

        return tiles;
    }

    int CalculateStepNum(float pos, float length, long steps)
    {
        Debug.Log("Calculating step number for " + pos + ", " + length + ", " + ",  " + steps);
        Debug.Log("Step number is " + (int)Mathf.Floor((pos / length) * steps));
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
        /*lastPlayer = currentPlayer; //update last player

        //check if the turn order is depleted
        if (count < playerCount) //turn order is not depleted
        {
            count++; //increase turn counter
            currentPlayer = turnOrder[count - 1]; //update current player
        }
        else //turn order is depleted
        {
            UpdateOrder(); //scramble order
            currentPlayer = turnOrder[0]; //update current player
            count = 1; //reset turn counter
        }*/

        if(currentPlayer == playerCount-1)
        {
            currentPlayer = 0;
        }
        else
        {
            currentPlayer++;
        }

        //send message to controller of next player
        Debug.Log("Sending message to player: " + currentPlayer + " at device ID " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
        AirConsole.instance.Message(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    private void Action(int player, string[] actions)
    {
        //Actions to be executed are sent to the appropriate player object.
        Debug.Log("Current player: " + currentPlayer);
        playerObjects[currentPlayer].SendMessage("Action", actions);
    }

    //method to scramble the turn order
    void UpdateOrder()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < playerCount; i++)
        {
            int rnd = random.Next(playerCount);
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
    //handles messages from controllers, gets called whenever a controller does something
    //device_id; the controller that did the thing, data; whatever it did
    void OnMessage(int device_id, JToken data)
    {
        //data gets logged to console for dev reasons
        Debug.Log(data);

        //has game started? if no, and the message says start game, start the game
        if (!isStarted)
        {
            if (data["action_1"].Equals("start_game"))
            {
                StartGame();
            }
        }
        else //if the game has started, the message will be actions for turns
        {
            Debug.Log("Message received from " + device_id + ". Current player is " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
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
            }
            else
            {
                //wtf how did you do that!?
            }
        }
    }

    void OnPlayerFinishedMoving()
    {
        //gets called by the player object - ends the turn
        isMoving = false;
    }

    void OnPlayerInteractWithTile()
    {
        Debug.Log("Player checking for map piece");
        int[] tile = CalculateTile(playerObjects[currentPlayer]);
        if (HasMapPiece(tile[0], tile[1]))
        {
            SendMapPiece(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
            board[tile[0], tile[1]] = false;
        }
        else
        {
            Debug.Log("No map piece found at " + tile);
        }
    }

    bool HasMapPiece(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a map piece
        return board[tile_x, tile_z];
    }

    void SendMapPiece(int device_id)
    {
        Debug.Log("Sending Mappiece, device id " + device_id);
        //send the map piece to the phone
        var message = new
        {

            action = "mapPieceFound"
        };

        AirConsole.instance.Message(device_id, message);
    }

    //When a device connects to the game
    void OnConnect(int device_id)
    {
        Debug.Log("Device no. " + device_id + " connected");
        //If the game has started (SetActivePlayers(int) sets this number to a non-zero value. Thus, if it is 0, the game has not started
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            //max players is 4
            if (AirConsole.instance.GetControllerDeviceIds().Count > 4)
            {
                //too many players
            }
            else if (AirConsole.instance.GetControllerDeviceIds().Count == 3)
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
        if (!isMoving && !isWaiting && !isPaused && isStarted)
        {
            //nextTurn();
            isWaiting = true;
        }
    }

}
