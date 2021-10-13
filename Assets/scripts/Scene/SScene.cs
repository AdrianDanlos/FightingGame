using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScene : MonoBehaviour
{
    // stores current scene 
    public static int scene = 0;
    public static bool toInitialMenu;

    private void Start()
    {
        Debug.Log(scene);
        Debug.Log(toInitialMenu);
    }
}
