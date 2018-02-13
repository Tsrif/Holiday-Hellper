using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    // Use this for initialization
    public string start;
    public string mainMenu;

    public GameObject inGameUI;



    public void LoadScene()
    {
        SceneManager.LoadScene(start);
    }

    public void QuitGame()
    {
        Debug.Log("Game Closed");
        Application.Quit();
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
