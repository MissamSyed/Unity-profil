using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Camera mainCamera;
    public float stoppingDistance = 1f; // Distance from the enemy to stop the player's movement

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform enemy; // The enemy's transform will be fetched dynamically

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // Optional for smoothing

        // Find the enemy by its tag
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObject != null)
        {
            enemy = enemyObject.transform; // Get the enemy's transform
        }
        else
        {
            Debug.LogWarning("No object with the 'Enemy' tag found in the scene.");
        }
    }

    void Update()
    {
        // Get input for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Rotate player to face the mouse position
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Smooth rotation based on mouse position
    }

    void FixedUpdate()
    {
        if (enemy != null)
        {
            // Check distance to the enemy
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

            // If the player is farther than the stopping distance from the enemy, apply movement
            if (distanceToEnemy > stoppingDistance)
            {
                // Apply normal movement
                rb.velocity = movement * moveSpeed;
            }
            else
            {
                // Calculate the direction from the player to the enemy (2D vector)
                Vector2 directionToEnemy = (enemy.position - transform.position).normalized;

                // Calculate the perpendicular direction (90 degrees) to the enemy direction in 2D
                Vector2 perpendicularDirection = new Vector2(-directionToEnemy.y, directionToEnemy.x); // Perpendicular to the enemy direction

                // Allow movement only in the perpendicular direction (not toward the enemy)
                float dotProduct = Vector2.Dot(movement, directionToEnemy);

                if (dotProduct > 0) // If the player is moving towards the enemy
                {
                    // Stop movement in the direction of the enemy but allow movement in the perpendicular direction
                    movement = perpendicularDirection * movement.magnitude;
                }

                // Apply the movement
                rb.velocity = movement * moveSpeed;
            }
        }
    }
}
