using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Camera mainCamera;
    public float stoppingDistance = 1f; //Distance from the enemy to stop the player's movement

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform enemy; 

    void Start()

    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; //For smoother movement

        //Find the enemy by tag
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObject != null)
        {
            enemy = enemyObject.transform; //Get the enemy's transform
        }
        else
        {
            Debug.LogWarning("No object with the 'Enemy' tag found in the scene.");
        }
    }

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z; // Keep the Z position the same

        // Calculate direction from player to mouse position
        Vector2 lookDir = (mousePos - transform.position).normalized;

        // Calculate the angle to rotate (adjust for a downward-facing sprite)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Apply the rotation to the player (adjust for downward-facing sprite)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90)); // Add 90 for a downward-facing sprite
    }

    void FixedUpdate()
    {
        // Get input for movement (Horizontal and Vertical axes)
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Normal movement logic (check distance to enemy)
        if (enemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

            if (distanceToEnemy > stoppingDistance)
            {
                // Apply normal movement
                rb.velocity = movement * moveSpeed;
            }
            else
            {
                // Calculate direction to stop near the enemy
                Vector2 directionToEnemy = (enemy.position - transform.position).normalized;
                Vector2 perpendicularDirection = new Vector2(-directionToEnemy.y, directionToEnemy.x); // Perpendicular movement to avoid moving directly towards the enemy

                // Stop movement in the direction of the enemy
                float dotProduct = Vector2.Dot(movement, directionToEnemy);
                if (dotProduct > 0)
                {
                    movement = perpendicularDirection * movement.magnitude;
                }

                rb.velocity = movement * moveSpeed;
            }
        }
        else
        {
            // If no enemy exists, apply normal movement
            rb.velocity = movement * moveSpeed;
        }
    }

}
