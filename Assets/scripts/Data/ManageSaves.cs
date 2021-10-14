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
        if(SScene.scene == (int)SceneIndex.INITIAL_MENU)
        {
            LoadMenuDataFromSave();
        }
    }

    public string GetSavePath()
    {
        return savePath;
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave(string userName)
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
            savedUserName = userName,
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
            savedUserName = gameData.userName,
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

    public void LoadMenuDataFromSave()
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
            gameData.userName = save.savedUserName;
            gameData.wins = save.savedWins;
            gameData.defeats = save.savedDefeats;

            gameData.ShowData();
            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log("No save file");
        }
    }

    public int[] GetWinrate()
    {
        // get values 
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
        gameData.userName = save.savedUserName;
        // 0 - wins || 1 - defeats
        int[] winrate = new int[2];
        winrate[0] = save.savedWins;
        winrate[1] = save.savedDefeats;

        return winrate;
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

    public void UpdateDataFromCombat(int winsCounter, int defeatsCounter)
    {
        int[] winrate = GetWinrate();

        winsCounter += winrate[0];
        defeatsCounter += winrate[1];
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = gameData.hp,
            savedDmg = gameData.dmg,
            savedAgility = gameData.agility,
            savedSpeed = gameData.speed,

            // User
            savedUserName = gameData.userName,
            savedWins = winsCounter,
            savedDefeats = defeatsCounter
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
