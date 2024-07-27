using Michsky.UI.Heat;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ESCController : MonoBehaviour
{

    [SerializeField] private GameObject escBlock;
    [SerializeField] private SceneReference mainMenuScene;
    [SerializeField] private ButtonManager restartButton;
    [SerializeField] private ButtonManager backToMenuButton;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        SetEvents();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escBlock.SetActive(!escBlock.activeInHierarchy);
        }
        Debug.Log("Working");
    }
    public void SetEvents()
    {
        restartButton.onClick.AddListener(() => { RestartGame(); });
        backToMenuButton.onClick.AddListener(() => { BackToMenu(); });
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
