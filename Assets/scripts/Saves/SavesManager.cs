using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameData))]
public class SavesManager : MonoBehaviour
{
    // gameData loses its values each time scene is loaded
    // loadTempData needs to be called in order to save data
    [Header("Data")]
    public string savePath;
    public Skills skills;

    // levels db + levelUp modify 
    public LevelDB levelDB;
    private GameData gameData;

    // need to know in which scene we are
    [Header("Scene")]
    public GameScene gameScene;
    public InitialMenuScene initialMenuScene;

    // UI to disable/enable
    [Header("UI")]
    public UIMainMenu uIMainMenu;
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] public Sprite[] iconsArray;
    [SerializeField] private GameObject fighterUI1;
    [SerializeField] private GameObject fighterUI2;
    [SerializeField] private GameObject backToMenu;

    [Header("LevelUp UI Option 1")]
    [SerializeField] private Button lvUpOption1Button;
    [SerializeField] private Text lvUp1Title;
    [SerializeField] private Text lvUp1Description;
    [SerializeField] private Image lvUp1Image;

    [Header("LevelUp UI Option 2")]
    [SerializeField] private Button lvUpOption2Button;
    [SerializeField] private Text lvUp2Title;
    [SerializeField] private Text lvUp2Description;
    [SerializeField] private Image lvUp2Image;


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


    public void SaveData
        (int lv, int xp, int hp, int strength, int agility, int speed, int counterRate,
        int reversalRate, int criticalRate, int sabotageRate, int armor, List<string> skills, string fighterName, int wins,
        int defeats)
    {
        // object initializer to instantiate the save
        var save = new Save()
        {
            // Level
            savedLv = lv,
            savedXp = xp,

            // Fighter
            savedHp = hp,
            savedStrength = strength,
            savedAgility = agility,
            savedSpeed = speed,
            savedCounterRate = counterRate,
            savedReversalRate = reversalRate,
            savedCriticalRate = criticalRate,
            savedSabotageRate = sabotageRate,
            savedArmor = armor,
            savedSkills = skills,

            // User
            savedFighterName = fighterName,
            savedWins = wins,
            savedDefeats = defeats
        };

        // using closes the stream automatically
        var binaryFormatter = new BinaryFormatter();
        using (var fileStream = File.Create(savePath))
        {
            binaryFormatter.Serialize(fileStream, save);
        }
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

    public Dictionary<string, int> LoadGameDataStats()
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
            {"criticalRate", save.savedCriticalRate},
            {"sabotageRate", save.savedSabotageRate},
            {"armor", save.savedArmor}
        };

        return playerFighterValues;
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

    public string LoadFighterName()
    {
        if (CheckIfFileExists())
        {
            Save save;

            var binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Open(savePath, FileMode.Open))
            {
                save = (Save)binaryFormatter.Deserialize(fileStream);
            }

            return save.savedFighterName;
        }
        return null;
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
                // need to save before entering levelUp logic in order to save the xp and wr related stats 
                // (variables not saved in gameData)
                SaveData(lv, newXp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
                gameData.counterRate, gameData.reversalRate, gameData.criticalRate, gameData.sabotageRate, gameData.armor, gameData.skills,
                gameData.fighterName, gameData.wins + winCount, gameData.defeats + defeatCount);
                LoadTempData(); // refresh tempData in order to save correctly in levelUp menu

                List<string> twoSkills = new List<string>();
                twoSkills.Add(skills.GetSkills(gameData.skills, skills.GetAvailableSkills(skills.GetAllSkills(), gameData.skills)));
                twoSkills.Add(skills.GetSkills(gameData.skills, skills.GetAvailableSkills(skills.GetAllSkills(), gameData.skills).Except(twoSkills).ToList()));

                // button options
                Dictionary<string, string> skillData1 = skills.GetSkillDataFromSkillName(twoSkills[0]);
                Dictionary<string, string> skillData2 = skills.GetSkillDataFromSkillName(twoSkills[1]);

                // set UI choice 1
                lvUp1Title.text = skillData1["Title"];
                lvUp1Title.color = skills.GetColorFromRarity(skillData1["Rarity"]);
                lvUp1Description.text = skillData1["Description"];
                string skillData1Name = "icons_" + skillData1["Icon"];
                for (int i = 0; i < iconsArray.Length; i++)
                {
                    if (string.Equals(skillData1Name, iconsArray[i].name)) lvUp1Image.sprite = iconsArray[i];
                }

                // set UI choice 2
                lvUp2Title.text = skillData2["Title"];
                lvUp2Title.color = skills.GetColorFromRarity(skillData2["Rarity"]);
                lvUp2Description.text = skillData2["Description"];
                string skillData2Name = "icons_" + skillData2["Icon"];
                for (int i = 0; i < iconsArray.Length; i++)
                {
                    if (string.Equals(skillData2Name, iconsArray[i].name)) lvUp2Image.sprite = iconsArray[i];
                }

                lvUpOption1Button.onClick.AddListener(delegate { CheckSkillAndAdd(twoSkills[0]); });
                lvUpOption2Button.onClick.AddListener(delegate { CheckSkillAndAdd(twoSkills[1]); });
            }
        }

        SaveData(lv, newXp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
            gameData.counterRate, gameData.reversalRate, gameData.criticalRate, gameData.sabotageRate, gameData.armor, gameData.skills,
            gameData.fighterName, gameData.wins + winCount, gameData.defeats + defeatCount);
    }

    public void CheckSkillAndAdd(string skill)
    {
        if (skills.CheckIfSkillIsAStatIncreaser(skill))
        {
            IncreaseBasicStat(skill);
        }
        else
        {
            gameData.skills.Add(skill);
        }

        SScene.levelUp = false;
        SaveData(gameData.lv, gameData.xp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
            gameData.counterRate, gameData.reversalRate, gameData.criticalRate, gameData.sabotageRate, gameData.armor, gameData.skills,
            gameData.fighterName, gameData.wins, gameData.defeats);
        gameScene.LoadMainMenu();
    }

    private void IncreaseBasicStat(string skillName)
    {
        switch (skillName)
        {
            case "HP_INCREASE":
                if (gameData.skills.Contains("SUPER_HP"))
                {
                    gameData.hp += 6;
                }
                gameData.hp += 18;
                break;
            case "STRENGTH_INCREASE":
                if (gameData.skills.Contains("SUPER_STRENGTH"))
                {
                    gameData.strength += 1;
                }
                gameData.strength += 3;
                break;
            case "AGILITY_INCREASE":
                if (gameData.skills.Contains("SUPER_AGILITY"))
                {
                    gameData.agility += 1;
                }
                gameData.agility += 3;
                break;
            case "SPEED_INCREASE":
                if (gameData.skills.Contains("SUPER_SPEED"))
                {
                    gameData.speed += 1;
                }
                gameData.speed += 3;
                break;
        }
    }



    public bool CheckIfFileExists()
    {
        return File.Exists(savePath);
    }

}