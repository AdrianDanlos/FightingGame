using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    [Header("Saves Manager")]
    public SMInitialMenu sMInitialMenu;

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
            if (uIManager.IsContinueButtonEnabled())
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    LoadMainMenuContinue();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    uIManager.EnterUsername();
                }
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
        if (uIManager.CheckIfNameIsValid().Equals("valid"))
        {
            // call UI
            fighterName = uIManager.ChangeFighterName();

            sMInitialMenu.CreateDefaultSave(fighterName);
            SScene.newGame = true;
            SScene.scene = (int)SceneIndex.INITIAL_MENU;
            SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
        }
        if(uIManager.CheckIfNameIsValid().Equals("length"))
        {
            uIManager.ShowInvalidLength();
        }
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
