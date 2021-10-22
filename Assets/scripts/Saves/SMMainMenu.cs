using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMMainMenu : MonoBehaviour
{
    [Header("Data")]
    public GameData gameData;

    [Header("UI")]
    public UIMainMenu uIMainMenu;

    private void Start()
    {
        if (SScene.scene == (int)SceneIndex.INITIAL_MENU || SScene.scene == (int)SceneIndex.GAME)
        {
            uIMainMenu.ShowData(gameData.xp, gameData.lv, gameData.hp, gameData.strength, gameData.agility,
                gameData.speed, gameData.skills, gameData.fighterName, gameData.wins, gameData.defeats);
        }
    }
}
