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
        // create async operation depending from which scene you came 
        // MAIN_MENU > GAME
        if (SScene.scene == 0)
        {
            StartCoroutine(LoadAsyncOperation(2));
        }

        //GAME > MAIN_MENU
        else if (SScene.scene == 2)
        {
            StartCoroutine(LoadAsyncOperation(0));
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
