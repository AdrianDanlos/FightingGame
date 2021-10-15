using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDB : MonoBehaviour
{
    // FIXME -- get lv and xp increment through a formula
    // LV 1 - 5 > 4 XP PER LV
    // LV 6 - 10 > 5 XP PER LV
    // LV 10 - 15 > 6 XP PER LV
    Dictionary<int, int> levelDB =
        new Dictionary<int, int>
        {
            {1, 0},
            {2, 4},
            {3, 8},
            {4, 12},
            {5, 16},
            {6, 21},
            {7, 26},
            {8, 31},
            {9, 36},
            {10, 41},
            {11, 47},
            {12, 53},
            {13, 59},
            {14, 65},
            {15, 71}
        };

    public int GetTargetXpBasedOnLv(int lv)
    {
        return levelDB[lv];
    }
}
