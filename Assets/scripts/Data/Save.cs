using UnityEngine;

// to convert it to a file and need to delete MonoBehaviour
[System.Serializable]
public class Save 
{
    // Fighter
    public int savedHp;
    public int savedDmg;
    public int savedBaseAgility;
    public int savedBaseSpeed;

    // User
    public string savedUserName;
    public int savedWins;
    public int savedDefeats;
}
