using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    [Header("UI")]
    public UIGame uIGame;
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject levelUpMenu;

    private void Update()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uIGame.ShowLevelUpMenu();
            }
        }
    }
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.GAME;
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

}
