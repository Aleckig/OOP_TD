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
    private int damage = 1;
    public int health = 40;


    void Start()
    {
        target = TurningPoints.points[0];
        targetAttackPoints = GameObject.FindWithTag("AttackPoints");
        baseObject = GameObject.FindWithTag("Base");

    }

    void Update()
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

        if (isAttacking == true)
        {
            if (timeBetweenAttacks <= 0)
            {
                StartCoroutine(Attack());
                timeBetweenAttacks = 2f;
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
        if (TurningPointIndex >= TurningPoints.points.Length - 1)
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
            target = TurningPoints.points[TurningPointIndex];
        }
    }

    IEnumerator Attack()
    {
        baseObject.GetComponent<BaseManager>().TakeDamage(damage);
        yield return null;
    }
}
