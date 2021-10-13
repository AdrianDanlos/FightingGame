using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialMenuScene : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
