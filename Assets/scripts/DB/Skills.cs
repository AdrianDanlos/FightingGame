using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillsList
{
    // icons used: 2, 4, 5, 6, 8, 9, 28, 34, 38, 52, 54, 64, 65, 74, 79, 87, 95, 94, 96
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
    SABOTAGE,
    SURVIVAL,
    DETERMINATION,
    FIRST_STRIKE,
    RESISTANT,
    BALLET_SHOES
}

public enum SkillsListByRarity // FIXME -- order one with code
{
    HP_INCREASE,
    STRENGTH_INCREASE,
    AGILITY_INCREASE,
    SPEED_INCREASE,
    SURVIVAL,
    FIRST_STRIKE,
    BALLET_SHOES,
    SUPER_STRENGTH,
    SUPER_AGILITY,
    SUPER_SPEED,
    SUPER_HP,
    RESISTANT,
    ARMOR,
    CRITICAL_STRIKE,
    SIXTH_SENSE,
    HOSTILITY,
    TOUGHENED_SKIN,
    DETERMINATION,
    SABOTAGE,
}
enum SkillType
{
    STAT_INCREASER,
    PASSIVES,
    STATBOOSTERS,
    SUPERS,
    TALENTS
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}

public class Skills : MonoBehaviour
{
    /*
    STAT INCREASERS: Permanently increase base stats
    PASSIVES: Skills that are always present during the fight (%chance based). Not a specific amount of uses. 
    STATBOOSTERS: Skills that award stats improvements (flat + % gain instantly + 50% increased stat reward per level).  A fighter can have a maximum of 3 statboosters.
    SUPERS: Special skills that are activated at some point during the fight. Can be activated N amount of times but chances decrease after each use.
    TALENTS: Skills that act as a balance tool and award fighters special stats after their first loss. (Daily -> 3 loses max. 10 games total.)
    */

