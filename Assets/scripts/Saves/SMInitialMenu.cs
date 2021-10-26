using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SMInitialMenu : MonoBehaviour
{
    [Header("Data")]
    private string savePath;
    public Skills skills;

    [Header("Skins")]
    public Skins skins;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave(string fighterName)
    {
        Dictionary<string, int> initialStats = GenerateAllInitialStats();
        string initialSkill;
        //FIXME: We dont really need a loop here
        do
        {
            initialSkill = skills.GetAllSkills()[Random.Range(1, skills.GetAllSkills().Count + 1) - 1];
        } while
            (skills.CheckIfSkillIsAStatIncreaser(initialSkill));

        // object initializer to instantiate the save
        var save = new Save()
        {
            // Default stats of new save
            // Level
            savedLv = initialStats["lv"],
            savedXp = initialStats["xp"],

            // Fighter 
            savedHp = initialStats["hitPoints"],
            savedStrength = initialStats["strength"],
            savedAgility = initialStats["agility"],
            savedSpeed = initialStats["speed"],
            savedCounterRate = initialStats["counterRate"],
            savedReversalRate = initialStats["reversalRate"],
            savedArmor = initialStats["armor"],
            savedCriticalRate = initialStats["criticalRate"],
            savedSabotageRate = initialStats["sabotageRate"],
            savedSkills = new List<string>() { initialSkill },

            // User
            savedFighterName = fighterName,
            savedWins = 0,
            savedDefeats = 0,

            // Skin
            savedSkin = skins.GetSkinSelected()
        };
        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    private Dictionary<string, int> GenerateAllInitialStats()
    {
        int lv, xp, baseHp, strength, agility, speed, counterRate, reversalRate, armor, criticalRate, sabotageRate;

        lv = 1;
        xp = 0;
        baseHp = 10; // 50 is ideal to make fights long
        int[] baseStats = generateBaseStats();
        strength = baseStats[0];
        agility = baseStats[1];
        speed = baseStats[2];

        counterRate = 4;
        reversalRate = 4;
        criticalRate = 4;
        sabotageRate = 0;
        armor = 0;


        Dictionary<string, int> initialStats =
        new Dictionary<string, int>
        {
            {"lv", lv},
            {"xp", xp},
            {"hitPoints", baseHp},
            {"strength", strength},
            {"agility", agility},
            {"speed", speed},
            {"counterRate", counterRate},
            {"reversalRate", reversalRate},
            {"criticalRate", criticalRate},
            {"sabotageRate", sabotageRate},
            {"armor", armor}
        };

        return initialStats;
    }

    private int[] generateBaseStats()
    {
        int strength, agility, speed;
        int minValue = 8;
        int maxValue = 12;
        strength = Random.Range(minValue, maxValue);

        if (strength == minValue)
        {
            agility = Random.Range(minValue, maxValue);
            if (agility == minValue) speed = maxValue;
            else speed = Random.Range(minValue, maxValue);
        }
        else
        {
            agility = Random.Range(minValue, maxValue);
            if (agility == maxValue) speed = minValue;
            else speed = Random.Range(minValue, maxValue);
        }

        return new int[]
        {
            strength, agility, speed
        };
    }
}
