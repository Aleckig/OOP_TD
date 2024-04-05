using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerSettings))]
public class TowerDealDamage : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject towerTop;
    [SerializeField] private GameObject towerTopBulletPlace;
    [SerializeField] private List<GameObject> enemyList;
    private TowerSettings towerSettings;
    [SerializeField] private GameObject activeTarget = null;
    private bool attackInProgress = false;

    private void Awake()
    {
        towerSettings = GetComponent<TowerSettings>();
    }

    private void FixedUpdate()
    {
        if (!activeTarget) return;
        TargetEnemy();
        if (!attackInProgress) StartCoroutine(DealDamageToEnemy());
    }

    private void TargetEnemy()
    {
        towerTop.transform.LookAt(activeTarget.transform);
    }
    private void ThrowProjectile()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, towerTopBulletPlace.transform.position, towerTop.transform.rotation);
        projectileObj.GetComponent<ProjectileFlying>().speed = projectileSpeed;
    }
    private IEnumerator DealDamageToEnemy()
    {
        if (attackInProgress) yield break;
        int damageAmount = towerSettings.GetTower().damageAmount;

        attackInProgress = true;

        ThrowProjectile();
        if (activeTarget.TryGetComponent<FlyingEnemy>(out FlyingEnemy flyingEnemy))
        {
            flyingEnemy.health -= damageAmount;
        }
        else if (activeTarget.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
        {
            enemyMovement.health -= damageAmount;
        }

        enemyList.Remove(activeTarget);
        activeTarget = activeTarget == null ? enemyList[0] : null;
        yield return new WaitForSeconds(towerSettings.GetTower().cooldown);

        attackInProgress = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyList.Add(other.gameObject);
            activeTarget = activeTarget == null ? enemyList[0] : activeTarget;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemyList.Contains(other.gameObject))
            {
                enemyList.Remove(other.gameObject);
                if (!enemyList.Contains(activeTarget))
                    activeTarget = enemyList.Count > 0 ? enemyList[0] : null;
            }
        }
    }
}
