using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public ManageSaves manageSaves;

    [Header("UI")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private Text changingFighterName;
    [SerializeField] private GameObject enterName;
    [SerializeField] private InputField enterNameInput;
    public FighterStats fighter;

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
    }

    // gets called on "new game" button
    public void EnterUsername()
    {
        enterName.SetActive(true);
        fighter.ChangeAnimationState(FighterStats.AnimationNames.RUN);
    }

    // gets called on "cancel" button
    public void CancelNewGame()
    {
        enterName.SetActive(false);
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
