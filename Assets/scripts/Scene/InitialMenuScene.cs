using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialMenuScene : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public ManageSaves manageSaves;

    [Header("UI")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Text changingFighterName;
    [SerializeField] private GameObject enterName;
    [SerializeField] private InputField enterNameInput;
    [SerializeField] private GameObject usernamePlaceholder;

    [Header("Fighter")]
    public FighterStats fighter;

    private string fighterName;

    // this script has to have Start() and ManageSaves.cs Awake() 
    // in order to load properly
    private void Start()
    {
        fighter.ChangeAnimationState(FighterStats.AnimationNames.IDLE_BLINK);
        if (manageSaves.CheckIfFileExists())
        {
            continueButton.SetActive(true);
        }
    }

    private void Update()
    {
        // changes text above fighter as input changes
        ChangeFighterNameOnInput();

        if (enterName.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadMainMenuNewGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void EnterUsername()
    {
        enterName.SetActive(true);
        fighter.ChangeAnimationState(FighterStats.AnimationNames.RUN);
    }
    public void CancelNewGame()
    {
        enterName.SetActive(false);
    }
    public void LoadMainMenuNewGame()
    {
        fighterName = enterNameInput.text;

        manageSaves.CreateDefaultSave(fighterName);
        SScene.newGame = true;
        SScene.scene = (int)SceneIndex.INITIAL_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void ChangeFighterNameOnInput()
    {
        changingFighterName.text = enterNameInput.text;
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
