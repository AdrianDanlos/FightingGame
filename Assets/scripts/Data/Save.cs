using UnityEngine;

// to convert it to a file and need to delete MonoBehaviour
[System.Serializable]
public class Save
{
    // Fighter
    public int savedHp;
    public int savedDmg;
    public int savedAgility;
    public int savedSpeed;

    // User
    public string savedFighterName;
    public int savedWins;
    public int savedDefeats;
}
