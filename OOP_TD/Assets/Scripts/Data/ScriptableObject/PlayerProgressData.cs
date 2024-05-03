using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "PlayerProgressData", menuName = "Game Data Containers/ Player Progress Data", order = 1)]
public class PlayerProgressData : ScriptableObject
{
  public List<SpecialMethodItem> specialMethodsList;
}

[Serializable]
public struct SpecialMethodItem
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