using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "Game Data Containers/GameData", order = 0)]
public class GameData : ScriptableObject
{
  public List<TowerCard> towerCardsList;
  public List<Tower> towersList;

}

