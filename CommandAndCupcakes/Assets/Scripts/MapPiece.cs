using UnityEngine;
using System.Collections;
using System;


public class MapPiece : MonoBehaviour
{

    float plane_length_x;
    float plane_length_z;
    long num_tiles = 4;
    bool[,] board;
    System.Random rnd;
    public GameObject pirate;

    // Use this for initialization
    void Start()
    {
        plane_length_x = this.GetComponent<Renderer>().bounds.size.x;
        plane_length_z = this.GetComponent<Renderer>().bounds.size.z;
        board = new bool[num_tiles, num_tiles];
        rnd = new System.Random();
        RandomiseTiles();
    }

    // Update is called once per frame
    void Update()
    {
        int[] tile = CalculateTile(pirate);
        GetMapPiece(tile[0], tile[1]);
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

        while (i < true_pos)
        {
            //gets the random position for the x direction
            int pos_x = rnd.Next(0, board.GetLength(0));

            //gets the random position for the z direction
            int pos_z = rnd.Next(0, board.GetLength(1));

            //TODO iterate only through those tiles that contain objects
            //the if condition makes sure not to assign a map piece twice
            if (!board[pos_x, pos_z] /*&& IsObject()*/)
            {
                board[pos_x, pos_z] = true;
                i++;
            }
            //TODO make condition if there is not enough tile to assign "true" to
        }
    }

    /// <summary>
    /// checks if there is an object on a given tile
    /// </summary>
    /// <returns></returns>
    bool IsObject()
    {
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
    void GetMapPiece(int tile_x, int tile_z)
    {
        //check if the player is on the tile that contains a map piece
        if (board[tile_x, tile_z])
        {
            Debug.Log("Success");
            //send the map piece to the phone
        }

    }
}

