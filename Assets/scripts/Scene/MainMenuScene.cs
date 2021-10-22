using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScene : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private UIMainMenu uIMainMenu;

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
        SScene.toInitialMenu = true;
        SScene.scene = (int)SceneIndex.MAIN_MENU;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }
}
