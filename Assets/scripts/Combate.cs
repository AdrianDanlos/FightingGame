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

    string[] fighterNames = { "FIGHTER 1", "FIGHTER 2" };

    public string attackerName;
    public string defenderName;


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
                    {"baseAgility", 30},

               }
            },
            {
                1,
                new Dictionary<string, int>
                {
                    {"hitPoints", 15},
                    {"baseDmg", 2},
                    {"baseAgility", 30},
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
        f1.currentWeapon = f1.weaponsList[3];
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
        float time = 1f;

        while (!gameIsOver)
        {

            //FIGHTER 1
            SetAttackerAndDefenderNames(fighterNames[0], fighterNames[1]);
            yield return StartCoroutine(MoveFighter(f1.transform, fighterOneInitialPosition, fighterOneDestinationPosition, time));
            StartCoroutine(PerformAttack(f1, f2));
            yield return StartCoroutine(MoveFighter(f1.transform, fighterOneDestinationPosition, fighterOneInitialPosition, time));

            if (gameIsOver)
            {
                yield break;
            }

            //FIGHTER 2
            SetAttackerAndDefenderNames(fighterNames[0], fighterNames[1]);
            yield return StartCoroutine(MoveFighter(f2.transform, fighterTwoInitialPosition, fighterTwoDestinationPosition, time));
            StartCoroutine(PerformAttack(f2, f1));
            yield return StartCoroutine(MoveFighter(f2.transform, fighterTwoDestinationPosition, fighterTwoInitialPosition, time));

            if (gameIsOver)
            {
                yield break;
            }
        }
    }

    private void SetAttackerAndDefenderNames(string attackerName, string defenderName)
    {
        this.attackerName = attackerName;
        this.defenderName = defenderName;
    }

    IEnumerator MoveFighter(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    IEnumerator PerformAttack(FighterStats attacker, FighterStats defender)
    {
        if (IsAttackDodged(defender))
        {
            StartCoroutine(attackDodgedAnimation(defender));
        }
        else
        {
            InflictDamageToFighter(attacker, defender);
            StartCoroutine(ReceiveDmgAnimation(defender));
            // SET CHANGE IN HEALTH BAR
            gameIsOver = defender.hitPoints <= 0 ? true : false;
            if (gameIsOver)
            {
                announceWinner();
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

    private void announceWinner()
    {
        WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR " + attackerName;
    }


    private IEnumerator attackDodgedAnimation(FighterStats defender)
    {
        float time = 0.15f;

        Vector3 defenderInitialPosition = defender.transform.position;
        Vector3 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = defenderName == fighterNames[0] ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
        defenderDodgeDestination.y += 1;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender.transform, defender.transform.position, defenderDodgeDestination, time));
        yield return StartCoroutine(MoveFighter(defender.transform, defenderDodgeDestination, defenderInitialPosition, time));
    }
}