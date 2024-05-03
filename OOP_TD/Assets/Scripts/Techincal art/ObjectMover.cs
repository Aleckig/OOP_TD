using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public float distance = 10f; // Total distance to move
    public bool moveRight = true; // Initial direction of movement

    private Vector3 initialPosition;
    private int directionSign = 1; // Direction of movement (+1 for right, -1 for left)

    void Start()
    {
        initialPosition = transform.position;
        directionSign = moveRight ? 1 : -1;
    }

    void Update()
    {
        // Calculate the target position based on direction
        Vector3 targetPosition = initialPosition + Vector3.right * distance * directionSign;

        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Reverse the direction
            directionSign *= -1;
        }
    }
}
