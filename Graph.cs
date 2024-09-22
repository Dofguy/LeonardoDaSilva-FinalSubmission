using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GraphBuilder
    {
    public readonly Graph graph;

    // Constructor that initialises the graph object based on the input matrix
    public GraphBuilder(GameObject[,] matrix)
    {
        graph = new Graph(matrix.GetLength(0) * matrix.GetLength(1));

        // Iterate through the matrix and add edges between adjacent nodes
        for (int i = 1; i < matrix.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < matrix.GetLength(1) - 1; j++)
            {
            int currentNode = (j + i * matrix.GetLength(0));

                // Check if there is no wall to the left and add an edge if possible
                if (matrix[i, j - 1].transform.gameObject.tag != "Walls") 
                {
                    int adjacentNode = (j + i * matrix.GetLength(0))-1;
                    graph.AddEdge(currentNode, adjacentNode);
                }

                // Check if there is no wall below and add an edge if possible
                if (matrix[i + 1, j].transform.gameObject.tag != "Walls") 
                {
                    int adjacentNode = (j + i * matrix.GetLength(0))+matrix.GetLength(0);
                    graph.AddEdge(currentNode, adjacentNode);
                }

                // Check if there is no wall to the right and add an edge if possible
                if (matrix[i, j + 1].transform.gameObject.tag != "Walls") 
                {
                    int adjacentNode = (j + i * matrix.GetLength(0))+1;
                    graph.AddEdge(currentNode, adjacentNode);
                }

                // Check if there is no wall above and add an edge if possible
                if (matrix[i - 1, j].transform.gameObject.tag != "Walls") 
                {
                    int adjacentNode = (j + i * matrix.GetLength(0))-matrix.GetLength(0);
                    graph.AddEdge(currentNode, adjacentNode);
                }
            }
        }
    }

    // Graph class to represent the adjacency list
    internal class Graph
    {
        // Array of adjacency lists for each vertex
        private LinkedList<int>[] _adj;

        // Property to expose the adjacency lists
        public LinkedList<int>[] Adj { get { return _adj; } }

        // Constructor that initialises the adjacency lists for a given number of vertices
        public Graph(int V)
        {
            _adj = new LinkedList<int>[V];
            for (int i = 0; i < _adj.Length; i++)
            {
                _adj[i] = (new LinkedList<int>());
            }
        }

        // Adds an edge between two vertices in the graph
        public void AddEdge(int v, int w)
        {
            _adj[v].AddLast(w);
        }
    }
}
