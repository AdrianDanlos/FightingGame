using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public SavesManager manageSaves;
    public Skills skills;

    private void Start()
    {
        manageSaves.UpdateDataFromCombat(true);

    }

}