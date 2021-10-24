using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInactive : MonoBehaviour
{
    // Sets inactive the GameObject this script is attached to after
    // number in var 'secondsToHide'
    private float secondsCounter;
    private float secondsToHide = 3f;

    // Update is called once per frame
    void Update()
    {
        secondsCounter += Time.deltaTime;

        if (secondsCounter >= secondsToHide)
        {
            gameObject.SetActive(false);
        }
    }
}
