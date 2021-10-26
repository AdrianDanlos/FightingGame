using System;
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

        int CPUBaseStat = UnityEngine.Random.Range(1, 3);
        int baseHealth = UnityEngine.Random.Range(5, 10);
        int playerLevel = playerFighterStats["lv"];
        Dictionary<string, int> cpuFighterValues =
        new Dictionary<string, int>
        {
            {"lv", 0},
            {"xp", 0},
            {"hitPoints", (playerLevel * baseHealth) / 2 },
            {"strength", (playerLevel * CPUBaseStat) / 2  },
            {"agility", (playerLevel * CPUBaseStat) / 2  },
            {"speed", (playerLevel * CPUBaseStat ) / 2 },
             // FIXME -- need to know how much each stat impacts fight
            {"counterRate", Convert.ToInt32(playerLevel * 0.2)},
            {"reversalRate",Convert.ToInt32(playerLevel * 0.2)},
            {"criticalRate", Convert.ToInt32(playerLevel * 0.2)},
            {"sabotageRate", Convert.ToInt32(playerLevel * 0.2)},
            {"armor", Convert.ToInt32(playerLevel * 0.1)}
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
        
        if(playerFighterStats["lv"] <= 10)
        {
            return SkinsList.Fallen_Angel.ToString();
        }
        else if (playerFighterStats["lv"] >= 11 && playerFighterStats["lv"] <= 20)
        {
            return SkinsList.Reaper.ToString();
        }
        else if(playerFighterStats["lv"] >= 21 && playerFighterStats["lv"] <= 30)
        {
            return SkinsList.Golem.ToString();
        }

        return SkinsList.Reaper.ToString();
    }
}
    

