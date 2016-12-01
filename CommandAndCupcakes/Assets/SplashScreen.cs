using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public Camera game;
    public Camera red;
    public Camera blue;
    public Camera green;
    public Camera yellow;
    void Start()
    {
        //camera = GetComponent<Camera>();    //Find attached Camera Component
        //camera2 = GetComponent<Camera>();
        game.enabled = true;
        red.enabled = false;
        blue.enabled = false;
        green.enabled = false;
        yellow.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            blue.enabled = true;
            red.enabled = false;
            green.enabled = false;
            yellow.enabled = false;

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            blue.enabled = false;
            red.enabled = true;
            green.enabled = false;
            yellow.enabled = false;

        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            blue.enabled = false;
            red.enabled = false;
            green.enabled = true;
            yellow.enabled = false;

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            blue.enabled = false;
            red.enabled = false;
            green.enabled = false;
            yellow.enabled = true;

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            blue.enabled = false;
            red.enabled = false;
            green.enabled = false;
            yellow.enabled = false;
        }

    }
}