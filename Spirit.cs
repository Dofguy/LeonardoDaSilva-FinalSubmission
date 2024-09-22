using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spirit : MonoBehaviour
{
    private int currentI;
    private int currentJ;
    private GameObject[,] matrix;
    private Pacwoman pacwoman;
    private int startI;
    private int startJ;
    public float detectionRadius = 0.2f;
    private bool isMoving;

    void Awake()
    {
        matrix = Node.getInstance().Matrix;

        // Set the enemy's initial position (you can change this to the desired starting position)
        int[] startingPos = GetStartingPos();
        startI = currentI = startingPos[0];
        startJ = currentJ = startingPos[1];
        transform.position = matrix[currentI, currentJ].transform.position;

        // Find the pacwoman object in the scene
        pacwoman = FindObjectOfType<Pacwoman>();
    }

    // Determines the starting position of spirit depending on the current scene
    private int[] GetStartingPos()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "SmallGridBFS": case "SmallGridDFS": case "SmallGridAStar": case "MultiSmallGridBFS": case "MultiSmallGridDFS": case "MultiSmallGridAStar":
                return new int[]{ 4, 1 };
            case "MediumGridBFS": case "MediumGridDFS": case "MediumGridAStar": case "MultiMediumGridBFS": case "MultiMediumGridDFS": case "MultiMediumGridAStar":
                return new int[]{ 14, 3 };
            case "LargeGridBFS": case "LargeGridDFS": case "LargeGridAStar": case "MultiLargeGridBFS": case "MultiLargeGridDFS": case "MultiLargeGridAStar":
                return new int[]{ 10, 4};
            default:
                return new int[]{ 4, 1 };
        }
    }

    // Starts moving when clickEvent is pressed
    public void StartMoving()
    {  
        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(MoveRandomly(0.5f));
        }
    }

    // When game is reset, spirit will stop moving
    public void StopMoving()
    {
        isMoving = false;
    }

    // Coroutine to move spirit randomly
    private IEnumerator MoveRandomly(float waitTime)
    {
        while (isMoving)
        {
            List<int> validDirections = new List<int>();

            for (int direction = 0; direction < 4; direction++)
            {
                int nextI = currentI + GetIDelta(direction);
                int nextJ = currentJ + GetJDelta(direction);

                // Check if the next position is within the grid and not a wall
                if (nextI >= 0 && nextI < matrix.GetLength(0) && nextJ >= 0 && nextJ < matrix.GetLength(1) && !matrix[nextI, nextJ].CompareTag("Walls"))
                {
                    validDirections.Add(direction);
                }
            }

            if (validDirections.Count > 0)
            {
                int randomDirection = validDirections[Random.Range(0, validDirections.Count)];
                currentI += GetIDelta(randomDirection);
                currentJ += GetJDelta(randomDirection);
                transform.position = matrix[currentI, currentJ].transform.position;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    private int GetIDelta(int direction)
    {
        // Up, Down, Left, Right
        int[] iDeltas = { 1, -1, 0, 0 };
        return iDeltas[direction];
    }

    private int GetJDelta(int direction)
    {
        // Up, Down, Left, Right
        int[] jDeltas = { 0, 0, -1, 1 };
        return jDeltas[direction];
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Pacwoman"));
        if (hit != null && hit.CompareTag("Pacwoman"))
        {
            pacwoman.ResetGame();
            currentI = startI;
            currentJ = startJ;
            transform.position = matrix[currentI, currentJ].transform.position;
        }
    }
}
