using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    // Globals
    public FighterStats figherModel;
    // f1 player - f2 CPU
    public FighterStats f1, f2;

    public HealthBar onehealthBar;
    public HealthBar twohealthBar;
    public Text WinnerBannerText;

    Vector2 fighterOneInitialPosition = new Vector2(-10, 0);
    Vector2 fighterTwoInitialPosition = new Vector2(10, 0);
    Vector2 fighterOneDestinationPosition;
    Vector2 fighterTwoDestinationPosition;

    bool gameIsOver = false;

    string[] fighterNames = { "FIGHTER 1", "FIGHTER 2" };

    string attackerName;
    string defenderName;

    // CPU values
    public Dictionary<string, int> initialCPUFighterValues =
    new Dictionary<string, int>
    {
        {"hitPoints", 1},
        {"baseDmg", 1},
        {"baseAgility", 1},
        {"baseSpeed", 1},
    };

    FighterStats animator;

    // Start is called before the first frame update
    void Start()
    {
        // create both fighter on the scene        
        f1 = Instantiate(figherModel, fighterOneInitialPosition, Quaternion.Euler(0, 0, 0));
        f2 = Instantiate(figherModel, fighterTwoInitialPosition, Quaternion.Euler(0, 0, 0));
        f2.transform.localScale = new Vector3(-1, 1, 1);


        // load data from save
        // set initial values 
        // FIXME -- refactor the way this is loaded when we implemente online mode
        if (manageSaves.CheckIfFileExists())
        {
            Dictionary<string, int> initialPlayerFighterValues = manageSaves.LoadGameData();
            f1.hitPoints = initialPlayerFighterValues["hitPoints"];
            f1.baseDmg = initialPlayerFighterValues["baseDmg"];
            f1.baseAgility = initialPlayerFighterValues["baseAgility"];
            f1.baseSpeed = initialPlayerFighterValues["baseSpeed"];
        }
        else
        {
            // fallback data if save file doesn't exist and this scene loads
            f1.hitPoints = 5;
            f1.baseDmg = 1;
            f1.baseAgility = 1;
            f1.baseSpeed = 1;
        }

        SetInitialValuesForCpuFighter(f2);

        // set attack destination of fighters
        fighterOneDestinationPosition = fighterTwoInitialPosition;
        fighterOneDestinationPosition.x -= 3;
        fighterTwoDestinationPosition = fighterOneInitialPosition;
        fighterTwoDestinationPosition.x += 3;

        //set list of weapons for the fighters
        int[] weaponLists = { 0, 1, 2, 3 };
        f1.weaponsList = weaponLists;
        f2.weaponsList = weaponLists;

        //set current weapon of the fighers 
        f1.currentWeapon = f1.weaponsList[0];
        f2.currentWeapon = f2.weaponsList[0];

        //set max health of players
        onehealthBar.SetMaxHealth(f1.hitPoints);
        twohealthBar.SetMaxHealth(f2.hitPoints);

        StartCoroutine(InitiateCombat());

    }

    public void SetInitialValuesForCpuFighter(FighterStats figther)
    {
        figther.hitPoints = initialCPUFighterValues["hitPoints"];
        figther.baseDmg = initialCPUFighterValues["baseDmg"];
        figther.baseAgility = initialCPUFighterValues["baseAgility"];
        figther.baseSpeed = initialCPUFighterValues["baseSpeed"];

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
        float time = 0.6f;

        //Move forward
        attacker.StartRunAnimation();
        yield return StartCoroutine(MoveFighter(attacker, fighterInitialPosition, fighterDestinationPosition, time));

        //Attack
        do
        {
            attacker.StartAttackAnimation();
            yield return StartCoroutine(PerformAttack(attacker, defender, healthbar));
        } while (IsAttackRepeated(attacker) && !gameIsOver);

        //Move back
        switchFighterOrientation(attacker, true);
        attacker.StartRunAnimation();
        yield return StartCoroutine(MoveFighter(attacker, fighterDestinationPosition, fighterInitialPosition, time));
        switchFighterOrientation(attacker, false);
        attacker.EndRunAnimation();

    }

    IEnumerator MoveFighter(FighterStats fighter, Vector2 startPos, Vector2 endPos, float time)
    {
        float i = 0.0f;
        float rate = 1.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            fighter.transform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    private void switchFighterOrientation(FighterStats attacker, bool reverseOrentation)
    {
        var spriteRenderer = attacker.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = reverseOrentation;
    }

    IEnumerator PerformAttack(FighterStats attacker, FighterStats defender, HealthBar healthbar)
    {
        if (IsAttackDodged(defender))
        {
            defender.StartDodgeAnimation();
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(dodgeMovement(defender));
            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.55f);
        }
        else
        {
            defender.StartHurtAnimation();
            yield return new WaitForSeconds(0.35f);
            InflictDamageToFighter(attacker, defender);
            StartCoroutine(ReceiveDmgAnimation(defender));
            healthbar.SetHealth(defender.hitPoints);
            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.2f);
            gameIsOver = defender.hitPoints <= 0 ? true : false;
            if (gameIsOver)
            {
                //WIP defender.StartDeathAnimation();
                Debug.Log(defender.animator.GetBool("Death"));
                announceWinner();
                yield return new WaitForSeconds(1f);
            }
        }
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
        yield return new WaitForSeconds(.2f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }

    private void announceWinner()
    {
        WinnerBannerText.text = "FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR " + attackerName;
    }

    private IEnumerator dodgeMovement(FighterStats defender)
    {
        float time = 0.15f;

        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = defenderName == fighterNames[0] ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
        defenderDodgeDestination.y += 1;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender, defender.transform.position, defenderDodgeDestination, time));
        yield return StartCoroutine(MoveFighter(defender, defenderDodgeDestination, defenderInitialPosition, time));
    }
}