using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCanvas : MonoBehaviour
{
    [Header("Fighter Portraits")]
    public Image portrait1;
    public Image portrait2;

    [Header("Defeat sprite")]
    public Image defeatSprite1;
    public Image defeatSprite2;

    // Start is called before the first frame update
    void Start()
    {
        var fighterPortrait = portrait2.GetComponent<Image>();
        fighterPortrait.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    public void RenderDefeatSprite(FighterStats f1, FighterStats winner)
    {
        if (winner == f1) defeatSprite2.enabled = true;
        else defeatSprite1.enabled = true;
    }

}
