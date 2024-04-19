using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerSettings))]
public class TowerDealDamage : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject towerTop;
    [SerializeField] private GameObject towerCore;
    [SerializeField] private GameObject towerTopBulletPlace;
    [SerializeField] private List<GameObject> enemyList;
    [SerializeField] private GameObject particleEffectPrefab;
    private TowerSettings towerSettings;
    //[SerializeField] private GameObject activeTarget = null;
    public GameObject activeTarget = null;
    public GameObject currentTarget = null;
    private bool attackInProgress = false;
    private float turningSpeed = 20f;

    private void Awake()
    {
        towerSettings = GetComponent<TowerSettings>();
    }

    private void FixedUpdate()
    {
        //if (!activeTarget) return;
        enemyList.RemoveAll(s => s == null);
        TargetEnemy();
        if (activeTarget == null)
        {
            StopCoroutine(DealDamageToEnemy());
        }
        if (!activeTarget) return;
        if (!attackInProgress) StartCoroutine(DealDamageToEnemy());
    }

    private void TargetEnemy()
    {

        //towerTop.transform.LookAt(activeTarget.transform);
        if (enemyList.Count != 0)
        {
            activeTarget = enemyList[0];
            //activeTarget.tag = "Target";
            Vector3 aimAt = new Vector3(activeTarget.transform.position.x, towerCore.transform.position.y, activeTarget.transform.position.z);
            float distToTarget = Vector3.Distance(aimAt, towerTop.transform.position);
            Vector3 relativeTargetPosition = towerTop.transform.position + (activeTarget.transform.position - towerTop.transform.position).normalized * distToTarget;
            towerTop.transform.rotation = Quaternion.Slerp(towerTop.transform.rotation, Quaternion.LookRotation(relativeTargetPosition - towerTop.transform.position) * Quaternion.Euler(-90, 0, 0), Time.deltaTime * turningSpeed);
            towerCore.transform.rotation = Quaternion.Slerp(towerCore.transform.rotation, Quaternion.LookRotation(aimAt - towerCore.transform.position), Time.deltaTime * turningSpeed);
        }
        else
        {
            //turn to original position with slerp
            activeTarget = null;
            return;
        }
    }
    private void ThrowProjectile()
    {
        currentTarget.tag = "Target";
        GameObject projectileObj = Instantiate(projectilePrefab, towerTopBulletPlace.transform.position, towerTop.transform.rotation * Quaternion.Euler(90, 0, 0));
        projectileObj.GetComponent<ProjectileFlying>().target = currentTarget;
        projectileObj.GetComponent<ProjectileFlying>().speed = projectileSpeed;
        projectileObj.GetComponent<ProjectileFlying>().tower = this;

        // Instantiate the particle effect at the bullet point position
        Instantiate(particleEffectPrefab, towerTopBulletPlace.transform.position, Quaternion.identity);
    }
    private IEnumerator DealDamageToEnemy()
    {
        if (attackInProgress) yield break;
        int damage = towerSettings.GetTower().damage;

        attackInProgress = true;
        yield return new WaitForSeconds(0.3f);
        currentTarget = activeTarget;
        ThrowProjectile();
        //if (activeTarget.TryGetComponent<FlyingEnemy>(out FlyingEnemy flyingEnemy))
        //{
        //    flyingEnemy.health -= damage;
        //}
        //else if (activeTarget.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
        //{
        //    enemyMovement.health -= damage;
        //}

        //enemyList.Remove(activeTarget);
        //activeTarget = activeTarget == null ? enemyList[0] : null;
        yield return new WaitForSeconds(towerSettings.GetTower().attackCooldown);

        attackInProgress = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Target"))
        {
            enemyList.Add(other.gameObject);
            //activeTarget = activeTarget == null ? enemyList[0] : activeTarget;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Target"))
        {
            if (enemyList.Contains(other.gameObject))
            {
                enemyList.Remove(other.gameObject);
                //if (!enemyList.Contains(activeTarget))
                //{
                //    activeTarget = enemyList.Count > 0 ? enemyList[0] : null;
                //}
            }
        }
    }
    public void DamageEnemyHealth()
    {
        int damage = towerSettings.GetTower().damage;
        if (currentTarget.TryGetComponent<FlyingEnemy>(out var flyingEnemy))
        {
            flyingEnemy.health -= damage;
        }
        else if (currentTarget.TryGetComponent<EnemyMovement>(out var enemyMovement))
        {
            enemyMovement.health -= damage;
            enemyMovement.GetComponent<EnemyMovement>().enemyHealthBar.SetHealth(enemyMovement.health);
        }
    }
}
