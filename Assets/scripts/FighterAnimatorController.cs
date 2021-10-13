using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimatorController : MonoBehaviour
{
    //FIXME: REMEMBER TO LINK WHAT NEEDED ON THE INTERFACE
    public string currentState;
    public Animator animator;

    const string IDLE = "idle";
    const string RUN = "run";
    const string ATTACK = "attack";
    const string HURT = "hurt";
    const string JUMP = "jump";
    const string DEATH = "death";
    const string IDLE_BLINK = "idle_blink";

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play(IDLE);
    }

    void ChangeAnimationState(string newState)
    {
        //if (currentState == newState) return;
        if (currentState == newState)
        {
            //STOP ANIMATION TO THEN RERUN IT WITH animator.Play(newState);
        }

        animator.Play(newState);

        currentState = newState;
    }
}
