using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    public int hitPoints { get; set; }
    public int baseDmg { get; set; }

    // The higher the number, the higher the canches of dodging an attack. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int baseAgility { get; set; }

    // The higher the number (0-100), the higher the canches of attacking multiple times. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int baseSpeed { get; set; }

    //Hidden stats (Stats given by skills, these don't increase by level). FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int counterRate { get; set; }

    public string[] skills;

    // Scene renders
    public Text hitPointsText;
    public Animator animator;
    public SpriteRenderer spriteRender;

    private void Awake()
    {

    }
    void Start()
    {

    }

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = position + new Vector3(60f, 150f, 0);
    }

    public void StartRunAnimation()
    {
        animator.SetBool("Run", true);
    }
    public void EndRunAnimation()
    {
        animator.SetBool("Run", false);
    }
    public void StartAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }
    public void StartDodgeAnimation()
    {
        animator.SetTrigger("Dodge");
    }
    public void StartHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }
    public void StartDeathAnimation()
    {
        animator.SetBool("Death", true);
    }
    public void StartIdleBlinkAnimation()
    {
        animator.SetBool("IdleBlink", true);
    }
}
