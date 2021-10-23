using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SMCore sMCore;
    public SMGame sMGame;
    public Skills skills;

    [Header("Fighters")]
    public Fighter player, cpu;
    Dictionary<string, int> playerFighterStats;
    List<string> playerFighterSkills;
    Dictionary<string, int> cpuFighterStats;
    List<string> cpuFighterSkills;
    int playerTotalHitPoints;
    int cpuTotalHitPoints;

    [Header("UI")]
    public UIGame uIGame;

    [Header("Game state and config")]
    [SerializeField] private float movementSpeed = 0.4f;
    [SerializeField] private bool gameIsOver = false;
    [SerializeField] private float distanceBetweenFightersOnAttack = 3.5f;
    //FIXME: Is it possible to get this value automatically from the canvas?
    float screenEdgeX = 22;


    void Start()
    {
        //Uncomment this to test the combat with data from the save file
        //loadPlayerData();
        setTestData();

        //Set properties on the fighters objects
        player.SetFighterStats(playerFighterStats, sMGame.LoadFighterName());
        cpu.SetFighterStats(cpuFighterStats, RandomNamesGenerator.generateRandomName());

        player.SetFighterSkills(playerFighterSkills);
        cpu.SetFighterSkills(cpuFighterSkills);

        player.SetFighterStatsBasedOnSkills(cpu);
        cpu.SetFighterStatsBasedOnSkills(player);

        player.SetFighterPositions(cpu.transform.position, -distanceBetweenFightersOnAttack);
        cpu.SetFighterPositions(player.transform.position, +distanceBetweenFightersOnAttack);

        //Set UI
        SetFightersHealthBars();
        SetFighterNamesOnUI();
        uIGame.LoadRandomArena();

        //Set global variables
        playerTotalHitPoints = player.hitPoints;
        cpuTotalHitPoints = cpu.hitPoints;

        //Start combat
        StartCoroutine(InitiateCombat());
    }
    public void setTestData()
    {
        playerFighterStats = FightersTestData.playerFighterStats;
        playerFighterSkills = FightersTestData.playerFighterSkills;
        cpuFighterStats = FightersTestData.cpuFighterStats;
        cpuFighterSkills = FightersTestData.cpuFighterSkills;
    }
    public void loadPlayerData()
    {
        if (sMCore.CheckIfFileExists())
        {
            playerFighterStats = sMGame.LoadGameDataStats();
            playerFighterSkills = sMGame.LoadGameDataSkills();
        }
    }
    public void SetFightersHealthBars()
    {
        uIGame.oneHealthBar.SetMaxHealth(player.hitPoints);
        uIGame.twoHealthBar.SetMaxHealth(cpu.hitPoints);
    }
    public void SetFighterNamesOnUI()
    {
        uIGame.SetFighterNamesOnUI(player.fighterName, cpu.fighterName);
    }


    IEnumerator InitiateCombat()
    {
        Dictionary<string, object> combatData = getDataForCombatStartingOrder();

        while (!gameIsOver)
        {
            yield return StartCoroutine(CombatLogicHandler((Fighter)combatData["firstAttacker"],
                (Fighter)combatData["secondAttacker"],
                (HealthBar)combatData["secondAttackerHeathBar"],
                (HealthBar)combatData["firstAttackerHeathBar"]));

            if (gameIsOver) break;

            yield return StartCoroutine(CombatLogicHandler((Fighter)combatData["secondAttacker"],
                (Fighter)combatData["firstAttacker"],
                (HealthBar)combatData["firstAttackerHeathBar"],
                (HealthBar)combatData["secondAttackerHeathBar"]));
        }
        getWinner().ChangeAnimationState(Fighter.AnimationNames.IDLE_BLINK);
    }

    IEnumerator CombatLogicHandler(Fighter attacker, Fighter defender, HealthBar defenderHealthbar, HealthBar attackerHealthbar)
    {
        //Move forward
        attacker.ChangeAnimationState(Fighter.AnimationNames.RUN);
        yield return StartCoroutine(MoveFighter(attacker, attacker.transform.position, attacker.destinationPosition, movementSpeed));

        //CounterAttack
        if (IsCounterAttack(defender)) yield return StartCoroutine(PerformAttack(defender, attacker, attackerHealthbar));

        int attackCounter = 0;

        //Attack
        while (!gameIsOver && (attackCounter == 0 || IsAttackRepeated(attacker)))
        {
            bool fighterShouldAdvanceToAttack = !isAtMeleeRange() && hasSpaceToKeepPushing(player == attacker, attacker.transform.position.x);

            if (fighterShouldAdvanceToAttack)
            {
                Vector2 newDestinationPosition = attacker.transform.position;
                newDestinationPosition.x += getBackwardMovement(player == defender);

                attacker.ChangeAnimationState(Fighter.AnimationNames.RUN);
                yield return StartCoroutine(MoveFighter(attacker, attacker.transform.position, newDestinationPosition, movementSpeed * 0.05f));
            }

            yield return StartCoroutine(PerformAttack(attacker, defender, defenderHealthbar));

            attackCounter++;
        };

        //ReversalAttack
        if (!gameIsOver)
        {
            if (IsReversalAttack(defender)) yield return StartCoroutine(PerformAttack(defender, attacker, attackerHealthbar));
            defender.ChangeAnimationState(Fighter.AnimationNames.IDLE);
        }

        //Move back if game is not over or if winner is the attacker (the defender can win by a counter attack)
        if (!gameIsOver || getWinner() == attacker)
        {
            switchFighterOrientation(attacker, true);
            attacker.ChangeAnimationState(Fighter.AnimationNames.RUN);
            yield return StartCoroutine(MoveFighter(attacker, attacker.transform.position, attacker.initialPosition, movementSpeed));
            switchFighterOrientation(attacker, false);
            attacker.ChangeAnimationState(Fighter.AnimationNames.IDLE);
        }
    }

    IEnumerator MoveFighter(Fighter fighter, Vector2 startPos, Vector2 endPos, float time)
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

    private void switchFighterOrientation(Fighter attacker, bool reverseOrentation)
    {
        var spriteRenderer = attacker.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = reverseOrentation;
    }

    IEnumerator PerformAttack(Fighter attacker, Fighter defender, HealthBar healthbar)
    {
        attacker.ChangeAnimationState(Fighter.AnimationNames.ATTACK);
        bool isBalletShoesActivated = !attacker.hasAttackedThisCombat && defender.hasSkill(SkillsList.BALLET_SHOES);
        attacker.hasAttackedThisCombat = true;

        if (IsAttackDodged(defender) || isBalletShoesActivated)
        {
            //Wait for anim attack to reach player and then dodge
            yield return new WaitForSeconds(0.1f);
            defender.ChangeAnimationState(Fighter.AnimationNames.JUMP);
            StartCoroutine(dodgeMovement(defender));
            //Wait for jump anim to finish
            yield return new WaitForSeconds(0.15f);
            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.2f);

            if (attacker.hasSkill(SkillsList.DETERMINATION) && IsDeterminationAttack(attacker)) PerformAttack(attacker, defender, healthbar);
            else yield break;
        }

        InflictDamageToFighter(attacker, defender);
        if (IsSabotageAttack(attacker) && defender.skills.Count > 0) defender.skills.RemoveAt(Random.Range(0, defender.skills.Count));

        gameIsOver = defender.hitPoints <= 0 ? true : false;
        if (gameIsOver)
        {
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            defender.ChangeAnimationState(Fighter.AnimationNames.DEATH);
            healthbar.SetRemainingHealth(defender.hitPoints);
            uIGame.combatCanvas.RenderDefeatSprite(player, getWinner());

            // UI effects
            announceWinner();
            uIGame.SetActiveBackToMenuButton(true);
            if (getWinner() == cpu) uIGame.SetActiveWinnerConfetti(true);

            // Save combat data
            sMGame.UpdateDataFromCombat(getWinner() == player);

            //Wait for attack anim to finish
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            defender.ChangeAnimationState(Fighter.AnimationNames.HURT);
            //wait to sync attack with red character animation
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(ReceiveDmgAnimation(defender));
            healthbar.SetRemainingHealth(defender.hitPoints);
            //wait for hurt animation to finish
            yield return new WaitForSeconds(0.25f);
        }
    }

    private bool IsAttackRepeated(Fighter attacker)
    {
        return IsHappening(attacker.speed);
    }
    private bool IsAttackDodged(Fighter defender)
    {
        return IsHappening(defender.agility);
    }
    private bool IsCounterAttack(Fighter defender)
    {
        return IsHappening(defender.counterRate);
    }
    private bool IsReversalAttack(Fighter defender)
    {
        return IsHappening(defender.reversalRate);
    }
    private bool IsCriticalAttack(Fighter attacker)
    {
        return IsHappening(attacker.criticalRate);
    }
    private bool IsSabotageAttack(Fighter attacker)
    {
        return IsHappening(attacker.sabotageRate);
    }
    private bool IsSurvival(Fighter defender, int hpAfterHit)
    {
        return defender.hasSkill(SkillsList.SURVIVAL) && defender.hitPoints > 1 && hpAfterHit <= 0;
    }
    private bool IsDeterminationAttack(Fighter attacker)
    {
        return attacker.hasSkill(SkillsList.DETERMINATION) && IsHappening(60);
    }
    private bool IsResistant(Fighter defender, int attackDamage)
    {
        if (defender.hasSkill(SkillsList.RESISTANT))
        {
            int defenderTotalHitpoints = defender == GameObject.Find("FighterOne").GetComponent<Fighter>() ? playerTotalHitPoints : cpuTotalHitPoints;
            double twentyPercentOfMaxHealth = defenderTotalHitpoints * 0.2;
            return attackDamage > twentyPercentOfMaxHealth;
        }
        return false;
    }

    private bool IsHappening(int fighterStatValue)
    {
        int randomNumber = Random.Range(0, 100) + 1;
        return randomNumber <= fighterStatValue ? true : false;
    }

    private void InflictDamageToFighter(Fighter attacker, Fighter defender)
    {
        bool damageModifiersApplied = false;
        int attackDamage = IsCriticalAttack(attacker) ? attacker.strength * 2 : attacker.strength;
        int hpAfterHit = defender.hitPoints - attackDamage;

        if (IsResistant(defender, attackDamage))
        {
            defender.hitPoints -= System.Convert.ToInt32(getDefenderTotalHitPoints(defender) * 0.2);
            hpAfterHit = defender.hitPoints;
            damageModifiersApplied = true;
        }
        if (IsSurvival(defender, hpAfterHit))
        {
            defender.hitPoints = 1;
            damageModifiersApplied = true;
        }
        if (!damageModifiersApplied) defender.hitPoints = hpAfterHit < 0 ? 0 : hpAfterHit;
    }

    private int getDefenderTotalHitPoints(Fighter defender)
    {
        return defender == GameObject.Find("FighterOne").GetComponent<Fighter>() ? playerTotalHitPoints : cpuTotalHitPoints;
    }

    private IEnumerator ReceiveDmgAnimation(Fighter f)
    {
        Renderer figtherRenderer = f.GetComponent<Renderer>();
        figtherRenderer.material.color = new Color(255, 1, 1);
        yield return new WaitForSeconds(.1f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }

    private void announceWinner()
    {
        uIGame.ShowWinnerText(getWinner().fighterName, getLoser().fighterName);
    }

    private IEnumerator dodgeMovement(Fighter defender)
    {
        float dodgeSpeed = 0.15f;

        //This initial position might be at the back if we are defending or at the front if we are attacking and the fighter got hit by a counter or reversal attack
        Vector2 defenderInitialPosition = defender.transform.position;
        Vector2 defenderMaxHeightInAirPosition = defender.transform.position;
        Vector2 defenderLandingPosition = defender.transform.position;

        bool isPlayerDodging = player == defender;
        int backwardMovement = getBackwardMovement(isPlayerDodging);
        int distanceFromJumpToMaxHeight = backwardMovement / 2;

        if (!isFighterInTheEdgeOfScreen(isPlayerDodging, defender.transform.position.x))
        {
            defenderMaxHeightInAirPosition.x += distanceFromJumpToMaxHeight;
            defenderLandingPosition.x += backwardMovement;
        }
        defenderMaxHeightInAirPosition.y += 1;

        //Dodge animation
        yield return StartCoroutine(MoveFighter(defender, defenderInitialPosition, defenderMaxHeightInAirPosition, dodgeSpeed));
        yield return StartCoroutine(MoveFighter(defender, defenderMaxHeightInAirPosition, defenderLandingPosition, dodgeSpeed));
    }

    private int getBackwardMovement(bool isPlayerDodging)
    {
        return isPlayerDodging ? -2 : 2;
    }

    private bool isFighterInTheEdgeOfScreen(bool isPlayerDodging, float defenderXPosition)
    {
        return isPlayerDodging && defenderXPosition <= -screenEdgeX || !isPlayerDodging && defenderXPosition >= screenEdgeX;
    }

    private bool hasSpaceToKeepPushing(bool isPlayerAttacking, float attackerXPosition)
    {
        return isPlayerAttacking && attackerXPosition <= screenEdgeX - distanceBetweenFightersOnAttack || !isPlayerAttacking && attackerXPosition >= -screenEdgeX + distanceBetweenFightersOnAttack;
    }

    private Fighter getWinner()
    {
        return player.hitPoints > 0 ? player : cpu;
    }

    private Fighter getLoser()
    {
        return cpu.hitPoints > 0 ? player : cpu;
    }

    private Dictionary<string, object> getDataForCombatStartingOrder()
    {
        Dictionary<string, object> combatData =
        new Dictionary<string, object>
        {
            {"firstAttacker", cpu},
            {"secondAttacker", player},
            {"firstAttackerHeathBar", uIGame.twoHealthBar},
            {"secondAttackerHeathBar", uIGame.oneHealthBar},
        };

        bool playerStarts = Random.Range(0, 2) == 0;

        if (player.hasSkill(SkillsList.FIRST_STRIKE) || playerStarts)
        {
            combatData["firstAttacker"] = player;
            combatData["secondAttacker"] = cpu;
            combatData["firstAttackerHeathBar"] = uIGame.oneHealthBar;
            combatData["secondAttackerHeathBar"] = uIGame.twoHealthBar;
        }

        return combatData;
    }

    private bool isAtMeleeRange()
    {
        return System.Math.Abs(player.transform.position.x - cpu.transform.position.x) <= distanceBetweenFightersOnAttack;
    }

}