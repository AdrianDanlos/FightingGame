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

    void Awake()
    {
        gameData = GetComponent<GameData>();
        // combatData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
        // load save data on scene 
        // if else de la escena
        if()
        {
            LoadMenuData();
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
            savedHp = 5,
            savedDmg = 1,
            savedBaseAgility = 0,
            savedBaseSpeed = 0,

            // User
            savedUserName = "FighterMaster86",
            savedWins = 0,
            savedDefeats = 0
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
        Debug.Log(savePath);
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = gameData.dmg,
            savedDmg = gameData.hp,
            savedBaseAgility = gameData.baseAgility,
            savedBaseSpeed = gameData.baseSpeed,

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

    public void LoadMenuData()
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
            gameData.baseAgility = save.savedBaseAgility;
            gameData.baseSpeed = save.savedBaseSpeed;

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
                {"baseDmg", save.savedDmg},
                {"baseAgility", save.savedBaseAgility},
                {"baseSpeed", save.savedBaseSpeed},
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

    public void UpdateDataFromCombat(int winsCounter, int losesCounter)
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
            gameData.baseAgility = save.savedBaseAgility;
            gameData.baseSpeed = save.savedBaseSpeed;

            // User
            gameData.userName = save.savedUserName;
            gameData.wins = save.savedWins + winsCounter;
            gameData.defeats = save.savedDefeats + losesCounter;
        }
        else
        {
            Debug.Log("No save file");
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
