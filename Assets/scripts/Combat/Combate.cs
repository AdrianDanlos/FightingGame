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
    string[] fighterNames = new string[2];

    public HealthBar oneHealthBar, twoHealthBar;
    public Text WinnerBannerText;

    Vector2 fighterOneInitialPosition, fighterTwoInitialPosition;
    Vector2 fighterOneDestinationPosition, fighterTwoDestinationPosition;

    float movementSpeed = 0.4f;
    bool gameIsOver = false;

    public CombatCanvas combatCanvas;
    public GameObject menuButton;

    // FIXME: Try to reuse confetti with different X position?
    public GameObject winnerConfetti1, winnerConfetti2;

    public Text fighter1Text;
    public Text fighter2Text;

    // Player values
    public Dictionary<string, int> playerFighterStats;

    // FIXME: These should be calculated/randomized depending on the players level
    // CPU values
    public Dictionary<string, int> cpuFighterStats =
    new Dictionary<string, int>
    {
        {"lv", 1},
        {"xp", 0},
        {"hitPoints", 2},
        {"strength", 3},
        {"agility", 2},
        {"speed", 2},
        {"counterRate", 1},
        {"reversalRate", 1},
        {"armor", 0},
    };

    void Start()
    {
        // load data from save
        // set initial values for player
        // FIXME -- refactor the way this is loaded when we implemente online mode
        if (manageSaves.CheckIfFileExists())
        {
            playerFighterStats = manageSaves.LoadGameData();
        }

        // set fighter names and UI
        fighterNames[0] = manageSaves.GetFighterName();
        fighterNames[1] = "Smasher";
        fighter1Text.text = fighterNames[0];
        fighter2Text.text = fighterNames[1];

        //FIXME: Player(f1) skills should come from save file as an array of int
        SetFighterSkills(f1, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });
        SetFighterSkills(f2, new string[] { Skills.SkillsList.SIXTHSENSE.ToString() });

        //FIXME: In the future receive a single object with all data where fighter name is included in object
        SetFighterStats(f1, playerFighterStats, fighterNames[0]);
        SetFighterStats(f2, cpuFighterStats, fighterNames[1]);

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

    public void SetFighterStats(FighterStats fighter, Dictionary<string, int> data, string fighterName)
    {
        fighter.fighterName = fighterName;
        fighter.hitPoints = data["hitPoints"];
        fighter.strength = data["strength"];
        fighter.agility = data["agility"];
        fighter.speed = data["speed"];
        //fighter.counterRate = data["counterRate"];
        //fighter.reversalRate = data["reversalRate"];
        //fighter.armor = data["armor"];
    }

    public void SetFighterStatsBasedOnSkills(FighterStats fighter)
    {
        if (fighter.skills.Contains(Skills.SkillsList.SIXTHSENSE.ToString())) fighter.counterRate += 10;
        if (fighter.skills.Contains(Skills.SkillsList.HOSTILITY.ToString())) fighter.reversalRate += 30;
        //FIXME FINISH THIS, GIVE THE OTHER FIGHTER LESS ATTACK
        //if (fighter.skills.Contains(Skills.SkillsList.TOUGHENEDSKIN.ToString())) fighter.reversalRate += 30;
    }

    IEnumerator InitiateCombat()
    {
        while (!gameIsOver)
        {
            //FIGHTER 1 ATTACKS
            yield return StartCoroutine(CombatLogicHandler(f1, f2, fighterOneInitialPosition, fighterOneDestinationPosition, twoHealthBar, oneHealthBar));

            if (gameIsOver) break;

            //FIGHTER 2 ATTACKS
            yield return StartCoroutine(CombatLogicHandler(f2, f1, fighterTwoInitialPosition, fighterTwoDestinationPosition, oneHealthBar, twoHealthBar));
        }
        getWinner().ChangeAnimationState(FighterStats.AnimationNames.IDLE_BLINK);
    }


    IEnumerator CombatLogicHandler(FighterStats attacker, FighterStats defender, Vector2 fighterInitialPosition, Vector2 fighterDestinationPosition, HealthBar defenderHealthbar, HealthBar attackerHealthbar)
    {
        //Move forward
        attacker.ChangeAnimationState(FighterStats.AnimationNames.RUN);
        yield return StartCoroutine(MoveFighter(attacker, fighterInitialPosition, fighterDestinationPosition, movementSpeed));

        //CounterAttack
        if (IsCounterAttack(defender)) yield return StartCoroutine(PerformAttack(defender, attacker, attackerHealthbar));

        int attackCounter = 0;

        //Attack
        while (!gameIsOver && (attackCounter == 0 || IsAttackRepeated(attacker)))
        {
            yield return StartCoroutine(PerformAttack(attacker, defender, defenderHealthbar));
            attackCounter++;
        };

        //ReversalAttack
        if (IsReversalAttack(defender)) yield return StartCoroutine(PerformAttack(defender, attacker, attackerHealthbar));

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
        if (gameIsOver)
        {
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            defender.ChangeAnimationState(FighterStats.AnimationNames.DEATH);
            healthbar.SetRemainingHealth(defender.hitPoints);
            combatCanvas.RenderDefeatSprite(f1, getWinner());

            // announce winner + enable UI
            announceWinner();
            menuButton.SetActive(true);

            // FIXME -- refactor this condition into methods
            if (getWinner() == f1)
            {
                manageSaves.UpdateDataFromCombat(true);
                winnerConfetti2.gameObject.SetActive(true);
                winnerConfetti2.GetComponent<ParticleSystem>().Play();
            }
            else if (getWinner() == f2)
            {
                manageSaves.UpdateDataFromCombat(false);
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
    //FIXME: Make only 1 probability function?
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

    private bool IsReversalAttack(FighterStats defender)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= defender.reversalRate ? true : false;
    }

    private void InflictDamageToFighter(FighterStats attacker, FighterStats defender)
    {
        int remainingLife = defender.hitPoints - attacker.strength;
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
        WinnerBannerText.text = getWinner().fighterName + " WINS THE COMBAT!\n" + getLoser().fighterName + " GOT SMASHED!";
    }

    private IEnumerator dodgeMovement(FighterStats defender)
    {
        float dodgeSpeed = 0.2f;

        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = f1 == defender ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
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

    private FighterStats getLoser()
    {
        return f2.hitPoints > 0 ? f1 : f2;
    }

}