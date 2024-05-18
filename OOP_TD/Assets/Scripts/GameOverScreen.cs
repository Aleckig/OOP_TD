using UnityEngine;
using UnityEngine.SceneManagement;
using Tymski;
using Michsky.UI.Heat;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private ButtonManager restartButton;
    [SerializeField] private ButtonManager backToMenuButton;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ShowGameOverScreen()
    {
        restartButton.onClick.AddListener(() => { RestartGame(); });
        backToMenuButton.onClick.AddListener(() => { BackToMenu(); });

        this.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
