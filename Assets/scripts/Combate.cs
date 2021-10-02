using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    // instanciar f
    public FighterStats f;
    // crear 2 pjs
    FighterStats f1, f2;



    // Start is called before the first frame update
    void Start()
    {
        f1 = Instantiate(f);
        f2 = Instantiate(f);

    }

    // Update is called once per frame
    void Update()
    {
        // combate
        while(f1.GetHitPoints() != 0 || f2.GetHitPoints() != 0)
        {
            /*
          -f1 ataca a f2(vidaf2 - dañof1)
          - f2 pasa a tener x vida
          - f2 ataca a f1(vidaf2 - dañof1)
          - f1 pasa a tener x vida
          - Final turno: f1 tiene x vida, f2 tiene x vida

          */

            f2.SetHitPoints(f2.GetHitPoints() - f1.GetBaseDmg());
            Debug.Log("f2: tiene " + f2.GetHitPoints());

            f1.SetHitPoints(f1.GetHitPoints() - f2.GetBaseDmg());
            Debug.Log("f1: tiene " + f1.GetHitPoints());
            Debug.Log("FINAL DE TURNO");
            Debug.Log("f1: tiene " + f1.GetHitPoints() + "f2 tiene: " + f2.GetHitPoints());

        }

    }
}