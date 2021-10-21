using System.Collections.Generic;
using UnityEngine;

public class LevelDB : MonoBehaviour
{
    // FIXME[Future] -- get lv and xp increment through a formula
    static int lvCap = 40;

    // LV 1 - 5 > 4 XP PER LV
    // LV 6 - 10 > 5 XP PER LV
    // LV 11 - 15 > 6 XP PER LV
    // LV 16 - 20 > 7 XP PER LV
    // LV 21 - 25 > 8 XP PER LV
    // LV 26 - 30 > 9 XP PER LV
    // LV 31 - 35 > 10 XP PER LV
    // LV 36 - 40 > 11 XP PER LV
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
            {15, 77},
            {16, 84},
            {17, 91},
            {18, 98},
            {19, 105},
            {20, 112},
            {21, 120},
            {22, 128},
            {23, 136},
            {24, 144},
            {25, 152},
            {26, 161},
            {27, 170},
            {28, 178},
            {29, 187},
            {30, 196},
            {31, 206},
            {32, 216},
            {33, 226},
            {34, 236},
            {35, 246},
            {36, 257},
            {37, 268},
            {38, 279},
            {39, 290},
            {40, 301}
        };

    // testing purposes
    Dictionary<int, int> testDB =
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
            {21, 2},
            {22, 2},
            {23, 2},
            {24, 2},
            {25, 2},
            {26, 2},
            {27, 2},
            {28, 2},
            {29, 2},
            {30, 2},
            {31, 2},
            {32, 2},
            {33, 2},
            {34, 2},
            {35, 2},
            {36, 2},
            {37, 2},
            {38, 2},
            {39, 2},
            {40, 2}
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
