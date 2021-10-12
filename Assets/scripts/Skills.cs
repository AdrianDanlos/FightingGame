using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Skills
{
    /*
    PASSIVES: Skills that are always present during the fight (%chance based). Not a specific amount of uses. 
    STATBOOSTERS: Skills that award base stats improvements (flat + % gain instantly + 50% increased stat reward per level).  A fighter can have a maximum of 3 statboosters.
    SUPERS: Special skills that are activated at some point during the fight. Can be activated N amount of times but chances decrease after each use.
    TALENTS: Skills that act as a balance tool and award fighters special stats after their first loss. (Daily -> 3 loses max. 10 games total.)
    */
    public enum SkillsList
    {
        SIXTHSENSE,
    }
    enum SkillType
    {
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
            SkillsList.SIXTHSENSE,
            new Dictionary<string, string>
            {
                {"Title", "6th Sense"},
                {"Description", "Increases counter rate stat by 10%"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.Passives.ToString()},
            }
        },
    };
}
