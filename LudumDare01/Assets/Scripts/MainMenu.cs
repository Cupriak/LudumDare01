using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string sceneNameToLoadAtStart = "Yanush";

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNameToLoadAtStart);
    }

    public void CloseGame()
    {
        Debug.Log("Closing Game function called!");
        Application.Quit();
    }
}
