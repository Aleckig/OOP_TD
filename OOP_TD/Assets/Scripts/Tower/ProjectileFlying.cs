using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlying : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public TowerDealDamage tower;
    [SerializeField] private GameObject particleEffectPrefab;

    void FixedUpdate()
    {
        transform.position += transform.forward * speed;
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        // transform.Translate(transform.forward*speed, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            tower.GetComponent<TowerDealDamage>().DamageEnemyHealth();
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
