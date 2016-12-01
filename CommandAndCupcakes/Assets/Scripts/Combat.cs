using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

public class Combat : MonoBehaviour {

    public GameObject[] pirate;
    List<int> device_ids = new List<int>();

    // Use this for initialization
    void Start () {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
    }

    GameObject getPirateFromDeviceId(int device_id)
    {
        int player_index = device_ids.IndexOf(device_id);

        return pirate[player_index];
    }

    void OnConnect(int device_id)
    {
        if (!device_ids.Contains(device_id))
        {
            device_ids.Add(device_id);
        }
    }

    void OnMessage(int from, JToken data)
    {
        //determines which pirate "sended the message"
        GameObject sendingPirate = getPirateFromDeviceId(from);

        string action = (string)data["action"];

        int index = -1;
        
        for (int i = 0; i < device_ids.Count; i++)
        {
            if (device_ids[i] != from)
            {
                index = device_ids[i];
                break;
            }
        }
        
        //if a player won the combat
        //TODO if necessary, prevent receiving other "win" messages 
        if (action == "win")
        {
            SendCombatResult(from, "Won");
            //send loosing message to the other pirate
            SendCombatResult(index, "Lost");
        }
        //if a player lost the combat
        else if (action.Substring(0, 5) == "loose")
        {
            
            SendCombatResult(from, "Lost");
            //send winning message to the other pirate
            SendCombatResult(index, "Won" + action.Substring(5, 9));
        }
        else if (action.Substring(0, 2) == "mp")
        {
            SendCombatResult(index, action);
        }
    }

    void SendCombatResult(int device_id, string mes)
    {
        var message = new
        {
            action = mes
        };

        AirConsole.instance.Message(device_id, message);
    }

   
    // Update is called once per frame
    void Update () {
	
	}
}
