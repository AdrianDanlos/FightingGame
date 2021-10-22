using System.Collections.Generic;

public static class FightersTestData
{

    // Player stats and skills
    public static Dictionary<string, int> playerFighterStats =
    new Dictionary<string, int>
    {
        {"lv", 1},
        {"xp", 0},
        {"hitPoints", 14},
        {"strength", 3},
        {"agility", 50},
        {"speed", 0},
        {"counterRate", 0},
        {"reversalRate", 0},
        {"armor", 0},
        {"criticalRate", 0},
        {"sabotageRate", 0},
    };

    public static List<string> playerFighterSkills = new List<string> {
        SkillsList.SIXTH_SENSE.ToString(),
        SkillsList.HOSTILITY.ToString(),
        SkillsList.TOUGHENED_SKIN.ToString(),
        SkillsList.ARMOR.ToString(),
        SkillsList.CRITICAL_STRIKE.ToString(),
        SkillsList.SABOTAGE.ToString(),
        SkillsList.SURVIVAL.ToString(),
        SkillsList.DETERMINATION.ToString(),
    };

    // CPU values
    // FIXME: These should be calculated/randomized depending on the players level
    public static Dictionary<string, int> cpuFighterStats =
    new Dictionary<string, int>
    {
        {"lv", 1},
        {"xp", 0},
        {"hitPoints", 14},
        {"strength", 3},
        {"agility", 50},
        {"speed", 0},
        {"counterRate", 0},
        {"reversalRate", 0},
        {"armor", 0},
        {"criticalRate", 0},
        {"sabotageRate", 0},
    };

    public static List<string> cpuFighterSkills = new List<string> {
        SkillsList.SIXTH_SENSE.ToString(),
        SkillsList.HOSTILITY.ToString(),
        SkillsList.TOUGHENED_SKIN.ToString(),
        SkillsList.ARMOR.ToString(),
        SkillsList.CRITICAL_STRIKE.ToString(),
        SkillsList.SABOTAGE.ToString(),
        SkillsList.SURVIVAL.ToString(),
        SkillsList.DETERMINATION.ToString(),
    };
}
