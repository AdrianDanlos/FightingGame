using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons
{
    public Dictionary<int, Dictionary<string, string>> weapons =
    new Dictionary<int, Dictionary<string, string>>
    {
        {
            0,
            new Dictionary<string, string>
            {
                {"weaponName", "fist"},
                {"damage", "0"},
            }
        },
        {
            1,
            new Dictionary<string, string>
            {
                {"weaponName", "knife"},
                {"damage", "1"},
            }
        },
        {
            2,
            new Dictionary<string, string>
            {
                {"weaponName", "whip"},
                {"damage", "2"},
            }
        },
        {
            3,
            new Dictionary<string, string>
            {
                {"weaponName", "spear"},
                {"damage", "3"},
            }
        },
        {
            4,
            new Dictionary<string, string>
            {
                {"weaponName", "katana"},
                {"damage", "4"},
            }
        },
    };
    // Start is called before the first frame update
    void Start()
    {

    }
}
