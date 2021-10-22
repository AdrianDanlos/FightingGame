using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMMainMenu : MonoBehaviour
{
    [Header("Data")]
    private string savePath;
    public Skills skills;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
    }
}
