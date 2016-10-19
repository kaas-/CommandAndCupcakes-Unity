using System.Collections;
using UnityEngine;

class GridMove : MonoBehaviour
{
    private float moveSpeed = 3f;
    private float gridSize = 1f;
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
    public enum Direction { Left, Right, Up, Down };

    public void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //get's the wasd key movement into a 2d vector, where Horizontal is AD and vertical is WS
            if (!allowDiagonals)
            {
                if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) //checks what way the player is moving, since we take the abeslout value, we get 0 or 1 from x or y
                {
                    input.y = 0;
                }
                else
                {
                    input.x = 0;
                }
            }

            if (input != Vector2.zero) //if the vectore is not empty
            {
                StartCoroutine(move(transform)); //call function move, which are run parallel with the code?
            }
        }
    }

    public IEnumerator move(Transform transform)
    {
        isMoving = true;
        startPosition = transform.position;
        t = 0;

        if (gridOrientation == Orientation.Horizontal) //Checks if the grid is Horizontal or vertical. If vertical the position.z is obselete. With horizontal the position.y is obeselete.
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y, startPosition.z + System.Math.Sign(input.y) * gridSize);
        }
        else
        {
            endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
                startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);
        }

        if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0) //As we don't allow Diagonals this doesn't really matter.
        {
            factor = 0.7071f;
        }
        else
        {
            factor = 1f;
        }

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed / gridSize) * factor; //starts to move it.
            transform.position = Vector3.Lerp(startPosition, endPosition, t); //changes the position of cube.
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }

    public void PlayerMove(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                move(transform);
                break;
            case Direction.Right:
                move(transform);
                break;
            case Direction.Up:
                move(transform);
                break;
            case Direction.Down:
                move(transform);
                break;
        }
    }
}