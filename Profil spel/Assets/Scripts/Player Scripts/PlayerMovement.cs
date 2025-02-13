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

    private float currentRotation = 0f;
    private float targetRotation = 0f;  // Target rotation considering recoil
    private float maxRecoilRotation = 10f; // Max recoil rotation (degrees)
    private float recoilRotationFactor = 0.6f; // Fraction of recoil applied
    private float recoilDampingSpeed = 3f;  // How fast recoil settles back to normal

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Initial player rotation
        transform.rotation = Quaternion.Euler(0, 0, 90);
        targetRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        // Input for movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Smooth movement
        Vector2 targetVelocity = input * moveSpeed;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, smoothTime);
    }

    void FixedUpdate()
    {
        // Rotate the player to face the cursor
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

        // Smoothly rotate the player to the target angle considering recoil
        targetRotation = Mathf.LerpAngle(targetRotation, angle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, targetRotation);
    }

    // Apply recoil rotation to the player smoothly
    public void ApplyRecoilRotation(Vector2 recoilOffset)
    {
        // Calculate the recoil angle based on the recoil offset
        float recoilAngle = Mathf.Atan2(recoilOffset.y, recoilOffset.x) * Mathf.Rad2Deg;

        // Clamp recoil angle to avoid extreme rotations
        recoilAngle = Mathf.Clamp(recoilAngle, -maxRecoilRotation, maxRecoilRotation);

        // Apply a fraction of the recoil angle to the player's rotation
        float recoilAdjustment = recoilAngle * recoilRotationFactor;

        // Apply recoil gradually (smooth it out)
        targetRotation += recoilAdjustment;

        // Smoothly return the target rotation back to the normal aiming rotation (using LerpAngle)
        targetRotation = Mathf.LerpAngle(targetRotation, transform.rotation.eulerAngles.z, recoilDampingSpeed * Time.deltaTime);
    }
}
