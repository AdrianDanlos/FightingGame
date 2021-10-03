using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // Globals
    public FighterStats figherModel;
    public Weapons weaponModel;

    public Weapons weapon;
    public Text WinnerBannerText;

    public float f1position;
    public float f2position;

    // Start is called before the first frame update
    void Start()
    {
        // crear 2 pjs
        // should we declare those as globals?
        FighterStats f1, f2;
        f1 = Instantiate(figherModel, new Vector3(-10, 0, 0), Quaternion.Euler(0, 90, 0));
        f2 = Instantiate(figherModel, new Vector3(10, 0, 0), Quaternion.Euler(0, -90, 0));
        f1.hitPoints = 3;
        f2.hitPoints = 7;

        // save initial position of fighers
        f1position = f1.transform.position.x;
        f2position = f2.transform.position.x;

        //In the future we won't need to instantiate the weapon prefab as it will be part of the fighter skin
        //crear weapon + set position
        weapon = Instantiate(weaponModel, f1.transform.position, Quaternion.Euler(0, 90, 0), f1.transform);
        Vector3 figher1Position = f1.transform.position;
        weapon.transform.position = new Vector3(figher1Position.x + 2, figher1Position.y, figher1Position.z);

        //set list of weapons for the fighter
        int[] weaponLists = { 0, 1 };
        f1.weaponsList = weaponLists;

        //set current weapon of the figher 
        f1.currentWeapon = f1.weaponsList[0];
        Debug.Log("current weapon");
        Debug.Log(f1.currentWeapon);

        //Get weapon dmg
        Debug.Log("weapon damage");
        Debug.Log(weapon.weapons[f1.currentWeapon]["damage"]);

        StartCoroutine(attack(f1, f2));
    }

    IEnumerator attack(FighterStats f1, FighterStats f2)
    {        
        while (f1.hitPoints > 0 || f2.hitPoints > 0)
        {
            Debug.Log(f1.transform.position.x);
            // F1 INITIATES MOVING FORWARD TO ATTACK
            while (f1.transform.position.x <= f2.transform.position.x - 2)
            {
                f1.transform.position += f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            f2.hitPoints = f2.hitPoints - int.Parse(weapon.weapons[f1.currentWeapon]["damage"]);

            StartCoroutine(receiveDmgAnimation(f2));


            // F1 RETURNS TO INITIAL POSITION
            while (f1.transform.position.x >= f1position)
            {
                f1.transform.position -= f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

  
            if(f2.hitPoints <= 0)
            {
                anounceWinner(1);
                yield break;
            }
          
            // F2 INITIATES MOVING FORWARD TO ATTACK
            while (f2.transform.position.x >= f1.transform.position.x + 2)
            {
                f2.transform.position += f2.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            f1.hitPoints = f1.hitPoints - f2.baseDmg;
            StartCoroutine(receiveDmgAnimation(f1));


            // F2 RETURNS TO INITIAL POSITION
            while (f2.transform.position.x <= f2position)
            {
                f2.transform.position -= f2.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            if (f1.hitPoints <= 0)
            {
                anounceWinner(2);
                yield break;
            }

        } 
    }

    IEnumerator receiveDmgAnimation(FighterStats f)
    {
        Renderer figtherRenderer = f.GetComponent<Renderer>();
        figtherRenderer.material.color = new Color(255, 1, 1);
        yield return new WaitForSeconds(.3f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }

    private void anounceWinner(int i)
    {
        WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR " + i.ToString();
    }

    /*
    CODIGO PARA EL COMBATE POR TEXTO EN EL PREFAB CANVASLOG
    public Text CombatLogText;

    CombatLogText.text += "EMPIEZA EL COMBATE!!!\n";
    CombatLogText.text += "f2: tiene " + f2.hitPoints + "\n";
    CombatLogText.text += "f1: tiene " + f1.hitPoints + "\n";
    CombatLogText.text += "FINAL DE TURNO \n";

    CombatLogText.text += "GANA EL JUGADOR 1";
    CombatLogText.text += "GANA EL JUGADOR 2";
    */

}