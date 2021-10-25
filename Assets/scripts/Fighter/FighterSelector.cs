using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSelector : MonoBehaviour
{

    [Header("Skins")]
    public Fighter fighter;
    public Skins skins;

    // Update is called once per frame
    void Update()
    {
        string chosenSkin = skins.GetSkinSelected();
        //Load player skin animations. Reads all folders from /Resources
        fighter.selectedSkinAnimations = Resources.LoadAll<AnimationClip>("Animations/" + chosenSkin);

        if (fighter.selectedSkinAnimations.Length > 0) fighter.SetTheAnimationsOfChosenSkin();
    }
}
