using System;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    public int hp { get; set; }
    public int dmg { get; set; }

    [SerializeField] private Text hpText;
    [SerializeField] private Text dmgText;

    public void GenerateData()
    {
        hp = UnityEngine.Random.Range(1, 10);
        dmg = UnityEngine.Random.Range(1, 10);

        Debug.Log(hp + " " + dmg);
    }

    public void ShowData()
    {
        hpText.text = Convert.ToString(hp);
        dmgText.text = Convert.ToString(dmg);
    }
}
