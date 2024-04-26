using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
