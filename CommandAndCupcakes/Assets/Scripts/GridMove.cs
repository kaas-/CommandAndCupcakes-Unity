using System.Collections;
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
    private int actionIterator = 2;
    private string[] actions;
    private bool checkKnockback = false;

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
    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

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

        if (endPosition.x < 0 || endPosition.x > boundary || endPosition.z < 0 || endPosition.z > boundary)
            endPosition = startPosition;

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            foreach (Transform child in transform)
            {
                child.position = transform.position; //takes the position of all children of the object at set the postion to the same as the parent. 
            }
            yield return null;
        }

        isMoving = false;

        //regular player action 
        if (actionIterator == 2 && !checkKnockback)
        {
            gameManager.SendMessage("OnPlayerFinishedMoving");
        }
        
        //knockback action
        else if (actionIterator == 2 && checkKnockback)
        {
            checkKnockback = false;
            gameManager.SendMessage("OnPlayerFinishedMovingKnockback");
        }
           

        yield return 0;
    }

    void Action(string[] actions)
    {
        Debug.Log(gameObject + " received Action");
        actionIterator = 0;
        this.actions = actions;
    }

    void Knockback(string[] actions)
    {
        Debug.Log(gameObject + " received Knockback");
        actionIterator = 0;
        this.actions = actions;
        checkKnockback = true;
    }

    void setBoundary (int boundary)
    {
        this.boundary = boundary;
    }

}