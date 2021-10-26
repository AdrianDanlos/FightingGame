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
    [SerializeField] private Text zoneTitle;

    [Header("Fighter")]
    public Fighter fighter;

    [Header("Backgrounds")]
    [SerializeField] private Image[] zones; 
    [SerializeField] private Image[] zoneFades;
    [SerializeField] private GameObject[] points;

    public void MoveFighterToZone()
    {
        level = sMMainMenu.LoadGameDataStats()["lv"];

        if (level <= 10)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[0].transform.position;
            zoneFades[0].color = new Color(0, 0, 0, 0);
            zoneTitle.text = "SUNRISE FOREST";
        }
        else if (level >= 11 && level <= 20)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[1].transform.position;
            zoneFades[1].color = new Color(0, 0, 0, 0);
            zoneTitle.text = "HIGH SKIES";
        }
        else if (level >= 21 && level <= 30)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[2].transform.position;
            zoneFades[2].color = new Color(0, 0, 0, 0);
            zoneTitle.text = "WASTELAND";
        }
        else if (level >= 31 && level <= 40)
        {
            fighter.ChangeAnimationState(Fighter.AnimationNames.RUN);
            fighter.transform.position = points[3].transform.position;
            zoneFades[3].color = new Color(0, 0, 0, 0);
            zoneTitle.text = "RUINED CASTLE";
        }
    }

}
