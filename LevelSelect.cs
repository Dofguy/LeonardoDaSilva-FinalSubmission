using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    // Loads the scene chosen
    public void LoadingLevels(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }

    // Returns back to the main menu
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
