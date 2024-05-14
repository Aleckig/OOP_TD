using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private SceneReference mainMenuScene;

    public void ShowGameWinScreen()
    {
        levelManager.OnLevelEnd(true);
        this.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
