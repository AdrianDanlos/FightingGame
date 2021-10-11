using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    public void LoadLoadingScene()
    {
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

}
