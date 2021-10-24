using System.Collections;
using System.Collections.Generic;
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
    public Fighter fighter;

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

    public bool CheckIfNameIsValid()
    {
        string name = enterNameInput.text;

        if (name.Length >= 4 && name.Length <= 10)
        {
            return true;
        }

        return false;
    }

    public IEnumerator ShowRegexText(Text nameRegexText)
    {
        Text nameRegex = Instantiate(nameRegexText);
        nameRegex.text = "Fighter name length must be between 4 to 10 characters!";
        nameRegex.transform.SetParent(enterNameMenu.transform, false);

        yield return new WaitForSeconds(3f);
    }
}
