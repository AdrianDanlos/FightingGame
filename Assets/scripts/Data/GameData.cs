using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
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
