using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterCombat : MonoBehaviour
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

    [Header("Data")]
    public SMCore sMCore;

    // Scene renders
    [Header("Scene Renders")]
    public Text hitPointsText;
    public SpriteRenderer spriteRender;

    [Header("Fighter")]
    public Fighter fighter;

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

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = cameraPosition + new Vector3(60f, 150f, 0);
    }

    public string[] GetFighterSkinArray()
    {
        string[] skinList = new string[2];
        skinList[0] = fighter.GetPlayerFighterSkin();
        skinList[1] = fighter.GetCPUFighterSkin();

        return skinList;
    }

    public void ChangeAnimationState(AnimationNames newState)
    {
        fighter.GetAnimator().Play(newState.ToString());
        fighter.currentState = newState.ToString();
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

    public List<string> GetFighterSkills()
    {
        return skills;
    }

    public void SetFighterPositions(Vector2 enemyInitialPosition, float distanceBetweenFightersOnAttack)
    {
        initialPosition = transform.position;
        destinationPosition = enemyInitialPosition + new Vector2(distanceBetweenFightersOnAttack, 0);
    }

    public void SetFighterStatsBasedOnSkills(FighterCombat opponent)
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
