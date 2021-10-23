using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Sprite lockedAchievement;

    private void Start()
    {
        Debug.Log(GetNumberOfAchievements());
    }

    public Dictionary<AchievementsList, Dictionary<string, string>> achievements =
    new Dictionary<AchievementsList, Dictionary<string, string>>
    {
        {
            AchievementsList.COMMON_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            AchievementsList.RARE_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Not so bad!"},
                {"Description", "Unlock a rare skill."},
                {"Icon", "37" }
            }
        },
        {
            AchievementsList.EPIC_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Good roll"},
                {"Description", "Unlock an epic skill."},
                {"Icon", "37" }
            }
        },
        {
            AchievementsList.LEGENDARY_SKILL,
            new Dictionary<string, string>
            {
                {"Title", "Legendary!!!"},
                {"Description", "Unlock a legendary skill."},
                {"Icon", "37" }
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

    public void ShowAchievements()
    {
        string title = "Locked";
        string description = "...";
        string icon = "49";

        // get fighter achievements from save
        List<string> fighterAchievements = new List<string>() { AchievementsList.COMMON_SKILL.ToString() };

        foreach (AchievementsList achievement in (AchievementsList[])Enum.GetValues(typeof(SkillsList)))
        { 
            GameObject achievementItem = Instantiate(achievementBlock);
            achievementItem.transform.SetParent(this.gameObject.transform, false);
            int i = 0;

            // set title, description and image
            if (fighterAchievements[i].Equals(achievement)){
                //achievementItem.transform.GetChild(0).GetComponent<Text>().text = achievement[i]["Title"];
                //achievementItem.transform.GetChild(1).GetComponent<Text>().text = achievement[i]["Description"];
                //achievementItem.transform.GetChild(2).GetComponent<Image>().sprite = "icons_" + achievement[i]["Icon"];
            } 

            // show locked achievement
            else
            {
                achievementItem.transform.GetChild(0).GetComponent<Text>().text = title;
                achievementItem.transform.GetChild(1).GetComponent<Text>().text = description;
                achievementItem.transform.GetChild(2).GetComponent<Image>().sprite = lockedAchievement;
            }
        }
    }

    public Dictionary<string, string> GetAchievementDataFromAchievementName(string achievementName)
    {
        Dictionary<string, string> achievementData = new Dictionary<string, string>();
        foreach (SkillsList skill in (SkillsList[])Enum.GetValues(typeof(SkillsList)))
        {/*
            if (skillChoice == skill.ToString())
            {
                achievementData.Add("Title", skills[skill]["Title"]);
                achievementData.Add("Description", skills[skill]["Description"]);
                achievementData.Add("Rarity", skills[skill]["Rarity"]);
                achievementData.Add("Category", skills[skill]["Category"]);
                achievementData.Add("Icon", skills[skill]["Icon"]);
            }*/
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
