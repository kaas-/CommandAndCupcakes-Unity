using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Turns : MonoBehaviour {

    public List<GameObject> gameObjects = new List<GameObject>();

    private bool turnIsExecuting;

	// Use this for initialization
	void Start () {

        gameObjects = GameObject.FindGameObjectsWithTag("Player").ToList<GameObject>(); //Add all players to a list 
	    
	}
	
	// Update is called once per frame
	void Update () {

        if(turnIsExecuting)
        {
            //continue
        }
        else
        {
            //stay idle
        }
	    
	}




}
