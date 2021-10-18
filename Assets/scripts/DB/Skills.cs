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
    }
    enum SkillType
    {
        StatIncreaser,
        Passives,
        Statboosters,
        Supers,
        Talents
    }
    enum Rarity
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
                {"Description", "Increases permanently hp by 3 points"},
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
                {"Rarity", Rarity.Common.ToString()},
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
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.Statboosters.ToString()},
                {"Icon", "54" }
            }
        },
        {
            SkillsList.SUPER_SPEED,
            new Dictionary<string, string>
            {
                {"Title", "Super Speed"},
                {"Description", "Increases all future Speed gains by 30%"},
                {"Rarity", Rarity.Common.ToString()},
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
                {"Rarity", Rarity.Common.ToString()},
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
                {"Rarity", Rarity.Common.ToString()},
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
                {"Rarity", Rarity.Common.ToString()},
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
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.Passives.ToString()},
                {"Icon", "87" }
            }
        }
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

    public List<string> GetTwoRandomSkill(List<string> availableSkills)
    { 
        System.Random random = new System.Random();
        List<string> twoSkills = availableSkills.OrderBy(x => random.Next()).Take(2).ToList();

        return twoSkills;
    }

    // doesn't let already known skills to be in the selectable skills
    public List<string> GetAvailableSkills(List<string> allSkills, List<string> fighterSkills)
    {
        return allSkills.Except(fighterSkills).ToList();
    }
}
