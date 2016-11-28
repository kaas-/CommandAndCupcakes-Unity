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
            SendCombatVictory(from);
            //send loosing message to the other pirate
            SendCombatLoosing(index);
        }
        //if a player lost the combat
        else if (action == "loose")
        {
            SendCombatLoosing(from);
            //send winning message to the other pirate
            SendCombatVictory(index);
        }
    }

    void SendCombatVictory(int device_id)
    {
        var message = new
        {
            action = "Won"
        };

        AirConsole.instance.Message(device_id, message);
    }

    void SendCombatLoosing(int device_id)
    {
        var message = new
        {

            action = "Lost"
        };

        AirConsole.instance.Message(device_id, message);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
