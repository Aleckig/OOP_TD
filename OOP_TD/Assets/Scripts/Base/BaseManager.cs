using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameOverScreen gameOverScreen;
    public GameObject targetObject; // Reference to the target GameObject
    public Material healthyMaterial;
    public Material mediumMaterial;
    public Material criticalMaterial;
    public GameObject baseShield;
    public Material shieldEffect;
    public float shieldValue = -0.30f;

    private Renderer targetRenderer; // Renderer of the target GameObject

    void Start()
    {
        if (targetObject != null)
        {
            targetRenderer = targetObject.GetComponent<Renderer>(); // Get the Renderer component of the target GameObject
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            targetRenderer.material = healthyMaterial; // Set initial material
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned in BaseManager script!");
        }
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            gameOverScreen.ShowGameOverScreen();
        }
        else if (currentHealth <= 30)
        {
            targetRenderer.material = criticalMaterial;
        }
        else if (currentHealth <= 60)
        {
            targetRenderer.material = mediumMaterial;
        }
        else
        {
            targetRenderer.material = healthyMaterial;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void SpawnShield()
    {
        baseShield.gameObject.SetActive(true);
        shieldEffect.SetFloat("_Fill", shieldValue);
        StartCoroutine(ShieldAppear());
    }

    IEnumerator ShieldAppear()
    {
        while (shieldEffect.GetFloat("_Fill") < 0f)
        {
            shieldValue += 0.02f;
            shieldEffect.SetFloat("_Fill", shieldValue);
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;
    }
}
