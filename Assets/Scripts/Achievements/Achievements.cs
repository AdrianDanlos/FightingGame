using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchievementsList
{
    // icons used: 37, 47, 74, 46, 29
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
    public Dictionary<string, Dictionary<string, string>> achievements =
    new Dictionary<string, Dictionary<string, string>>
    {
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
        {
            "common_skill",
            new Dictionary<string, string>
            {
                {"Title", "Woah! This is worthless!"},
                {"Description", "Unlock a common skill."},
                {"Icon", "37" }
            }
        },
    };
}
