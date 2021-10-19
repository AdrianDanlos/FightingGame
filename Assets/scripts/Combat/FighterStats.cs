using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    public int lv { get; set; }
    public int xp { get; set; }
    public string fighterName { get; set; }
    public int hitPoints { get; set; }
    public int strength { get; set; }

    // The higher the number, the higher the canches of dodging an attack. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int agility { get; set; }

    // The higher the number (0-100), the higher the canches of attacking multiple times. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int speed { get; set; }

    //Hidden stats (Stats given by skills, these don't increase by level). 
    public int counterRate { get; set; }
    public int reversalRate { get; set; }
    public int armor { get; set; }
    public int criticalRate { get; set; }
    public int sabotageRate { get; set; }
    public List<string> skills { get; set; }

    // Scene renders
    [Header("Scene Renders")]
    public Text hitPointsText;
    public SpriteRenderer spriteRender;
    public GameObject shadowCircle;

    // Animation management
    [Header("Animation")]
    public string currentState;
    [SerializeField] private Animator animator;

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
        animator = GetComponent<Animator>();
        animator.Play(AnimationNames.IDLE.ToString());
    }

    public void ChangeAnimationState(AnimationNames newState)
    {
        //if (currentState == newState) return;
        //should we replay the animation or stop the recast?

        animator.Play(newState.ToString());

        currentState = newState.ToString();
    }

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        // need to add a Vector3 to correct the text above the fighter
        hitPointsText.transform.position = cameraPosition + new Vector3(60f, 150f, 0);
        // need to add a Vector3 to correct the shadow below the fighter
        shadowCircle.transform.position = cameraPosition + new Vector3(0, -135f, 0);
    }
}
