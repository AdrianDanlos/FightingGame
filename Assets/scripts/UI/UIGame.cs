using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SavesManager savesManager;
    public Skills skills;

    [Header("UI")]
    //Arena render
    public SpriteRenderer arenaRenderer;
    public Sprite[] spriteArray;
    public CombatCanvas combatCanvas;
    public HealthBar oneHealthBar, twoHealthBar;
    public Text fighterOneNameBanner, fighterTwoNameBanner;
    public Text WinnerBannerText;
    public GameObject backToMenuButton;
    public GameObject winnerConfetti;
}
