using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combate : MonoBehaviour
{
    // instanciar f
    public FighterStats f;
    

    // Start is called before the first frame update
    void Start()
    {
        // crear 2 pjs
        FighterStats f1, f2;
        f1 = Instantiate(f, new Vector3(-10, 0, 0), Quaternion.Euler(0, 90, 0));
        f2 = Instantiate(f, new Vector3(10, 0, 0), Quaternion.Euler(0, -90, 0));
        f1.SetHitPoints(3);
        f2.SetHitPoints(3);
        Debug.Log(f1.hitPoints);
        Debug.Log(f1.GetHitPoints());
        StartCoroutine(attack(f1, f2));
    }

    IEnumerator attack(FighterStats f1, FighterStats f2)
    {        
        Debug.Log(f1.GetHitPoints());
        while (f1.GetHitPoints() != 0 || f2.GetHitPoints() != 0)
        {
            Debug.Log("entro a while");
            // F1 ATTACKS
            float f1position = f1.transform.position.x;

            // F1 GOES FORWARD
            while (f1.transform.position.x <= f2.transform.position.x - 2)
            {
                f1.transform.position += f1.transform.forward * Time.deltaTime * 15;
                yield return new WaitForFixedUpdate();
            }

            f2.SetHitPoints(f2.GetHitPoints() - f1.GetBaseDmg());
            Debug.Log("f2: tiene " + f2.GetHitPoints());
            StartCoroutine(receiveDmgAnimation(f2));


            // F1 RETURNS TO INITIAL POSITION
            // FIXME: Position x is not an integer so the figther goes further back from the initial position (e.g. 10.023)
            while (f1.transform.position.x >= f1position)
            {
                f1.transform.position -= f1.transform.forward * Time.deltaTime * 15;
                yield return new WaitForFixedUpdate();

            }

  
            // FIXME BETTER LOGIC OUT OF THE LOOP TO CHECK IF ONE OF THE FIGHTERS ARE DEAD
            if(f2.GetHitPoints() == 0)
            {
                //Especificar cual gana
                Debug.Log("FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR 1");
                yield break;
            }
          
            // F2 ATTACKS
            float f2position = f2.transform.position.x;

            // F2 GOES FORWARD
            while (f2.transform.position.x >= f1.transform.position.x + 2)
            {
                f2.transform.position += f2.transform.forward * Time.deltaTime * 15;
                yield return new WaitForFixedUpdate();
            }

            f1.SetHitPoints(f1.GetHitPoints() - f2.GetBaseDmg());
            Debug.Log("f1: tiene " + f1.GetHitPoints());
            StartCoroutine(receiveDmgAnimation(f1));


            // F2 RETURNS TO INITIAL POSITION
            // FIXME: Position x is not an integer so the figther goes further back from the initial position (e.g. 10.023)
            while (f2.transform.position.x <= f2position)
            {
                f2.transform.position -= f2.transform.forward * Time.deltaTime * 15;
                yield return new WaitForFixedUpdate();

            }

            // FIXME BETTER LOGIC OUT OF THE LOOP TO CHECK IF ONE OF THE FIGHTERS ARE DEAD
            if (f1.GetHitPoints() == 0)
            {
                //Especificar cual gana
                Debug.Log("FINAL DE COMBATEEEEEEEEEE, GANA EL JUGADOR 2");
                yield break;
            }

            Debug.Log("FINAL DE TURNO");
            Debug.Log("f1: tiene " + f1.GetHitPoints() + "f2 tiene: " + f2.GetHitPoints());
        } 
    }

    IEnumerator receiveDmgAnimation(FighterStats f)
    {
        Renderer figtherRenderer = f.GetComponent<Renderer>();
        figtherRenderer.material.color = new Color(255, 1, 1);
        yield return new WaitForSeconds(.3f);
        figtherRenderer.material.color = new Color(1, 1, 1);
    }


}