using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ControllerManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
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



	// Update is called once per frame
	void Update () {
	
	}
}
