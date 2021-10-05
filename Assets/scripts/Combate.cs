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
    public FighterStats f1, f2;
    public Text WinnerBannerText;

    public float f1position;
    public float f2position;

    // These values will come from the database in the future
    public Dictionary<int, Dictionary<string, int>> initialFighterValues =
    new Dictionary<int, Dictionary<string, int>>
    {
            {
                0,
                new Dictionary<string, int>
                {
                    {"hitPoints", 8},
                    {"baseDmg", 1},
                    {"baseAgility", 1},

               }
            },
            {
                1,
                new Dictionary<string, int>
                {
                    {"hitPoints", 20},
                    {"baseDmg", 2},
                    {"baseAgility", 1},
                }
            },
    };

    // Start is called before the first frame update
    void Start()
    {
        // crear 2 pjs        
        f1 = Instantiate(figherModel, new Vector3(-10, 0, 0), Quaternion.Euler(0, 90, 0));
        f2 = Instantiate(figherModel, new Vector3(10, 0, 0), Quaternion.Euler(0, -90, 0));

        setInitialValuesForFighters(f1, 0);
        setInitialValuesForFighters(f2, 1);

        // save initial position of fighters
        f1position = f1.transform.position.x;
        f2position = f2.transform.position.x;

        //In the future we won't need to instantiate the weapon prefab as it will be part of the fighter skin
        //crear weapon + set position
        weapon = Instantiate(weaponModel, f1.transform.position, Quaternion.Euler(0, 90, 0), f1.transform);
        Vector3 figher1Position = f1.transform.position;
        weapon.transform.position = new Vector3(figher1Position.x + 2, figher1Position.y, figher1Position.z);

        //set list of weapons for the fighter
        int[] weaponLists = { 0, 1, 2, 3 };
        f1.weaponsList = weaponLists;

        //set current weapon of the figher 
        f1.currentWeapon = f1.weaponsList[2];
        Debug.Log("current weapon");
        Debug.Log(f1.currentWeapon);
        Debug.Log("weapon damage");
        Debug.Log(weapon.weapons[f1.currentWeapon]["damage"]);

        StartCoroutine(attack(f1, f2));
    }

    IEnumerator attack(FighterStats f1, FighterStats f2)
    {
        while (f1.hitPoints > 0 || f2.hitPoints > 0)
        {
            // F1 INITIATES MOVING FORWARD TO ATTACK
            while (f1.transform.position.x <= f2.transform.position.x - 2)
            {
                f1.transform.position += f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }

            // DAMAGE LOGIC
            inflictDamageToFighter(f1, f2);
            StartCoroutine(receiveDmgAnimation(f2));
            //hacer que la barra de vida prefab baje


            // F1 RETURNS TO INITIAL POSITION
            while (f1.transform.position.x >= f1position)
            {
                f1.transform.position -= f1.transform.forward * Time.deltaTime * 40;
                yield return new WaitForFixedUpdate();
            }


            if (f2.hitPoints <= 0)
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

            // DAMAGE LOGIC
            inflictDamageToFighter(f2, f1);
            StartCoroutine(receiveDmgAnimation(f1));
            //hacer que la barra de vida prefab baje


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

    private void setInitialValuesForFighters(FighterStats figther, int fighterNumber)
    {
        figther.hitPoints = initialFighterValues[fighterNumber]["hitPoints"];
        figther.baseDmg = initialFighterValues[fighterNumber]["baseDmg"];
        figther.baseAgility = initialFighterValues[fighterNumber]["baseAgility"];
    }

    private void inflictDamageToFighter(FighterStats attacker, FighterStats defender)
    {
        int weaponDamage = int.Parse(weapon.weapons[attacker.currentWeapon]["damage"]);
        int damageOnHit = weaponDamage + attacker.baseDmg;
        int remainingLife = defender.hitPoints - damageOnHit;
        defender.hitPoints = remainingLife < 0 ? 0 : defender.hitPoints - damageOnHit;
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