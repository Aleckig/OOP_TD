using System;
using UnityEngine;

[Serializable]
public class Tower
{
  public GameObject towerPrefab;
  public string name;
  public int price;
  public int damage;
  public float damageRange;
  public float attackCooldown;


  public Tower(GameObject _towerPrefab, string _name, int _price, int _damage, float _damageRange, float _attackCooldown)
  {
    this.towerPrefab = _towerPrefab;
    this.name = _name;
    this.price = _price;
    this.damage = _damage;
    this.damageRange = _damageRange;
    this.attackCooldown = _attackCooldown;
  }
  public Tower(Tower tower)
  {
    this.towerPrefab = tower.towerPrefab;
    this.name = tower.name;
    this.price = tower.price;
    this.damage = tower.damage;
    this.damageRange = tower.damageRange;
    this.attackCooldown = tower.attackCooldown;
  }
}
