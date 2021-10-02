using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStats : MonoBehaviour
{

    public int hitPoints;
    public int baseDmg;

    public int GetHitPoints()
    {
        return hitPoints;
    }

    public void SetHitPoints(int newHitPoints)
    {
        hitPoints = newHitPoints;
    }

    public int GetBaseDmg()
    {
        return baseDmg;
    }



    void Start()
    {
        baseDmg = 1;
    }
}
