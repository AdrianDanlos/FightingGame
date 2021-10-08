using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // Menu data
    // Fighter data
    public int hp { get; set; }
    public int dmg { get; set; }
    public int baseAgility { get; set; }
    public int baseSpeed { get; set; }

    // TODO weapon list

    [Header("Fighter")]
    [SerializeField] private Text hpText;
    [SerializeField] private Text dmgText;
    [SerializeField] private Text baseAgilityText;
    [SerializeField] private Text baseSpeedText;

    // User data
    public string userName { get; set; }
    public int wins { get; set; }
    public int defeats { get; set; }

    [Header("User")]
    [SerializeField] private Text usernameText;
    [SerializeField] private Text winsText;
    [SerializeField] private Text defeatsText;

    /*
    public void GenerateData()
    {
        hp = UnityEngine.Random.Range(1, 10);
        dmg = UnityEngine.Random.Range(1, 10);

        Debug.Log(hp + " " + dmg);
    } */

    public void ShowData()
    {
        // Fighter
        hpText.text = Convert.ToString(hp);
        dmgText.text = Convert.ToString(dmg);
        baseAgilityText.text = Convert.ToString(baseAgility);
        baseSpeedText.text = Convert.ToString(baseSpeed);

        // User
        usernameText.text = userName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }

}
