using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFlying : MonoBehaviour
{
    public float speed;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.forward * speed;
        // transform.Translate(transform.forward*speed, Space.Self);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
