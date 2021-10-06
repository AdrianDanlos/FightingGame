using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // Globals
    public FighterStats figherModel;
    public FighterStats f1, f2;

    public Text WinnerBannerText;

    public Vector3 fighterOneInitialPosition = new Vector3(-10, 0, 0);
    public Vector3 fighterTwoInitialPosition = new Vector3(10, 0, 0);
    public Vector3 fighterOneDestinationPosition;
    public Vector3 fighterTwoDestinationPosition;

    public bool gameIsOver = false;

    readonly float time = 0.5f;

    // These values will come from the database in the future
    public Dictionary<int, Dictionary<string, int>> initialFighterValues =
    new Dictionary<int, Dictionary<string, int>>
    {
            {
                0,
                new Dictionary<string, int>
                {
                    {"hitPoints", 4},
                    {"baseDmg", 1},
                    {"baseAgility", 10},

               }
            },
            {
                1,
                new Dictionary<string, int>
                {
                    {"hitPoints", 6},
                    {"baseDmg", 2},
                    {"baseAgility", 10},
                }
            },
    };

    // Start is called before the first frame update
    void Start()
    {
        // create both fighter on the scene        
        f1 = Instantiate(figherModel, fighterOneInitialPosition, Quaternion.Euler(0, 90, 0));
        f2 = Instantiate(figherModel, fighterTwoInitialPosition, Quaternion.Euler(0, -90, 0));

        SetInitialValuesForFighters(f1, 0);
        SetInitialValuesForFighters(f2, 1);

        // set attack destination of fighters
        fighterOneDestinationPosition = fighterTwoInitialPosition;
        fighterOneDestinationPosition.x -= 2;
        fighterTwoDestinationPosition = fighterOneInitialPosition;
        fighterTwoDestinationPosition.x += 2;

        //set list of weapons for the fighters
        int[] weaponLists = { 0, 1, 2, 3 };
        f1.weaponsList = weaponLists;
        f2.weaponsList = weaponLists;

        //set current weapon of the fighers 
        f1.currentWeapon = f1.weaponsList[2];
        f2.currentWeapon = f2.weaponsList[0];


        StartCoroutine(InitiateAttack(f1, f2));
    }

    private void SetInitialValuesForFighters(FighterStats figther, int fighterNumber)
    {
        figther.hitPoints = initialFighterValues[fighterNumber]["hitPoints"];
        figther.baseDmg = initialFighterValues[fighterNumber]["baseDmg"];
        figther.baseAgility = initialFighterValues[fighterNumber]["baseAgility"];
    }

    IEnumerator InitiateAttack(FighterStats f1, FighterStats f2)
    {
        while (!gameIsOver)
        {
            //FIGHTER 1
            yield return StartCoroutine(MoveFighter(f1.transform, fighterOneInitialPosition, fighterOneDestinationPosition));
            StartCoroutine(PerformAttack(f1, f2, "FIGTHER 1"));
            yield return StartCoroutine(MoveFighter(f1.transform, fighterOneDestinationPosition, fighterOneInitialPosition));

            if (gameIsOver)
            {
                yield break;
            }

            //FIGHTER 2
            yield return StartCoroutine(MoveFighter(f2.transform, fighterTwoInitialPosition, fighterTwoDestinationPosition));
            StartCoroutine(PerformAttack(f2, f1, "FIGTHER 2"));
            yield return StartCoroutine(MoveFighter(f2.transform, fighterTwoDestinationPosition, fighterTwoInitialPosition));

            if (gameIsOver)
            {
                yield break;
            }
        }
    }

    IEnumerator MoveFighter(Transform thisTransform, Vector3 startPos, Vector3 endPos)
    {
        var i = 0.0f;
        var rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    IEnumerator PerformAttack(FighterStats attacker, FighterStats defender, string winnerName)
    {
        if (IsAttackDodged(defender))
        {
            //StartCoroutine(attackDodgedAnimation(defender));
        }
        else
        {
            InflictDamageToFighter(attacker, defender);
            StartCoroutine(ReceiveDmgAnimation(defender));
            // SET CHANGE IN HEALTH BAR
            gameIsOver = defender.hitPoints <= 0 ? true : false;
            if (gameIsOver)
            {
                announceWinner(winnerName);
            }
        }
        yield return null;
    }

    private bool IsAttackDodged(FighterStats defender)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= defender.baseAgility ? true : false;
    }

    private void InflictDamageToFighter(FighterStats attacker, FighterStats defender)
    {
        Weapons weapon = new Weapons();
        int weaponDamage = int.Parse(weapon.weapons[attacker.currentWeapon]["damage"]);
        int damageOnHit = weaponDamage + attacker.baseDmg;
        int remainingLife = defender.hitPoints - damageOnHit;
        defender.hitPoints = remainingLife < 0 ? 0 : defender.hitPoints - damageOnHit;
    }

    private IEnumerator ReceiveDmgAnimation(FighterStats f)
    {
        Renderer figtherRenderer = f.GetComponent<Renderer>();
        figtherRenderer.material.color = new Color(255, 1, 1);
        yield return new WaitForSeconds(.3f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }

    private void announceWinner(string winnerName)
    {
        WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR " + winnerName;
    }


    private IEnumerator attackDodgedAnimation(FighterStats defender)
    {
        yield return StartCoroutine(MoveFighter(defender.transform, fighterTwoInitialPosition, fighterOneInitialPosition));


        //F2 DODGES
        //Backward movement
        /*while (f2.transform.position.x <= fighterTwoInitialPosition + 3)
        {
            f2.transform.position -= f2.transform.forward * Time.deltaTime * 40;
            yield return new WaitForFixedUpdate();
        }

        //Forward movement, back to initial position
        while (f2.transform.position.x >= fighterTwoInitialPosition)
        {
            f2.transform.position += f2.transform.forward * Time.deltaTime * 20;
            yield return new WaitForFixedUpdate();
        }

        //F1 DODGES
        //Backward movement
        while (f1.transform.position.x >= fighterOneInitialPosition - 3)
        {
            f1.transform.position -= f1.transform.forward * Time.deltaTime * 40;
            yield return new WaitForFixedUpdate();
        }

        //Back to initial position (Forward)
        while (f1.transform.position.x <= fighterOneInitialPosition)
        {
            f1.transform.position += f1.transform.forward * Time.deltaTime * 20;
            yield return new WaitForFixedUpdate();
        }*/
    }

    /*CREATE WEAPON ON THE SCENE AND SET POSITION
    In the future we won't need to instantiate the weapon prefab as it will be part of the fighter skin
    weapon = Instantiate(weaponModel, f1.transform.position, Quaternion.Euler(0, 90, 0), f1.transform);
    Vector3 figher1Position = f1.transform.position;
    weapon.transform.position = new Vector3(figher1Position.x + 1, figher1Position.y, figher1Position.z);*/

}