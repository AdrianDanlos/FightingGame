using System;
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
        // icons used: 2, 4, 5, 6, 8, 9, 28, 54, 65, 79, 87, 95, 94, 96
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
        CRITICAL_STRIKE,
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
                {"Icon", "4" }
            }
        },
        {
            SkillsList.CRITICAL_STRIKE,
            new Dictionary<string, string>
            {
                {"Title", "Critical Strike"},
                {"Description", "Increases the chances of a critical strike by 15%."},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.Passives.ToString()},
                {"Icon", "6" }
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
                {"Icon", "95" }
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

    public string GetSkills(List<string> fighterSkills)
    {
        List<int> rarityTable = new List<int>();
        int commonChance = 50;
        int rareChance = 30;
        int epicChance = 10;
        int legendaryChance = 5;

        List<string> commonSkills = new List<string>();
        List<string> rareSkills = new List<string>();
        List<string> epicSkills = new List<string>();
        List<string> legendarySkills = new List<string>();

        List<string> availableSkills = new List<string>();
        availableSkills = GetAvailableSkills(GetAllSkills(), fighterSkills);

        // gets all available skills sorted by rarity
        for (int i = 0; i < availableSkills.Count; i++)
        {
            switch (GetSkillDataFromSkillName(availableSkills[i])["Rarity"])
            {
                case "Common":
                    commonSkills.Add(availableSkills[i]);
                    break;
                case "Rare":
                    rareSkills.Add(availableSkills[i]);
                    break;
                case "Epic":
                    epicSkills.Add(availableSkills[i]);
                    break;
                case "Legendary":
                    legendarySkills.Add(availableSkills[i]);
                    break;
            }
        }

        List<string> rarities = new List<string>();

        if (commonSkills.Count > 0)
        {
            rarityTable.Add(commonChance);
            rarities.Add("Common");
        }
            
        if (rareSkills.Count > 0)
        {
            rarityTable.Add(rareChance);
            rarities.Add("Rare");
        }
            
        if (epicSkills.Count > 0)
        {
            rarityTable.Add(epicChance);
            rarities.Add("Epic");
        }
            
        if (legendarySkills.Count > 0)
        {
            rarityTable.Add(legendaryChance);
            rarities.Add("Legendary");
        }
            
        int total = 0;

        foreach (int skill in rarityTable)
        {
            total += skill;
        }

        int skillRoll = UnityEngine.Random.Range(0, total + 1);
        string skillRarity = "";

        for (int i = 0; i < rarityTable.Count; i++)
        {
            if (skillRoll <= rarityTable[i])
            {
                skillRarity =  rarities[i];
                break;
            }
            else
            {
                skillRoll -= rarityTable[i];
            }
        }

        string skillChosen = "";

        switch (skillRarity)
        {
            case "Common":
                skillChosen = SkillFromAvailableSkillsFiltered(commonSkills);
                break;
            case "Rare":
                skillChosen = SkillFromAvailableSkillsFiltered(rareSkills);
                break;
            case "Epic":
                skillChosen = SkillFromAvailableSkillsFiltered(epicSkills);
                break;
            case "Legendary":
                skillChosen = SkillFromAvailableSkillsFiltered(legendarySkills);
                break;
        }

        return skillChosen;
    }

    public string SkillFromAvailableSkillsFiltered(List<string> availableSkills)
    {
        if(availableSkills.Count == 1)
        {
            return availableSkills[0];
        }

        return availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
    }

    // FIXME -- use random numbers according to skill rarity
    public List<string> GetTwoRandomSkill(List<string> availableSkills)
    {
        System.Random random = new System.Random();
        List<string> twoSkills = availableSkills.OrderBy(x => random.Next()).Take(2).ToList();

        return twoSkills;
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
