using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float damage = 1f;
    public float health = 100f;
    public HealthBar enemyHealthBar;
    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioSource;
    public GameObject turningPoints;
    public int pathnumber;
    public GameObject waveSpawner;
    public bool isTeleporting = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        waveSpawner = GameObject.FindWithTag("WaveSpawner");
        pathnumber = waveSpawner.GetComponent<WaveSpawner>().pathnumb;
        turningPoints = GameObject.FindWithTag("TurningPoints");
        if (turningPoints.transform.GetChild(pathnumber - 1).gameObject.activeSelf == true) //Checks if the enemy's path has been disabled with the Path Block ability
        {
            target = turningPoints.transform.GetChild(pathnumber - 1).GetComponent<Path>().points[0];
        }
        else target = turningPoints.transform.GetChild(Mathf.Abs(pathnumber - 2)).GetComponent<Path>().points[0]; //If the path is blocked, it chooses a one next to it
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
                Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
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
            else target = turningPoints.transform.GetChild(Mathf.Abs(pathnumber - 2)).GetComponent<Path>().points[TurningPointIndex];
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
            //if (target.tag == "TunnelEntrance" && isTeleporting == false)
            //{
            //    StartCoroutine(Teleport());
            //}
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
        if (target.tag == "TunnelExit")// && isTeleporting == false)
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
