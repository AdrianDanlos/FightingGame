using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;

    private void Start()
    {

        int randomSkill1 = 0;
        int common = 0, rare = 0, epic = 0, legendary = 0;
        for (int i = 0; i < 1000; i++)
        {
            randomSkill1 = UnityEngine.Random.Range(0, 100) + 1;
            if (randomSkill1 <= 60)
            {
                common++;
            }
            if (randomSkill1 <= 85)
            {
                rare++;
            }
            if (randomSkill1 <= 94)
            {
                epic++;
            }
            if (randomSkill1 <= 100)
            {
                legendary++;
            }
        }

        Debug.Log(common + "-" + rare + "-" + epic + "-" + legendary);
    }

}
