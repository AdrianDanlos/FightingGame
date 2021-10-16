using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

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

    // levels db + levelUp modify 
    public LevelDB levelDB;

    // UI to disable/enable
    public GameObject levelUpMenu;
    public GameObject fightersUI;
    public Text lvUpOption1;
    public Text lvUpOption2;

    void Awake()
    {
        gameData = GetComponent<GameData>();
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
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
        int lv, xp, baseHp, hp, strength, agility, speed, counterRate, reversalRate, armor, totalAttributes;
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

        counterRate = 1;
        reversalRate = 1;
        armor = 0;
        totalAttributes = strength + agility + speed;
        // Debug.Log("hp: " + hp + " || dmg: " + strength + " || agility: " + agility + " || speed: " + speed + " || total: " + totalAttributes);

        Dictionary<string, int> initialStats =
        new Dictionary<string, int>
        {
            {"lv", lv},
            {"xp", xp},
            {"hitPoints", hp},
            {"strength", strength},
            {"agility", agility},
            {"speed", speed},
            {"counterRate", counterRate},
            {"reversalRate", reversalRate},
            {"armor", armor}
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
            // Level
            savedLv = initialStats["lv"],
            savedXp = initialStats["xp"],

            // Fighter 
            savedHp = initialStats["hitPoints"],
            savedStrength = initialStats["strength"],
            savedAgility = initialStats["agility"],
            savedSpeed = initialStats["speed"],
            savedCounterRate = initialStats["counterRate"],
            savedReversalRate = initialStats["reversalRate"],
            savedArmor = initialStats["armor"],

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
            // Level
            savedLv = gameData.lv,
            savedXp = gameData.xp,

            // Fighter
            savedHp = gameData.hp,
            savedStrength = gameData.strength,
            savedAgility = gameData.agility,
            savedSpeed = gameData.speed,
            savedCounterRate = gameData.counterRate,
            savedReversalRate= gameData.reversalRate,
            savedArmor = gameData.armor,

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
            gameData.armor = save.savedArmor;

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
                {"lv", save.savedLv},
                {"xp", save.savedXp},
                {"hitPoints", save.savedHp},
                {"strength", save.savedStrength},
                {"agility", save.savedAgility},
                {"speed", save.savedSpeed},
                {"counterRate", save.savedCounterRate},
                {"reversalRate", save.savedReversalRate},
                {"armor", save.savedArmor}
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
        // wr, lv and xp update
        int winCount = 0;
        int defeatCount = 0;
        int xpGained;
        int lv = gameData.lv;
        int targetXp = levelDB.GetTargetXpBasedOnLv(lv);

        if(win)
        {
            winCount = 1;
            xpGained = 2;
        } 
        else
        {
            defeatCount = 1;
            xpGained = 1;
        }

        int newXp = gameData.xp;

        if(lv < levelDB.GetLvCap())
        {
            newXp = gameData.xp + xpGained;

            if (newXp >= targetXp)
            {
                lv++;
                levelUpMenu.SetActive(true);
                // FIXME -- animator needs to be paused (warnings on console)
                fightersUI.SetActive(false);

                // display options and return which option user clicks
                System.Random randomN = new System.Random();
                HashSet<int> numbers = new HashSet<int>();
                List<int> abilities = new List<int>();
                while (numbers.Count < 2)
                {
                    numbers.Add(randomN.Next(1, 4));
                }

                if(numbers.Contains(1))
                {
                    abilities.Add(1);
                }
                if (numbers.Contains(2))
                {
                    abilities.Add(2);
                }
                if (numbers.Contains(3))
                {
                    abilities.Add(3);
                }
                if (numbers.Contains(4))
                {
                    abilities.Add(4);
                }

                // DATA
                switch (Random.Range(1, 5))
                {
                    case 1:
                        gameData.hp += 18;
                        break;
                    case 2:
                        gameData.strength += 3;
                        break;
                    case 3:
                        gameData.agility += 3;
                        break;
                    case 4:
                        gameData.speed += 3;
                        break;
                }
                // OPTION 1
                switch (abilities[0])
                {
                    case 1:
                        lvUpOption1.text = "HP INCREASE";
                        break;
                    case 2:
                        lvUpOption1.text = "STR INCREASE";
                        break;
                    case 3:
                        lvUpOption1.text = "AGI INCREASE";
                        break;
                    case 4:
                        lvUpOption1.text = "SPD INCREASE";
                        break;
                }
                // OPTION 2
                switch (abilities[1])
                {
                    case 1:
                        lvUpOption2.text = "HP INCREASE";
                        break;
                    case 2:
                        lvUpOption2.text = "STR INCREASE";
                        break;
                    case 3:
                        lvUpOption2.text = "AGI INCREASE";
                        break;
                    case 4:
                        lvUpOption2.text = "SPD INCREASE";
                        break;
                }
            }
        }
        
        // FIXME -- call saveData() instead of this code
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Level
            savedLv = lv,
            savedXp = newXp,

            // Fighter
            savedHp = gameData.hp,
            savedStrength = gameData.strength,
            savedAgility = gameData.agility,
            savedSpeed = gameData.speed,
            savedCounterRate = gameData.counterRate,
            savedReversalRate = gameData.reversalRate,
            savedArmor= gameData.armor,

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


