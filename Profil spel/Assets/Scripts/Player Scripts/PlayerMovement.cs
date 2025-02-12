using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    private Vector2 currentVelocity;

    private Rigidbody2D rb;
    public float rotationSpeed = 10f;

    // Recoil variables
    public float recoilAmount = 0f;  // Amount of recoil applied to the rotation
    public float recoilRecoverySpeed = 30f;  // Speed at which the recoil wears off

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 90 degrees rotation to point in the right direction
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        // Input for movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Smooth movement
        Vector2 targetVelocity = input * moveSpeed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);

        // Apply recoil over time
        if (recoilAmount != 0f)
        {
            recoilAmount = Mathf.Lerp(recoilAmount, 0f, recoilRecoverySpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        // Rotate the player to face the cursor and apply recoil
        RotatePlayerToCursor();
    }

    void RotatePlayerToCursor()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Get the direction from the player to the mouse
        Vector2 direction = (mousePosition - transform.position).normalized;

        // Calculate the angle from the direction and convert it to degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;  // Adjust for sprite's initial facing direction

        // Apply recoil by modifying the target angle
        angle += recoilAmount;

        // Smoothly rotate the player to the target angle
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Method to apply recoil when firing
    public void ApplyRecoil()
    {
        // Apply a recoil of 5 degrees when shooting (you can change this value)
        recoilAmount = 5f;
    }
}