    public Dictionary<SkillsListByRarity, Dictionary<string, string>> skills =
    new Dictionary<SkillsListByRarity, Dictionary<string, string>>
    {
        {
            SkillsListByRarity.HP_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Hp Increase"},
                {"Description", "Increases permanently hp by 12 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.STAT_INCREASER.ToString()},
                {"Icon", "5" }
            }
        },
        {
            SkillsListByRarity.STRENGTH_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Strength Increase"},
                {"Description", "Increases permanently strength by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.STAT_INCREASER.ToString()},
                {"Icon", "65" }
            }
        },
        {
            SkillsListByRarity.AGILITY_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Agility Increase"},
                {"Description", "Increases permanently agility by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.STAT_INCREASER.ToString()},
                {"Icon", "54" }
            }
        },
        {
            SkillsListByRarity.SPEED_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Speed Increase"},
                {"Description", "Increases permanently speed by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.STAT_INCREASER.ToString()},
                {"Icon", "94" }
            }
        },
        {
            SkillsListByRarity.BALLET_SHOES,
            new Dictionary<string, string>
            {
                {"Title", "Ballet shoes"},
                {"Description", "Dodges the first hit."},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "38" }
            }
        },
        {
            SkillsListByRarity.SURVIVAL,
            new Dictionary<string, string>
            {
                {"Title", "Survival"},
                {"Description", "Leaves you with 1 HP instead of knocking you out"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "74" }
            }
        },
        {
            SkillsListByRarity.FIRST_STRIKE,
            new Dictionary<string, string>
            {
                {"Title", "First strike"},
                {"Description", "Allows you to be the first one attacking at the start of the combat"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "34" }
            }
        },
        {
            SkillsListByRarity.SUPER_STRENGTH,
            new Dictionary<string, string>
            {
                {"Title", "Super Strength"},
                {"Description", "Increases +1 Strength and all future Strength gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.STATBOOSTERS.ToString()},
                {"Icon", "96" }
            }
        },
        {
            SkillsListByRarity.SUPER_AGILITY,
            new Dictionary<string, string>
            {
                {"Title", "Super Agility"},
                {"Description", "Increases +1 Agility and all future Agility gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.STATBOOSTERS.ToString()},
                {"Icon", "2" }
            }
        },
        {
            SkillsListByRarity.SUPER_SPEED,
            new Dictionary<string, string>
            {
                {"Title", "Super Speed"},
                {"Description", "Increases +1 Speed and all future Speed gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.STATBOOSTERS.ToString()},
                {"Icon", "28" }
            }
        },
        {
            SkillsListByRarity.RESISTANT,
            new Dictionary<string, string>
            {
                {"Title", "Resistant"},
                {"Description", "Attacks can't do more than 1/5th damage of your total health."},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "52" }
            }
        },
        {
            SkillsListByRarity.SUPER_HP,
            new Dictionary<string, string>
            {
                {"Title", "Super HP"},
                {"Description", "Increases +6 HP and all future HP gains by 30%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.STATBOOSTERS.ToString()},
                {"Icon", "8" }
            }
        },
        {
            SkillsListByRarity.ARMOR,
            new Dictionary<string, string>
            {
                {"Title", "Armor"},
                {"Description", "Increases the Armor by 5 but deccreases the speed by 10%"},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "4" }
            }
        },
        {
            SkillsListByRarity.CRITICAL_STRIKE,
            new Dictionary<string, string>
            {
                {"Title", "Critical Strike"},
                {"Description", "Increases the chances of a critical strike by 15%."},
                {"Rarity", Rarity.Rare.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "6" }
            }
        },
        {
            SkillsListByRarity.SIXTH_SENSE,
            new Dictionary<string, string>
            {
                {"Title", "6th Sense"},
                {"Description", "Increases counter rate stat by 10%"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "9" }
            }
        },
        {
            SkillsListByRarity.HOSTILITY,
            new Dictionary<string, string>
            {
                {"Title", "Hostility"},
                {"Description", "Increases reversal rate stat by 30%"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "79" }
            }
        },
        {
            SkillsListByRarity.TOUGHENED_SKIN,
            new Dictionary<string, string>
            {
                {"Title", "Toughened Skin"},
                {"Description", "Increases the Armor by 2"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "87" }
            }
        },
        {
            SkillsListByRarity.DETERMINATION,
            new Dictionary<string, string>
            {
                {"Title", "Determination"},
                {"Description", "If you miss, you have a 50% chance to attack again"},
                {"Rarity", Rarity.Epic.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "64" }
            }
        },

        {
            SkillsListByRarity.SABOTAGE,
            new Dictionary<string, string>
            {
                {"Title", "Sabotage"},
                {"Description", "You get a 15% chance of disabling a skill from your opponent each time you hit him."},
                {"Rarity", Rarity.Legendary.ToString()},
                {"Category", SkillType.PASSIVES.ToString()},
                {"Icon", "95" }
            }
        }
    };

    public List<string> GetAllSkills()
    {
        List<string> skills = new List<string>();
        foreach (SkillsList skill in (SkillsList[])Enum.GetValues(typeof(SkillsList))) skills.Add(skill.ToString());
        return skills;
    }

    public Dictionary<SkillsListByRarity, Dictionary<string, string>> GetSkillsList()
    {
        return skills;
    }

    public string GetRandomSkill(List<string> fighterSkills)
    {
        List<string> availableSkills = GetAvailableSkills(GetAllSkills(), fighterSkills);
        return availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
    }

    public string GetRandomSkillByRarityChance(List<string> fighterSkills, List<string> availableSkills)
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

        int totalValueOfRarities = 0;

        foreach (int skillRate in rarityTable)
        {
            totalValueOfRarities += skillRate;
        }

        int skillRoll = UnityEngine.Random.Range(0, totalValueOfRarities + 1);
        string skillRarity = "";

        for (int i = 0; i < rarityTable.Count; i++)
        {
            if (skillRoll <= rarityTable[i])
            {
                skillRarity = rarities[i];
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
                skillChosen = SkillFromFilteredAvailableSkills(commonSkills);
                break;
            case "Rare":
                skillChosen = SkillFromFilteredAvailableSkills(rareSkills);
                break;
            case "Epic":
                skillChosen = SkillFromFilteredAvailableSkills(epicSkills);
                break;
            case "Legendary":
                skillChosen = SkillFromFilteredAvailableSkills(legendarySkills);
                break;
        }

        return skillChosen;
    }

    public string SkillFromFilteredAvailableSkills(List<string> availableSkills)
    {
        if (availableSkills.Count == 1)
        {
            return availableSkills[0];
        }

        return availableSkills[UnityEngine.Random.Range(0, availableSkills.Count)];
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

    public Dictionary<string, string> GetSkillDataFromSkillName(string skillName)
    {
        Dictionary<string, string> skillData = new Dictionary<string, string>();
        foreach (SkillsListByRarity skill in (SkillsListByRarity[])Enum.GetValues(typeof(SkillsListByRarity)))
        {
            if (skillName == skill.ToString())
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
