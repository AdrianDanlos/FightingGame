using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIMainMenu uIMainMenu;

    [Header("Skins")]
    public Skins skins;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            uIMainMenu.DisplayAchievements();
        }
    }

    public void LoadLoadingSceneToGame()
    {
        SScene.toInitialMenu = false;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void LoadLoadingSceneToInitialMenu()
    {
        skins.SetDefaultSkin(); // set default skin
        SScene.toInitialMenu = true;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
