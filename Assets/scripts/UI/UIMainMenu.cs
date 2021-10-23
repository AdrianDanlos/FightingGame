using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    // LevelDB 
    [Header("Data")]
    public LevelDB levelDB;
    public Skills skillsScript;

    [Header("Level")]
    [SerializeField] private Text lvText;
    [SerializeField] private GameObject experience;
    [SerializeField] private Text xpText;
    [SerializeField] private Text xpTargetText;

    [Header("Fighter")]
    [SerializeField] private Text hpText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text agilityText;
    [SerializeField] private Text speedText;

    [Header("Skills")]
    [SerializeField] private GameObject[] fighterIconsArray;
    [SerializeField] public Sprite[] iconsArray;

    [Header("User")]
    [SerializeField] private Text fighterNameText;
    [SerializeField] private Text winsText;
    [SerializeField] private Text defeatsText;

    [Header("Achievements")]
    [SerializeField] private GameObject achievements;

    public void ShowData(int xp, int lv, int hp, int strength, int agility, int speed, List<string> skills,
        string fighterName, int wins, int defeats) 
    {
        // Level
        lvText.text = Convert.ToString(lv);
        DisplayLv(lv);
        DisplayXp(lv, xp);

        // Fighter
        hpText.text = Convert.ToString(hp);
        strengthText.text = Convert.ToString(strength);
        agilityText.text = Convert.ToString(agility);
        speedText.text = Convert.ToString(speed);
        DisplaySkillIcons(skills);

        // User
        fighterNameText.text = fighterName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }

    private void DisplayXp(int lv, int xp)
    {
        if (lv <= 1)
        {
            xpText.text = Convert.ToString(xp);
            xpTargetText.text = Convert.ToString(levelDB.GetTargetXpBasedOnLv(lv));
        }
        else
        {
            xpText.text = Convert.ToString(xp - levelDB.GetTargetXpBasedOnLv(lv - 1));
            xpTargetText.text = Convert.ToString(levelDB.GetTargetXpBasedOnLv(lv) - levelDB.GetTargetXpBasedOnLv(lv - 1));
        }
    }

    private void DisplayLv(int lv)
    {
        if (lv == levelDB.GetLvCap())
        {
            lvText.text = lv + " [MAX LV]";
            experience.SetActive(false);
        }
    }

    private void DisplaySkillIcons(List<string> fighterSkillsNames)
    {
        Dictionary<string, Dictionary<string, string>> fighterSkillsData =
            GetDataOfFighterSkills(fighterSkillsNames);

        string skillDataIconNumber = "";
        int i = 0;

        fighterSkillsNames.ForEach(delegate (string skill)
        {
            skillDataIconNumber = "icons_" + fighterSkillsData[skill]["Icon"];

            for (int j = 0; j < iconsArray.Length; j++)
            {
                if (string.Equals(skillDataIconNumber, iconsArray[j].name))
                {
                    fighterIconsArray[i].GetComponent<Image>().sprite = iconsArray[j];
                }
            }
            fighterIconsArray[i].SetActive(true);
            i++;
        });
    }

    private Dictionary<string, Dictionary<string, string>>
        GetDataOfFighterSkills(List<string> fighterSkillsNames)
    {
        Dictionary<string, Dictionary<string, string>> fighterSkillsData =
            new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < fighterSkillsNames.Count; i++)
        {
            fighterSkillsData.Add(
                fighterSkillsNames[i], skillsScript.GetSkillDataFromSkillName(fighterSkillsNames[i]));
        }

        return fighterSkillsData;
    }

    public void DisplayAchievements()
    {
        if (!achievements.activeSelf)
        {
            achievements.SetActive(true);
        }
        else if (achievements.activeSelf)
        {
            achievements.SetActive(false);
        }
    }

    public bool IsAchievementsActive()
    {
        return achievements.activeSelf;
    }

}
