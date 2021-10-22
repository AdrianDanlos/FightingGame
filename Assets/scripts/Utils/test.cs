using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public SMGame sMGame;
    public Skills skills;

    private void Start()
    {
        sMGame.UpdateDataFromCombat(true);
    }

}
