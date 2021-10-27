using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;
    public SMGame sMGame;
    public Skills skills;

    [Header("Scene")]
    public GameScene gameScene;

    [Header("Game UI")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private SpriteRenderer arenaRenderer;
    [SerializeField] private Sprite[] spriteArray;
    public HealthBar oneHealthBar, twoHealthBar;
    [SerializeField] private Text fighterOneNameBanner, fighterTwoNameBanner;
    [SerializeField] private Text WinnerBannerText;
    [SerializeField] private GameObject FightStartBanner;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private GameObject winnerConfetti;
    public GameObject[] fighter1Skills;
    public GameObject[] fighter2Skills;
    public GameObject UIFade;

    [Header("Fighters")]
    [SerializeField] private Image portrait1;
    [SerializeField] private Image portrait2;
    [SerializeField] private Sprite[] portraistList;
    [SerializeField] private GameObject defeatCross;
    public FighterCombat fighterCombat;
    public FighterCombat fighter1;
    public FighterCombat fighter2;

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

    // Need this to load as a coroutine in order to get the data from the fighter
    private IEnumerator Start()
    {
        yield return null; // waits 1 frame

        SetPortraitImages();

        if(fighter1.GetFighterSkills().Count > 11)
        {
            RectTransform rt = UIFade.GetComponent<RectTransform>();
            rt.sizeDelta += new Vector2(0, 80f);
            oneHealthBar.transform.position += new Vector3(0, -80f, 0);
        }
        if (fighter1.GetFighterSkills().Count > 11)
        {
            RectTransform rt = UIFade.GetComponent<RectTransform>();
            rt.sizeDelta += new Vector2(0, 80f);
            twoHealthBar.transform.position += new Vector3(0, -80f, 0);
        }

        DisplaySkillIcons(fighter1.GetFighterSkills(), fighter1Skills, true);
        DisplaySkillIcons(fighter2.GetFighterSkills(), fighter2Skills, false);
    }

    private void DisplaySkillIcons(List<string> fighterSkillsNames, GameObject[] fighterSkills, bool isPlayer)
    {
        Dictionary<string, Dictionary<string, string>> fighterSkillsData =
        GetDataOfFighterSkills(fighterSkillsNames);

        string skillDataIconNumber = "";
        int i = 0;

        fighterSkillsNames.ForEach(delegate (string skill)
        {
            skillDataIconNumber = "icons_" + fighterSkillsData[skill]["Icon"];

            for (int j = 0; j < iconsArray.Length; j++)
            {
                if (string.Equals(skillDataIconNumber, iconsArray[j].name))
                {
                    fighterSkills[i].GetComponent<Image>().sprite = iconsArray[j];
                    fighterSkills[i].GetComponent<Image>().color = GetColorOfFighter(isPlayer);
                }
            }
            fighterSkills[i].SetActive(true);
            i++;
        });
    }

    private Dictionary<string, Dictionary<string, string>>
    GetDataOfFighterSkills(List<string> fighterSkillsNames)
    {
        Dictionary<string, Dictionary<string, string>> fighterSkillsData =
            new Dictionary<string, Dictionary<string, string>>();

        for (int i = 0; i < fighterSkillsNames.Count; i++)
        {
            fighterSkillsData.Add(
                fighterSkillsNames[i], skills.GetSkillDataFromSkillName(fighterSkillsNames[i]));
        }

        return fighterSkillsData;
    }

    public Color GetColorOfFighter(bool isPlayer)
    {
        Color playerColor = new Color(0, 230, 255, 255);
        Color cpuColor = new Color(255, 0, 0, 255);

        return isPlayer ? playerColor : cpuColor;
    }

    public void SetPortraitImages()
    {
        string[] skinList = fighterCombat.GetFighterSkinArray();

        for (int i = 0; i < portraistList.Length; i++)
        {
            if (skinList[0].Equals(portraistList[i].name))
                portrait1.GetComponent<Image>().sprite = portraistList[i];

            if (skinList[1].Equals(portraistList[i].name))
                portrait2.GetComponent<Image>().sprite = portraistList[i];
        }

        // flips the portrait
        var fighter2Portrait = portrait2.GetComponent<Image>();
        fighter2Portrait.transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    // Fixme
    public void RenderDefeatSprite(FighterCombat player, FighterCombat winner)
    {
        GameObject defeatCrossClone = Instantiate(defeatCross);

        if (winner == player) defeatCrossClone.transform.SetParent(portrait2.transform, false);
        else defeatCrossClone.transform.SetParent(portrait1.transform, false); ;
    }

    public IEnumerator LoadNextMenu()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (gameScene.GetLevelUpState())
            {
                yield return new WaitForEndOfFrame();
                DisplayLevelUpMenu();
            }
            else 
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(gameScene.LoadMainMenu());
            }
        }
    }

    public void WrapperLoadNextMenu()
    {
        StartCoroutine(LoadNextMenu());
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
        lvUpOption1Button.onClick.AddListener(delegate { sMGame.WrapperCheckSkillAndAdd(twoSkills[0]); });
        lvUpOption2Button.onClick.AddListener(delegate { sMGame.WrapperCheckSkillAndAdd(twoSkills[1]); });
    }

    public void LoadRandomArena()
    {
        int indexOfArena = Random.Range(0, spriteArray.Length);
        arenaRenderer.sprite = spriteArray[indexOfArena];
    }

    public void LoadArenaBasedOnLevel()
    {
        int level = sMGame.LoadPlayerLevel();

        if(level <= 10)
        {
            arenaRenderer.sprite = spriteArray[0];
        }
        else if (level >= 11 && level <= 20)
        {
            arenaRenderer.sprite = spriteArray[1];
        }
        else if (level >= 21 && level <= 30)
        {
            arenaRenderer.sprite = spriteArray[2];
        }
        else if (level >= 31 && level <= 40)
        {
            arenaRenderer.sprite = spriteArray[3];
        }
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

    public void SetActiveWinnerConfetti(string winner)
    {
        Vector3 playerPosition = new Vector3(1f, 0, 0);
        Vector3 cpuPosition = new Vector3(16f, 0, 0);
        GameObject winnerConfettiObject = Instantiate(winnerConfetti);

        if (winner.Equals("player"))
            winnerConfettiObject.transform.position += playerPosition;
        else
            winnerConfettiObject.transform.position += cpuPosition;
        winnerConfettiObject.gameObject.SetActive(true);
        winnerConfettiObject.GetComponent<ParticleSystem>().Play();
    }

    public void ShowWinnerText(string winner, string loser)
    {
        WinnerBannerText.text = winner + " WINS THE COMBAT!\n" + loser + " GOT SMASHED!";
    }

    public IEnumerator ShowStartFightBanner()
    {
        GameObject fightStartBannerObject = Instantiate(FightStartBanner);
        fightStartBannerObject.transform.SetParent(mainCanvas.transform, false);

        yield return new WaitForSeconds(2f);
        Object.Destroy(fightStartBannerObject);
    }
}
