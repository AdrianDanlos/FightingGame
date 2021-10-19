using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;

    private void Start()
    {
        int randomSkill1 = UnityEngine.Random.Range(0, 100) + 1;
        for (int i = 0; i < 50; i++)
        {
            Debug.Log(GetRandomRarity(randomSkill1));
        }
    }

    public string GetRandomRarity(int random)
    {
        string rarity = "";
        int common = 0, rare = 0, epic = 0, legendary = 0;


        if (random >= 1 || random <= 60)
        {
            rarity = Skills.Rarity.Common.ToString();
            common++;
        }
        if (random >= 61 || random <= 85)
        {
            rarity = Skills.Rarity.Common.ToString();
            rare++;
        }
        if (random >= 86 || random <= 94)
        {
            rarity = Skills.Rarity.Common.ToString();
            epic++;
        }
        if (random >= 95 || random <= 100)
        {
            rarity = Skills.Rarity.Common.ToString();
            legendary++;
        }


        return "common : " + common + " rare: " + rare + " epic: " + epic;
    }
}
