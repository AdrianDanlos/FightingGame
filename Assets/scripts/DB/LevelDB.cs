using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDB : MonoBehaviour
{
    // xp = (level/x)^y
    // FIXME -- get lv and xp increment through a formula
    // LV 1 - 5 > 4 XP PER LV
    // LV 6 - 10 > 5 XP PER LV
    // LV 10 - 15 > 6 XP PER LV
    Dictionary<int, int> levelDB =
        new Dictionary<int, int>
        {
            {1, 4},
            {2, 8},
            {3, 12},
            {4, 16},
            {5, 21},
            {6, 26},
            {7, 31},
            {8, 36},
            {9, 41},
            {10, 47},
            {11, 53},
            {12, 59},
            {13, 65},
            {14, 71},
            {15, 77}
        };

    public int GetTargetXpBasedOnLv(int lv)
    {
        return levelDB[lv];
    }
}
