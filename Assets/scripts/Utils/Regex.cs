using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Regex : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField enterNameInput;
    [SerializeField] private Text enterNameError;

    public string CheckIfNameIsValid() 
    {
        string name = enterNameInput.text;

        if (name.Length < 4 || name.Length > 10)
            return "length";
        if (name.Any(ch => !System.Char.IsLetterOrDigit(ch)))
            return "char";

        return "valid";
    }

    public void ShowLengthError()
    {
        enterNameError.gameObject.SetActive(true);
        enterNameError.text = "Fighter name length must be between 4 to 10 characters!";
    }

    public void ShowSpecialCharactersError()
    {
        enterNameError.gameObject.SetActive(true);
        enterNameError.text = "Fighter name can't contain special characters!";
    }

    public void HideErrorText()
    {
        enterNameError.gameObject.SetActive(false);
    }
}
