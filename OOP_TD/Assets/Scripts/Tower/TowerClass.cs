using System;
using UnityEngine;

[Serializable]
public class Tower
{
  public GameObject towerPrefab;
  public string name;
  public int price;
  public float damageRange;
  public int damage;
  public float attackCooldown;

  public Tower(GameObject _towerPrefab, string _name, int _price, float _damageRange, int _damage, float _attackCooldown)
  {
    this.towerPrefab = _towerPrefab;
    this.name = _name;
    this.price = _price;
    this.damageRange = _damageRange;
    this.damage = _damage;
    this.attackCooldown = _attackCooldown;
  }
  public Tower(Tower tower)
  {
    this.towerPrefab = tower.towerPrefab;
    this.name = tower.name;
    this.price = tower.price;
    this.damageRange = tower.damageRange;
    this.damage = tower.damage;
    this.attackCooldown = tower.attackCooldown;
  }
}
