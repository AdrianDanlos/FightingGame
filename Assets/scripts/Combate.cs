using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // instanciar f
    public FighterStats figherModel;
    public Weapons weaponModel;
    Weapons weapon;

    public Text WinnerBannerText;
    
    // Start is called before the first frame update
    void Start()
    {
        // crear 2 pjs
        // should we declare those as globals?
        FighterStats f1, f2;
        f1 = Instantiate(figherModel, new Vector3(-10, 0, 0), Quaternion.Euler(0, 90, 0));
        f2 = Instantiate(figherModel, new Vector3(10, 0, 0), Quaternion.Euler(0, -90, 0));
        f1.SetHitPoints(3);
        f2.SetHitPoints(7);

        //crear weapon
        weapon = Instantiate(weaponModel, f1.transform.position, Quaternion.Euler(0, 90, 0), f1.transform);

        //set position of the weapon
        Vector3 figher1Position = f1.transform.position;
        weapon.transform.position = new Vector3(figher1Position.x + 2, figher1Position.y, figher1Position.z);

        //set list of weapons for the fighter
        int[] weaponLists = { 0, 1 };
        f1.SetWeaponsList(weaponLists);

        //set current weapon of the figher 
        f1.SetCurrentWeapon(f1.GetWeaponsList()[0]);
        Debug.Log(f1.GetCurrentWeapon());

        //Get weapon dmg
        Debug.Log(weapon.weapons[f1.GetCurrentWeapon()]["damage"]);

        StartCoroutine(attack(f1, f2));
    }

    IEnumerator attack(FighterStats f1, FighterStats f2)
    {        
        Debug.Log(f1.GetHitPoints());
        while (f1.GetHitPoints() > 0 || f2.GetHitPoints() > 0)
        {
            // F1 ATTACKS
            float f1position = f1.transform.position.x;

            // F1 GOES FORWARD
            while (f1.transform.position.x <= f2.transform.position.x - 2)
            {
                f1.transform.position += f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            f2.SetHitPoints(f2.GetHitPoints() - int.Parse(weapon.weapons[f1.GetCurrentWeapon()]["damage"]));
            Debug.Log("f2: tiene " + f2.GetHitPoints());

            StartCoroutine(receiveDmgAnimation(f2));


            // F1 RETURNS TO INITIAL POSITION
            // FIXME: Position x is not an integer so the figther goes further back from the initial position (e.g. 10.023)
            while (f1.transform.position.x >= f1position)
            {
                f1.transform.position -= f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

  
            if(f2.GetHitPoints() <= 0)
            {
                //Especificar cual gana
                anunciarGanador(1);
                yield break;
            }
          
            // F2 ATTACKS
            float f2position = f2.transform.position.x;

            // F2 GOES FORWARD
            while (f2.transform.position.x >= f1.transform.position.x + 2)
            {
                f2.transform.position += f2.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            f1.SetHitPoints(f1.GetHitPoints() - f2.GetBaseDmg());
            StartCoroutine(receiveDmgAnimation(f1));


            // F2 RETURNS TO INITIAL POSITION
            // FIXME: Position x is not an integer so the figther goes further back from the initial position (e.g. 10.023)
            while (f2.transform.position.x <= f2position)
            {
                f2.transform.position -= f2.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            if (f1.GetHitPoints() <= 0)
            {
                //Especificar cual gana
                anunciarGanador(2);
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

    private void anunciarGanador(int i)
    {
        if(i == 1)
        {
            WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR 1";
        }
            
        else if (i == 2)
        {
            WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR 2";
        }
    }

    /*
    CODIGO PARA EL COMBATE POR TEXTO EN EL PREFAB CANVASLOG
    public Text CombatLogText;

    CombatLogText.text += "EMPIEZA EL COMBATE!!!\n";
    CombatLogText.text += "f2: tiene " + f2.GetHitPoints() + "\n";
    CombatLogText.text += "f1: tiene " + f1.GetHitPoints() + "\n";
    CombatLogText.text += "FINAL DE TURNO \n";

    CombatLogText.text += "GANA EL JUGADOR 1";
    CombatLogText.text += "GANA EL JUGADOR 2";
    */

}