using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [Header("UI")]
    public UILoading uILoading;
    [SerializeField] private Image progressBar;

    private IEnumerator Start()
    {
        // make a transition 
        uILoading.LoadRandomTip();

        // create async operation depending from which scene you came 
        switch (SScene.scene)
        {
            // INITIAL_MENU > MAIN_MENU
            case 0:
                yield return new WaitForSeconds(1f);
                StartCoroutine(LoadAsyncOperation((int)SceneIndex.MAIN_MENU));
                break;
            // MAIN_MENU > GAME || MAIN_MENU > INITIAL_MENU
            case 1:
                yield return new WaitForSeconds(1f);
                if (SScene.toInitialMenu)
                {
                    SScene.scene = -1;
                    StartCoroutine(LoadAsyncOperation((int)SceneIndex.INITIAL_MENU));
                }
                else if(!SScene.toInitialMenu) {
                    StartCoroutine(LoadAsyncOperation((int)SceneIndex.GAME));
                }
                break;
            // GAME > MAIN_MENU
            case 3:
                yield return new WaitForSeconds(1f);
                StartCoroutine(LoadAsyncOperation((int)SceneIndex.MAIN_MENU));
                break;

            default:
                break;
        }        
    }
    
    public IEnumerator LoadAsyncOperation(int sceneNumber)
    {
        AsyncOperation loadProgress = SceneManager.LoadSceneAsync(sceneNumber);

        // fill progress bar with the progress of the operation load
        while (loadProgress.progress < 1)
        {
            progressBar.fillAmount = loadProgress.progress;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
    }
}
