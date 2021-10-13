using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // Fighter data
    public int hp { get; set; }
    public int dmg { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }

    [Header("Fighter")]
    [SerializeField] private Text hpText;
    [SerializeField] private Text dmgText;
    [SerializeField] private Text agilityText;
    [SerializeField] private Text speedText;

    // User data
    public string userName { get; set; }
    public int wins { get; set; }
    public int defeats { get; set; }

    [Header("User")]
    [SerializeField] private Text usernameText;
    [SerializeField] private Text winsText;
    [SerializeField] private Text defeatsText;

    public void ShowData()
    {
        // Fighter
        hpText.text = Convert.ToString(hp);
        dmgText.text = Convert.ToString(dmg);
        agilityText.text = Convert.ToString(agility);
        speedText.text = Convert.ToString(speed);

        // User
        usernameText.text = userName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }

}
