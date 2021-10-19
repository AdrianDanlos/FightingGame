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
    public FighterStats figherModel;
    public FighterStats f1, f2;
    string[] fighterNames = new string[2];

    [Header("UI")]
    public CombatCanvas combatCanvas;
    public HealthBar oneHealthBar, twoHealthBar;
    public Text fighter1Text;
    public Text fighter2Text;
    public Text WinnerBannerText;
    public GameObject backToMenuButton;
    public GameObject winnerConfetti;

    Vector2 fighterOneInitialPosition, fighterTwoInitialPosition;
    Vector2 fighterOneDestinationPosition, fighterTwoDestinationPosition;

    private float movementSpeed = 0.4f;
    private bool gameIsOver = false;

    // Player values and skills
    public Dictionary<string, int> playerFighterStats;
    public List<string> playerFighterSkills;

    // FIXME: These should be calculated/randomized depending on the players level
    // CPU values
    public Dictionary<string, int> cpuFighterStats =
    new Dictionary<string, int>
    {
        {"lv", 1},
        {"xp", 0},
        {"hitPoints", 2},
        {"strength", 3},
        {"agility", 20},
        {"speed", 20},
        {"counterRate", 0},
        {"reversalRate", 0},
        {"armor", 0},
        {"criticalRate", 0},
        {"sabotageRate", 0},
    };
    public List<string> cpuSkills;

    void Start()
    {
        // FIXME -- refactor the way this is loaded when we implemente online mode
        if (manageSaves.CheckIfFileExists())
        {
            playerFighterStats = manageSaves.LoadGameDataStats();
            playerFighterSkills = manageSaves.LoadGameDataSkills();
        }

        // set fighter names on the UI
        fighter1Text.text = figherModel.fighterName;
        fighter2Text.text = "Smasher";

        SetFighterSkills(f1, figherModel.skills);
        SetFighterSkills(f2, new List<string> {
            Skills.SkillsList.SIXTH_SENSE.ToString(),
            Skills.SkillsList.HOSTILITY.ToString(),
            Skills.SkillsList.TOUGHENED_SKIN.ToString(),
            Skills.SkillsList.ARMOR.ToString(),
            Skills.SkillsList.CRITICAL_ATTACK.ToString(),
            Skills.SkillsList.SABOTAGE.ToString(),
        });

        //FIXME ADRI: In the future receive a single object with all data where fighter name is included in object
        SetFighterStats(f1, playerFighterStats, fighterNames[0]);
        SetFighterStats(f2, cpuFighterStats, fighterNames[1]);

        // SetFighterStatsBasedOnSkills(f1);
        // SetFighterStatsBasedOnSkills(f2);

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

    public void SetFighterSkills(FighterStats fighter, List<string> skills)
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
        fighter.counterRate = data["counterRate"];
        fighter.reversalRate = data["reversalRate"];
        fighter.armor = data["armor"];
        fighter.criticalRate = data["criticalRate"];
        fighter.sabotageRate = data["sabotageRate"];
    }

    public void SetFighterStatsBasedOnSkills(FighterStats fighter, FighterStats opponent)
    {
        if (hasSkill(fighter, Skills.SkillsList.SIXTH_SENSE)) fighter.counterRate += 10;
        if (hasSkill(fighter, Skills.SkillsList.HOSTILITY)) fighter.reversalRate += 30;
        if (hasSkill(fighter, Skills.SkillsList.TOUGHENED_SKIN)) fighter.armor += 2;
        if (hasSkill(fighter, Skills.SkillsList.ARMOR)) fighter.armor += 5; fighter.speed -= fighter.speed / 10;
        if (hasSkill(fighter, Skills.SkillsList.CRITICAL_ATTACK)) fighter.criticalRate += 15;
        if (hasSkill(fighter, Skills.SkillsList.SABOTAGE)) fighter.sabotageRate += 15;

        //FIXME ADRI: If we apply percentages on the dmg of the opponent fighter later, the calculation will be affected (slightly)
        //Apply armor effects
        opponent.strength = opponent.strength - fighter.armor >= 1 ? opponent.strength - fighter.armor : 1;
    }

    public bool hasSkill(FighterStats fighter, Skills.SkillsList skill)
    {
        return fighter.skills.Contains(skill.ToString());
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