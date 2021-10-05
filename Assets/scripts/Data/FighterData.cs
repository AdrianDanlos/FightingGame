using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterData : MonoBehaviour
{
    public Text hp;
    public Text dmg;

    public int number;

    // Start is called before the first frame update
    void Start()
    {
        hp.text = "3";
    }

    public void GenerateData()
    {
        number = 3;
    }
    public void ShowData()
    {

    }
}