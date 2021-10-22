using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SMGame : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;
    private string savePath;
    public Skills skills;
    public LevelDB levelDB;
    public GameData gameData;

    [Header("Scene")]
    public GameScene gameScene;

    [Header("UI")]
    public UIGame uIGame;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
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
        if (sMCore.CheckIfFileExists())
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
                gameScene.SetLevelUpState(true);
                // need to save before entering levelUp logic in order to save the xp and wr related stats 
                // (variables not saved in gameData)
                sMCore.SaveData(lv, newXp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
                gameData.counterRate, gameData.reversalRate, gameData.criticalRate, gameData.sabotageRate, gameData.armor, gameData.skills,
                gameData.fighterName, gameData.wins + winCount, gameData.defeats + defeatCount);
                sMCore.LoadTempData(); // refresh tempData in order to save correctly in levelUp menu

                List<string> twoSkills = new List<string>();
                twoSkills.Add(skills.GetRandomSkillByRarityChance(gameData.skills, skills.GetAvailableSkills(skills.GetAllSkills(), gameData.skills)));
                twoSkills.Add(skills.GetRandomSkillByRarityChance(gameData.skills, skills.GetAvailableSkills(skills.GetAllSkills(), gameData.skills).Except(twoSkills).ToList()));

                // button options
                Dictionary<string, string> skillData1 = skills.GetSkillDataFromSkillName(twoSkills[0]);
                Dictionary<string, string> skillData2 = skills.GetSkillDataFromSkillName(twoSkills[1]);

                uIGame.SetLevelUpMenuValues(skillData1, skillData2);
                uIGame.AddListenersToLvUpButtons(twoSkills);
            }
        }

        sMCore.SaveData(lv, newXp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
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

        gameScene.SetLevelUpState(false);
        sMCore.SaveData(gameData.lv, gameData.xp, gameData.hp, gameData.strength, gameData.agility, gameData.speed,
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
}
