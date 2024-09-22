using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding
{
    // Bfs method that returns parent-child pairs for the shortest path
    // in the adjacency graph and updates the counter
    public Dictionary<int, LinkedList<int>> BFS(LinkedList<int>[] adjGraph, int numVertices, int currentIndex, int finalIndex, out int performanceCounter)
    {
        performanceCounter = 0;
        Dictionary<int, LinkedList<int>> parentChildPairs = new Dictionary<int, LinkedList<int>>();
        bool[] visited = new bool[numVertices];

        // Initialise the visited array
        for (int i = 0; i < numVertices; i++)
        {
            visited[i] = false;
        }

        LinkedList<int> queue = new LinkedList<int>();
        visited[currentIndex] = true;
        queue.AddLast(currentIndex);

        // Bfs loop queue contains any elements
        while(queue.Any())
        {
            performanceCounter++;
            currentIndex = queue.First();
            queue.RemoveFirst();

            // Debug.Log($"Current index: {currentIndex}, Final index: {finalIndex}"); 

            if (currentIndex == finalIndex)
            {
                return parentChildPairs;
            }

            LinkedList<int> adjList = adjGraph[currentIndex];
            parentChildPairs.Add(currentIndex, adjList);

            // Debug.Log($"Current index: {currentIndex}, Neighbors: {string.Join(", ", list)}");

            // Enqueue unvisited neighbors
            foreach (var neighbor in adjList)
            {
                if (!visited[neighbor])
                {
                    visited[neighbor] = true;
                    queue.AddLast(neighbor);
                }
            }
        }
        return null;
    }

    // DFS method that returns parent-child pairs for a path
    // in the adjacency graph and updates the counter
    public Dictionary<int, LinkedList<int>> DFS(LinkedList<int>[] adjGraph, int numVertices, int currentIndex, int finalIndex, out int performanceCounter)
    {
        performanceCounter = 0;
        Dictionary<int, LinkedList<int>> parentChildPairs = new Dictionary<int, LinkedList<int>>();
        bool[] visited = new bool[numVertices];

        DFSUtil(adjGraph, currentIndex, finalIndex, visited, parentChildPairs, ref performanceCounter);
        return parentChildPairs;
    }

    //Helper DFS function that performs dfs of the adjacency graph using recursion
    private void DFSUtil(LinkedList<int>[] adjGraph, int currentIndex, int finalIndex, bool[] visited, Dictionary<int, LinkedList<int>> parentChildPairs, ref int performanceCounter)
    {
        visited[currentIndex] = true;
        performanceCounter++;

        // Debug.Log($"Current index: {currentIndex}, Final index: {finalIndex}");

        if (currentIndex == finalIndex)
        {
            return;
        }

        LinkedList<int> adjList = adjGraph[currentIndex];
        parentChildPairs.Add(currentIndex, adjList);

        // Debug.Log($"Current index: {currentIndex}, Neighbors: {string.Join(", ", adjList)}");

        // Recursively call DFSUtil for unvisited neighbors
        foreach(var neighbor in adjList)
        {
            if (!visited[neighbor])
            {
                DFSUtil(adjGraph, neighbor, finalIndex, visited, parentChildPairs, ref performanceCounter);
            }
        }
    }

    // A* search that returns parent-child pairs for the shortest path
    // in the adjacency graph and updates the counter
    public Dictionary<int, LinkedList<int>> AStar(LinkedList<int>[] adjGraph, int numVertices, int currentIndex, int finalIndex, out int performanceCounter, GameObject[,] matrix)
    {
        performanceCounter = 0;
        Dictionary<int, LinkedList<int>> parentChildPairs = new Dictionary<int, LinkedList<int>>();
        bool[] visited = new bool[numVertices];
        int[] gScore = new int[numVertices];
        int[] fScore = new int[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            gScore[i] = int.MaxValue;
            fScore[i] = int.MaxValue;
        }

        gScore[currentIndex] = 0;
        fScore[currentIndex] = ManhattanDistance(matrix, currentIndex, finalIndex);

        List<int> openList = new List<int> { currentIndex };

        while(openList.Any())
        {
            performanceCounter++;

            int current = openList.OrderBy(i => fScore[i]).First();
            openList.Remove(current);

            if (current == finalIndex)
            {
                return parentChildPairs;
            }

            visited[current] = true;
            LinkedList<int> adjList = adjGraph[current];
            parentChildPairs.Add(current, adjList);

            foreach (var neighbor in adjList)
            {
                if (visited[neighbor])
                {
                    continue;
                }

                int tentativeGScore = gScore[current] + 1;
                if (tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + ManhattanDistance(matrix, neighbor, finalIndex);

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    // Calculates the Manhattan distance between two nodes given their indices
    private int ManhattanDistance(GameObject[,] matrix, int nodeIndex1, int nodeIndex2)
    {
        int matrixWidth = matrix.GetLength(1);
        int i1 = nodeIndex1 / matrixWidth;
        int j1 = nodeIndex1 % matrixWidth;
        int i2 = nodeIndex2 / matrixWidth;
        int j2 = nodeIndex2 % matrixWidth;

        return Mathf.Abs(i1 - i2) + Mathf.Abs(j1 - j2);
    }
}
