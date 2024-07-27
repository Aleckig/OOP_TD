using Michsky.UI.Heat;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWinScreen : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private ButtonManager backToMenuButton;

    public void ShowGameWinScreen()
    {
        levelManager.OnLevelEnd(true);

        backToMenuButton.onClick.AddListener(() => { BackToMenu(); });

        this.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
