using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;

    private void Start()
    {
        GetRandomSkill();
    }

    public string GetRandomSkill()
    {
        Array values = Enum.GetValues(typeof(Skills.SkillsList));
        System.Random rand = new System.Random();
        Skills.SkillsList ability = (Skills.SkillsList)values.GetValue(rand.Next(values.Length));
        Debug.Log(ability);
        return ability.ToString();
    }
}
