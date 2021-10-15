using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // Level
    public int lv { get; set; }
    public int xp { get; set; }

    [Header("Level")]
    [SerializeField] private Text lvText;
    [SerializeField] private Text xpText;

    // Fighter data
    public int hp { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }

    [Header("Fighter")]
    [SerializeField] private Text hpText;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text agilityText;
    [SerializeField] private Text speedText;

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
        xpText.text = Convert.ToString(xp);

        // Fighter
        hpText.text = Convert.ToString(hp);
        strengthText.text = Convert.ToString(strength);
        agilityText.text = Convert.ToString(agility);
        speedText.text = Convert.ToString(speed);

        // User
        fighterNameText.text = fighterName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }

}
