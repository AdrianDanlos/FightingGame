using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SMMainMenu : MonoBehaviour
{
    [Header("Data")]
    public GameData gameData;
    private string savePath;

    [Header("UI")]
    public UIMainMenu uIMainMenu;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name

        if (SScene.scene == (int)SceneIndex.INITIAL_MENU || SScene.scene == (int)SceneIndex.GAME)
        {
            uIMainMenu.ShowData(gameData.xp, gameData.lv, gameData.hp, gameData.strength, gameData.agility,
                gameData.speed, gameData.skills, gameData.fighterName, gameData.wins, gameData.defeats);
        }
    }

    public Dictionary<string, int> LoadGameDataStats()
    {
        Save save;
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Open(savePath, FileMode.Open))
        {
            save = (Save)binaryFormatter.Deserialize(fileStream);
        }

        return new Dictionary<string, int>
        {
            {"lv", save.savedLv},
            {"wins", save.savedWins }
        };
    }

    public List<string> LoadGameDataSkills()
    {
        Save save;
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Open(savePath, FileMode.Open))
        {
            save = (Save)binaryFormatter.Deserialize(fileStream);
        }
        return save.savedSkills;
    }

}
