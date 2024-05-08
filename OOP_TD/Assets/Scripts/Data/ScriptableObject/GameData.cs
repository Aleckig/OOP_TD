using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "Game Data Containers/ Game Data", order = 0)]
public class GameData : ScriptableObject
{
  [Title("References")]
  public PlayerProgressData playerProgressData;
  [Title("Towers Data")]
  public List<TowerCard> towerCardsList;
  public List<Tower> towersList;
  [Title("Enemies Data")]
  public List<string> EnemyNamesList;
}