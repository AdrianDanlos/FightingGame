using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointsDisplay : MonoBehaviour
{
    string hitPoints;
    public FighterStats fighter;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update() 
    {
        hitPoints = fighter.GetHitPoints().ToString();
        Debug.Log(hitPoints);
        // Debug.Log("1");
        // Debug.Log(fighter.GetHitPoints().ToString());
    }
}
