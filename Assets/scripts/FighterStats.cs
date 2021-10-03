using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterStats : MonoBehaviour
{
    private int hitPoints;
    private int baseDmg;
    private int currentWeapon;
    private int[] weaponsList;
    public Text hitPointsText;

    public int GetHitPoints()
    {
        return hitPoints;
    }

    public void SetHitPoints(int hitPoints)
    {
        this.hitPoints = hitPoints;
    }

    public int GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void SetCurrentWeapon(int currentWeapon)
    {
        this.currentWeapon = currentWeapon;
    }

    public int GetBaseDmg()
    {
        return baseDmg;
    }

    public int[] GetWeaponsList()
    {
        return weaponsList;
    }

    public void SetWeaponsList(int[] weaponsList)
    {
        this.weaponsList = weaponsList;
    }



    void Start()
    {
        baseDmg = 1;
    }

    void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(this.transform.position);
        hitPointsText.transform.position = position + new Vector3(60f, 150f, 0);
    }
}
