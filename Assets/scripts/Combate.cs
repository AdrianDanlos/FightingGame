using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // Globals
    public FighterStats figherModel;
    public FighterStats f1, f2;

    public HealthBar onehealthBar;
    public HealthBar twohealthBar;
    public Text WinnerBannerText;

    public Vector2 fighterOneInitialPosition = new Vector2(-10, 0);
    public Vector2 fighterTwoInitialPosition = new Vector2(10, 0);
    public Vector2 fighterOneDestinationPosition;
    public Vector2 fighterTwoDestinationPosition;

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
                    {"hitPoints", 12},
                    {"baseDmg", 1},
                    {"baseAgility", 25},
                    {"baseSpeed", 25},

               }
            },
            {
                1,
                new Dictionary<string, int>
                {
                    {"hitPoints", 20},
                    {"baseDmg", 2},
                    {"baseAgility", 25},
                    {"baseSpeed", 25},
                }
            },
    };

    // Start is called before the first frame update
    void Start()
    {
        // create both fighter on the scene        
        f1 = Instantiate(figherModel, fighterOneInitialPosition, Quaternion.Euler(0, 0, 0));
        f2 = Instantiate(figherModel, fighterTwoInitialPosition, Quaternion.Euler(0, 0, 0));
        f2.transform.localScale = new Vector3(-1, 1, 1);

        // -- load game data --
        // set initial values 
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

        //set max health of players
        onehealthBar.SetMaxHealth(f1.hitPoints);
        twohealthBar.SetMaxHealth(f2.hitPoints);

        StartCoroutine(InitiateCombat());
    }

    public void SetInitialValuesForFighters(FighterStats figther, int fighterNumber)
    {
        figther.hitPoints = initialFighterValues[fighterNumber]["hitPoints"];
        figther.baseDmg = initialFighterValues[fighterNumber]["baseDmg"];
        figther.baseAgility = initialFighterValues[fighterNumber]["baseAgility"];
        figther.baseSpeed = initialFighterValues[fighterNumber]["baseSpeed"];

    }

    IEnumerator InitiateCombat()
    {
        while (!gameIsOver)
        {
            //FIGHTER 1 ATTACKS
            SetAttackerAndDefenderNames(fighterNames[0], fighterNames[1]);
            yield return StartCoroutine(CombatLogicHandler(f1, f2, fighterOneInitialPosition, fighterOneDestinationPosition, twohealthBar));

            if (gameIsOver)
            {
                yield break;
            }

            //FIGHTER 2 ATTACKS
            SetAttackerAndDefenderNames(fighterNames[1], fighterNames[0]);
            yield return StartCoroutine(CombatLogicHandler(f2, f1, fighterTwoInitialPosition, fighterTwoDestinationPosition, onehealthBar));
        }
    }

    private void SetAttackerAndDefenderNames(string attackerName, string defenderName)
    {
        this.attackerName = attackerName;
        this.defenderName = defenderName;
    }


    IEnumerator CombatLogicHandler(FighterStats attacker, FighterStats defender, Vector2 fighterInitialPosition, Vector2 fighterDestinationPosition, HealthBar healthbar)
    {
        //Movement speed
        float time = 0.2f;

        //Move forward
        yield return StartCoroutine(MoveFighter(attacker.transform, fighterInitialPosition, fighterDestinationPosition, time));

        //Attack
        do
        {
            yield return StartCoroutine(PerformAttack(attacker, defender, healthbar));
        } while (IsAttackRepeated(attacker) && !gameIsOver);

        //Move back
        yield return StartCoroutine(MoveFighter(attacker.transform, fighterDestinationPosition, fighterInitialPosition, time));
    }

    IEnumerator MoveFighter(Transform thisTransform, Vector2 startPos, Vector2 endPos, float time)
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

    IEnumerator PerformAttack(FighterStats attacker, FighterStats defender, HealthBar healthbar)
    {
        if (IsAttackDodged(defender))
        {
            StartCoroutine(attackDodgedAnimation(defender));
        }
        else
        {
            InflictDamageToFighter(attacker, defender);
            StartCoroutine(ReceiveDmgAnimation(defender));
            healthbar.SetHealth(defender.hitPoints);

            gameIsOver = defender.hitPoints <= 0 ? true : false;
            if (gameIsOver)
            {
                //Death death = new Death();
                //death.playDeathAnimation(defender);
                announceWinner();
            }
        }
        // In the future instead of waiting we display the attack animation
        yield return new WaitForSeconds(0.3f);
    }

    private bool IsAttackRepeated(FighterStats attacker)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= attacker.baseSpeed ? true : false;
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

    private void deathAnimation(FighterStats defender)
    {
        //Load a Sprite (Assets/Resources/Sprites/sprite01.png)
        //var sprite = Resources.Load<Sprite>("0_Reaper_Man_Dying_014", "0_Reaper_Man_Dying_014");


        //SpriteRenderer renderer = defender.GetComponent<SpriteRenderer>();
        //Object[] sprites;
        //sprites = Resources.LoadAll("Death_sequence");
        //renderer.sprite = (Sprite)sprites[15];
        // spriteRenderer.sprite = defender.newSprite;

        //Animator
        //Animator animator = defender.gameObject.GetComponent<Animator>();
        //animator.runtimeAnimatorController = Resources.Load("0_Reaper_Man_Dying_000") as RuntimeAnimatorController;
    }

    private IEnumerator attackDodgedAnimation(FighterStats defender)
    {
        float time = 0.15f;

        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = defenderName == fighterNames[0] ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
        defenderDodgeDestination.y += 1;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender.transform, defender.transform.position, defenderDodgeDestination, time));
        yield return StartCoroutine(MoveFighter(defender.transform, defenderDodgeDestination, defenderInitialPosition, time));
    }
}