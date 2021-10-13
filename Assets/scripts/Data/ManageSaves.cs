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
        if(SScene.scene == (int)SceneIndex.INITIAL_MENU && !SScene.newGame)
        {
            LoadMenuDataFromSave();
        }
        else if(SScene.scene == (int)SceneIndex.GAME && SScene.newGame)
        {
            // TODO -- create new game save and load menu data
        }

    }

    public string GetSavePath()
    {
        return savePath;
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave()
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = 8,
            savedDmg = 6,
            savedAgility = 1,
            savedSpeed = 1,

            // User
            savedUserName = "FighterMaster86",
            savedWins = 5,
            savedDefeats = 10
        };
        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

        // FIXME -- create menu before main menu where all save management is done
        Debug.Log("File created with default values");
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

    // FIXME -- method created to debug and test > delete later (maybe use it on pre game menu)
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
        return (File.Exists(savePath)) ? true : false;
    }

}
