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
    public Sprite newSprite;
    void Start()
    {

    }

    void Update()
    {
        //This can be removed once we don't need the hp number on top of the fighter
        Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
        //hitPointsText.transform.position = position + new Vector3(60f, 150f, 0);
    }
}
