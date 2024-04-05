using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float timeBetweenAttacks = 0f;
    private int damage = 4;
    public float speed = 6f;
    public int health = 40;
    private float rotationSpeed = 10.0f;
    public bool isAttacking = false;
    public Transform target;
    public GameObject baseObject;
    public GameObject flyingTargetAttackPoints;
    private bool reachedDestination = false;

    void Start()
    {
        baseObject = GameObject.FindWithTag("Base");
        flyingTargetAttackPoints = GameObject.FindWithTag("FlyingAttackPoints");
        target = flyingTargetAttackPoints.transform.GetChild(0);
    }

    void Update()
    {
        if (isAttacking == false && reachedDestination == false)
        {
            Vector3 direction = target.position - transform.position;
            direction = direction + new Vector3(0, Mathf.Cos(Time.time * 3), 0);
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }
        if (reachedDestination == true)
        {
            Vector3 direction = target.position - transform.position;
            Vector3 lookDirection = flyingTargetAttackPoints.transform.position - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
            transform.rotation = rot;
        }

        if (isAttacking == true)
        {
            if (timeBetweenAttacks <= 0)
            {
                StartCoroutine(Attack());
                timeBetweenAttacks = 3f;
                return;
            }
            timeBetweenAttacks -= Time.deltaTime;
        }
        if (Vector3.Distance(transform.position, flyingTargetAttackPoints.transform.GetChild(flyingTargetAttackPoints.transform.childCount - 1).position) <= 1f && isAttacking == false)
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
            if (target != flyingTargetAttackPoints.transform.GetChild(flyingTargetAttackPoints.transform.childCount - 1))
            {
                isAttacking = true;
            }
            return;
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
    

    IEnumerator Attack()
    {
        baseObject.GetComponent<BaseManager>().TakeDamage(damage);
        yield return null;
    }
}
