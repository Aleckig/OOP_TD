using UnityEngine;
using Tymski;
using TMPro;
using Michsky.UI.Heat;

public class CreateLevelSelection : MonoBehaviour
{
  [SerializeField] private string levelName;
  [SerializeField] private GameObject LevelList;
  [SerializeField] private GameObject LevelSelectionPrefab;
  [SerializeField] private SceneReference easyLevelScene;
  [SerializeField] private SceneReference hardLevelScene;

  public void CreateLevelSelectionBlock()
  {
    LevelList.SetActive(false);

    GameObject levelSelection = Instantiate(LevelSelectionPrefab, LevelList.transform.parent);

    levelSelection.GetComponent<LevelSelection>().SetData(levelName, LevelList, easyLevelScene, hardLevelScene);
  }

  private void OnDisable()
  {

    GetComponent<BoxButtonManager>().UpdateUI();
  }
}
