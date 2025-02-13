using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float captureTime = 20f;  // Time to capture the checkpoint
    [SerializeField] private int checkpointIndex; // Unique index for each checkpoint
    private bool isCaptured = false;  // Whether the checkpoint is captured
    private bool isCapturing = false;  // Whether the player is currently capturing this checkpoint
    private static Transform lastCheckpointPosition;  // Last checkpoint position for respawn
    private bool playerInTrigger = false; // Whether the player is in the checkpoint trigger area
    private static int capturedCheckpoints = 0; // Track the number of captured checkpoints
    private static int totalCheckpoints = 3; // Total number of checkpoints in the level (adjust if needed)

    // Property to access the last checkpoint position
    public static Transform LastCheckpointPosition => lastCheckpointPosition;

    void Start()
    {
        // You can initialize other elements if needed
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCaptured && !isCapturing)
        {
            // Start capturing the checkpoint when the player enters the area
            playerInTrigger = true;
            StartCoroutine(CaptureCheckpoint());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Stop capturing if the player exits the trigger area
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
            // If the player leaves the area during capture, stop the capture process
            if (isCapturing)
            {
                StopCoroutine(CaptureCheckpoint());
                isCapturing = false; // Reset isCapturing so we can capture again
                Debug.Log("Capture process canceled: Player left the area.");
            }
        }
    }

    private IEnumerator CaptureCheckpoint()
    {
        isCapturing = true;
        Debug.Log("Started capturing checkpoint...");

        float captureProgress = 0f;

        // Continue if the player is still in the area
        while (captureProgress < captureTime && playerInTrigger)
        {
            captureProgress += Time.deltaTime;
            yield return null;
        }

        // If capture is successful (captureProgress >= captureTime)
        if (captureProgress >= captureTime)
        {
            CompleteCapture();
        }
    }

    private void CompleteCapture()
    {
        // Mark checkpoint as captured
        isCaptured = true;

        // Save this checkpoint position for respawn
        lastCheckpointPosition = transform;

        // Log checkpoint capture
        Debug.Log("Checkpoint captured!");

        // Increment captured checkpoints
        capturedCheckpoints++;

        // After capturing, check if all checkpoints are captured
        if (capturedCheckpoints >= totalCheckpoints)
        {
            // Transition to the next level as soon as all checkpoints are captured
            TransitionToNextLevel();
        }

        // Reset capturing state for future use
        isCapturing = false;
    }

    private void TransitionToNextLevel()
    {
        // This assumes your scenes are ordered correctly in the build settings.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Property to get how many checkpoints have been captured
    public static int GetCapturedCheckpoints()
    {
        return capturedCheckpoints;
    }
}
