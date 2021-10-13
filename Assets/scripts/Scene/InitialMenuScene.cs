using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;
    public void LoadMainMenuNewGame()
    {
        // create new save
        SScene.newGame = true;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        Debug.Log(SScene.scene);
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void LoadMainMenuContinue()
    {
        // load data
        SScene.newGame = false;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
