using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    public void LoadLoadingSceneToGame()
    {
        SScene.toInitialMenu = false;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void LoadLoadingSceneToInitialMenu()
    {
        SScene.toInitialMenu = true;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
