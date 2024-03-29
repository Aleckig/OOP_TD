using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public float speed = 3f;
    private Transform target;
    private int TurningPointIndex = 0;
    private float rotationSpeed = 10.0f;


    void Start()
    {
        target = TurningPoints.points[0];
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.rotation = rot;

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            GetNextTurningPoint();
        }
    }

    void GetNextTurningPoint()
    {
        if (TurningPointIndex >= TurningPoints.points.Length -1)
        {
            Destroy(gameObject);
            //TODO: enemy will start attacking base
            return;
        }
        TurningPointIndex++;
        target = TurningPoints.points[TurningPointIndex];
    }
}
