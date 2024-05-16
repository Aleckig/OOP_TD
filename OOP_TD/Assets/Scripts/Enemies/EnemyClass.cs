using System;
using UnityEngine;
using System.Collections.Generic;

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
    //public List<string> resistance = new();
    //public List<string> weakness = new();
    public string resistance;
    public string weakness;


    public Enemy(string _typeName, int _health, int _movementSpeed, int _damage, float _attackCooldown, string _resistance, string _weakness)
    {
        this.typeName = _typeName;
        this.health = _health;
        this.movementSpeed = _movementSpeed;
        this.damage = _damage;
        this.attackCooldown = _attackCooldown;
        this.resistance = _resistance;
        this.weakness = _weakness;
    }
    public Enemy(Enemy enemy)
    {
        this.typeName = enemy.typeName;
        this.health = enemy.health;
        this.movementSpeed = enemy.movementSpeed;
        this.damage = enemy.damage;
        this.attackCooldown = enemy.attackCooldown;
        this.resistance = enemy.resistance;
        this.weakness = enemy.weakness;
    }
}
