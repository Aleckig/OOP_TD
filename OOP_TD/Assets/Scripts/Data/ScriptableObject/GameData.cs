using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Tymski;

[Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "Game Data Containers/ Game Data", order = 0)]
public class GameData : SerializedScriptableObject
{
  [Title("References")]
  public PlayerProgressData playerProgressData;
  [Title("Towers Data")]
  public List<TowerCard> towerCardsList;
  public List<Tower> towersList;
  [Title("Enemies Data")]
  public List<string> EnemyNamesList;
  [Title("Levels Data")]
  public int activeLevelId = -1;
  [OnValueChanged("UpdateLevelsId")]
  public List<LevelSettingsItem> levelsDataList;

  private void UpdateLevelsId()
  {
    int idCount = 1;
    foreach (var item in levelsDataList)
    {
      item.levelId = idCount;
      idCount++;
    }
  }
}

[Serializable]
public class LevelSettingsItem
{
  public int levelId;
  // public string levelName; //Name removed
  public Sprite levelImage;
  public SceneReference easyLevelScene;
  public SceneReference hardLevelScene;
  private void LoadLevel(SceneReference sceneReference)
  {
    if (sceneReference == null) return;

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