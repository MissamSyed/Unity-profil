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


    void Start()

    {

        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; //Smoother movement

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

        rb.velocity = movement * moveSpeed;
    }

}
