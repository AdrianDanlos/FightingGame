using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIInitialMenu : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SMCore sMCore;

    [Header("UI")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Text changingFighterName;
    [SerializeField] private GameObject enterNameMenu;
    [SerializeField] private InputField enterNameInput;

    [Header("Skins")]
    public Fighter fighter;
    public Skins skins;

    [Header("Regex")]
    [SerializeField] private Regex regexManager;

    // this script has to have Start() and ManageSaves.cs Awake() 
    // in order to load properly
    private void Start()
    {
        fighter.ChangeAnimationState(Fighter.AnimationNames.IDLE_BLINK);
        if (sMCore.CheckIfFileExists())
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

        if (regexManager.CheckIfNameIsValid().Equals("valid")){
            regexManager.HideErrorText();
        }
    }

    public bool IsContinueButtonEnabled()
    {
        return continueButton.activeSelf ? true : false;
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
        skins.SetDefaultSkin(); // set default skin
        fighter.ChangeAnimationState(Fighter.AnimationNames.IDLE_BLINK);
        enterNameMenu.SetActive(false);
    }

    public string ChangeFighterName()
    {
        return enterNameInput.text;
    }

    private void ChangeFighterNameOnInput()
    {
        changingFighterName.text = enterNameInput.text;
    }

    public void SelectGolem()
    {
        Skins.skinSelected = "Golem";
    }

    public void SelectReaper()
    {
        Skins.skinSelected = "Reaper";
    }

    public void SelectAngel()
    {
        Skins.skinSelected = "Fallen_Angel";
    }
}
