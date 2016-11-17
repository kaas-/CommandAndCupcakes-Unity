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

    //Variables used to control the game state
    //IsWaiting; waiting for player to make their turn
    private bool isWaiting = false;
    //isPaused; game is paused because of disconnect
    private bool isPaused = false;
    //isStarted; game has started
    private bool isStarted = false;
    //waiting for turn to execute
    private bool isMoving = false;
    


	// Use this for initialization
	void Start () {

        //This is important for reasons. I guess we don't receive messages unless we do this.
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to an array
        currentPlayer = 0;

        Debug.Log("Start log");

    }

    //Gets called to start the game
    void StartGame()
    {
        //Start the game with a certain amount of players. playerCount defines the amount of players.
        //A number of devices corresponding to playerCount is each designated a player number from 0 to playerCount-1
        AirConsole.instance.SetActivePlayers(playerCount);
        isStarted = true;
    }

    private void Action(int player, string[] actions)
    {
        //Actions to be executed are sent to the appropriate player object.
        playerObjects[currentPlayer].SendMessage("Action", actions);
    }

    private void nextTurn()
    {
        //Go to next player
        if (currentPlayer == playerCount - 1)
            currentPlayer = 0;
        else
            currentPlayer++;

        var message = new { turn = true };

        //send message to controller of next player
        AirConsole.instance.Message(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), message);
    }

    // Update is called once per frame
    void Update () {
        
        //isMoving == false, means active player is not moving,
        //isWaiting == false, means active player has executed their turn
        //isPaused == false, because we don't want to continue if paused
        if (!isMoving && !isWaiting && !isPaused && isStarted)
        {
            nextTurn();
            isWaiting = true;
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
            //If the controller that sent the message corresponds to active player
            if (AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id) == AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer))
            {

                //execute turn
                string[] actions = new string[2];
                actions[0] = (string)data["action_1"];
                actions[1] = (string)data["action_2"];

                //send actions to the player object
                Action(currentPlayer, actions);

                //so that the game knows the player has executed their turn
                isWaiting = false;
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

}
