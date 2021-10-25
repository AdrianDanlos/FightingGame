using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{
    // Level
    public int lv { get; set; }
    public int xp { get; set; }

    // Fighter data
    public int hp { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }
    public int counterRate { get; set; }
    public int reversalRate { get; set; }
    public int armor { get; set; }
    public int criticalRate { get; set; }
    public int sabotageRate { get; set; }
    public List<string> skills { get; set; }

    // User data
    public string fighterName { get; set; }
    public int wins { get; set; }
    public int defeats { get; set; }

    // Skin
    public string skin { get; set; }
}
