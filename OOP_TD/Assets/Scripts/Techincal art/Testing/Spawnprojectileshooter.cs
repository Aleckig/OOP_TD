using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnprojectileshooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float projectileSpeed = 10f;

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = spawnPoint.forward * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }
}
