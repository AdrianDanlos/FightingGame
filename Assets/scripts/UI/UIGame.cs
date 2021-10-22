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

    public void ShowLevelUpMenu()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (SScene.levelUp)
            {
                levelUpMenu.SetActive(true);
                fighterUI1.SetActive(false);
                fighterUI2.SetActive(false);
            }
            else if (!SScene.levelUp)
            {
                gameScene.LoadMainMenu();
            }
        }
    }
}
