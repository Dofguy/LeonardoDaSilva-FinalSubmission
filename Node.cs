using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.SceneManagement;

public class Node
{   
    // Singleton pattern related variables
    private static Node instance;
    private static readonly object padlock = new object();

    // Grid matrix and GameObject reference
    private static GameObject nodes;
    private static GameObject[,] matrix;
    // Matrix property to get the grid matrix
    public GameObject[,] Matrix{ get { return matrix; }}

    // Singleton getInstance method to get the instance of Node
    public static Node getInstance()
    {
        if (instance == null)
        {
            lock(padlock)
            {
                if (instance == null)
                {
                    instance = new Node();
                }
            }
        }
        return instance;
    }

    // Constructor for the Singleton pattern
    private Node()
    {
        // Finds the gameObject
        nodes = GameObject.Find("Nodes");
        
        int[] matrixSize = GetMatrixSize();
        matrix = new GameObject[matrixSize[0], matrixSize[1]];
        SetMatrix(matrix, nodes);
    }

    // Determines the size of the matrix based on the current scene
    private int[] GetMatrixSize()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "SmallGridBFS": case "SmallGridDFS": case "SmallGridAStar": case "MultiSmallGridBFS": case "MultiSmallGridDFS": case "MultiSmallGridAStar":
                return new int[]{ 12, 12 };
            case "MediumGridBFS": case "MediumGridDFS": case "MediumGridAStar": case "MultiMediumGridBFS": case "MultiMediumGridDFS": case "MultiMediumGridAStar":
                return new int[]{ 16, 16 };
            case "LargeGridBFS": case "LargeGridDFS": case "LargeGridAStar": case "MultiLargeGridBFS": case "MultiLargeGridDFS": case "MultiLargeGridAStar":
                return new int[]{ 20, 20 };
            default:
                return new int[]{ 12, 12 };
        }
    }  

    // Initialises the matrix with GameObjects from the "Nodes' GameObject
    private void SetMatrix(GameObject[,] matrix, GameObject nodes)
    {
        int counter = 0;

        // Loop through the matrix and set each element to the corresponding child GameObject (node)
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] = nodes.transform.GetChild(counter).gameObject;
                counter ++;
            }
        }
    }
}