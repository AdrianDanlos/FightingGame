using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        Debug.Log(SScene.scene);
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
