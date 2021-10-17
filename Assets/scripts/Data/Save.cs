using System.Collections.Generic;
using UnityEngine;

// to convert it to a file and need to delete MonoBehaviour
[System.Serializable]
public class Save
{
    // Level
    public int savedLv;
    public int savedXp;

    // Fighter
    public int savedHp;
    public int savedStrength;
    public int savedAgility;
    public int savedSpeed;
    // Hidden stats
    public int savedCounterRate;
    public int savedReversalRate;
    public int savedArmor;

    // Abilities
    public List<string> savedSkills;

    // User
    public string savedFighterName;
    public int savedWins;
    public int savedDefeats;
}
