using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    public int GiveRandomSkill()
    {
        // only stat increasers
        int skillToGive = Random.Range(1, 5);

        return skillToGive;
    }
}
