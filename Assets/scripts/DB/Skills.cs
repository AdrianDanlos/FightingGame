using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Skills
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
        ENDURANCE_INCREASE,
        STRENGTH_INCREASE,
        AGILITY_INCREASE,
        SPEED_INCREASE,

        // Passives
        SIXTHSENSE,
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

    public static Dictionary<SkillsList, Dictionary<string, string>> skills =
    new Dictionary<SkillsList, Dictionary<string, string>>
    {
        {
            SkillsList.ENDURANCE_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Endurance increase"},
                {"Description", "Increases permanently endurance by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
            }
        },
        {
            SkillsList.STRENGTH_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Strength increase"},
                {"Description", "Increases permanently strength by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
            }
        },
        {
            SkillsList.AGILITY_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Agility increase"},
                {"Description", "Increases permanently agility by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
            }
        },
        {
            SkillsList.SPEED_INCREASE,
            new Dictionary<string, string>
            {
                {"Title", "Speed increase"},
                {"Description", "Increases permanently speed by 3 points"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.StatIncreaser.ToString()},
            }
        },
        {
            SkillsList.SIXTHSENSE,
            new Dictionary<string, string>
            {
                {"Title", "6th Sense"},
                {"Description", "Increases counter rate stat by 10%"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.Passives.ToString()},
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
            }
        },
    };

    public static Dictionary<SkillsList, Dictionary<string, string>> getRandomSkill()
    {
        return skills;
    }
}
