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
    [Header("Data")]
    public string savePath;
    // levels db + levelUp modify 
    public LevelDB levelDB;
    private GameData gameData;

    // need to know in which scene we are
    [Header("Scene")]
    public GameScene gameScene;
    public InitialMenuScene initialMenuScene;

    // UI to disable/enable
    [Header("UI")]
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] private GameObject fightersUI;
    [SerializeField] private GameObject lvUpOption1Button;
    [SerializeField] private GameObject lvUpOption2Button;
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

    public Dictionary<string, int> GenerateAllInitialStats()
    {
        // need to give 1 ability, weapon or stat boost (+3 to an ability or +2/+1)
        int lv, xp, baseHp, hp, strength, agility, speed, counterRate, reversalRate, armor;
        
        lv = 1;
        xp = 0;
        baseHp = 5; // 50
        hp = (int)((lv - 1) * 1.5 + baseHp);
        int[] baseStats = generateBaseStats();
        strength = baseStats[0];
        agility = baseStats[1];
        speed = baseStats[2];

        counterRate = 1;
        reversalRate = 1;
        armor = 0;
        

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

    public int[] generateBaseStats()
    {
        int strength, agility, speed;

        strength = Random.Range(2, 4);

        if (strength == 2)
        {
            agility = Random.Range(2, 4);
            if (agility == 2)
            {
                speed = 3;
            }
            else
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
            }
            else
            {
                speed = Random.Range(2, 4);
            }
        }

        return new int[]
        {
            strength, agility, speed
        };
    }

    // creates a save with base stats fighter 
    public void CreateDefaultSave(string fighterName)
    {
        Dictionary<string, int> initialStats = GenerateAllInitialStats();

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
            savedSkills = new List<string>()
            {Skills.SkillsList.SIXTHSENSE.ToString(),
            Skills.SkillsList.TOUGHENED_SKIN.ToString()
            },

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
            savedSkills = gameData.skills,

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
            gameData.skills = save.savedSkills;

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

    public Dictionary<string, int> LoadGameDataStats()
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

    public List<string> LoadGameDataSkills()
    {
        if (CheckIfFileExists())
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            return save.savedSkills;
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

        if (win)
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

        if (lv < levelDB.GetLvCap())
        {
            newXp = gameData.xp + xpGained;

            if (newXp >= targetXp)
            {
                lv++;
                SScene.levelUp = true;
                // FIXME -- animator needs to be paused (warnings on console)
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
            savedArmor = gameData.armor,
            savedSkills = gameData.skills,

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

    /* TESTING
    private void GenerateLevelUpOptions()
    {
        // options
        int option1, option2;
        // generate 2 random options
        System.Random randomN = new System.Random();
        HashSet<int> numbers = new HashSet<int>();
        List<int> abilities = new List<int>();
        while (numbers.Count < 2)
        {
            numbers.Add(randomN.Next(1, 4));
        }

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers.Contains(i))
            {
                abilities.Add(i);
            }
        }

        // display options and return which option user clicks
        // OPTION 1
        option1 = abilities[0];
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
        option2 = abilities[1];
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

        // IncreaseBasicStat();
    }

    private void IncreaseBasicStat(int abilityId)
    {
        // DATA
        switch (abilityId)
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
    } */

    public void ShowLevelUpMenu()
    {
        levelUpMenu.SetActive(true);
        fightersUI.SetActive(false);
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


