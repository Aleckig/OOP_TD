using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeAttack : MonoBehaviour
{
    public GameObject baseManager;
    public float timeBetweenCodeAttacks = 0f;
    public EnemyMovement enemyMovement;

    void Start()
    {
        baseManager = GameObject.FindWithTag("Base");
    }


    void Update()
    {
        if (enemyMovement.GetComponent<EnemyMovement>().isAttacking == true)
        {
            if (timeBetweenCodeAttacks <= 0)
            {
                IncreaseBaseDamage();
                timeBetweenCodeAttacks = 5f;
            }
            timeBetweenCodeAttacks -= Time.deltaTime;
        }
    }
    public void IncreaseBaseDamage()
    {
        baseManager.GetComponent<BaseManager>().damageMultiplier += 0.1f;
    }
}
