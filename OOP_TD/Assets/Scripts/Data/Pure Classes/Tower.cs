using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Tower
{
  // --> Fields Basic
  [BoxGroup("Basic")]
  public GameObject towerPrefab;
  [BoxGroup("Basic")]
  public string towerName;
  // <--

  // --> Fields for Inheritance
  [BoxGroup("Inheritance")]
  public int price;
  [BoxGroup("Inheritance")]
  public int damage;
  [BoxGroup("Inheritance")]
  public float damageRange;
  [BoxGroup("Inheritance")]
  public float attackCooldown;
  // <--

  // --> Fields for Polymorphism
  [BoxGroup("Polymorphism")]
  static public List<string> towerDamageTypeAvailable = new() { "Electro", "Fire", "Frost" };
  [BoxGroup("Polymorphism")]
  public List<string> towerDamageTypeAdded = new() { };
  [BoxGroup("Polymorphism")]
  public List<string> specialMethods = new();
  [BoxGroup("Polymorphism")]
  public string targetEnemyName;
  // <--

  public Tower(GameObject _towerPrefab, string _name, int _price, int _damage, float _damageRange, float _attackCooldown, List<string> _specialMethods)
  {
    this.towerPrefab = _towerPrefab;
    this.towerName = _name;
    this.price = _price;
    this.damage = _damage;
    this.damageRange = _damageRange;
    this.attackCooldown = _attackCooldown;

    if (_specialMethods != null)
      foreach (var item in _specialMethods)
      {
        this.specialMethods.Add(item);
      }
  }
  public Tower(Tower tower)
  {
    this.towerPrefab = tower.towerPrefab;
    this.towerName = tower.towerName;
    this.price = tower.price;
    this.damage = tower.damage;
    this.damageRange = tower.damageRange;
    this.attackCooldown = tower.attackCooldown;
        this.towerDamageTypeAdded = tower.towerDamageTypeAdded;


    if (tower.specialMethods != null)
      foreach (var item in tower.specialMethods)
      {
        this.specialMethods.Add(item);
      }
  }
}
