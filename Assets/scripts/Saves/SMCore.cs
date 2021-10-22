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

    // need to know in which scene we are
    [Header("Scene")]
    public GameScene gameScene;
    public InitialMenuScene initialMenuScene;

    [Header("UI")]
    public UIMainMenu uIMainMenu;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
        LoadTempData(); // need to load data on every scene we might save
        if (SScene.scene == (int)SceneIndex.INITIAL_MENU || SScene.scene == (int)SceneIndex.GAME)
        {
            uIMainMenu.ShowData(gameData.xp, gameData.lv, gameData.hp, gameData.strength, gameData.agility,
                gameData.speed, gameData.skills, gameData.fighterName, gameData.wins, gameData.defeats);
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
        }
        else
        {
            return;
        }
    }

    public bool CheckIfFileExists()
    {
        return File.Exists(savePath);
    }
}
