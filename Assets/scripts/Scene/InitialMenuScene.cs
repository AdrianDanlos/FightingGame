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
    public GameObject enterNameInput;
    public GameObject usernamePlaceholder;

    bool usernameCreated = false;
    string username;

    private void Awake()
    {
        if (manageSaves.CheckIfFileExists())
        {
            Debug.Log(manageSaves.CheckIfFileExists());
            continueButton.SetActive(true);
        }
    }

    public void EnterUsername()
    {
        enterName.SetActive(true);
    }
    public void LoadMainMenuNewGame()
    {
        username = enterNameInput.GetComponent<Text>().text;
        Debug.Log(username);

        if (username.Length > 5 && username.Length < 16)
        {
            usernameCreated = true;
        }

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
