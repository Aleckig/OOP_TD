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
    public GameObject activeTarget = null; //The target that the tower is aiming at
    public GameObject currentTarget = null; //The target that the projectile is flying at
    private bool attackInProgress = false;
    private float turningSpeed = 20f;

    private void Awake()
    {
        towerSettings = GetComponent<TowerSettings>();
    }

    private void FixedUpdate()
    {
        enemyList.RemoveAll(s => s == null); //Keeps the enemy list clean from null enemy objects (for example enemies that died)
        TargetEnemy(); //Chooses the first enemy from the enemy list and aims at it
        if (activeTarget == null)
        {
            StopCoroutine(DealDamageToEnemy());
        }
        if (!activeTarget) return;
        if (!attackInProgress) StartCoroutine(DealDamageToEnemy());
    }

    private void TargetEnemy()
    {

        if (enemyList.Count != 0)
        {
            activeTarget = enemyList[0];
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
        yield return new WaitForSeconds(0.3f); //Waits for 0.3 seconds so that the tower has time to aim at the enemy before shooting
        currentTarget = activeTarget;
        ThrowProjectile(); //Instantiates a projectile that flies towards current target (and also changes current target's tag to "Target", which is used in ProjectileFlying script)
        yield return new WaitForSeconds(towerSettings.GetTower().attackCooldown);
        attackInProgress = false;
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Target"))
        {
            enemyList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Target"))
        {
            if (enemyList.Contains(other.gameObject))
            {
                enemyList.Remove(other.gameObject);
            }
        }
    }
    public void DamageEnemyHealth()
    {
        float damage = towerSettings.GetTower().damage;
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
