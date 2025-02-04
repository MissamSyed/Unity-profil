using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float fieldOfViewAngle = 90f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f; // Speed at which the enemy rotates to face the player
    public float stoppingDistance = 1f; // Distance at which the enemy will stop moving towards the player

    void Update()
    {
        if (player == null) return; // If no player reference, stop enemy movement

        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(transform.right, direction);

        if (distance <= detectionRange && angle <= fieldOfViewAngle * 0.5f)
        {
            // Stop moving if within the stopping distance
            if (distance > stoppingDistance)
            {
                // Move towards the player
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }

            // Rotate smoothly to face the player
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate angle to player
            float angleToTurn = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime); // Smoothly rotate the enemy towards the target angle
            transform.rotation = Quaternion.Euler(0f, 0f, angleToTurn); // Apply the rotation in 2D
        }
        else
        {
            // You can add logic for what happens if the enemy is not detecting the player
        }
    }

}
