using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SavesManager savesManager;

    [Header("UI")]
    [SerializeField] public UIInitialMenu uIManager;
    [SerializeField] private GameObject enterName;
    private string fighterName;

    private void Update()
    {
        if (enterName.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadMainMenuNewGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadMainMenuContinue();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    // loads main_menu from initial_menu from enter name menu
    public void LoadMainMenuNewGame()
    {
        // call UI
        fighterName = uIManager.ChangeFighterName();

        savesManager.CreateDefaultSave(fighterName);
        SScene.newGame = true;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    // loads main_menu from initial_menu on "continue" button
    public void LoadMainMenuContinue()
    {
        SScene.newGame = false;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
