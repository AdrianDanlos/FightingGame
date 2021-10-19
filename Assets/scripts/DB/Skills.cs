using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skills : MonoBehaviour
{
    /*
    STAT INCREASERS: Permanently increase base stats
    PASSIVES: Skills that are always present during the fight (%chance based). Not a specific amount of uses. 
    STATBOOSTERS: Skills that award stats improvements (flat + % gain instantly + 50% increased stat reward per level).  A fighter can have a maximum of 3 statboosters.
    SUPERS: Special skills that are activated at some point during the fight. Can be activated N amount of times but chances decrease after each use.
    TALENTS: Skills that act as a balance tool and award fighters special stats after their first loss. (Daily -> 3 loses max. 10 games total.)
    */
    public enum SkillsList
    {
        // Stat increasers
        HP_INCREASE,
        STRENGTH_INCREASE,
        AGILITY_INCREASE,
        SPEED_INCREASE,

        // Stat boosters
        SUPER_STRENGTH,
        SUPER_AGILITY,
        SUPER_SPEED,
        SUPER_HP,

        // Passives
        SIXTH_SENSE,
        HOSTILITY,
        TOUGHENED_SKIN,
        ARMOR,
        CRITICAL_ATTACK,
        SABOTAGE
    }
    enum SkillType
    {
        StatIncreaser,
        Passives,
        Statboosters,
        Supers,
        Talents
    }

    /*
     * FIXME -- implement rarity chances in level up menu
    common > green
    rare > blue
    epic > purple
    legendary > orange
    */
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public Dictionary<SkillsList, Dictionary<string, string>> skills =
    new Dictionary<SkillsList, Dictionary<string, string>>
    {
        {
            SkillsList.HP_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Hp Increase"},
                {"Description", "Increases permanently hp by 18 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
                {"Icon", "5" }
            }
        },
        {
            SkillsList.STRENGTH_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Strength Increase"},
                {"Description", "Increases permanently strength by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
                {"Icon", "65" }
            }
        },
        {
            SkillsList.AGILITY_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Agility Increase"},
                {"Description", "Increases permanently agility by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
                {"Icon", "54" }
            }
        },
        {
            SkillsList.SPEED_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Speed Increase"},
                {"Description", "Increases permanently speed by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
                {"Icon", "94" }
            }
        },
        {
            SkillsList.SUPER_STRENGTH,
            new Dictionary<string, string>
            {
                {"Title", "Super Strength"},
                {"Description", "Increases all future Strength gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Statboosters.ToString()},
                {"Icon", "96" }
            }
        },
        {
            SkillsList.SUPER_AGILITY,
            new Dictionary<string, string>
            {
                {"Title", "Super Agility"},
                {"Description", "Increases all future Agility gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Statboosters.ToString()},
                {"Icon", "2" }
            }
        },
        {
            SkillsList.SUPER_SPEED,
            new Dictionary<string, string>
            {
                {"Title", "Super Speed"},
                {"Description", "Increases all future Speed gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Statboosters.ToString()},
                {"Icon", "28" }
            }
        },
        {
            SkillsList.SUPER_HP,
            new Dictionary<string, string>
            {
                {"Title", "Super HP"},
                {"Description", "Increases all future HP gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Statboosters.ToString()},
                {"Icon", "8" }
            }
        },
        {
            SkillsList.SIXTH_SENSE,
            new Dictionary<string, string>
            {
                {"Title", "6th Sense"},
                {"Description", "Increases counter rate stat by 10%"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.Passives.ToString()},
                {"Icon", "9" }
            }
        },
        {
            SkillsList.HOSTILITY,
            new Dictionary<string, string>
            {
                {"Title", "Hostility"},
                {"Description", "Increases reversal rate stat by 30%"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.Passives.ToString()},
                {"Icon", "79" }
            }
        },
        {
            SkillsList.TOUGHENED_SKIN,
            new Dictionary<string, string>
            {
                {"Title", "Toughened Skin"},
                {"Description", "Increases the Armor by 2"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.Passives.ToString()},
                {"Icon", "87" }
            }
        },
        {
            SkillsList.ARMOR,
            new Dictionary<string, string>
            {
                {"Title", "Armor"},
                {"Description", "Increases the Armor by 5 but deccreases the speed by 10%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Passives.ToString()},
            }
        },
        {
            SkillsList.CRITICAL_ATTACK,
            new Dictionary<string, string>
            {
                {"Title", "Critical attack"},
                {"Description", "Increases the chances of a critical attack by 15%. Criticals attack deal x2 damage."},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Passives.ToString()},
            }
        },
        {
            SkillsList.SABOTAGE,
            new Dictionary<string, string>
            {
                {"Title", "Sabotage"},
                {"Description", "You get a 15% chance of disabling a skill from your opponent each time you hit him."},
                {"Rarity", Rarity.Legendary.ToString()},
                {"Category", SkillType.Passives.ToString()},
            }
        },
    };

    public List<string> GetAllSkills()
    {
        List<string> skills = new List<string>();

        foreach (SkillsList skill in (SkillsList[])Enum.GetValues(typeof(SkillsList)))
        {
            skills.Add(skill.ToString());
        }
        return skills;
    }

    public List<string> GetAllRarities()
    {
        List<string> rarities = new List<string>();

        foreach (Rarity rarity in (Rarity[])Enum.GetValues(typeof(Rarity)))
        {
            rarities.Add(rarity.ToString());
        }
        return rarities;
    }

    public Dictionary<SkillsList, Dictionary<string, string>> GetAllSkillsDictionary()
    {
        return skills;
    }

    public List<string> GetSkillLvUpOptionsByRarity(List<string> fighterSkills)
    {
        int randomSkill1 = UnityEngine.Random.Range(0, 100) + 1;
        int randomSkill2 = UnityEngine.Random.Range(0, 100) + 1;

        List<string> common = new List<string>();
        List<string> rare = new List<string>();
        List<string> epic = new List<string>();
        List<string> legendary = new List<string>();

        List<string> skillsPool = new List<string>();
        List<string> availableSkills = GetAvailableSkills(GetAllSkills(), fighterSkills);

        for (int i = 0; i < availableSkills.Count; i++)
        {
            switch (GetSkillDataFromSkillName(availableSkills[i])["Rarity"])
            {
                case "Common":
                    common.Add(availableSkills[i]);
                    break;
                case "Rare":
                    rare.Add(availableSkills[i]);
                    break;
                case "Epic":
                    epic.Add(availableSkills[i]);
                    break;
                case "Legendary":
                    legendary.Add(availableSkills[i]);
                    break;
            }
        }

        Debug.Log(rare.Count + "-" + epic.Count);

        List<string> skillsPoolOfAllRarities = new List<string>();
        skillsPoolOfAllRarities.AddRange(common);
        skillsPoolOfAllRarities.AddRange(rare);
        skillsPoolOfAllRarities.AddRange(epic);
        skillsPoolOfAllRarities.AddRange(legendary);

        // FIXME RE -- need to unity all pools into one so it doesnt need to look out for
        // another one
        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        // Debug.Log(skillsPool[0] + "-" + skillsPool[1]);
        return skillsPool;
    }

    // FIXME -- use random numbers according to skill rarity
    public List<string> GetTwoRandomSkill(List<string> availableSkills)
    {
        System.Random random = new System.Random();
        List<string> twoSkills = availableSkills.OrderBy(x => random.Next()).Take(2).ToList();

        return twoSkills;
    }

    public string GetRandomRarity(int random)
    {
        string rarity = "";

        if (random >= 1 || random <= 60)
            rarity = Rarity.Common.ToString();
        if (random >= 61 || random <= 85)
            rarity = Rarity.Rare.ToString();
        if (random >= 86 || random <= 94)
            rarity = Rarity.Epic.ToString();
        if (random >= 95 || random <= 100)
            rarity = Rarity.Legendary.ToString();

        return rarity;
    }

    public string GetRandomSkill(List<string> availableSkills)
    {
        System.Random random = new System.Random();
        return availableSkills.OrderBy(x => random.Next()).Take(1).ToString();
    }

    public bool CheckIfSkillIsAStatIncreaser(string skill)
    {
        return (skill == "HP_INCREASE" || skill == "STRENGTH_INCREASE"
            || skill == "AGILITY_INCREASE" || skill == "SPEED_INCREASE") ? true : false;

    }

    public List<string> GetAvailableSkills(List<string> allSkills, List<string> fighterSkills)
    {
        return allSkills.Except(fighterSkills).ToList();
    }

    public Dictionary<string, string> GetSkillDataFromSkillName(string skillChoice)
    {
        Dictionary<string, string> skillData = new Dictionary<string, string>();
        foreach (SkillsList skill in (SkillsList[])Enum.GetValues(typeof(SkillsList)))
        {
            if (skillChoice == skill.ToString())
            {
                skillData.Add("Title", skills[skill]["Title"]);
                skillData.Add("Description", skills[skill]["Description"]);
                skillData.Add("Rarity", skills[skill]["Rarity"]);
                skillData.Add("Category", skills[skill]["Category"]);
                skillData.Add("Icon", skills[skill]["Icon"]);
            }
        }
        return skillData;
    }

    public Color GetColorFromRarity(string rarity)
    {
        Color rarityColor = Color.white;
        switch (rarity)
        {
            case "Common":
                ColorUtility.TryParseHtmlString("#00FF00", out rarityColor);
                break;
            case "Rare":
                ColorUtility.TryParseHtmlString("#0088FF", out rarityColor);
                break;
            case "Epic":
                ColorUtility.TryParseHtmlString("#8A00FF", out rarityColor);
                break;
            case "Legendary":
                ColorUtility.TryParseHtmlString("#FF8300", out rarityColor);
                break;
        }
        return rarityColor;
    }
}
