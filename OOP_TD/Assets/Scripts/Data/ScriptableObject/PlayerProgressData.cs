using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "PlayerProgressData", menuName = "Game Data Containers/ Player Progress Data", order = 1)]
public class PlayerProgressData : SerializedScriptableObject
{
  public List<SpecialMethodItem> specialMethodsList;
  public LevelDataSaveManager levelDataSaveManager = new();

  public void UnlockSpecialMethod(string methodNameCode)
  {
    for (int i = 0; i < specialMethodsList.Count; i++)
    {
      if (specialMethodsList[i].methodNameCode == methodNameCode)
        specialMethodsList[i].unlockedStatus = true;
    }
  }
}

[Serializable]
public class SpecialMethodItem
{
  //If plaeyr able to use this method
  [BoxGroup]
  public bool unlockedStatus;
  [BoxGroup]
  public string methodNameCode;
  [BoxGroup]
  public string methodNameDisplay;
  [BoxGroup]
  public string paramListDisplay;
  [BoxGroup]
  public int methodPointsPrice;
}