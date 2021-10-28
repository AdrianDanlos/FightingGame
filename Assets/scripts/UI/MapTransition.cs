using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] RectTransform fader;

    public void OpenMenu()
    {
        fader.gameObject.SetActive(true);

        LeanTween.scale(fader, Vector3.zero, 0f);
        LeanTween.scale(fader, new Vector3(0.5f, 0.5f, 0.5f), 0.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void CloseMenu()
    {
        LeanTween.scale(fader, new Vector3(0.5f, 0.5f, 0.5f), 0f);
        LeanTween.scale(fader, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });
    }
}
