using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointsDisplay : MonoBehaviour
{
    public FighterStats fighter;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    void Update()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        fighter.hitPointsText.text = fighter.hitPoints.ToString();
    }
}
