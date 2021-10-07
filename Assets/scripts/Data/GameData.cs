using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // Fighter data
    public int hp { get; set; }
    public int dmg { get; set; }

    [SerializeField] private Text hpText;
    [SerializeField] private Text dmgText;

    // User data
    public string userName { get; set; }
    public int wins { get; set; }
    public int defeats { get; set; }

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

        // User
        usernameText.text = userName;
        winsText.text = Convert.ToString(wins);
        defeatsText.text = Convert.ToString(defeats);
    }
}
