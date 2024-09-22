using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{   
    // Number of steps taken in the current algorithm
    private int numSteps = 0;
    private Pacwoman pacwoman;
    private GameObject stepsInd; 

    // Initialises the pacwoman reference and finds the steps indicator in the UI
    private void Start()
    {
        pacwoman = GameObject.Find("Pacwoman").GetComponent<Pacwoman>();
        FindStepsInd();
    }

    // Stops the game if its over
    private void Update()
    {
        if (pacwoman.IsGameOver)
        {
            Time.timeScale = 0;
        }
    }

    // Finds the steps indicator UI element
    private void FindStepsInd()
    {
        var comps = transform.GetChild(0).GetComponentsInChildren<RectTransform>();

        foreach(var comp in comps)
        {
            if (comp.name == "Steps")
            {
                stepsInd = comp.gameObject;
                Debug.Log("Steps GameObject found!");
            }
        }
    }

    // Handles click events for the BFS button
    public void ClickEventsBFS()
    {
        pacwoman.ResetGame();
        Time.timeScale = 1;
        numSteps = pacwoman.MoveForBFS(0.1f);
        Debug.Log("BFS NumSteps: " + numSteps);
        stepsInd.GetComponent<Text>().text = numSteps.ToString();
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Multi"))
        {
            Spirit spirit = GameObject.Find("Spirit").GetComponent<Spirit>();
            spirit.StartMoving();
        }
    }

    // Handles click events for the DFS button
    public void ClickEventsDFS()
    {
        pacwoman.ResetGame();
        Time.timeScale = 1;
        numSteps = pacwoman.MoveForDFS(0.1f);
        Debug.Log("DFS NumSteps: " + numSteps);
        stepsInd.GetComponent<Text>().text = numSteps.ToString();
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Multi"))
        {
            Spirit spirit = GameObject.Find("Spirit").GetComponent<Spirit>();
            spirit.StartMoving();
        }
    }

    // Handles click events for the DFS button
    public void ClickEventsAStar()
    {
        pacwoman.ResetGame();
        Time.timeScale = 1;
        numSteps = pacwoman.MoveForAStar(0.1f);
        Debug.Log("AStar NumSteps: " + numSteps);
        stepsInd.GetComponent<Text>().text = numSteps.ToString();
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Multi"))
        {
            Spirit spirit = GameObject.Find("Spirit").GetComponent<Spirit>();
            spirit.StartMoving();
        }
    }

    // Handles click events for the exit button
    public void ClickEventsForExit()
    {
        Application.Quit();
    }

}
