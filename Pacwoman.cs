using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

// Used videos from code monkey and brackeys to help implement A* and other algorithms https://www.youtube.com/watch?v=alU04hvz6L4
// This class manages the movement of Pacwoman in a grid, as well as the pathfinding algorithms.
public class Pacwoman : MonoBehaviour
{
    // Current position of pacwoman in the grid
    public int currentI;
    public int currentJ;

    // Indices for the starting position and goal position
    private int pacwomanStartIndex;
    private int goalIndex;

    // Matrix representing the grid
    private GameObject[,] matrix;
    private GameObject goal;

    private bool _isGameOver;
    public bool IsGameOver { get{ return _isGameOver; } }

    public void GameOver()
    {
        _isGameOver = true;
    }

    // Initialise pacwomans position and related indices
    private void Awake()
    {
        matrix = Node.getInstance().Matrix;

        // Initialize currentI and currentJ to the starting position
        currentI = 1;
        currentJ = 1;

        // Calculate pacwomanStartIndex and goalIndex
        pacwomanStartIndex = goalIndex = (currentJ + currentI * matrix.GetLength(0));
    }

    // Find the goal object in the scene
    private void Start()
    {
        goal = GameObject.Find("Goal");
        ResetGame();
    }

    // Moves pacwoman using bfs algorithm
    public int MoveForBFS(float waitTime)
    {   
        // initialise graph and pathfinding
        GraphBuilder graphBuilder = new GraphBuilder(matrix);
        LinkedList<int>[] adjGraph = graphBuilder.graph.Adj;
        var numVertices = matrix.GetLength(0) * matrix.GetLength(1);
        int numSteps;
        Pathfinding pathfinding = new Pathfinding();

        // Find the path using bfs
        var parentChildPairs = pathfinding.BFS(adjGraph, numVertices, pacwomanStartIndex, goalIndex, out numSteps);
        // Get pacwomans movements and start the coroutine to move pacwoman
        // Debug.Log($"BFS parentChildPairs count: {parentChildPairs.Count}");
        var pacwomanMovements = GetPacwomanMovements(parentChildPairs);
        // Debug.Log($"Pacwoman movements count: {pacwomanMovements.Count}");

        StartCoroutine(MoveForBFSScorona(pacwomanMovements, waitTime));
        return numSteps;
    }

    // Moves pacwoman using dfs algorithm
    public int MoveForDFS(float waitTime)
    {   
        // initialise graph and pathfinding
        GraphBuilder graphBuilder = new GraphBuilder(matrix);
        LinkedList<int>[] adjGraph = graphBuilder.graph.Adj;
        var numVertices = matrix.GetLength(0) * matrix.GetLength(1);
        int numSteps;
        Pathfinding pathfinding = new Pathfinding();
        // Find the path using dfs
        var parentChildPairs = pathfinding.DFS(adjGraph, numVertices, pacwomanStartIndex, goalIndex, out numSteps);
        // Get pacwomans movements and start the coroutine to move pacwoman
        // Debug.Log($"BFS parentChildPairs count: {parentChildPairs.Count}");
        var pacwomanMovements = GetPacwomanMovements(parentChildPairs);
        // Debug.Log($"Pacwoman movements count: {pacwomanMovements.Count}");

        StartCoroutine(MoveForDFSScorona(pacwomanMovements, waitTime));
        return numSteps;
    }

    // Moves pacwoman using A* algorithm
    public int MoveForAStar(float waitTime)
    {
        // Initialise graph and pathfinding
        GraphBuilder graphBuilder = new GraphBuilder(matrix);
        LinkedList<int>[] adjGraph = graphBuilder.graph.Adj;
        var numVertices = matrix.GetLength(0) * matrix.GetLength(1);
        int numSteps;
        Pathfinding pathfinding = new Pathfinding();
        // Find the path using A*
        var parentChildPairs = pathfinding.AStar(adjGraph, numVertices, pacwomanStartIndex, goalIndex, out numSteps, matrix);
        // Get pacwomans movements and start the coroutine to move pacwoman
        // Debug.Log($"BFS parentChildPairs count: {parentChildPairs.Count}");
        var pacwomanMovements = GetPacwomanMovements(parentChildPairs);
        // Debug.Log($"Pacwoman movements count: {pacwomanMovements.Count}");

        StartCoroutine(MoveForAStarScorona(pacwomanMovements, waitTime));
        return numSteps;
    }

