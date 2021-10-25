using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
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
    public bool hasAttackedThisCombat { get; set; } = false;
    public List<string> skills { get; set; }

    // Skin
    public string skin { get; set; }

    [Header("Data")]
    public SMCore sMCore;

    // Scene renders
    [Header("Scene Renders")]
    public Text hitPointsText;
    public SpriteRenderer spriteRender;

    // Animation management
    [Header("Animation")]
    public string currentState;
    [SerializeField] private Animator animator;
    private AnimationClip[] selectedSkinAnimations;

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
        if (!sMCore.GetSkinData().Equals("error"))
            chosenSkin = sMCore.GetSkinData();
        else
            chosenSkin = "Reaper"; // default skin when there is no save yet to show in initialMenu

        //Load player skin animations. Reads all folders from /Resources
        selectedSkinAnimations = Resources.LoadAll<AnimationClip>("Animations/" + chosenSkin);

        if (selectedSkinAnimations.Length > 0) SetTheAnimationsOfChosenSkin();

        animator = GetComponent<Animator>();
        animator.Play(AnimationNames.IDLE.ToString());
    }

    void SetTheAnimationsOfChosenSkin()
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

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        // Commenting this to avoid errors on initialMenu and mainMenu
        //Vector3 cameraPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        //hitPointsText.transform.position = cameraPosition + new Vector3(60f, 150f, 0);
    }

    public void ChangeAnimationState(AnimationNames newState)
    {
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

    public void SetFighterPositions(Vector2 enemyInitialPosition, float distanceBetweenFightersOnAttack)
    {
        initialPosition = transform.position;
        destinationPosition = enemyInitialPosition + new Vector2(distanceBetweenFightersOnAttack, 0);
    }

    public void SetFighterStatsBasedOnSkills(Fighter opponent)
    {
        if (hasSkill(SkillsList.SIXTH_SENSE)) counterRate += 10;
        if (hasSkill(SkillsList.HOSTILITY)) reversalRate += 30;
        if (hasSkill(SkillsList.TOUGHENED_SKIN)) armor += 2;
        if (hasSkill(SkillsList.CRITICAL_STRIKE)) criticalRate += 15;
        if (hasSkill(SkillsList.SABOTAGE)) sabotageRate += 15;

        if (hasSkill(SkillsList.ARMOR))
        {
            armor += 5; speed -= speed / 10;
        }
        //FIXME ADRI: If we apply percentages on the dmg of the opponent fighter later, the calculation will be affected (slightly)
        //Apply armor effects
        if (armor > 0)
        {
            int reducedDamageBasedOnArmor = opponent.strength - armor;
            opponent.strength = reducedDamageBasedOnArmor >= 1 ? reducedDamageBasedOnArmor : 1;
        }

    }

    public bool hasSkill(SkillsList skill)
    {
        return skills.Contains(skill.ToString());
    }
}
