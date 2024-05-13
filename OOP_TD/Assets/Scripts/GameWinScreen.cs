using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour
{
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    public void ShowGameWinScreen()
    {
        this.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    //TODO: Create method for next level?
}
