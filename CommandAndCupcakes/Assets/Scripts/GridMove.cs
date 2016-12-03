﻿using System.Collections;
using UnityEngine;

class GridMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float gridSize = 1;
    private enum Orientation
    {
        Horizontal,
        Vertical
    };

    private GameObject gameManager;
    private Orientation gridOrientation = Orientation.Horizontal;
    private bool allowDiagonals = false;
    private bool correctDiagonalSpeed = true;
    private Vector2 input;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;
    private float factor;
    private int actionIterator = 2;
    private string[] actions;

    public int boundary;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Update()
    {
        //Debug.Log("Action iterator: " + actionIterator);

        if (actionIterator != 2 && !isMoving)
        {
            Debug.Log("Gridmove: actionIterator: " + actionIterator);
            Debug.Log("Gridmove: Starting action: " + actions[actionIterator]);
            switch (actions[actionIterator])
            {
                //TODO: input vectors don't match movement
                case "left_down":
                    input = new Vector2(0, -1);
                    transform.localEulerAngles = new Vector3(0, -180, 0);
                    StartCoroutine(move(transform));
                    break;
                case "right_down":
                    input = new Vector2(1, 0);
                    transform.localEulerAngles = new Vector3(0, -270, 0);
                    StartCoroutine(move(transform));
                    break;
                case "left_up":
                    input = new Vector2(-1, 0);
                    transform.localEulerAngles = new Vector3(0, -90, 0);
                    StartCoroutine(move(transform));
                    break;
                case "right_up":
                    input = new Vector2(0, 1);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    StartCoroutine(move(transform));
                    break;
                case "interact":
                    Debug.Log("Gridmove: Player interacting with tile");
                    gameManager.SendMessage("OnPlayerInteractWithTile");
                    input = new Vector2(0, 0);
                    StartCoroutine(move(transform));
                    break;
                default:
                    input = new Vector2(0, 0);
                    StartCoroutine(move(transform));
                    break;
            }

            actionIterator++;
            
        }

        //REAL-TIME CONTROLLER CODE FOR TESTING ONLY

        /* if (!isMoving)
         {
             input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
             if (!allowDiagonals)
             {
                 if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                 {
                     input.y = 0;
                 }
                 else
                 {
                     input.x = 0;
                 }
             }

             if (input != Vector2.zero)
             {
                 StartCoroutine(move(transform));
             }
         } */

        //code to control what way the character is facing. 
        if (Input.GetKeyDown(KeyCode.D))
         {
             transform.localEulerAngles = new Vector3(0, -90, 0);     
         }

         if (Input.GetKeyDown(KeyCode.A))
         {
             transform.localEulerAngles = new Vector3(0, -270, 0);
         }

         if (Input.GetKeyDown(KeyCode.W))
         {
             transform.localEulerAngles = new Vector3(0, 0, 0);
         }

         if (Input.GetKeyDown(KeyCode.S))
         {
             transform.localEulerAngles = new Vector3(0, -180, 0);
         }

    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        /*
        if (input.x != 0 || input.y != 0)
        {
            this.SendMessage("setMove", 1);
        }
        else
        {
            this.SendMessage("setMove", 0);
        }
        */

        if (gridOrientation == Orientation.Horizontal)
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
        }
        else
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.7071f;
        }
        else
        {
            factor = 1f;
        }

        if (endPosition.x < 0 || endPosition.x > boundary || endPosition.z < 0 || endPosition.z > boundary)
            endPosition = startPosition;

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            foreach (Transform child in transform)
            {
                child.position = transform.position; //takes the position of all children of the object at set the postion to the same as the parent. 
            }
            yield return null;
        }

        isMoving = false;
        if (actionIterator == 2)
            gameManager.SendMessage("OnPlayerFinishedMoving");

        yield return 0;
    }

    void Action(string[] actions)
    {
        Debug.Log(gameObject + " received Action");
        actionIterator = 0;
        this.actions = actions;
    }

    void setBoundary (int boundary)
    {
        this.boundary = boundary;
    }

}