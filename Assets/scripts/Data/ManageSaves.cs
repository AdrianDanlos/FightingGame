using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class ManageSaves : MonoBehaviour
{
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

    // creates a save with base stats fighter 
    public void CreateDefaultSave(string fighterName)
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Default stats of new save
            // Fighter 
            savedHp = 5,
            savedDmg = 5,
            savedAgility = 5,
            savedSpeed = 5,

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
            savedDmg = gameData.dmg,
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
            gameData.dmg = save.savedDmg;
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
                {"damage", save.savedDmg},
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
            savedDmg = gameData.dmg,
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
