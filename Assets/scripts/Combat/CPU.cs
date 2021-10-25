using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{
    [Header("Data")]
    public SMGame sMGame;
    public Skills skills;

    private void Start()
    {
        GenerateCPUSkills();
    }

    public Dictionary<string, int> GenerateCPUStats()
    {
        Dictionary<string, int> playerFighterStats = sMGame.LoadGameDataStats();

        int cpuMinRange = 4; // inclusive
        int cpuMaxRange = 2; // exclusive
        int healthModifier = 1;
        Dictionary<string, int> cpuFighterValues =
        new Dictionary<string, int>
        {
            {"lv", 0},
            {"xp", 0},
            {"hitPoints", Random.Range(playerFighterStats["hitPoints"] - (cpuMinRange * healthModifier), playerFighterStats["hitPoints"] + (cpuMaxRange * healthModifier))},
            {"strength", Random.Range(playerFighterStats["strength"] - cpuMinRange, playerFighterStats["strength"] + cpuMaxRange)},
            {"agility", Random.Range(playerFighterStats["agility"] - cpuMinRange, playerFighterStats["agility"] + cpuMaxRange)},
            {"speed", Random.Range(playerFighterStats["speed"] - cpuMinRange, playerFighterStats["speed"] + cpuMaxRange)},
             // FIXME -- need to know how much each stat impacts fight
            {"counterRate", Random.Range(playerFighterStats["counterRate"] - cpuMinRange, playerFighterStats["counterRate"] + cpuMaxRange)},
            {"reversalRate", Random.Range(playerFighterStats["reversalRate"] - cpuMinRange, playerFighterStats["reversalRate"] + cpuMaxRange)},
            {"criticalRate", Random.Range(playerFighterStats["criticalRate"] - cpuMinRange, playerFighterStats["criticalRate"] + cpuMaxRange)},
            {"sabotageRate", Random.Range(playerFighterStats["sabotageRate"] - cpuMinRange, playerFighterStats["sabotageRate"] + cpuMaxRange)},
            {"armor", Random.Range(playerFighterStats["armor"], playerFighterStats["armor"])}
        };

        return cpuFighterValues;
    }

    public List<string> GenerateCPUSkills()
    {
        List<string> cpuFighterSkills = new List<string>();
        List<string> playerFighterSkills = sMGame.LoadGameDataSkills();
        int playerSkillsCount = playerFighterSkills.Count;

        for(int i = 0; i <  playerFighterSkills.Count; i++)
        {
            cpuFighterSkills.Add(skills.GetRandomSkill(cpuFighterSkills));
        }

        return cpuFighterSkills;
    }

    public string SetCPUSkin()
    {
        Dictionary<string, int> playerFighterStats = sMGame.LoadGameDataStats();
        
        if(playerFighterStats["lv"] <= 5)
        {
            return SkinsList.Fallen_Angel.ToString();
        }
        else if (playerFighterStats["lv"] >= 6 && playerFighterStats["lv"] <= 10)
        {
            return SkinsList.Reaper.ToString();
        }
        else if(playerFighterStats["lv"] >= 11)
        {
            return SkinsList.Golem.ToString();
        }

        return "";
    }
}
    

