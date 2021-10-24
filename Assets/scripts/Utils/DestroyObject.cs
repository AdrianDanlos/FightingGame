using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Destroys the GameObject this script is attached to after
    // number in var 'secondsToDestroy'
    private float secondsCounter;
    private float secondsToDestroy = 3f;

    // Update is called once per frame
    void Update()
    {
        secondsCounter += Time.deltaTime;

        if(secondsCounter >= secondsToDestroy)
        {
            Destroy(gameObject);
        }
    }
}
