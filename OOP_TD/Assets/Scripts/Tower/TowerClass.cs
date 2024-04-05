using System;
using UnityEngine;

[Serializable]
public class Tower
{
  public GameObject towerPrefab;
  public string name;
  public int price;
  public int damageAmount;
  public float cooldown;


  public Tower(GameObject _towerPrefab, string _name, int _price, int _damageAmount, float _cooldown)
  {
    this.towerPrefab = _towerPrefab;
    this.name = _name;
    this.price = _price;
    this.damageAmount = _damageAmount;
    this.cooldown = _cooldown;
  }
  public Tower(Tower tower)
  {
    this.towerPrefab = tower.towerPrefab;
    this.name = tower.name;
    this.price = tower.price;
    this.damageAmount = tower.damageAmount;
    this.cooldown = tower.cooldown;
  }
}
