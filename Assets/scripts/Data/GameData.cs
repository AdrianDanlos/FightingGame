using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // LevelDB 
    [Header("LevelDB")]
    public LevelDB levelDB;

    // Level
    public int lv { get; set; }
    public int xp { get; set; }

    [Header("Level")]
    [SerializeField] private Text lvText;
    [SerializeField] private GameObject experience;
    [SerializeField] private Text xpText;
    [SerializeField] private Text xpTargetText;

    // Fighter data
    public int hp { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }
    public int counterRate { get; set; }
    public int reversalRate { get; set; }
    public int armor { get; set; }
    public List<string> skills { get; set; }

    [Header("Fighter")]
    [SerializeField] private Text hpText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text agilityText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text skillsText;

    // User data
    public string fighterName { get; set; }
    public int wins { get; set; }
    public int defeats { get; set; }

    [Header("User")]
    [SerializeField] private Text fighterNameText;
    [SerializeField] private Text winsText;
    [SerializeField] private Text defeatsText;

    public void ShowData()
    {
        // Level
        lvText.text = Convert.ToString(lv);
        CheckIfCapReached(lv);
        CheckIfFighterIsLevelOne(lv);
        
        // Fighter
        hpText.text = Convert.ToString(hp);
        strengthText.text = Convert.ToString(strength);
        agilityText.text = Convert.ToString(agility);
        speedText.text = Convert.ToString(speed);
        skills.ForEach(delegate (string skill) {
            skillsText.text += "- " + skill + "\n";
        });

        // User
        fighterNameText.text = fighterName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }

    private void CheckIfFighterIsLevelOne(int level)
    {
        if (lv == 1)
        {
            xpText.text = Convert.ToString(xp);
            xpTargetText.text = Convert.ToString(levelDB.GetTargetXpBasedOnLv(lv));
        }
        else
        {
            xpText.text = Convert.ToString(xp - levelDB.GetTargetXpBasedOnLv(lv - 1));
            xpTargetText.text = Convert.ToString(levelDB.GetTargetXpBasedOnLv(lv) - levelDB.GetTargetXpBasedOnLv(lv - 1));
        }
    }

    private void CheckIfCapReached(int level)
    {
        if(level == levelDB.GetLvCap())
        {
            lvText.text = lv + " [MAX LV]";
            experience.SetActive(false);
        }
    }

    public void ConsoleStats()
    {
        Debug.Log(hp + "-" + strength + "-" + agility + "-" + speed);
    }
}
