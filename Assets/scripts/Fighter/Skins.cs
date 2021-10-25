using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkinsList
{
    Fallen_Angel,
    Golem,
    Reaper
}

public class Skins : MonoBehaviour
{
    public static string skinSelected = "Reaper"; // default skin

    public string GetSkinSelected()
    {
        return Skins.skinSelected;
    }

    public List<string> GetSkinsList()
    {
        List<string> skins = new List<string>();
        foreach (SkinsList skin in (SkinsList[])Enum.GetValues(typeof(SkinsList))) skins.Add(skin.ToString());
        return skins;
    }
}
