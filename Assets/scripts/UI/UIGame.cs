using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SMGame sMGame;
    public Skills skills;

    [Header("Scene")]
    public GameScene gameScene;

    [Header("Game UI")]
    //Arena render
    public SpriteRenderer arenaRenderer;
    public Sprite[] spriteArray;
    public CombatCanvas combatCanvas;
    public HealthBar oneHealthBar, twoHealthBar;
    public Text fighterOneNameBanner, fighterTwoNameBanner;
    public Text WinnerBannerText;
    public GameObject backToMenuButton;
    public GameObject winnerConfetti;

    [Header("Level Up UI")]
    [SerializeField] private GameObject levelUpMenu;
    [SerializeField] public Sprite[] iconsArray;
    [SerializeField] private GameObject fighterUI1;
    [SerializeField] private GameObject fighterUI2;
    [SerializeField] private GameObject backToMenu;

    [Header("LevelUp UI Option 1")]
    [SerializeField] private Button lvUpOption1Button;
    [SerializeField] private Text lvUp1Title;
    [SerializeField] private Text lvUp1Description;
    [SerializeField] private Image lvUp1Image;

    [Header("LevelUp UI Option 2")]
    [SerializeField] private Button lvUpOption2Button;
    [SerializeField] private Text lvUp2Title;
    [SerializeField] private Text lvUp2Description;
    [SerializeField] private Image lvUp2Image;

    public void LoadNextMenu()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (gameScene.GetLevelUpState())
            {
                DisplayLevelUpMenu();
            }
            else if (!gameScene.GetLevelUpState())
            {
                gameScene.LoadMainMenu();
            }
        }
    }

    private void DisplayLevelUpMenu()
    {
        levelUpMenu.SetActive(true);
        fighterUI1.SetActive(false);
        fighterUI2.SetActive(false);
    }

    public void SetLevelUpMenuValues(Dictionary<string, string> skillData1, Dictionary<string, string> skillData2)
    {
        // set UI choice 1
        lvUp1Title.text = skillData1["Title"];
        lvUp1Title.color = skills.GetColorFromRarity(skillData1["Rarity"]);
        lvUp1Description.text = skillData1["Description"];
        string skillData1Name = "icons_" + skillData1["Icon"];
        for (int i = 0; i < iconsArray.Length; i++)
        {
            if (string.Equals(skillData1Name, iconsArray[i].name)) lvUp1Image.sprite = iconsArray[i];
        }

        // set UI choice 2
        lvUp2Title.text = skillData2["Title"];
        lvUp2Title.color = skills.GetColorFromRarity(skillData2["Rarity"]);
        lvUp2Description.text = skillData2["Description"];
        string skillData2Name = "icons_" + skillData2["Icon"];
        for (int i = 0; i < iconsArray.Length; i++)
        {
            if (string.Equals(skillData2Name, iconsArray[i].name)) lvUp2Image.sprite = iconsArray[i];
        }
    }

    public void AddListenersToLvUpButtons(List<string> twoSkills)
    {
        lvUpOption1Button.onClick.AddListener(delegate { sMGame.CheckSkillAndAdd(twoSkills[0]); });
        lvUpOption2Button.onClick.AddListener(delegate { sMGame.CheckSkillAndAdd(twoSkills[1]); });
    }

    public void LoadRandomArena()
    {
        int indexOfArena = Random.Range(0, spriteArray.Length);
        arenaRenderer.sprite = spriteArray[indexOfArena];
    }

    public void SetFighterNamesOnUI(string fighterName1, string fighterName2)
    {
        fighterOneNameBanner.text = fighterName1;
        fighterTwoNameBanner.text = fighterName2;
    }

    public void SetActiveBackToMenuButton(bool isActive)
    {
        backToMenuButton.SetActive(true);
    }

    public void SetActiveWinnerConfetti(bool isActive)
    {
        winnerConfetti.gameObject.SetActive(true);
        winnerConfetti.GetComponent<ParticleSystem>().Play();
        winnerConfetti.gameObject.transform.position += new Vector3(+16, 0, 0);
    }

    public void ShowWinnerText(string winner, string loser)
    {
        WinnerBannerText.text = winner + " WINS THE COMBAT!\n" + loser + " GOT SMASHED!";
    }
}
