using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Animator")]
    public Animator transition;
    public float transitionTime = 1f;

    public IEnumerator DisplayAnimation()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}
