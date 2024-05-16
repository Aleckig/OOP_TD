using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] public Enemy enemyData;
    private float currentHealth;
    public float timeBetweenAttacks = 0f;
    private float rotationSpeed = 10.0f;
    public bool isAttacking = false;
    public Transform target;
    public GameObject baseObject;
    public GameObject flyingTargetAttackPoints;
    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioSource;
    private bool reachedDestination = false;
    public HealthBar enemyHealthBar;
    public GameObject waveSpawner;
    public GameObject abilityManager;
    public GameObject gameManager;

    void Start()
    {
        currentHealth = enemyData.health;
        timeBetweenAttacks = enemyData.attackCooldown;

        audioSource = GetComponent<AudioSource>();
        baseObject = GameObject.FindWithTag("Base");
        flyingTargetAttackPoints = GameObject.FindWithTag("FlyingAttackPoints");
        target = flyingTargetAttackPoints.transform.GetChild(0);
        enemyHealthBar.SetMaxHealth(enemyData.health);
        waveSpawner = GameObject.FindWithTag("WaveSpawner");
        abilityManager = GameObject.FindWithTag("AbilityManager");
        gameManager = GameObject.FindWithTag("GameManager");
    }

    void FixedUpdate()
    {
        Move();

        if (isAttacking == true)
        {
            if (timeBetweenAttacks <= 0)
            {
                StartCoroutine(Attack());
                timeBetweenAttacks = 3f;

                audioSource.PlayOneShot(attackSound); // Play attack sound

                // Spawn particle effect at the position of the object
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
                return;
            }
            timeBetweenAttacks -= Time.deltaTime;
        }

        ChooseAttackPoint();

        if (Vector3.Distance(transform.position, target.position) <= 0.1f) //Start attacking when attack point is reached
        {
            isAttacking = true;
        }

        //if (currentHealth <= 0f)
        //{
        //    if (isAttacking == true)
        //    {
        //        target.gameObject.SetActive(true); //Re-enable attack point for next enemy to take the spot
        //    }
        //    waveSpawner.GetComponent<WaveSpawner>().aliveEnemies -= 1;
        //    if (Random.value < 0.1f)
        //    {
        //        abilityManager.GetComponent<AbilityManager>().IncreaseShieldCount();
        //    }
        //    else if (Random.value < 0.1f)
        //    {
        //        abilityManager.GetComponent<AbilityManager>().IncreasePathAbilityCount();
        //    }
        //    Destroy(gameObject); //Kill the enemy
        //}
    }

    private void Move()
    {
        if (isAttacking == false && reachedDestination == false) //Moves straight towards FlyingDestination, which is the point where enemies will turn towards the first available attack point around the base
        {
            Vector3 direction = target.position - transform.position;
            direction = direction + new Vector3(0, Mathf.Cos(Time.time * 3), 0);
            transform.Translate(direction.normalized * enemyData.movementSpeed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
        if (isAttacking == false && reachedDestination == true) //Moves towards the first available spot around the base
        {
            Vector3 direction = target.position - transform.position;
            Vector3 lookDirection = flyingTargetAttackPoints.transform.position - transform.position;
            transform.Translate(direction.normalized * enemyData.movementSpeed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
    }

    private void ChooseAttackPoint() //Choose the first available attack point around the base and occupy it by disabling the attackpoint object
    {
        if (Vector3.Distance(transform.position, flyingTargetAttackPoints.transform.GetChild(flyingTargetAttackPoints.transform.childCount - 1).position) <= 1f && reachedDestination == false)
        {
            for (int i = 0; i < flyingTargetAttackPoints.transform.childCount; i++)
            {
                if (flyingTargetAttackPoints.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    target = flyingTargetAttackPoints.transform.GetChild(i);
                    break;
                }
            }
            target.gameObject.SetActive(false);
            reachedDestination = true;
            return;
        }
    }

    IEnumerator Attack()
    {
        baseObject.GetComponent<BaseManager>().TakeDamage(enemyData.damage, "Flying enemy");
        yield return null;
    }

    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;
        enemyHealthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            if (isAttacking == true)
            {
                target.gameObject.SetActive(true); //If enemy dies while being on an attack point around the base, then the spot is made available for the next enemy to take
            }
            waveSpawner.GetComponent<WaveSpawner>().aliveEnemies -= 1;
            if (Random.value < 0.1f)
            {
                abilityManager.GetComponent<AbilityManager>().IncreaseShieldCount();
            }
            else if (Random.value < 0.1f)
            {
                abilityManager.GetComponent<AbilityManager>().IncreasePathAbilityCount();
            }

            gameManager.GetComponent<LevelManager>().ChangeMoneyValue(enemyData.moneyReward);
            Destroy(gameObject);
        }
    }
}
