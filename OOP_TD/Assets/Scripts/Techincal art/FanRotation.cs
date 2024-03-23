using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    void Update()
    {
        // Rotate the game object around its up axis (Y-axis) at a constant speed
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
