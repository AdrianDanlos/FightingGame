using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Image progressBar;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(SScene.scene);
        // create async operation depending from which scene you came 
        switch (SScene.scene)
        {
            // INITIAL_MENU > MAIN_MENU
            case 0:
                StartCoroutine(LoadAsyncOperation((int)SceneIndex.MAIN_MENU));
                break;
            // MAIN_MENU > GAME
            case 1:
                StartCoroutine(LoadAsyncOperation((int)SceneIndex.GAME));
                break;
            //GAME > MAIN_MENU
            case 3:
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
    }
}
