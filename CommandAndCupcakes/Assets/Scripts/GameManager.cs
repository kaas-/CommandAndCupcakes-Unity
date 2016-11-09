using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class GameManager : MonoBehaviour {

    [Range(2, 4)][SerializeField] int playerCount = 4;

    //static GameObject[] playerObjects = new GameObject[4];

    private AirConsole airconsole = AirConsole.instance;
    private int currentPlayer;
    public bool moving;
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
	void Start () {

        airconsole.onMessage += OnMessage;
        airconsole.onConnect += OnConnect;
        airconsole.onDisconnect += OnDisconnect;
        airconsole.SetActivePlayers(playerCount);

        //playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to the array
        currentPlayer = 0;

    }

    private void action(string action)
    {
        

        /*switch(action)
        {
            case playerActions.attack:
                    
           
        }
           */ 


    }

    private void nextTurn()
    {

        if (currentPlayer == playerCount - 1)
            currentPlayer = 0;
        else
            currentPlayer++;

        //send message to controller of next player
        airconsole.Message(airconsole.ConvertPlayerNumberToDeviceId(currentPlayer), "turn");
    }

    // Update is called once per frame
    void Update () {

	}

    void OnMessage(int device_id, JToken data)
    {
        Debug.Log(data);



    }

    void OnConnect(int device_id)
    {

    }

    void OnDisconnect(int device_id)
    {

    }

    void onActivePlayersChange(int device_id)
    {

    }

}
