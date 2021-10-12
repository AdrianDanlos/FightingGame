using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Combate : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    public FighterStats figherModel;
    public SpriteRenderer arenaRenderer;
    public Sprite[] spriteArray;

    // f1 player - f2 CPU
    public FighterStats f1, f2;

    public HealthBar oneHealthBar;
    public HealthBar twoHealthBar;
    public Text WinnerBannerText;

    Vector2 fighterOneInitialPosition;
    Vector2 fighterTwoInitialPosition;
    Vector2 fighterOneDestinationPosition;
    Vector2 fighterTwoDestinationPosition;

    float movementSpeed = 0.3f;
    bool gameIsOver = false;

    string[] fighterNames = { "FIGHTER 1", "FIGHTER 2" };

    string attackerName;
    string defenderName;

    public CombatCanvas combatCanvas;

    // FIXME: Try to reuse confetti with different X position?
    public GameObject winnerConfetti1;
    public GameObject winnerConfetti2;

    // Player values (Fallback if no values found to avoid crashes)
    public Dictionary<string, int> playerFighterStats =
    new Dictionary<string, int>
    {
        {"hitPoints", 10},
        {"baseDmg", 1},
        {"baseAgility", 1},
        {"baseSpeed", 1},
        {"counterRate", 1},
    };

    // FIXME: These should be calculated/randomized depending on the players level
    // CPU values
    public Dictionary<string, int> cpuFighterStats =
    new Dictionary<string, int>
    {
        {"hitPoints", 10},
        {"baseDmg", 1},
        {"baseAgility", 1},
        {"baseSpeed", 1},
        {"counterRate", 1},
    };

    void Start()
    {


        // load data from save
        // set initial values for player
        // FIXME -- refactor the way this is loaded when we implemente online mode
        /*if (manageSaves.CheckIfFileExists())
        {
            Dictionary<string, int> playerFighterStats = manageSaves.LoadGameData();
            f1.hitPoints = playerFighterStats["hitPoints"];
            f1.baseDmg = playerFighterStats["baseDmg"];
            f1.baseAgility = playerFighterStats["baseAgility"];
            f1.baseSpeed = playerFighterStats["baseSpeed"];
        }*/

        //FIXME: Player(f1) skills should come from save file as an array of int
        SetFighterSkills(f1, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });
        SetFighterSkills(f2, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });

        SetFighterStats(f1, playerFighterStats);
        SetFighterStats(f2, cpuFighterStats);

        SetFighterStatsBasedOnSkills(f1);
        SetFighterStatsBasedOnSkills(f2);

        LoadRandomArena();

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

    public void SetFighterSkills(FighterStats fighter, string[] skills)
    {
        fighter.skills = skills;
    }

    public void SetFighterStats(FighterStats figther, Dictionary<string, int> data)
    {
        figther.hitPoints = data["hitPoints"];
        figther.baseDmg = data["baseDmg"];
        figther.baseAgility = data["baseAgility"];
        figther.baseSpeed = data["baseSpeed"];
    }

    public void SetFighterStatsBasedOnSkills(FighterStats fighter)
    {
        if (fighter.skills.Contains(Skills.SkillsList.SIXTHSENSE.ToString()))
        {
            fighter.counterRate += 10;
        }
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
        //Move forward
        attacker.StartRunAnimation();
        yield return StartCoroutine(MoveFighter(attacker, fighterInitialPosition, fighterDestinationPosition, movementSpeed));

        //Attack
        do
        {
            /*if (IsCounterAttack())
            {

            }*/
            yield return StartCoroutine(PerformAttack(attacker, defender, healthbar));
        } while (IsAttackRepeated(attacker) && !gameIsOver);

        //Move back
        switchFighterOrientation(attacker, true);
        attacker.StartRunAnimation();
        yield return StartCoroutine(MoveFighter(attacker, fighterDestinationPosition, fighterInitialPosition, movementSpeed));
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



        attacker.StartAttackAnimation();
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

            combatCanvas.RenderDefeatSprite(defenderName);
            announceWinner();
            attacker.StartIdleBlinkAnimation();

            // update save file (exp, wr, abilities)
            // FIXME -- if condition swapped? + refactor this condition into methods
            if (attackerName == "FIGHTER 1")
            {
                manageSaves.UpdateDataFromCombat(1, 0);
                winnerConfetti2.gameObject.SetActive(true);
                winnerConfetti2.GetComponent<ParticleSystem>().Play();
            }
            else if (attackerName == "FIGHTER 2")
            {
                manageSaves.UpdateDataFromCombat(0, 1);
                winnerConfetti1.gameObject.SetActive(true);
                winnerConfetti1.GetComponent<ParticleSystem>().Play();
            }
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

    private bool IsCounterAttack(FighterStats defender)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= defender.counterRate ? true : false;
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
        WinnerBannerText.text = attackerName + " WINS THE COMBAT. " + defenderName + " GOT SMASHED!";
    }

    private IEnumerator dodgeMovement(FighterStats defender)
    {
        float dodgeSpeed = 0.15f;

        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = defenderName == fighterNames[0] ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
        defenderDodgeDestination.y += 1;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender, defender.transform.position, defenderDodgeDestination, dodgeSpeed));
        yield return StartCoroutine(MoveFighter(defender, defenderDodgeDestination, defenderInitialPosition, dodgeSpeed));
    }
    private void LoadRandomArena()
    {
        int indexOfArena = Random.Range(0, spriteArray.Length);
        arenaRenderer.sprite = spriteArray[indexOfArena];
    }

}