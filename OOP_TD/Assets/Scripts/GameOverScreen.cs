using UnityEngine;
using UnityEngine.SceneManagement;
using Tymski;

public class GameOverScreen : MonoBehaviour
{
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ShowGameOverScreen()
    {
        this.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }
}
