using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    // Data management
    [Header("Data")]
    public SavesManager savesManager;

    [Header("UI")]
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject levelUpMenu;

    private void Update()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SScene.levelUp)
                {
                    savesManager.ShowLevelUpMenu();
                }
                else if (!SScene.levelUp)
                {
                    LoadMainMenu();
                }
            }
        }

    }
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.GAME;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

}