    // Coroutine to move pacwoman with BFS algorithm, courtine is used to execute a piece of code across multiple frames
    private IEnumerator MoveForBFSScorona(LinkedList<int> pacwomanMovements, float waitTime)
    {
        foreach(var movement in pacwomanMovements)
        {
            MoveByPathfinding(movement);
            findGoal();
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Coroutine to move pacwoman with DFS algorithm
    private IEnumerator MoveForDFSScorona(LinkedList<int> pacwomanMovements, float waitTime)
    {
        foreach(var movement in pacwomanMovements)
        {
            MoveByPathfinding(movement);
            findGoal();
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Coroutine to move pacwoman with A* algorithm
    private IEnumerator MoveForAStarScorona(LinkedList<int> pacwomanMovements, float waitTime)
    {
        foreach(var movement in pacwomanMovements)
        {
            MoveByPathfinding(movement);
            findGoal();
            yield return new WaitForSeconds(waitTime);
        }
    }
    // Moves pacwoman by updating its position and rotation based on movement direction
    private void MoveByPathfinding(int movement)
    {  
        // Debug.Log("MoveByPathfinding called with movement: " + movement);
        int[] directions = { FindUpNode(), FindDownNode(), FindLeftNode(), FindRightNode() };

        for (int i = 0; i < directions.Length; i++)
        {
            if (movement == directions[i])
            {
                MoveAndRotate(i);
                break;
            }
        }
    }

    // Retrieve pacwoman's movements from the parent-child pairs of the pathfinding algorithm
    private LinkedList<int> GetPacwomanMovements(Dictionary<int, LinkedList<int>> parentChildPairs)
    {
        // Debug.Log("GetPacwomanMovements called");
        LinkedList<int> pacwomanMovements = new LinkedList<int>();
        var tempGoal = goalIndex;

        foreach (var parentChildPair in parentChildPairs.Reverse())
        {
            // Debug.Log($"Checking parent: {parentChildPair.Key}, child count: {parentChildPair.Value.Count}");
            if (parentChildPair.Value.Contains(tempGoal))
            {
                pacwomanMovements.AddFirst(tempGoal);
                tempGoal = parentChildPair.Key;
            }
        }
        return pacwomanMovements;
    }

    // Checks if pacwoman has reached the goal
    private void findGoal()
    {
        if (transform.position == goal.transform.position)
        {
            goal.SetActive(false);
            _isGameOver = true;
        }
    }

    // Resets the game state and repositions both pacwoman and the goal
    public void ResetGame()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Multi"))
        {
            Spirit spirit = GameObject.Find("Spirit").GetComponent<Spirit>();
            spirit.StopMoving();
        }
        int[] startingPos = GetStartingPos();
        currentI = startingPos[0];
        currentJ = startingPos[1];

        pacwomanStartIndex = (currentJ + currentI * matrix.GetLength(0));
        transform.position = matrix[currentI, currentJ].transform.position;
        goal.SetActive(true);
        _isGameOver = false;
        goal.transform.position = matrix[1, 1].transform.position;
        goalIndex = (1 + 1 * matrix.GetLength(0));
    }

    // Determines the starting position of pacwoman depending on the current scene
    private int[] GetStartingPos()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "SmallGridBFS": case "SmallGridDFS": case "SmallGridAStar": case "MultiSmallGridBFS": case "MultiSmallGridDFS": case "MultiSmallGridAStar":
                return new int[]{ 10, 10 };
            case "MediumGridBFS": case "MediumGridDFS": case "MediumGridAStar": case "MultiMediumGridBFS": case "MultiMediumGridDFS": case "MultiMediumGridAStar":
                return new int[]{ 14, 14 };
            case "LargeGridBFS": case "LargeGridDFS": case "LargeGridAStar": case "MultiLargeGridBFS": case "MultiLargeGridDFS": case "MultiLargeGridAStar":
                return new int[]{ 18, 18};
            default:
                return new int[]{ 10, 10 };
        }
    }

    // Find the index of a node in a given direction based on the provided iDelta and jDelta
    private int FindNodeInDirection(int iDelta, int jDelta)
    {
        return (currentJ + jDelta + (currentI + iDelta) * matrix.GetLength(0));
    }
    // Find the index of the various nodes from where the current node is
    private int FindUpNode() => FindNodeInDirection(1, 0);
    private int FindDownNode() => FindNodeInDirection(-1, 0);
    private int FindLeftNode() => FindNodeInDirection(0, -1);
    private int FindRightNode() => FindNodeInDirection(0, 1);

    // Move pacwoman to a new position based on the provided directio and rotate accordingly
    private void MoveAndRotate(int direction)
    {   
        // Arrays to store the change in i and j values and corresponding rotations for each directions
        int[] iDeltas = {1, -1, 0, 0};
        int[] jDeltas = {0, 0, -1, 1};
        float[] rotations = { -90f, 90f, 180f, 0f };

        // Update the current position
        currentI += iDeltas[direction];
        currentJ += jDeltas[direction];

        // Rotate the object
        transform.rotation = Quaternion.Euler(0, 0, rotations[direction]);
        // Move the object to new position
        transform.position = matrix[currentI, currentJ].transform.position;
    }
}