using UnityEngine;
using Tymski;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] TMP_Text levelNameText;
    private GameObject levelGrid;
    private SceneReference easyLevelScene;
    private SceneReference hardLevelScene;
    public void SetData(string _levelNameStr, GameObject _levelGrid, SceneReference _easyLevelScene, SceneReference _hardLevelScene)
    {
        easyLevelScene = _easyLevelScene;
        hardLevelScene = _hardLevelScene;

        levelGrid = _levelGrid;
        levelNameText.text = _levelNameStr;
    }
    public void ReturnBack()
    {
        // Is it better to deActivate or destroy game object?
        gameObject.SetActive(false);
        // Destroy(gameObject);
        levelGrid.SetActive(true);
    }

    private void LoadLevel(SceneReference sceneReference)
    {
        SceneManager.LoadScene(sceneReference);
    }
    public void LoadEasyLevel()
    {
        LoadLevel(easyLevelScene);
    }
    public void LoadHardLevel()
    {
        LoadLevel(hardLevelScene);
    }
}
