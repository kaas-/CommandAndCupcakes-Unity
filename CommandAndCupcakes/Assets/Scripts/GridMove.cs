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
    private enum Actions
    {
        move_left,
        move_right,
        move_down,
        move_up,
        attack,
        dig,
        interact
    };
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
    private Actions[] actions;

    public void Update()
    {

        if (actionIterator != 2 && !isMoving)
        {
            switch (actions[actionIterator])
            {
                case Actions.move_left:
                    input = new Vector2(1, 0);
                    break;
                case Actions.move_right:
                    input = new Vector2(-1, 0);
                    break;
                case Actions.move_up:
                    input = new Vector2(0, 1);
                    break;
                case Actions.move_down:
                    input = new Vector2(0, -1);
                    break;
                case Actions.attack:
                    break;
                case Actions.interact:
                    break;
                case Actions.dig:
                    break;
                default:
                    break;
            }

            StartCoroutine(move(transform));
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
        } 

        */

        //code to control what way the character is facing. 
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localEulerAngles = new Vector3(0, 45, 0);     
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localEulerAngles = new Vector3(0, -45, 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
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

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
        {
            factor = 0.7071f;
        }
        else
        {
            factor = 1f;
        }

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
        GameObject.FindGameObjectWithTag("GameManager").SendMessage("OnPlayerFinishedMoving");

        yield return 0;
    }

    void Action(Actions[] actions)
    {
        actionIterator = 0;
        this.actions = actions;
    }

    public void OnTriggerStay (Collider other) //Runs code as long as the object is touching the trigger
    {
        if (other.gameObject.CompareTag("interactable") && Input.GetKeyDown(KeyCode.E)) //checks if the game object being touched is tagged "interactable" and if the E key is pressed down
        { //checks the object that is hit what the objects tag is. if it in this case is untagged it decreasse the speed by 5
            gameObject.tag = "test";
        }
    }
    
    public void OnTriggerExit (Collider other) //Runs the code when the colider exits.
    {//Checks if the game object which is being touched is tagged ""

        if (other.gameObject.CompareTag("sea")) 
        {
            endPosition = startPosition;
        }
        if (other.gameObject.CompareTag("interactable"))
        { //checks the object that is hit what the objects tag is.
            gameObject.tag = "Untagged";
        }
        if (other.gameObject.CompareTag("Player2"))
        {
            gameObject.tag = "Untagged";
        }
    }

    public void OnTriggerEnter (Collider other) //Runs code when the colider enters.
    {

        if (other.gameObject.CompareTag("Player2"))
        {
            gameObject.tag = "battle";
        }
    }
}