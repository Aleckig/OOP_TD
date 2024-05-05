using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBar healthBar;
    public GameOverScreen gameOverScreen;
    public GameObject targetObject; // Reference to the target GameObject
    public Material healthyMaterial;
    public Material mediumMaterial;
    public Material criticalMaterial;
    public GameObject SmokeEffect01;
    public GameObject SmokeEffect02;
    public GameObject baseShield;
    public Material shieldEffect;
    public float shieldValue = -0.4f;
    public HealthBar shieldHealthBar;
    private bool isBaseShielded = false;
    private float baseHealthSave = 100f;
    public HealthBar originalHealthBar;
    public float damageMultiplier = 1f;
    public GameObject shieldButton;
    [SerializeField] private AudioClip shieldSound;
    private AudioSource audioSource;

    private Renderer targetRenderer; // Renderer of the target GameObject

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        if (isBaseShielded == true && currentHealth <= 0f)
        {
            healthBar = originalHealthBar;
            StartCoroutine(ShieldDestroyed());
            currentHealth = baseHealthSave;
        }

        if (currentHealth <= 0f && isBaseShielded == false)
        {
            gameOverScreen.ShowGameOverScreen();
        }
        else if (currentHealth <= 30f && isBaseShielded == false)
        {
            SmokeEffect02.SetActive(true);
            targetRenderer.material = criticalMaterial;
        }
        else if (currentHealth <= 60f && isBaseShielded == false)
        {
            SmokeEffect01.SetActive(true);
            targetRenderer.material = mediumMaterial;
        }
        else if (isBaseShielded == false)
        {
            targetRenderer.material = healthyMaterial;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage * damageMultiplier;
        healthBar.SetHealth(currentHealth);
    }

    public void SpawnShield()
    {
        audioSource.PlayOneShot(shieldSound);
        baseShield.gameObject.SetActive(true);
        shieldHealthBar.gameObject.SetActive(true);
        baseHealthSave = currentHealth;
        originalHealthBar = healthBar;
        healthBar = shieldHealthBar;
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        isBaseShielded = true;
        shieldEffect.SetFloat("_Fill", shieldValue);
        StartCoroutine(ShieldAppear());
        shieldButton.gameObject.SetActive(false);
    }

    IEnumerator ShieldAppear() //Gradually makes the shield appear by changing the Fill value on it
    {
        while (shieldEffect.GetFloat("_Fill") < 0f)
        {
            shieldValue += 0.02f;
            shieldEffect.SetFloat("_Fill", shieldValue);
            yield return new WaitForSeconds(0.01f);
        }
        yield return null;
    }

    IEnumerator ShieldDestroyed() //Destroys the shield health bar and gradually makes the shield effect disappear
    {
        shieldHealthBar.gameObject.SetActive(false);
        isBaseShielded = false;
        while (shieldEffect.GetFloat("_Fill") > -0.4f)
        {
            shieldValue -= 0.02f;
            shieldEffect.SetFloat("_Fill", shieldValue);
            yield return new WaitForSeconds(0.01f);
        }
        baseShield.gameObject.SetActive(false);
        yield return null;
    }
}
