using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public GameObject innerCrosshair;  // Reference to the inner crosshair GameObject (main one)
    public GameObject outerCrosshair;  // Reference to the outer crosshair GameObject
    public float innerOffset = 0f;     // Offset for the inner crosshair (if needed)
    public float outerOffset = 0f;     // Offset for the outer crosshair (if needed)
    public float smoothTime = 0.1f;    // Smooth damping time for crosshair movement

    // Recoil settings
    public float recoilAmount = 10f;  // How much recoil to apply to the crosshair
    public float recoilRecoverySpeed = 5f;  // Speed at which recoil recovers
    public float playerRotationSpeed = 10f; // Speed at which the player rotates with recoil

    private Vector2 innerVelocity = Vector2.zero;
    private Vector2 outerVelocity = Vector2.zero;

    private Vector2 innerRecoilOffset = Vector2.zero;  // Recoil offset for the inner crosshair

    private bool isRecoiling = false;  // To check if recoil is happening

    private Transform playerTransform;  // Reference to the player transform for rotation

    void Start()
    {
        // Hide the default system cursor (optional)
        Cursor.visible = false;

        // Get the player's transform (if this script is attached to a different object)
        playerTransform = transform;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world space (in 2D coordinates)
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Smoothly move the inner crosshair to the mouse position, adding recoil offset
        Vector2 innerTargetPosition = mousePosition + new Vector2(innerOffset, innerOffset) + innerRecoilOffset;
        innerCrosshair.transform.position = Vector2.SmoothDamp(innerCrosshair.transform.position, innerTargetPosition, ref innerVelocity, smoothTime);

        // Smoothly move the outer crosshair to the mouse position (no recoil here)
        Vector2 outerTargetPosition = mousePosition + new Vector2(outerOffset, outerOffset);
        outerCrosshair.transform.position = Vector2.SmoothDamp(outerCrosshair.transform.position, outerTargetPosition, ref outerVelocity, smoothTime);

        // Smoothly recover the recoil offset (inner crosshair recoil recovery)
        if (!isRecoiling)
        {
            // Recover recoil effect back to zero smoothly
            innerRecoilOffset = Vector2.Lerp(innerRecoilOffset, Vector2.zero, Time.deltaTime * recoilRecoverySpeed);
        }

        // Rotate the player according to the inner crosshair recoil
        RotatePlayerWithRecoil();
    }

    // This method will be called when shooting to apply recoil to the crosshair
    public void ApplyRecoil()
    {
        // Apply a random recoil to the inner crosshair only
        innerRecoilOffset = new Vector2(Random.Range(-recoilAmount, recoilAmount), Random.Range(-recoilAmount, recoilAmount));

        isRecoiling = true;

        // Start the recoil recovery after a short delay
        StartCoroutine(RecoverRecoil());
    }

    // Rotate the player based on the inner crosshair's recoil
    private void RotatePlayerWithRecoil()
    {
        // Only rotate the player when there's recoil
        if (innerRecoilOffset != Vector2.zero)
        {
            // Calculate the recoil direction (angle) from the recoil offset
            float angle = Mathf.Atan2(innerRecoilOffset.y, innerRecoilOffset.x) * Mathf.Rad2Deg;

            // Apply rotation to the player based on the recoil
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, Quaternion.Euler(0, 0, angle), playerRotationSpeed * Time.deltaTime);
        }
    }

    // Coroutine to handle recoil recovery after a short delay (for realism)
    private IEnumerator RecoverRecoil()
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short delay after recoil
        isRecoiling = false;
    }
}
