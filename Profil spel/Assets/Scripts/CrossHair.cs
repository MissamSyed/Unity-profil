using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public GameObject innerCrosshair;  // Reference to the inner crosshair GameObject
    public GameObject outerCrosshair;  // Reference to the outer crosshair GameObject
    public float innerOffset = 0f;     // Offset for the inner crosshair (if needed)
    public float outerOffset = 0f;     // Offset for the outer crosshair (if needed)
    public float smoothTime = 0.1f;    // Smooth damping time for crosshair movement

    private Vector2 innerVelocity = Vector2.zero;
    private Vector2 outerVelocity = Vector2.zero;

    void Start()
    {
        // Hide the default system cursor (optional)
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world space (in 2D coordinates)
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Smoothly move the inner crosshair to the mouse position
        Vector2 innerTargetPosition = mousePosition + new Vector2(innerOffset, innerOffset);
        innerCrosshair.transform.position = Vector2.SmoothDamp(innerCrosshair.transform.position, innerTargetPosition, ref innerVelocity, smoothTime);

        // Smoothly move the outer crosshair to the mouse position
        Vector2 outerTargetPosition = mousePosition + new Vector2(outerOffset, outerOffset);
        outerCrosshair.transform.position = Vector2.SmoothDamp(outerCrosshair.transform.position, outerTargetPosition, ref outerVelocity, smoothTime);
    }
}
