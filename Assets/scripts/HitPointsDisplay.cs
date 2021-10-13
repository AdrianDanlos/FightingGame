using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPointsDisplay : MonoBehaviour
{
    public FighterStats fighter;

    void Update()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        fighter.hitPointsText.text = fighter.hitPoints.ToString();
    }
}
