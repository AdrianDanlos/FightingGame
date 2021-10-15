using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;

    private void Start()
    {
        LevelXPSetUp();
    }

    public void GenerateData()
    {
        manageSaves.GenerateInitialValues();
    }

    void LevelXPSetUp()
    {
        int[] toLevelUp = new int[30];

        for (int lv = 1; lv < toLevelUp.Length; lv++)
        {
            toLevelUp[lv] = (int)(Mathf.Floor(4 * (Mathf.Pow(lv, 0.4f))));
            Debug.Log(toLevelUp[lv]);
        }
    }
}
