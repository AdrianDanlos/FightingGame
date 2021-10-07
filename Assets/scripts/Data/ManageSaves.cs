using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class ManageSaves : MonoBehaviour
{
    private GameData gameData;
    private string savePath;
    

    // Start is called before the first frame update
    void Start()
    {
        gameData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.save"; // doesn't matter the extension                
    }

    public void SaveData()
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Fighter
            savedHp = gameData.dmg,
            savedDmg = gameData.hp,

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

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using(var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            // Fighter
            gameData.hp = save.savedHp;
            gameData.dmg = save.savedDmg;

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
}
