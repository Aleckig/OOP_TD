using System;
using UnityEngine;

//Class which is container with all needed parameters for enemy
[Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public string typeName;
    public int health;
    public int movementSpeed;
    public int damage;
    public float attackCooldown; //how often enemy will attack Base

    public Enemy(GameObject _enemyPrefab, string _typeName, int _health, int _movementSpeed, int _damage, float _attackCooldown)
    {
        this.enemyPrefab = _enemyPrefab;
        this.typeName = _typeName;
        this.health = _health;
        this.movementSpeed = _movementSpeed;
        this.damage = _damage;
        this.attackCooldown = _attackCooldown;
    }
    public Enemy(Enemy enemy)
    {
        this.enemyPrefab = enemy.enemyPrefab;
        this.typeName = enemy.typeName;
        this.health = enemy.health;
        this.movementSpeed = enemy.movementSpeed;
        this.damage = enemy.damage;
        this.attackCooldown = enemy.attackCooldown;
    }
}
