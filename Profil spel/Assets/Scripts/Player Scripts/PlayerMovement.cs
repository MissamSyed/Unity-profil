using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    public Camera mainCamera;
    public float stoppingDistance = 1f; 

    private Rigidbody2D rb;
    private Vector2 movement;
    private Transform enemy; 

    void Start()

    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; //Smoother movement

        
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObject != null)
        {
            enemy = enemyObject.transform; 
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
        mousePos.z = transform.position.z; 

        // Calculate direction from player to mouse position
        Vector2 lookDir = (mousePos - transform.position).normalized;

        // Calculate the angle to rotate (adjust for a downward-facing sprite)
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        //Add 90 for a downward-facing sprite because of the cursor missplace
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90)); 
    }

    void FixedUpdate()
    {
        //Get input for movement (Horizontal and Vertical axes)
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

       
        if (enemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

            if (distanceToEnemy > stoppingDistance)
            {
                
                rb.velocity = movement * moveSpeed;
            }
            else
            {
                //Direction to stop near the enemy
                Vector2 directionToEnemy = (enemy.position - transform.position).normalized;
                Vector2 perpendicularDirection = new Vector2(-directionToEnemy.y, directionToEnemy.x); 

                //Stop movement in enemy direction
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
            //If no enemy exists, apply normal movement (Isn't going to happen really)
            rb.velocity = movement * moveSpeed;
        }
    }

}
