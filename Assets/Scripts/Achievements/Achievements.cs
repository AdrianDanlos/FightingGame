using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum AchievementsList
{
    // icons used: 37, 47, 74, 46, 29
    // icon of locked achievement: 49
    COMMON_SKILL,
    RARE_SKILL,
    EPIC_SKILL, 
    LEGENDARY_SKILL,
    LEVEL_5,
    LEVEL_10,
    LEVEL_15,
    LEVEL_20,
    MAX_LEVEL,
    WIN_50,
    GET_ALL_SKILLS
}

public class Achievements : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField] private GameObject achievementBlock;
    [SerializeField] private GameObject achievementsContainer;
    [SerializeField] private Sprite[] achievementIcons;

    [Header("Data")]
    public SMMainMenu sMMainMenu;
    public LevelDB levelDB;
    public Skills skills;

    public Dictionary<AchievementsList, Dictionary<string, string>> achievements =
    new Dictionary<AchievementsList, Dictionary<string, string>>
    {
        {
            AchievementsList.COMMON_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "47" }
            }
        },
        {
            AchievementsList.RARE_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Not so bad!"},
                {"Description", "Unlock a rare skill."},
                {"Icon", "47" }
            }
        },
        {
            AchievementsList.EPIC_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Good roll"},
                {"Description", "Unlock an epic skill."},
                {"Icon", "47" }
            }
        },
        {
            AchievementsList.LEGENDARY_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Legendary!!!"},
                {"Description", "Unlock a legendary skill."},
                {"Icon", "47" }
            }
        },
        {
            AchievementsList.LEVEL_5,
            new Dictionary<string, string>
            {
                {"Title", "Level 5!"},
                {"Description", "Reach level 5."},
                {"Icon", "74" }
            }
        },
        {
            AchievementsList.LEVEL_10,
            new Dictionary<string, string>
            {
                {"Title", "Level 10!"},
                {"Description", "Reach level 10."},
                {"Icon", "74" }
            }
        },
        {
            AchievementsList.LEVEL_15,
            new Dictionary<string, string>
            {
                {"Title", "Level 15!"},
                {"Description", "Reach level 15."},
                {"Icon", "74" }
            }
        },
        {
            AchievementsList.LEVEL_20,
            new Dictionary<string, string>
            {
                {"Title", "Level 20!"},
                {"Description", "Reach level 20."},
                {"Icon", "74" }
            }
        },
        {
            AchievementsList.MAX_LEVEL,
            new Dictionary<string, string>
            {
                {"Title", "Hero"},
                {"Description", "Reach max level."},
                {"Icon", "37" }
            }
        },
        {
            AchievementsList.WIN_50,
            new Dictionary<string, string>
            {
                {"Title", "Conquerer!"},
                {"Description", "Defeat 50 enemies."},
                {"Icon", "46" }
            }
        },
        {
            AchievementsList.GET_ALL_SKILLS,
            new Dictionary<string, string>
            {
                {"Title", "Collectionist!"},
                {"Description", "Unlock every skill."},
                {"Icon", "29" }
            }
        },
    };



    public void LoadAchievements()
    {
        int j = 0;

        foreach (AchievementsList achievement in (AchievementsList[])Enum.GetValues(typeof(SkillsList)))
        {
            GameObject achievementItem = Instantiate(achievementBlock);
            achievementItem.transform.SetParent(achievementsContainer.transform, false);
            achievementItem.transform.GetChild(0).GetComponent<Text>().text = achievements[achievement]["Title"];
            achievementItem.transform.GetChild(1).GetComponent<Text>().text = achievements[achievement]["Description"];
            for (int i = 0; i < achievementIcons.Length; i++)
            {
                if (string.Equals("icons_" + achievements[achievement]["Icon"], achievementIcons[i].name)) 
                    achievementItem.transform.GetChild(2).GetComponent<Image>().sprite = achievementIcons[i];
            }

            j++;
            if (j == Enum.GetNames(typeof(AchievementsList)).Length) return;
        }
    }

    public Dictionary<AchievementsList, bool> CheckAchievements()
    {
        Dictionary<AchievementsList, bool> achievementsDone = new Dictionary<AchievementsList, bool>();
        Dictionary<string, int> fighterData = sMMainMenu.LoadGameDataStats();
        List<string> fighterSkills = sMMainMenu.LoadGameDataSkills();
        List<AchievementsList> achievementsList = GetAchievementsList();

        // setup all achievements to false
        for(int j = 0; j < Enum.GetNames(typeof(AchievementsList)).Length; j++)
        {
            achievementsDone.Add(achievementsList[j], false);
        }

        // rarities
        for(int i = 0; i < fighterSkills.Count; i++)
        {
            if (skills.GetSkillDataFromSkillName(fighterSkills[i])["Rarity"] == "Common")
                achievementsDone.Add(AchievementsList.COMMON_SKILL, true);
            if (skills.GetSkillDataFromSkillName(fighterSkills[i])["Rarity"] == "Rare")
                achievementsDone.Add(AchievementsList.RARE_SKILL, true);
            if (skills.GetSkillDataFromSkillName(fighterSkills[i])["Rarity"] == "Epic")
                achievementsDone.Add(AchievementsList.EPIC_SKILL, true);
            if (skills.GetSkillDataFromSkillName(fighterSkills[i])["Rarity"] == "Legendary")
                achievementsDone.Add(AchievementsList.LEGENDARY_SKILL, true);
        }

        // levels
        if (fighterData["lv"] >= 5)
            achievementsDone.Add(AchievementsList.LEVEL_5, true);
        if (fighterData["lv"] >= 10)
            achievementsDone.Add(AchievementsList.LEVEL_10, true);
        if (fighterData["lv"] >= 15)
            achievementsDone.Add(AchievementsList.LEVEL_15, true);
        if (fighterData["lv"] >= 20)
            achievementsDone.Add(AchievementsList.LEVEL_20, true);
        if (fighterData["lv"] == levelDB.GetLvCap())
            achievementsDone.Add(AchievementsList.MAX_LEVEL, true);

        // winrate
        if (fighterData["wins"] >= 50)
            achievementsDone.Add(AchievementsList.WIN_50, true);

        return achievementsDone;
    }

    public List<AchievementsList> GetAchievementsList()
    {
        return Enum.GetValues(typeof(AchievementsList)).Cast<AchievementsList>().ToList();
    }

    public Dictionary<string, string> GetAchievementDataFromAchievementName(string achievementName)
    {
        Dictionary<string, string> achievementData = new Dictionary<string, string>();
        foreach (AchievementsList achievement in (AchievementsList[])Enum.GetValues(typeof(AchievementsList)))
        {
            if (achievementName == achievement.ToString())
            {
                achievementData.Add("Title", achievements[achievement]["Title"]);
                achievementData.Add("Description", achievements[achievement]["Description"]);
                achievementData.Add("Rarity", achievements[achievement]["Icon"]);
            }
        }
        return achievementData;
    }

    public List<string> GetAllAchievements()
    {
        List<string> achievementsList = new List<string>();
        foreach (AchievementsList achievement in (AchievementsList[])Enum.GetValues(typeof(SkillsList))) achievementsList.Add(achievement.ToString());
        return achievementsList;
    }

    public int GetNumberOfAchievements()
    {
        return achievements.Count;
    }
}
