using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Text tipText;

    List<string> tipsDB = new List<string>
    {
        "Super Strength increases all future Strength gains by 30%...",
        "Skills are classified by their rarity: Common, Rare, Epic and Legendary...",
        "Strength increases the damage done by your fighter...",
        "Hp increases the amount of hits your fighter can receive..."
    };

    public void LoadRandomTip()
    { 
        int indexOfTips = Random.Range(0, tipsDB.Count);
        tipText.text = tipsDB[indexOfTips];
    }
}
