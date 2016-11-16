using UnityEngine;
using System.Collections;
using System;


public class MapPiece : MonoBehaviour {
   
    long plane_length_x = 100;
    long plane_length_z = 100;
    long tile_size = 10;
    bool[,] board;
    System.Random rnd;
 
    // Use this for initialization
    void Start()
    {
        board = new bool[tile_size, tile_size];
        rnd = new System.Random();
        RandomiseTiles();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTile();
    }


    /// <summary>
    /// Converts a position into a step number along a line of a given length.
    /// </summary>
    /// <param name="pos">The distance to convert to a step number</param>
    /// <param name="length">The length of the line</param>
    /// <param name="steps">The number of steps to divide the line into</param>
    /// <returns></returns>
    long CalculateStepNum(float pos, float length, long steps)
    {
        return (long)Mathf.Floor((pos / length) * steps);
    }

    /// <summary>
    /// Determines on which tile the player is
    /// </summary>
    void CalculateTile()
    {
        float pirate_x = gameObject.transform.position.x;
        float pirate_z = gameObject.transform.position.z;

        long num_tiles_x = plane_length_x / tile_size;
        long num_tiles_z = plane_length_z / tile_size;

        long tile_num_x = CalculateStepNum(pirate_x, plane_length_x, num_tiles_x);
        long tile_num_z = CalculateStepNum(pirate_z, plane_length_z, num_tiles_z);

    }

    /// <summary>
    /// Assings randomly some tiles map pieces
    /// </summary>
    void RandomiseTiles()
    {
        int i = 0;

        //defines what percent of tiles contains a map piece, in this case 10%
        int percentage = 10;

        //number of tiles with a map piece/pieces 
        int true_pos = (board.GetLength(0) * board.GetLength(1)) / percentage;

        while (i < true_pos)
        {
            //gets the random position for the x direction
            int pos_x = rnd.Next(0, board.GetLength(0));

            //gets the random position for the z direction
            int pos_z = rnd.Next(0, board.GetLength(1));

            //TODO iterate only through those tiles that contain objects
            //the if condition makes sure not to assign a map piece twice
            if (!board[pos_x, pos_z])
            {
                board[pos_x, pos_z] = true;
                i++;
            }
        }
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
            //send the map piece to the phone
        }

    }
}
