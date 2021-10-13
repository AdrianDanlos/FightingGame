using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScene : MonoBehaviour
{
    // last scene loaded to track from where current 
    // scene is loading from
    public static int scene = -1;

    // where to go from MAIN_MENU > INITIAL_MENU || GAME
    public static bool toInitialMenu;

    // creating new game save or loading the stored one
    public static bool newGame;
}
