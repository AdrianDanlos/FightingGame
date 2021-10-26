using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
    [Header("Library")]
    [SerializeField] private GameObject skillBlock;
    [SerializeField] private GameObject skillsContainer;
    [SerializeField] private Sprite[] skillsIcons;

    [Header("Skills")]
    public Skills skills;

    public void LoadLibrary()
    {
        Dictionary<SkillsListByRarity, Dictionary<string, string>> skillsList = skills.GetSkillsList();
        int j = 0;

        foreach (SkillsListByRarity skill in (SkillsListByRarity[])Enum.GetValues(typeof(SkillsListByRarity)))
        {
            GameObject achievementItem = Instantiate(skillBlock);
            achievementItem.transform.SetParent(skillsContainer.transform, false);
            achievementItem.transform.GetChild(0).GetComponent<Text>().text = skillsList[skill]["Title"];
            achievementItem.transform.GetChild(0).GetComponent<Text>().color = 
                skills.GetColorFromRarity(skills.GetSkillDataFromSkillName(skill.ToString())["Rarity"]);
            achievementItem.transform.GetChild(1).GetComponent<Text>().text = skillsList[skill]["Description"];
            for (int i = 0; i < skillsIcons.Length; i++)
            {
                if (string.Equals("icons_" + skillsList[skill]["Icon"], skillsIcons[i].name))
                    achievementItem.transform.GetChild(2).GetComponent<Image>().sprite = skillsIcons[i];
            }

            j++;


            if (j == Enum.GetNames(typeof(SkillsList)).Length) return;
        }
    }
}
