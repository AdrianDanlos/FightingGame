using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class SMCore : MonoBehaviour
{
    [Header("Data")]
    private string savePath;
    private GameData gameData;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
        LoadTempData(); // need to load data on every scene we might save
    }

    public void SaveData
        (int lv, int xp, int hp, int strength, int agility, int speed, int counterRate,
        int reversalRate, int criticalRate, int sabotageRate, int armor, List<string> skills, string fighterName, int wins,
        int defeats)
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Level
            savedLv = lv,
            savedXp = xp,

            // Fighter
            savedHp = hp,
            savedStrength = strength,
            savedAgility = agility,
            savedSpeed = speed,
            savedCounterRate = counterRate,
            savedReversalRate = reversalRate,
            savedCriticalRate = criticalRate,
            savedSabotageRate = sabotageRate,
            savedArmor = armor,
            savedSkills = skills,

            // User
            savedFighterName = fighterName,
            savedWins = wins,
            savedDefeats = defeats,

            // Skin 
            savedSkin = gameData.skin
        };

        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    // this method needs to be called on every scene that needs the save files content
    public void LoadTempData()
    {
        if (CheckIfFileExists())
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            // Level
            gameData.lv = save.savedLv;
            gameData.xp = save.savedXp;

            // Fighter
            gameData.hp = save.savedHp;
            gameData.strength = save.savedStrength;
            gameData.agility = save.savedAgility;
            gameData.speed = save.savedSpeed;
            gameData.counterRate = save.savedCounterRate;
            gameData.reversalRate = save.savedReversalRate;
            gameData.criticalRate = save.savedCriticalRate;
            gameData.sabotageRate = save.savedSabotageRate;
            gameData.armor = save.savedArmor;
            gameData.skills = save.savedSkills;

            // User
            gameData.fighterName = save.savedFighterName;
            gameData.wins = save.savedWins;
            gameData.defeats = save.savedDefeats;

            // Skin
            gameData.skin = save.savedSkin;
        }
        else
        {
            return;
        }
    }

    public string GetSkinData()
    {
        if (CheckIfFileExists())
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            // Skin
            return save.savedSkin;
        }
        else
        {
            return "";
        }
    }

    public bool CheckIfFileExists()
    {
        return File.Exists(savePath);
    }
}
