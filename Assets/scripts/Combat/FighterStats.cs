using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    public string fighterName { get; set; }
    public int hitPoints { get; set; }
    public int strength { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }

    // Fighter position
    public Vector2 initialPosition { get; set; }
    public Vector2 destinationPosition { get; set; }

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

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = cameraPosition + new Vector3(60f, 150f, 0);
        shadowCircle.transform.position = cameraPosition + new Vector3(0, -135f, 0);
    }

    public void ChangeAnimationState(AnimationNames newState)
    {
        //if (currentState == newState) return;
        //should we replay the animation or stop the recast?

        animator.Play(newState.ToString());

        currentState = newState.ToString();
    }

    // Setters
    public void SetFighterStats(Dictionary<string, int> data, string fighterName)
    {
        this.fighterName = fighterName;
        hitPoints = data["hitPoints"];
        strength = data["strength"];
        agility = data["agility"];
        speed = data["speed"];
        counterRate = data["counterRate"];
        reversalRate = data["reversalRate"];
        armor = data["armor"];
        criticalRate = data["criticalRate"];
        sabotageRate = data["sabotageRate"];
    }

    public void SetFighterSkills(List<string> skills)
    {
        this.skills = skills;
    }

    public void SetFighterPositions(Vector2 initialPosition, Vector2 enemyInitialPosition, float distanceBetweenFightersOnAttack)
    {
        this.initialPosition = initialPosition;
        destinationPosition = enemyInitialPosition + new Vector2(distanceBetweenFightersOnAttack, 0);
    }
}
