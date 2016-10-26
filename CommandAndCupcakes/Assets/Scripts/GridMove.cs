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
    private Orientation gridOrientation = Orientation.Horizontal;
    private bool allowDiagonals = false;
    private bool correctDiagonalSpeed = true;
    private Vector2 input;
    private bool isMoving = false;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;
    private float factor;

    public void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //sets the controls
            if (!allowDiagonals) 
            {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) //checks if the absolute value of x is larger than the absolute value of y. 
                {
                    input.y = 0; //if x is larger then y = 0;
                }
                else
                {
                    input.x = 0; //if y is larger then x = 0;
                }
            }

            if (input != Vector2.zero) //Returns false if Vector2.zero is equal to input
            {
                StartCoroutine(move(transform)); //Start the move function. 
            }
        }
    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal) //checks if the gridOrientation is equal to orientation.Horizontal
        { //if it is uses Vector movement where y is static. 
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
        }
        else
        {//else it uses Vector movement where z is static. 
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

        while (t < 1f) //as long as t is smaller than 1f run this code.
        {
            t += Time.deltaTime * (moveSpeed / gridSize) * factor; //t adds and becomes a new t.
            transform.position = Vector3.Lerp(startPosition, endPosition, t); //moves the player to the interpol between start and end position with t being the distance. 
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
    public void OnTriggerStay(Collider other) //triggers as long as it stays on the trigger
    {
        if (other.gameObject.CompareTag("interactable") && Input.GetKeyDown(KeyCode.E)) //checks if the player using the script is touching another gameobject with the tag "interactable" and if you have given it an input.
        { 
            gameObject.tag = "test"; //changes the players tag to "test"
            moveSpeed = 3f; //sets the moveSpeed to 3f;
        }

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("interactable")) //checks if it is off the ground and moves it back with negative move speed. (find a way to get positive move speed back)
        {
            moveSpeed = 3f; //sets the moveSpeed to 3f;
            gameObject.tag = "Untagged"; //changes the players tag to "Untagged"
        }
    }

    public void OnTriggerExit(Collider other) //Trigger when the player leaves the object. 
    {
        if (gameObject.CompareTag("test")) //checks if the player is tagged "test"
        {
            gameObject.tag = "Untagged"; //assign the player with the "Untagged" tag
        }

        if (other.gameObject.CompareTag("sea")) //checks if the players leaves the area tagged "PlayArea"
        {
            endPosition = startPosition; //moves the player back to where they moved from.
        }
    }
}