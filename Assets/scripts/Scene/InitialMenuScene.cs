using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    public GameObject continueButton;
    public GameObject enterName;
    public InputField enterNameInput;
    public GameObject usernamePlaceholder;

    string username;

    // this script has to have Start() and ManageSaves.cs Awake() 
    // in order to load properly
    private void Start()
    {
        if (manageSaves.CheckIfFileExists())
        {
            continueButton.SetActive(true);
        }
    }

    public void EnterUsername()
    {
        enterName.SetActive(true);
    }
    public void CancelNewGame()
    {
        enterName.SetActive(false);
    }
    public void LoadMainMenuNewGame()
    {
        username = enterNameInput.text;
        Debug.Log(username);

        manageSaves.CreateDefaultSave(username);
        SScene.newGame = true;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        Debug.Log(SScene.scene);
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

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
