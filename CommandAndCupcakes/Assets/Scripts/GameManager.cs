using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    [Range(2, 4)][SerializeField] int playerCount = 4;

    static GameObject[] playerObjects = new GameObject[4];

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

        playerObjects = GameObject.FindGameObjectsWithTag("Player"); //Add all players to a list 
        currentPlayer = 0;
	    
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void nextTurn()
    {

        if (currentPlayer == playerCount - 1)
            currentPlayer = 0;
        else
            currentPlayer++;

        //send message to controller of next player
    }

    private void sendMoveMessage()
    {

        
        //send message to appropriate player object

    }



    

}
