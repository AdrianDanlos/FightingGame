using System.Collections;
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
                StartCoroutine(uIGame.LoadNextMenu());
            }
        }
    }

    public void WrapperLoadMainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    public IEnumerator LoadMainMenu()
    {
        SScene.scene = (int)SceneIndex.GAME;
        StartCoroutine(sceneTransition.DisplayAnimation());
        yield return new WaitForSeconds(1f);
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
