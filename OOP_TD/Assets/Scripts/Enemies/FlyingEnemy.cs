using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float timeBetweenAttacks = 0f;
    private float damage = 4f;
    public float speed = 6f;
    public float health = 40f;
    private float rotationSpeed = 10.0f;
    public bool isAttacking = false;
    public Transform target;
    public GameObject baseObject;
    public GameObject flyingTargetAttackPoints;
    [SerializeField] private GameObject particleEffectPrefab;
    private bool reachedDestination = false;

    void Start()
    {
        baseObject = GameObject.FindWithTag("Base");
        flyingTargetAttackPoints = GameObject.FindWithTag("FlyingAttackPoints");
        target = flyingTargetAttackPoints.transform.GetChild(0);
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
        
        if (health <= 0f)
        {
            if (isAttacking == true)
            {
                target.gameObject.SetActive(true); //Re-enable attack point for next enemy to take the spot
            }
            Destroy(gameObject); //Kill the enemy
        }
    }
    
    private void Move()
    {
        if (isAttacking == false && reachedDestination == false) //Moves straight towards FlyingDestination, which is the point where enemies will turn towards the first available attack point around the base
        {
            Vector3 direction = target.position - transform.position;
            direction = direction + new Vector3(0, Mathf.Cos(Time.time * 3), 0);
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
        if (isAttacking == false && reachedDestination == true) //Moves towards the first available spot around the base
        {
            Vector3 direction = target.position - transform.position;
            Vector3 lookDirection = flyingTargetAttackPoints.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
    }

    private void ChooseAttackPoint() //Choose the first available attack point around the base and then disable it
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
        baseObject.GetComponent<BaseManager>().TakeDamage(damage);
        yield return null;
    }
}
