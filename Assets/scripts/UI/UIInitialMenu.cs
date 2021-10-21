using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInitialMenu : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SavesManager savesManager;

    [Header("UI")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Text changingFighterName;
    [SerializeField] private GameObject enterNameMenu;
    [SerializeField] private InputField enterNameInput;
    public Fighter fighter;

    // this script has to have Start() and ManageSaves.cs Awake() 
    // in order to load properly
    private void Start()
    {
        fighter.ChangeAnimationState(Fighter.AnimationNames.IDLE_BLINK);
        if (savesManager.CheckIfFileExists())
        {
            continueButton.SetActive(true);
        }
    }

    private void Update()
    {
        if (enterNameMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelNewGame();
            }
        };

        // changes text above fighter as input changes
        ChangeFighterNameOnInput();
    }

    // gets called on "new game" button
    public void EnterUsername()
    {
        enterNameMenu.SetActive(true);
        fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
    }

    // gets called on "cancel" button
    public void CancelNewGame()
    {
        fighter.ChangeAnimationState(Fighter.AnimationNames.IDLE_BLINK);
        enterNameMenu.SetActive(false);
    }

    public string ChangeFighterName()
    {
        return enterNameInput.text;
    }

    public void ChangeFighterNameOnInput()
    {
        changingFighterName.text = enterNameInput.text;
    }

}
