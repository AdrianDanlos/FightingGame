using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;
    public SMMainMenu sMMainMenu;
    private int level;

    [Header("UI")]
    [SerializeField] private UIMainMenu uIMainMenu;

    [Header("Fighter")]
    public Fighter fighter;

    [Header("Backgrounds")]
    [SerializeField] private Image[] zones;
    [SerializeField] private Image[] zoneFades;
    [SerializeField] private GameObject[] points;

    public void MoveFighterToZone()
    {
        level = sMMainMenu.LoadGameDataStats()["lv"];

        if (level >= 1 && level <= 5)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[0].transform.position;
            zoneFades[0].color = new Color(0, 0, 0, 0);
        }
        else if (level >= 6 && level <= 10)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[1].transform.position;
            zoneFades[1].color = new Color(0, 0, 0, 0);
        }
        else if (level >= 11 && level <= 15)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[2].transform.position;
            zoneFades[2].color = new Color(0, 0, 0, 0);
        }
        else if (level >= 16 && level <= 20)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[3].transform.position;
            zoneFades[3].color = new Color(0, 0, 0, 0);
        }
    }

}
