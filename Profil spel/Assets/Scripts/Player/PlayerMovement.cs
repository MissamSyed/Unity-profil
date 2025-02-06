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

        // Find the enemy by tag
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
        //Get input for movement
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        //Get the mouse position in world space
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        //Ensure the Z position is set to the same as the player's Z (as it's a 2D game)
        mousePos.z = transform.position.z;

        //Calculate direction from the player to mouse position
        Vector2 lookDir = (mousePos - transform.position).normalized;

        //Calculate the angle to rotate to face the mouse
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        //Apply the rotation to the player object
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void FixedUpdate()
    {
        if (enemy != null)
        {
            //Check distance to the enemy
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

          
            if (distanceToEnemy > stoppingDistance)
            {
                //Apply normal movement
                rb.velocity = movement * moveSpeed;
            }
            else
            {
                //Calculate the direction from the player to the enemy 
                Vector2 directionToEnemy = (enemy.position - transform.position).normalized;

                
                Vector2 perpendicularDirection = new Vector2(-directionToEnemy.y, directionToEnemy.x); // Perpendicular to the enemy direction

                
                float dotProduct = Vector2.Dot(movement, directionToEnemy);

                if (dotProduct > 0) 
                {
                    //Stop movement in the direction of the enemy but allow movement in different direction
                    movement = perpendicularDirection * movement.magnitude;
                }

                
                rb.velocity = movement * moveSpeed;
            }
        }
        else
        {
            //If no enemy exists, apply normal movement with no checking
            rb.velocity = movement * moveSpeed;
        }
    }
}
