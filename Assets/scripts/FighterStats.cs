using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    public int hitPoints { get; set; }
    public int damage { get; set; }

    // The higher the number, the higher the canches of dodging an attack. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int agility { get; set; }

    // The higher the number (0-100), the higher the canches of attacking multiple times. FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int speed { get; set; }

    //Hidden stats (Stats given by skills, these don't increase by level). FIXME: Set from 0 - 100 on the % randomizer for attacks but should create a cap. 
    public int counterRate { get; set; }

    public string[] skills;

    // Scene renders
    public Text hitPointsText;
    public Animator animator;
    public SpriteRenderer spriteRender;

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = position + new Vector3(60f, 150f, 0);
    }
}
