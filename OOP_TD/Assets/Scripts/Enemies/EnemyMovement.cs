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
    public GameObject turningPoints;
    public int pathnumber;
    public GameObject waveSpawner;


    void Start()
    {
        waveSpawner = GameObject.FindWithTag("WaveSpawner");
        pathnumber = waveSpawner.GetComponent<WaveSpawner>().pathnumb;
        turningPoints = GameObject.FindWithTag("TurningPoints");
        if (turningPoints.transform.GetChild(pathnumber - 1).gameObject.activeSelf == true)
        {
            target = turningPoints.transform.GetChild(pathnumber - 1).GetComponent<Path>().points[0];
        }
        else target = turningPoints.transform.GetChild(Mathf.Abs(pathnumber - 2)).GetComponent<Path>().points[0];
        targetAttackPoints = GameObject.FindWithTag("AttackPoints");
        baseObject = GameObject.FindWithTag("Base");
        enemyHealthBar.SetMaxHealth(health);
    }

    void FixedUpdate()
    {
        Move();

        if (isAttacking == true)
        {
            this.transform.GetChild(0).GetComponent<Animator>().Rebind();
            this.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            //TODO: Play attack animation
            if (timeBetweenAttacks <= 0)
            {
                StartCoroutine(Attack());
                timeBetweenAttacks = 2f;

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
                target.gameObject.SetActive(true);
            }
            Destroy(gameObject);
        }

    }

    void GetNextTurningPoint()
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
                    //TODO: Do something with enemies that cannot fit around the base
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
    }

    IEnumerator Attack()
    {
        baseObject.GetComponent<BaseManager>().TakeDamage(damage);
        yield return null;
    }
}
