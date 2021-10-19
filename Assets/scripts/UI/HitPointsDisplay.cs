using UnityEngine;

public class HitPointsDisplay : MonoBehaviour
{
    [Header("Fighter")]
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
