using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;


public class MapPiece : MonoBehaviour
{
    float plane_length_x;
    float plane_length_z;
    long num_tiles = 4;
    bool[,] board;
    System.Random rnd;
    public GameObject[] pirate;
    List<int> device_ids = new List<int>();

    // Use this for initialization
    void Start()
    {
        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;
        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();

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
        //determines which pirate "sends the message"
        GameObject sendingPirate = getPirateFromDeviceId(from); 

        string action = (string)data["action"];
        if (action == "inspect")
        {
            int[] tile = CalculateTile(sendingPirate);
            if (HasMapPiece(tile[0], tile[1]))
            {
                SendMapPiece(from);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Assings randomly some tiles map pieces
    /// </summary>
    void RandomiseTiles()
    {
        int i = 0;

        //defines what percent of tiles contains a map piece
        float percentage = 0.25f;

        //number of tiles with a map piece/pieces 
        int true_pos = (int)((board.GetLength(0) * board.GetLength(1)) * percentage);

        GameObject[] interactable_objects = GameObject.FindGameObjectsWithTag("InteractableObject");

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
            
            //the if condition makes sure not to assign a map piece twice
            // and iterates only through those tiles that contain objects
            if (!board[pos_x, pos_z] && IsObject(pos_x, pos_z, interactable_objects))
            {
                board[pos_x, pos_z] = true;
                i++;
            }
        }
    }


    /// <summary>
    /// checks if there is an object on a given tile
    /// </summary>
    /// <returns></returns>
    bool IsObject(int tile_x, int tile_z, GameObject [] int_objects)
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


    /// <summary>
    /// Converts a position into a step number along a line of a given length.
    /// </summary>
    /// <param name="pos">The distance to convert to a step number</param>
    /// <param name="length">The length of the line</param>
    /// <param name="steps">The number of steps to divide the line into</param>
    /// <returns></returns>
    int CalculateStepNum(float pos, float length, long steps)
    {
        return (int)Mathf.Floor((pos / length) * steps);
    }

    /// <summary>
    /// Determines on which tile the player is
    /// </summary>
    int[] CalculateTile(GameObject g)
    {
        float pirate_x = g.transform.position.x;
        float pirate_z = g.transform.position.z;

        int[] tiles = new int[2];
        tiles[0] = CalculateStepNum(pirate_x, plane_length_x, num_tiles);
        tiles[1] = CalculateStepNum(pirate_z, plane_length_z, num_tiles);

        return tiles;
    }




    /// <summary>
    /// Sends a map piece to a player if there is a map piece on the tile
    /// </summary>
    /// <param name="tile_x"> the tile in the x direction the player is in </param>
    /// <param name="tile_z"> the tile in the z direction the player is in </param>
    bool HasMapPiece(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a map piece
        if (board[tile_x, tile_z])
        {
            return true;
        }
        else
        {
            return false;
        }
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
}

