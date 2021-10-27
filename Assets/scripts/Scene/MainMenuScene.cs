using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    [Header("Skins")]
    public Skins skins;

    [Header("Scenes")]
    public SceneTransition sceneTransition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadLoadingSceneToInitialMenu());
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadLoadingSceneToGame());
        }
    }

    public void WrapperLoadLoadingSceneToGame()
    {
        StartCoroutine(LoadLoadingSceneToGame());
    }

    public void WrapperLoadLoadingSceneToInitialMenu()
    {
        StartCoroutine(LoadLoadingSceneToInitialMenu());
    }

    public IEnumerator LoadLoadingSceneToGame()
    {
        StartCoroutine(sceneTransition.DisplayAnimation());
        SScene.toInitialMenu = false;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public IEnumerator LoadLoadingSceneToInitialMenu()
    {
        StartCoroutine(sceneTransition.DisplayAnimation());
        skins.SetDefaultSkin(); // set default skin
        SScene.toInitialMenu = true;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
