using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyMovement : MonoBehaviour
{

    public float speed = 3f;
    public Transform target;
    private int TurningPointIndex = 0;
    private float rotationSpeed = 10.0f;
    public bool isAttacking = false;
    public GameObject targetAttackPoints;
    public bool reachedEnd = false;
    public GameObject baseObject;
    public float timeBetweenAttacks = 0f;
    [SerializeField] private float damage = 1f;
    public float health = 100f;
    public HealthBar enemyHealthBar;
    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private bool isCoder = false;
    private AudioSource audioSource;
    public GameObject turningPoints;
    public int pathnumber;
    public GameObject waveSpawner;
    public bool isTeleporting = false;
    public GameObject abilityManager;
    public GameObject gameManager;

    void Start()
    {
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
        enemyHealthBar.SetMaxHealth(health);
    }

    void FixedUpdate()
    {
        if (isTeleporting == false)
        {
            Move();
        }

        if (isAttacking == true)
        {
            //this.transform.GetChild(0).GetComponent<Animator>().Rebind();
            //this.transform.GetChild(0).GetComponent<Animator>().enabled = false; //Stops the animation when attacking begins. TODO: Edit animations with Unity's animation states
            //TODO: Play attack animation
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

        if (health <= 0)
        {
            if (isAttacking == true)
            {
                target.gameObject.SetActive(true); //If enemy dies while being on an attack point around the base, then the spot is made available for the next enemy to take
            }
            waveSpawner.GetComponent<WaveSpawner>().aliveEnemies -= 1;
            if (Random.value < 0.3f)
            {
                abilityManager.GetComponent<AbilityManager>().IncreaseShieldCount();
            }
            else if (Random.value < 0.3f)
            {
                abilityManager.GetComponent<AbilityManager>().IncreasePathAbilityCount();
            }
            gameManager.GetComponent<LevelManager>().ChangeMoneyValue(10);
            Destroy(gameObject);
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
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
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
        baseObject.GetComponent<BaseManager>().TakeDamage(damage);
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
}
