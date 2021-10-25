using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;

    // Animation management
    [Header("Animation")]
    public string currentState;
    [SerializeField] private Animator animator;
    public AnimationClip[] selectedSkinAnimations;

    [Header("Skins")]
    public Skins skins;

    public enum AnimationNames
    {
        IDLE,
        RUN,
        ATTACK,
        HURT,
        JUMP,
        DEATH,
        IDLE_BLINK
    }

    public void Start()
    {
        bool isPlayer = name == GameObject.Find("FighterOne").name;

        string chosenSkin;
        if (!sMCore.GetSkinData().Equals("error") && isPlayer)
            chosenSkin = sMCore.GetSkinData();
        else
            chosenSkin = "Reaper"; // default skin when there is no save yet to show in initialMenu

        //Load player skin animations. Reads all folders from /Resources
        selectedSkinAnimations = Resources.LoadAll<AnimationClip>("Animations/" + chosenSkin);

        if (selectedSkinAnimations.Length > 0) SetTheAnimationsOfChosenSkin();
    }

    public void LoadSkin(string fighterSkin)
    {
        //Load player skin animations. Reads all folders from /Resources
        selectedSkinAnimations = Resources.LoadAll<AnimationClip>("Animations/" + fighterSkin);

        if (selectedSkinAnimations.Length > 0) SetTheAnimationsOfChosenSkin();
    }

    public void SetTheAnimationsOfChosenSkin()
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        int index = 0;

        foreach (var defaultClip in aoc.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(defaultClip, selectedSkinAnimations[index]));
            index++;
        }

        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;
    }

    public void ChangeAnimationState(AnimationNames newState)
    {
        animator.Play(newState.ToString());
        currentState = newState.ToString();
    }

    public Animator GetAnimator()
    {
        return animator;
    }

}
