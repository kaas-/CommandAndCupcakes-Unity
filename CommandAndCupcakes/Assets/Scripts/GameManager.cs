﻿using UnityEngine;
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
    private bool isWaiting = false;
    private bool isPaused = false;
    private bool isStarted = false;
    private bool isMoving = false;
    private enum playerActions
    {
        moveLeft,
        moveRight,
        moveUp,
        moveDown,
        attack,
        dig,
        interact
    };


	// Use this for initialization
	void Awake () {

        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;

        playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to the array
        currentPlayer = 0;

    }

    void StartGame()
    {
        AirConsole.instance.SetActivePlayers(playerCount);
    }

    private void Action(int player, string[] actions)
    {
        playerObjects[currentPlayer].SendMessage("Action", actions);
    }

    private void nextTurn()
    {
        lastPlayer = currentPlayer; //update last player

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
        }

        //send message to controller of next player
        Debug.Log("Sending message to player: " + currentPlayer + " at device ID " + AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer));
        AirConsole.instance.Message(AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
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

    // Update is called once per frame
    void Update () {
        
        if (!isMoving && !isWaiting && !isPaused)
        {
            nextTurn();
            isWaiting = true;
        }
	}

    void OnMessage(int device_id, JToken data)
    {
        Debug.Log(data);

        if (!isStarted)
        {
            if (data["action_1"].Equals("start_game"))
            {
                StartGame();
            }
        }
        else
        {
            if (AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id) == AirConsole.instance.ConvertPlayerNumberToDeviceId(currentPlayer))
            {

                //execute turn
                string[] actions = new string[2];
                actions[0] = (string)data["action_1"];
                actions[1] = (string)data["action_2"];

                Action(currentPlayer, actions);

                isWaiting = false;
            }
            else
            {
                //wtf
            }
        }
    }

    void OnPlayerFinishedMoving()
    {
        isMoving = false;
    }

    void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count > 4)
            {
                //too many players
            }
            else
            {
                playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
            }
        }
        else if (AirConsole.instance.GetControllerDeviceIds().Count == playerCount)
        {
            isPaused = false;
        }
    }

    void OnDisconnect(int device_id)
    {
        isPaused = true;
    }

    void onActivePlayersChange(int device_id)
    {

    }

}
