using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Heat;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [ShowInInspector]
    private Dictionary<string, float> enemiesThatDealtDamageDict = new() { };
    private float totalDamage = 0;
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
    public BaseShieldTimer shieldTimer;
    private bool isBaseShielded = false;
    private float baseHealthSave = 100f;
    public HealthBar originalHealthBar;
    public float damageMultiplier = 1f;
    public ButtonManager shieldButton;
    public ButtonManager fixButton;
    private Renderer targetRenderer; // Renderer of the target GameObject
    public AbilityManager abilityManager;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemiesThatDealtDamageDict = new() { };
        shieldButton.enabled = false;
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

        shieldButton.onClick.AddListener(() => { SpawnShield(); });
        fixButton.onClick.AddListener(() => { FixBaseCode(); });
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
            levelManager.SaveEnemyDamage(totalDamage, enemiesThatDealtDamageDict);
            levelManager.OnLevelEnd(false);
            gameOverScreen.ShowGameOverScreen();
            this.enabled = false;
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

    public void TakeDamage(float damage, string enemyTypeName)
    {
        currentHealth -= damage * damageMultiplier;
        healthBar.SetHealth(currentHealth);
        totalDamage += damage;


        if (enemiesThatDealtDamageDict.ContainsKey(enemyTypeName))
        {
            enemiesThatDealtDamageDict[enemyTypeName] += damage;
            return;
        }

        enemiesThatDealtDamageDict.Add(enemyTypeName, damage);
    }

    public void SpawnShield()
    {
        audioSource.PlayOneShot(shieldSound);
        if (abilityManager.shieldsAmount > 0)
        {
            shieldButton.enabled = true;
            abilityManager.DecreaseShieldCount();
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
            StartCoroutine(ShieldTimer());
        }
        else
        {
            shieldButton.enabled = false;
        }
        if (!abilityManager.abilityDictionary.ContainsKey("ShieldAbility"))
        {
            abilityManager.abilityDictionary.Add("ShieldAbility", 1);
        }
        else
        {
            abilityManager.abilityDictionary["ShieldAbility"]++;
        }

    }

    public void FixBaseCode()
    {
        damageMultiplier = 1f;
        abilityManager.DecreaseBaseFixCount();
        if (!abilityManager.abilityDictionary.ContainsKey("BaseFix"))
        {
            abilityManager.abilityDictionary.Add("BaseFix", 1);
        }
        else
        {
            abilityManager.abilityDictionary["BaseFix"]++;
        }
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

    IEnumerator ShieldTimer()
    {
        shieldTimer.gameObject.SetActive(true);
        float counter = 15f;
        shieldTimer.SetMaxHealth(counter);
        while (counter > 0f)
        {
            counter -= 0.01f;
            yield return new WaitForSeconds(0.1f);
            shieldTimer.SetHealth(counter);
        }
        if (counter <= 0f)
        {
            healthBar = originalHealthBar;
            StartCoroutine(ShieldDestroyed());
            currentHealth = baseHealthSave;
            shieldTimer.gameObject.SetActive(false);
        }
        yield return null;
    }
}
