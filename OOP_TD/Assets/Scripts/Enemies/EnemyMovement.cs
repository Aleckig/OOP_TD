using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Enemy enemyData;
    private float currentHealth;
    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private bool isCoder = false;
    public Transform target;
    public HealthBar enemyHealthBar;
    public GameObject targetAttackPoints;
    public GameObject baseObject;
    public GameObject turningPoints;
    public GameObject waveSpawner;
    public GameObject abilityManager;
    public GameObject gameManager;
    public int pathnumber;
    public bool isAttacking = false;
    public bool reachedEnd = false;
    public bool isTeleporting = false;
    private AudioSource audioSource;
    private float rotationSpeed = 10.0f;
    public float timeBetweenAttacks = 0f;
    private int TurningPointIndex = 0;

    void Start()
    {
        currentHealth = enemyData.health;

        audioSource = GetComponent<AudioSource>();
        waveSpawner = GameObject.FindWithTag("WaveSpawner");
        pathnumber = waveSpawner.GetComponent<WaveSpawner>().pathnumb;
        turningPoints = GameObject.FindWithTag("TurningPoints");
        abilityManager = GameObject.FindWithTag("AbilityManager");
        gameManager = GameObject.FindWithTag("GameManager");

        for (int i = 0; i < turningPoints.transform.childCount; i++)
        {
            if (turningPoints.transform.GetChild(pathnumber - 1).gameObject.activeSelf == true)
            {
                target = turningPoints.transform.GetChild(pathnumber - 1).GetComponent<Path>().points[0];
                break;
            }
            else
            {
                pathnumber++;
                if (pathnumber > turningPoints.transform.childCount)
                {
                    pathnumber = pathnumber - turningPoints.transform.childCount;
                }
            }
        }
        targetAttackPoints = GameObject.FindWithTag("AttackPoints");
        baseObject = GameObject.FindWithTag("Base");
        enemyHealthBar.SetMaxHealth(enemyData.health);
    }

    void FixedUpdate()
    {
        if (isTeleporting == false)
        {
            Move();
        }

        if (isAttacking == true)
        {
            if (timeBetweenAttacks <= 0)
            {
                StartCoroutine(Attack());
                timeBetweenAttacks = 2f;

                audioSource.PlayOneShot(attackSound); // Play attack sound

                // Spawn particle effect at the position of the object
                if (isCoder == true)
                {
                    // Calculate the position slightly above the character
                    Vector3 spawnPosition = transform.position + Vector3.up * 0.5f; // Adjust the value to fit your needs

                    // Calculate the rotation to point downwards
                    Quaternion spawnRotation = Quaternion.LookRotation(Vector3.down, Vector3.forward);

                    // Instantiate the particle effect at the calculated position with the calculated rotation
                    Instantiate(particleEffectPrefab, spawnPosition, spawnRotation);
                }
                else
                {
                    Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
                }

                return;
            }
            timeBetweenAttacks -= Time.deltaTime;
        }

    }

    void GetNextTurningPoint() //Gets the next point from the path
    {
        if (TurningPointIndex >= turningPoints.transform.GetChild(pathnumber - 1).GetComponent<Path>().points.Length - 1)
        {
            for (int i = 0; i < targetAttackPoints.transform.childCount; i++)
            {
                if (targetAttackPoints.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    target = targetAttackPoints.transform.GetChild(i);
                    reachedEnd = true;
                    break;
                }
                if (i >= targetAttackPoints.transform.childCount - 1)
                {
                    waveSpawner.GetComponent<WaveSpawner>().aliveEnemies -= 1;
                    //TODO: Do something with enemies that cannot fit around the base instead of destroying them
                    Destroy(gameObject);
                }
            }
            target.gameObject.SetActive(false);
            return;
        }
        if (reachedEnd == false)
        {
            TurningPointIndex++;
            if (turningPoints.transform.GetChild(pathnumber - 1).gameObject.activeSelf == true)
            {
                target = turningPoints.transform.GetChild(pathnumber - 1).GetComponent<Path>().points[TurningPointIndex];
            }
            //else target = turningPoints.transform.GetChild(Mathf.Abs(pathnumber - 2)).GetComponent<Path>().points[TurningPointIndex];
        }
    }

    private void Move()
    {
        if (isAttacking == false)
        {
            Vector3 direction = target.position - transform.position;
            transform.Translate(direction.normalized * enemyData.movementSpeed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
        if (isAttacking == true)
        {
            Vector3 direction = targetAttackPoints.transform.position - transform.position;
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }

        if (Vector3.Distance(transform.position, target.position) <= 0.1f && isAttacking == false)
        {
            if (target.transform.parent == targetAttackPoints.transform)
            {
                isAttacking = true;
                return;
            }
            if (isAttacking == false)
            {
                GetNextTurningPoint();
            }
        }
        if (target.tag == "TunnelExit")
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Attack()
    {
        baseObject.GetComponent<BaseManager>().TakeDamage(enemyData.damage, enemyData.typeName);
        yield return null;
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;
        //Play teleport animation
        yield return new WaitForSeconds(1f);
        transform.position = target.position;
        //Play teleport animation
        yield return new WaitForSeconds(1f);
        isTeleporting = false;
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
            else if (Random.value < 0.05f)
            {
                abilityManager.GetComponent<AbilityManager>().IncreaseBaseFixCount();
            }
            gameManager.GetComponent<LevelManager>().ChangeMoneyValue(enemyData.moneyReward);
            Destroy(gameObject);
        }
    }
}
