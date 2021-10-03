using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{

    public int hitPoints;
    public int baseDmg;
    public Text hitPointsText;

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

    void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = position + new Vector3(60f, 150f, 0);
    }
}
