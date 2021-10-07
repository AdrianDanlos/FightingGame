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
            savedHp = gameData.dmg,
            savedDmg = gameData.hp
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

            gameData.hp = save.savedHp;
            gameData.dmg = save.savedDmg;
            gameData.ShowData();

            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log("No save file");
        }
    }
}
