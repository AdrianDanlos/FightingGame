using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{    
    public Dictionary<int, Dictionary<string, string>> weapons =
    new Dictionary<int, Dictionary<string, string>>
    {
        {
            0,
            new Dictionary<string, string>
            {
                {"weaponName", "knife"},
                {"damage", "2"},
            }
        },
        {
            1,
            new Dictionary<string, string>
            {
                {"weaponName", "katana"},
                {"damage", "3"},
            }
        },
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
