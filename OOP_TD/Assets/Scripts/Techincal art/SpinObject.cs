using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Speed of rotation
    public float rotationSpeed = 500f;

    void Update()
    {
        // Rotate the object around its local up axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
