using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of rotation
    public float recoilAmount = 5f;  // Amount the cursor moves due to recoil
    public float recoilDuration = 0.1f; // Duration of the recoil effect

    private Vector3 originalMousePos; // Original mouse position
    private bool isRecoiling = false; // Flag to track recoil state

    void Update()
    {
        if (isRecoiling) return; // Skip rotation update during recoil

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;  // Ensure the Z position is 0 for 2D

        // Calculate direction from player to mouse position
        Vector3 direction = (mousePos - transform.position).normalized;

        // Get the target angle for the rotation
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate towards the target angle
        float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);

        // Apply the smooth rotation to the player
        transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
    }

    // This function is called to apply the recoil effect when the player shoots
    public void ApplyRecoil()
    {
        StartCoroutine(RecoilCoroutine());
    }

    // Coroutine to apply recoil (shift the aim temporarily)
    private IEnumerator RecoilCoroutine()
    {
        // Save the current mouse position to reset after recoil
        originalMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        originalMousePos.z = 0;  // Set Z to 0 for 2D

        // Determine the recoil direction (can be adjusted for left/right or up/down)
        Vector3 recoilDirection = new Vector3(Random.Range(-recoilAmount, recoilAmount), Random.Range(-recoilAmount, recoilAmount), 0);

        // Apply the recoil by shifting the mouse position temporarily
        Vector3 newMousePos = originalMousePos + recoilDirection;
        Camera.main.ScreenToWorldPoint(newMousePos);  // Update mouse position

        // Set the recoil flag to true to prevent normal rotation during recoil
        isRecoiling = true;

        // Wait for the recoil to last for the given duration
        yield return new WaitForSeconds(recoilDuration);

        // After recoil, reset the mouse position back to the original position
        Camera.main.ScreenToWorldPoint(originalMousePos);

        // Set recoil flag back to false
        isRecoiling = false;
    }
}
