using System.Collections.Generic;
using UnityEngine;

public class LevelDB : MonoBehaviour
{
    // FIXME[Future] -- get lv and xp increment through a formula
    // xp = (level/x) pow(y)  | values > X: 0.3, Y: 2
    // level = 0.07 * root(XP)
    static int lvCap = 20;

    // LV 1 - 5 > 4 XP PER LV
    // LV 6 - 10 > 5 XP PER LV
    // LV 10 - 15 > 6 XP PER LV
    // LV 16 - 20 > 7 XP PER LV
    Dictionary<int, int> levelDB =
        new Dictionary<int, int>
        {
            {1, 2},
            {2, 2},
            {3, 2},
            {4, 2},
            {5, 2},
            {6, 2},
            {7, 2},
            {8, 2},
            {9, 2},
            {10, 2},
            {11, 2},
            {12, 2},
            {13, 2},
            {14, 2},
            {15, 2},
            {16, 2},
            {17, 2},
            {18, 2},
            {19, 2},
            {20, 2},
        };

    public int GetTargetXpBasedOnLv(int lv)
    {
        return levelDB[lv];
    }

    public int GetLvCap()
    {
        return lvCap;
    }
}
