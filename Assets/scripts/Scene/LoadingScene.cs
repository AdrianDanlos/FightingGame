using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [Header("UI")]
    public UILoading uILoading;

    [Header("Scenes")]
    public SceneTransition sceneTransition;

    private void Start()
    {
        StartCoroutine(sceneTransition.DisplayAnimation());
        // uILoading.LoadRandomTip(); -- unnecessary because loading is almost instantaneous

        // create async operation depending from which scene you came 
        switch (SScene.scene)
        {
            // INITIAL_MENU > MAIN_MENU
            case 0:
                SceneManager.LoadScene((int)SceneIndex.MAIN_MENU);
                break;
            // MAIN_MENU > GAME || MAIN_MENU > INITIAL_MENU
            case 1:
                if (SScene.toInitialMenu)
                {
                    SScene.scene = -1;
                    SceneManager.LoadScene((int)SceneIndex.INITIAL_MENU);
                }
                else if(!SScene.toInitialMenu) {
                    SceneManager.LoadScene((int)SceneIndex.GAME);
                }
                break;
            // GAME > MAIN_MENU
            case 3:
                SceneManager.LoadScene((int)SceneIndex.MAIN_MENU);
                break;

            default:
                break;
        }        
    }
}
