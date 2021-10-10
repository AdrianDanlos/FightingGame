using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combate : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    public FighterStats figherModel;

    // f1 player - f2 CPU
    public FighterStats f1, f2;

    public HealthBar oneHealthBar;
    public HealthBar twoHealthBar;
    public Text WinnerBannerText;

    Vector2 fighterOneInitialPosition;
    Vector2 fighterTwoInitialPosition;
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
        {"baseDmg", 2},
        {"baseAgility", 50},
        {"baseSpeed", 50},
    };

    void Start()
    {
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
            f1.hitPoints = 8;
            f1.baseDmg = 2;
            f1.baseAgility = 50;
            f1.baseSpeed = 50;
        }

        SetInitialValuesForCpuFighter(f2);

        fighterOneInitialPosition = f1.transform.position;
        fighterTwoInitialPosition = f2.transform.position;

        int distanceBetweenFightersOnAttack = 3;
        fighterOneDestinationPosition = fighterTwoInitialPosition + new Vector2(-distanceBetweenFightersOnAttack, 0);
        fighterTwoDestinationPosition = fighterOneInitialPosition + new Vector2(+distanceBetweenFightersOnAttack, 0);

        //set max health of players
        oneHealthBar.SetMaxHealth(f1.hitPoints);
        twoHealthBar.SetMaxHealth(f2.hitPoints);

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
            yield return StartCoroutine(CombatLogicHandler(f1, f2, fighterOneInitialPosition, fighterOneDestinationPosition, twoHealthBar));

            if (gameIsOver)
            {
                yield break;
            }

            //FIGHTER 2 ATTACKS
            SetAttackerAndDefenderNames(fighterNames[1], fighterNames[0]);
            yield return StartCoroutine(CombatLogicHandler(f2, f1, fighterTwoInitialPosition, fighterTwoDestinationPosition, oneHealthBar));
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
        float time = 0.5f;

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
            yield return new WaitForSeconds(0.4f);
            yield break;
        }

        InflictDamageToFighter(attacker, defender);
        gameIsOver = defender.hitPoints <= 0 ? true : false;
        if (gameIsOver)
        {
            defender.StartDeathAnimation();
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            yield return new WaitForSeconds(0.2f);
            announceWinner();
            attacker.StartIdleBlinkAnimation();
        }
        else
        {
            defender.StartHurtAnimation();
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(ReceiveDmgAnimation(defender));
        }

        healthbar.SetRemainingHealth(defender.hitPoints);
        //Wait for attack anim to finish
        yield return new WaitForSeconds(0.35f);

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
        int remainingLife = defender.hitPoints - attacker.baseDmg;
        defender.hitPoints = remainingLife < 0 ? 0 : remainingLife;
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