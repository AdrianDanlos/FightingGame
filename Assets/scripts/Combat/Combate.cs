using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Combate : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public ManageSaves manageSaves;
    public Skills skills;

    //Arena render
    [Header("Arena Render")]
    public SpriteRenderer arenaRenderer;
    public Sprite[] spriteArray;

    [Header("Fighters")]
    public FighterStats f1, f2;
    Dictionary<string, int> playerFighterStats = FightersTestData.playerFighterStats;
    List<string> playerFighterSkills = FightersTestData.playerFighterSkills;
    Dictionary<string, int> cpuFighterStats = FightersTestData.cpuFighterStats;
    List<string> cpuFighterSkills = FightersTestData.cpuFighterSkills;
    string cpuFighterName = FightersTestData.cpuFighterName;

    [Header("UI")]
    public CombatCanvas combatCanvas;
    public HealthBar oneHealthBar, twoHealthBar;
    public Text fighterOneNameBanner, fighterTwoNameBanner;
    public Text WinnerBannerText;
    public GameObject backToMenuButton;
    public GameObject winnerConfetti;

    // Game state and config
    private float movementSpeed = 0.4f;
    private bool gameIsOver = false;
    private float distanceBetweenFightersOnAttack = 3.5f;


    void Start()
    {
        //Uncomment this to test the combat with data from the save file
        //loadPlayerData();

        //Set properties on the fighters objects
        //FIXME ADRI: In the future receive a single object with all data where fighter name is included in object
        f1.SetFighterStats(playerFighterStats, manageSaves.LoadFighterName());
        f2.SetFighterStats(cpuFighterStats, cpuFighterName);

        f1.SetFighterSkills(playerFighterSkills);
        f2.SetFighterSkills(cpuFighterSkills);

        f1.SetFighterStatsBasedOnSkills(f2);
        f2.SetFighterStatsBasedOnSkills(f1);

        f1.SetFighterPositions(f2.transform.position, -distanceBetweenFightersOnAttack);
        f2.SetFighterPositions(f1.transform.position, +distanceBetweenFightersOnAttack);

        //Set UI
        SetFightersHealthBars();
        SetFighterNamesOnUI();
        LoadRandomArena();

        //Start combat
        StartCoroutine(InitiateCombat());
    }
    public void loadPlayerData()
    {
        if (manageSaves.CheckIfFileExists())
        {
            playerFighterStats = manageSaves.LoadGameDataStats();
            playerFighterSkills = manageSaves.LoadGameDataSkills();
        }
    }
    public void SetFightersHealthBars()
    {
        oneHealthBar.SetMaxHealth(f1.hitPoints);
        twoHealthBar.SetMaxHealth(f2.hitPoints);
    }
    public void SetFighterNamesOnUI()
    {
        fighterOneNameBanner.text = f1.fighterName;
        fighterTwoNameBanner.text = f2.fighterName;
    }

    IEnumerator InitiateCombat()
    {
        while (!gameIsOver)
        {
            //FIGHTER 1 ATTACKS
            yield return StartCoroutine(CombatLogicHandler(f1, f2, twoHealthBar, oneHealthBar));

            if (gameIsOver) break;

            //FIGHTER 2 ATTACKS
            yield return StartCoroutine(CombatLogicHandler(f2, f1, oneHealthBar, twoHealthBar));
        }
        getWinner().ChangeAnimationState(FighterStats.AnimationNames.IDLE_BLINK);
    }


    IEnumerator CombatLogicHandler(FighterStats attacker, FighterStats defender, HealthBar defenderHealthbar, HealthBar attackerHealthbar)
    {
        //Move forward
        attacker.ChangeAnimationState(FighterStats.AnimationNames.RUN);
        yield return StartCoroutine(MoveFighter(attacker, attacker.initialPosition, attacker.destinationPosition, movementSpeed));

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
            yield return StartCoroutine(MoveFighter(attacker, attacker.destinationPosition, attacker.initialPosition, movementSpeed));
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
        if (IsSabotageAttack(attacker) && defender.skills.Count > 0) defender.skills.RemoveAt(Random.Range(0, defender.skills.Count));

        gameIsOver = defender.hitPoints <= 0 ? true : false;
        if (gameIsOver)
        {
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            defender.ChangeAnimationState(FighterStats.AnimationNames.DEATH);
            healthbar.SetRemainingHealth(defender.hitPoints);
            combatCanvas.RenderDefeatSprite(f1, getWinner());

            // UI effects
            announceWinner();
            backToMenuButton.SetActive(true);
            if (getWinner() == f2) winnerConfetti.gameObject.transform.position += new Vector3(+16, 0, 0);
            winnerConfetti.gameObject.SetActive(true);
            winnerConfetti.GetComponent<ParticleSystem>().Play();

            // Save combat data
            manageSaves.UpdateDataFromCombat(getWinner() == f1);

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
        return IsHappening(attacker.speed);
    }

    private bool IsAttackDodged(FighterStats defender)
    {
        return IsHappening(defender.agility);
    }

    private bool IsCounterAttack(FighterStats defender)
    {
        return IsHappening(defender.counterRate);
    }

    private bool IsReversalAttack(FighterStats defender)
    {
        return IsHappening(defender.reversalRate);
    }
    private bool IsCriticalAttack(FighterStats attacker)
    {
        return IsHappening(attacker.criticalRate);
    }
    private bool IsSabotageAttack(FighterStats attacker)
    {
        return IsHappening(attacker.sabotageRate);
    }

    private bool IsHappening(int fighterStatValue)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= fighterStatValue ? true : false;
    }

    private void InflictDamageToFighter(FighterStats attacker, FighterStats defender)
    {
        int attackDamage = IsCriticalAttack(attacker) ? attacker.strength * 2 : attacker.strength;
        int remainingLife = defender.hitPoints - attackDamage;
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

        Vector2 defenderDodgeDestination = defender.transform.position;

        defenderDodgeDestination.x = f1 == defender ? defenderDodgeDestination.x -= 2 : defenderDodgeDestination.x += 2;
        defenderDodgeDestination.y += 2;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender, defender.initialPosition, defenderDodgeDestination, dodgeSpeed));
        yield return StartCoroutine(MoveFighter(defender, defenderDodgeDestination, defender.initialPosition, dodgeSpeed));
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