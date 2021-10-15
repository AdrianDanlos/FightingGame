using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public GameObject backToMenu;

    private void Update()
    {
        if (backToMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenu();
            }
        }
            
    }
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.GAME;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

}
