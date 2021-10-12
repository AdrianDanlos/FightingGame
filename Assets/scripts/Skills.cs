using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    /*
    PASSIVES: Skills that are always present during the fight (%chance based). Not a specific amount of uses. 
    STATBOOSTERS: Skills that award base stats improvements (flat + % gain instantly + 50% increased stat reward per level).  A fighter can have a maximum of 3 statboosters.
    SUPERS: Special skills that are activated at some point during the fight. Can be activated N amount of times but chances decrease after each use.
    TALENTS: Skills that act as a balance tool and award fighters special stats after their first loss. (Daily -> 3 loses max. 10 games total.)
    */
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

    public Dictionary<int, Dictionary<string, string>> skills =
    new Dictionary<int, Dictionary<string, string>>
    {
        {
            0,
            new Dictionary<string, string>
            {
                {"Title", "6th Sense"},
                {"Description", "Increases counter rate stat by 10%"},
                {"Rarity", Rarity.Common.ToString()},
                {"Category", SkillType.Passives.ToString()},
            }
        },
    };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
