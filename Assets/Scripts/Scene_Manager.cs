using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
   
    public void MainMenu()
    {
       SceneManager.LoadScene("Main Menu");
    }
    public void EndGame()
    {
        Application.Quit();
    }
}
