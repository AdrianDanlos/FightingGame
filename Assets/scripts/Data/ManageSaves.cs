using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class ManageSaves : MonoBehaviour
{
    // gameData loses its values each time scene is loaded
    // loadTempData needs to be invoked in order to save data
    private GameData gameData;
    public string savePath;

    // need to know in which scene we are
    public GameScene gameScene;
    public InitialMenuScene initialMenuScene;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
        Debug.Log(SScene.newGame + " "+ SScene.scene);
        LoadTempData();
        if (SScene.scene == (int)SceneIndex.INITIAL_MENU || SScene.scene == (int)SceneIndex.GAME)
        { 
            gameData.ShowData();
        }
    }

    public string GetSavePath()
    {
        return savePath;
    }

    public Dictionary<string, int> GenerateInitialValues()
    {
        // need to give 1 ability, weapon or stat boost (+3 to an ability or +2/+1)
        int lv, xp, baseHp, hp, strength, agility, speed, totalAttributes;
        lv = 1;
        xp = 0;
        baseHp = 5; // 50
        hp = (int)((lv - 1) * 1.5 + baseHp);
        strength = Random.Range(2, 4);
        if (strength == 2)
        {
            agility = Random.Range(2, 4);
            if (agility == 2)
            {
                speed = 3;
            } else
            {
                speed = Random.Range(2, 4);
            }
        }
        else 
        {
            agility = Random.Range(2, 4);
            if (agility == 3)
            {
                speed = 2;
            } else
            {
                speed = Random.Range(2, 4);
            }
        }

        totalAttributes = strength + agility + speed;
        // Debug.Log("hp: " + hp + " || dmg: " + strength + " || agility: " + agility + " || speed: " + speed + " || total: " + totalAttributes);

        Dictionary<string, int> initialStats =
        new Dictionary<string, int>
        {
            {"hitPoints", hp},
            {"strength", strength},
            {"agility", agility},
            {"speed", speed},
            /*{"counterRate", 1},
            {"reversalRate", 1},
            {"armor", 0},*/
        };

        return initialStats;
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave(string fighterName)
    {
        Dictionary<string, int> initialStats = GenerateInitialValues();

        // object initializer to instantiate the save
        var save = new Save()
        {
            // Default stats of new save
            // Fighter 
            savedHp = initialStats["hitPoints"],
            savedStrength = initialStats["strength"],
            savedAgility = initialStats["agility"],
            savedSpeed = initialStats["speed"],

            // User
            savedFighterName = fighterName,
            savedWins = 0,
            savedDefeats = 0
        };
        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
    }

    public void SaveData()
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = gameData.hp,
            savedStrength = gameData.strength,
            savedAgility = gameData.agility,
            savedSpeed = gameData.speed,

            // User
            savedFighterName = gameData.fighterName,
            savedWins = gameData.wins,
            savedDefeats = gameData.defeats
        };

        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

        Debug.Log("Saved");
    }

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

            // Fighter
            gameData.hp = save.savedHp;
            gameData.strength = save.savedStrength;
            gameData.agility = save.savedAgility;
            gameData.speed = save.savedSpeed;

            // User
            gameData.fighterName = save.savedFighterName;
            gameData.wins = save.savedWins;
            gameData.defeats = save.savedDefeats;

            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log("No save file");
        }
    }

    public string GetFighterName()
    {
        return gameData.fighterName;
    }

    public Dictionary<string, int> LoadGameData()
    {
        if (CheckIfFileExists())
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            Dictionary<string, int> playerFighterValues =
            new Dictionary<string, int>
            {
                {"hitPoints", save.savedHp},
                {"strength", save.savedStrength},
                {"agility", save.savedAgility},
                {"speed", save.savedSpeed},
            };

            Debug.Log("Loaded");

            return playerFighterValues;
        }
        else
        {
            Debug.Log("No save file");
            return null;
        }
    }

    public void UpdateDataFromCombat(bool win)
    {
        int winCount = 0, defeatCount = 0;
        if(win)
        {
            winCount = 1;
        } else
        {
            defeatCount = 1;
        }

        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = gameData.hp,
            savedStrength = gameData.strength,
            savedAgility = gameData.agility,
            savedSpeed = gameData.speed,

            // User
            savedFighterName = gameData.fighterName,
            savedWins = gameData.wins + winCount,
            savedDefeats = gameData.defeats + defeatCount
        };

        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

    }

    public void EraseSave()
    {
        if (CheckIfFileExists())
        {
            File.Delete(savePath);
            Debug.Log("Save deleted");
        }
        else
        {
            Debug.Log("No save file");
        }
    }

    public bool CheckIfFileExists()
    {
        return File.Exists(savePath);
    }

}
