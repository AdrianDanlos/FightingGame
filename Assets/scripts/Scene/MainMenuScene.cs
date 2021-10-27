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
            LoadLoadingSceneToInitialMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadLoadingSceneToGame();
        }
    }

    public void LoadLoadingSceneToGame()
    {
        StartCoroutine(sceneTransition.DisplayAnimation());
        SScene.toInitialMenu = false;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void LoadLoadingSceneToInitialMenu()
    {
        StartCoroutine(sceneTransition.DisplayAnimation());
        skins.SetDefaultSkin(); // set default skin
        SScene.toInitialMenu = true;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
