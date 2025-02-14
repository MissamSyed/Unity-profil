using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    public GameObject innerCrosshair;  // Reference to the inner crosshair GameObject
    public GameObject outerCrosshair;  // Reference to the outer crosshair GameObject
    public float innerOffset = 0f;     // Offset for the inner crosshair
    public float outerOffset = 0f;     // Offset for the outer crosshair
    public float smoothTime = 0.1f;    // Smooth damping time for crosshair movement

    // Recoil settings
    public float recoilAmount = 10f;  // How much recoil to apply to the crosshair
    public float recoilRecoverySpeed = 5f;  // Speed at which recoil recovers

    private Vector2 innerVelocity = Vector2.zero;
    private Vector2 outerVelocity = Vector2.zero;

    private Vector2 innerRecoilOffset = Vector2.zero;  // Recoil offset for the inner crosshair
    private bool isRecoiling = false;  // To check if recoil is happening

    private Transform playerTransform;  // Reference to the player transform for rotation

    // Reference to the player movement script
    public PlayerMovement playerMovement;

    void Start()
    {
        // Hide the default system cursor (optional)
        Cursor.visible = false;

        // Get the player's movement script (for applying recoil rotation)
        playerTransform = playerMovement.transform;
    }

    void Update()
    {
        // Get the mouse position in screen space
        Vector2 mousePosition = Input.mousePosition;

        // Convert mouse position to world space (in 2D coordinates)
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Apply recoil to the mouse position by adjusting it with the recoil offset
        Vector2 adjustedMousePosition = mousePosition + new Vector2(innerRecoilOffset.x, innerRecoilOffset.y);

        // Smoothly move the inner crosshair to the adjusted mouse position (recoil applied here)
        Vector2 innerTargetPosition = adjustedMousePosition + new Vector2(innerOffset, innerOffset);
        innerCrosshair.transform.position = Vector2.SmoothDamp(innerCrosshair.transform.position, innerTargetPosition, ref innerVelocity, smoothTime);

        // Smoothly move the outer crosshair to the mouse position (no recoil here)
        Vector2 outerTargetPosition = mousePosition + new Vector2(outerOffset, outerOffset);
        outerCrosshair.transform.position = Vector2.SmoothDamp(outerCrosshair.transform.position, outerTargetPosition, ref outerVelocity, smoothTime);

        // Smoothly recover the recoil offset (inner crosshair recoil recovery)
        if (!isRecoiling)
        {
            innerRecoilOffset = Vector2.Lerp(innerRecoilOffset, Vector2.zero, Time.deltaTime * recoilRecoverySpeed);
        }

        // Apply the recoil to both the crosshair and player rotation
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
        // Apply recoil to player rotation
        playerMovement.ApplyRecoilRotation(innerRecoilOffset);
    }

    // Coroutine to handle recoil recovery after a short delay (for realism)
    private IEnumerator RecoverRecoil()
    {
        yield return new WaitForSeconds(0.1f); // Wait for a short delay after recoil
        isRecoiling = false;
    }
}
