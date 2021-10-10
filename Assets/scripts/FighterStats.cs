using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    public int hitPoints { get; set; }
    public int baseDmg { get; set; }
    // The higher the number (0-100), the higher the canches of dodging an attack
    public int baseAgility { get; set; }
    // The higher the number (0-100), the higher the canches of attacking multiple times
    public int baseSpeed { get; set; }
    public int currentWeapon { get; set; }
    public int[] weaponsList { get; set; }
    public Text hitPointsText;
    public Animator animator;
    public SpriteRenderer spriteRender;
    private void Awake()
    {
        // set all animations to false by default
        animator.SetBool("Run", false);
        //animator.SetBool("Attack", false);
        animator.SetBool("Dodge", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Dead", false);
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
        Debug.Log("run a true");
    }
    public void EndRunAnimation()
    {
        animator.SetBool("Run", false);
    }
    public void StartAttackAnimation()
    {
        //Debug.Log("empiezo animacion");
        animator.SetTrigger("Attack");
        Debug.Log("hemos llamado settrigger");
    }
    public void EndAttackAnimation()
    {
        //animator.SetBool("Attack");
    }
}
