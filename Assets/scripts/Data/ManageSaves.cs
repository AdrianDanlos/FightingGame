using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class ManageSaves : MonoBehaviour
{
    private GameData gameData;
    // private GameData combatData;
    private string savePath;
    

    // Start is called before the first frame update
    void Start()
    {
        gameData = GetComponent<GameData>();
        // combatData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.save"; // doesn't matter the extension                
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave()
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = 20,
            savedDmg = 2,
            savedBaseAgility = 10,
            savedBaseSpeed = 10,

            // User
            savedUserName = "",
            savedWins = 0,
            savedDefeats = 0
        };
        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }

        Debug.Log("Base file created and saved");
    }
    

    public void SaveData()
    {
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

            // deberia showear dentro de load??
            // gameData.ShowData();

            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log("No save file");
        }
    }

    public void LoadGameData()
    {
        if (CheckIfFileExists())
        {

        } else
        {
            Debug.Log("No save file");
        }
    }

    public void DeleteSave()
    {
        if (CheckIfFileExists())
        {
            File.Delete(savePath);
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
