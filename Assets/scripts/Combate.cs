using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Combate : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;

    //Arena render
    public SpriteRenderer arenaRenderer;
    public Sprite[] spriteArray;


    public FighterStats figherModel;
    public FighterStats f1, f2;
    public FighterStats fighterAttackingThisTurn, fighterDefendingThisTurn;
    string[] fighterNames = { "FIGHTER 1", "FIGHTER 2" };

    public HealthBar oneHealthBar, twoHealthBar;
    public Text WinnerBannerText;

    Vector2 fighterOneInitialPosition, fighterTwoInitialPosition;
    Vector2 fighterOneDestinationPosition, fighterTwoDestinationPosition;

    float movementSpeed = 0.4f;
    bool gameIsOver = false;

    public CombatCanvas combatCanvas;

    // FIXME: Try to reuse confetti with different X position?
    public GameObject winnerConfetti1, winnerConfetti2;

    // Player values (Fallback if no values found to avoid crashes)
    public Dictionary<string, dynamic> playerFighterStats =
    new Dictionary<string, dynamic>
    {
        {"fighterName", "Adrian"},
        {"hitPoints", 10},
        {"damage", 1},
        {"agility", 1},
        {"speed", 1},
        {"counterRate", 1},
    };

    // FIXME: These should be calculated/randomized depending on the players level
    // CPU values
    public Dictionary<string, dynamic> cpuFighterStats =
    new Dictionary<string, dynamic>
    {
        {"fighterName", "Jowi"},
        {"hitPoints", 2},
        {"damage", 1},
        {"agility", 1 },
        {"speed", 1},
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
            f1.damage = playerFighterStats["damage"];
            f1.agility = playerFighterStats["agility"];
            f1.speed = playerFighterStats["speed"];
        }*/

        //FIXME: Player(f1) skills should come from save file as an array of int
        SetFighterSkills(f1, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });
        SetFighterSkills(f2, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });

        SetFighterStats(f1, playerFighterStats);
        SetFighterStats(f2, cpuFighterStats);

        SetFighterStatsBasedOnSkills(f1);
        //SetFighterStatsBasedOnSkills(f2);

        LoadRandomArena();

        fighterOneInitialPosition = f1.transform.position;
        fighterTwoInitialPosition = f2.transform.position;

        float distanceBetweenFightersOnAttack = 3.5f;
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

    public void SetFighterStats(FighterStats figther, Dictionary<string, dynamic> data)
    {
        figther.fighterName = data["fighterName"];
        figther.hitPoints = data["hitPoints"];
        figther.damage = data["damage"];
        figther.agility = data["agility"];
        figther.speed = data["speed"];
    }

    public void SetFighterStatsBasedOnSkills(FighterStats fighter)
    {
        if (fighter.skills.Contains(Skills.SkillsList.SIXTHSENSE.ToString())) fighter.counterRate += 100;
    }

    IEnumerator InitiateCombat()
    {
        while (!gameIsOver)
        {
            //FIGHTER 1 ATTACKS
            //SetAttackerAndDefenderNames(fighterNames[0], fighterNames[1]);
            yield return StartCoroutine(CombatLogicHandler(f1, f2, fighterOneInitialPosition, fighterOneDestinationPosition, twoHealthBar, oneHealthBar));

            Debug.Log("ENTRETURNOS PARA BREAK");
            if (gameIsOver) break;

            //FIGHTER 2 ATTACKS
            //SetAttackerAndDefenderNames(fighterNames[1], fighterNames[0]);
            yield return StartCoroutine(CombatLogicHandler(f2, f1, fighterTwoInitialPosition, fighterTwoDestinationPosition, oneHealthBar, twoHealthBar));
        }
        Debug.Log("blink");
        getWinner().ChangeAnimationState(FighterStats.AnimationNames.IDLE_BLINK);
    }

    private void SetAttackerAndDefender(FighterStats attacker, FighterStats defender)
    {
        this.fighterAttackingThisTurn = attacker;
        this.fighterDefendingThisTurn = defender;
    }


    IEnumerator CombatLogicHandler(FighterStats attacker, FighterStats defender, Vector2 fighterInitialPosition, Vector2 fighterDestinationPosition, HealthBar defenderHealthbar, HealthBar attackerHealthbar)
    {
        //Move forward
        attacker.ChangeAnimationState(FighterStats.AnimationNames.RUN);
        yield return StartCoroutine(MoveFighter(attacker, fighterInitialPosition, fighterDestinationPosition, movementSpeed));

        //CounterAttack
        if (IsCounterAttack(defender)) yield return StartCoroutine(PerformAttack(defender, attacker, attackerHealthbar));

        int attackCounter = 0;
        Debug.Log("ANTES DE WHILE GAME IS OVER O QUE");
        Debug.Log(gameIsOver);
        //Attack
        while (!gameIsOver && (attackCounter == 0 || IsAttackRepeated(attacker)))
        {
            Debug.Log(gameIsOver);
            Debug.Log(attackCounter == 0);
            Debug.Log(IsAttackRepeated(attacker));
            Debug.Log(attackCounter == 0 || IsAttackRepeated(attacker));
            yield return StartCoroutine(PerformAttack(attacker, defender, defenderHealthbar));
            attackCounter++;
        };

        if (!gameIsOver) defender.ChangeAnimationState(FighterStats.AnimationNames.IDLE);

        //Move back if game is not over or if winner is the attacker (the defender can win by a counter attack)
        if (!gameIsOver || getWinner() == attacker)
        {
            switchFighterOrientation(attacker, true);
            attacker.ChangeAnimationState(FighterStats.AnimationNames.RUN);
            yield return StartCoroutine(MoveFighter(attacker, fighterDestinationPosition, fighterInitialPosition, movementSpeed));
            switchFighterOrientation(attacker, false);
            attacker.ChangeAnimationState(FighterStats.AnimationNames.IDLE);
        }
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
        SetAttackerAndDefender(attacker, defender);
        attacker.ChangeAnimationState(FighterStats.AnimationNames.ATTACK);
        if (IsAttackDodged(defender))
        {
            //Wait for anim attack to reach player and then dodge
            yield return new WaitForSeconds(0.1f);
            defender.ChangeAnimationState(FighterStats.AnimationNames.JUMP);
            StartCoroutine(dodgeMovement(defender));
            //Wait for jump anim to finish
            yield return new WaitForSeconds(0.20f);
            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.1f);
            yield break;
        }

        InflictDamageToFighter(attacker, defender);
        gameIsOver = defender.hitPoints <= 0 ? true : false;
        Debug.Log("OVERRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
        Debug.Log(gameIsOver);
        if (gameIsOver)
        {
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            defender.ChangeAnimationState(FighterStats.AnimationNames.DEATH);
            healthbar.SetRemainingHealth(defender.hitPoints);
            combatCanvas.RenderDefeatSprite(f1, f2, defender);
            announceWinner();

            // update save file (exp, wr, abilities)
            // FIXME -- if condition swapped? + refactor this condition into methods
            if (getWinner() == f1)
            {
                manageSaves.UpdateDataFromCombat(1, 0);
                winnerConfetti2.gameObject.SetActive(true);
                winnerConfetti2.GetComponent<ParticleSystem>().Play();
            }
            else if (getWinner() == f2)
            {
                manageSaves.UpdateDataFromCombat(0, 1);
                winnerConfetti1.gameObject.SetActive(true);
                winnerConfetti1.GetComponent<ParticleSystem>().Play();
            }

            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            defender.ChangeAnimationState(FighterStats.AnimationNames.HURT);
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            healthbar.SetRemainingHealth(defender.hitPoints);
            //wait for hurt animation to finish
            yield return new WaitForSeconds(0.25f);
        }
    }

    private bool IsAttackRepeated(FighterStats attacker)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= attacker.speed ? true : false;
    }

    private bool IsAttackDodged(FighterStats defender)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= defender.agility ? true : false;
    }

    private bool IsCounterAttack(FighterStats defender)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= defender.counterRate ? true : false;
    }

    private void InflictDamageToFighter(FighterStats attacker, FighterStats defender)
    {
        int remainingLife = defender.hitPoints - attacker.damage;
        defender.hitPoints = remainingLife < 0 ? 0 : remainingLife;
    }

    private IEnumerator ReceiveDmgAnimation(FighterStats f)
    {
        Renderer figtherRenderer = f.GetComponent<Renderer>();
        figtherRenderer.material.color = new Color(255, 1, 1);
        yield return new WaitForSeconds(.1f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }

    private void announceWinner()
    {
        //WinnerBannerText.text = attackerName + " WINS THE COMBAT. " + defenderName + " GOT SMASHED!";
    }

    private IEnumerator dodgeMovement(FighterStats defender)
    {
        float dodgeSpeed = 0.2f;

        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = this.fighterDefendingThisTurn == defender ? defenderDodgeDestination.x -= 3 : defenderDodgeDestination.x += 3;
        defenderDodgeDestination.y += 2;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender, defender.transform.position, defenderDodgeDestination, dodgeSpeed));
        yield return StartCoroutine(MoveFighter(defender, defenderDodgeDestination, defenderInitialPosition, dodgeSpeed));
    }
    private void LoadRandomArena()
    {
        int indexOfArena = Random.Range(0, spriteArray.Length);
        arenaRenderer.sprite = spriteArray[indexOfArena];
    }

    private FighterStats getWinner()
    {
        return f1.hitPoints > 0 ? f1 : f2;
    }

}