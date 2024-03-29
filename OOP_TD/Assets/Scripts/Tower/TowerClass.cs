using System;
using UnityEngine;

[Serializable]
public class Tower
{
  public GameObject towerPrefab;
  public string name;
  public int price;

  public Tower(GameObject _towerPrefab, string _name, int _price)
  {
    this.towerPrefab = _towerPrefab;
    this.name = _name;
    this.price = _price;
  }
  public Tower(Tower tower)
  {
    this.towerPrefab = tower.towerPrefab;
    this.name = tower.name;
    this.price = tower.price;
  }
}
