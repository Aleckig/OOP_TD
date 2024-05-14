using System;
using UnityEngine;

//Class which is container with all needed parameters for enemy
[Serializable]
public class Enemy
{
    public string typeName;
    public float health;
    public float movementSpeed;
    public int damage;
    public float attackCooldown; //how often enemy will attack Base
    public int moneyReward;

    public Enemy(string _typeName, int _health, int _movementSpeed, int _damage, float _attackCooldown)
    {
        this.typeName = _typeName;
        this.health = _health;
        this.movementSpeed = _movementSpeed;
        this.damage = _damage;
        this.attackCooldown = _attackCooldown;
    }
    public Enemy(Enemy enemy)
    {
        this.typeName = enemy.typeName;
        this.health = enemy.health;
        this.movementSpeed = enemy.movementSpeed;
        this.damage = enemy.damage;
        this.attackCooldown = enemy.attackCooldown;
    }
}
