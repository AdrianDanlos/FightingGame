using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    [Header("UI")]
    public UIGame uIGame;
    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject levelUpMenu;

    [Header("Scenes")]
    public SceneTransition sceneTransition;

    private void Update()
    {
        if (backToMenu.activeSelf && !levelUpMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uIGame.LoadNextMenu();
            }
        }
    }
    public void LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.GAME;
        StartCoroutine(sceneTransition.DisplayAnimation());
        SceneManager.LoadScene((int)SceneIndex.LOADING_SCREEN);
    }

    public void SetLevelUpState(bool value)
    {
        SScene.levelUp = value;
    }

    public bool GetLevelUpState()
    {
        return SScene.levelUp;
    }
}
