using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float captureTime = 20f;  // Time to capture the checkpoint
    [SerializeField] private GameObject finalCheckpoint; // Final checkpoint object
    [SerializeField] private int checkpointIndex; // Unique index for each checkpoint
    private bool isCaptured = false;  // Whether the checkpoint is captured
    private bool isCapturing = false;  // Whether the player is currently capturing this checkpoint
    private static Transform lastCheckpointPosition;  // Last checkpoint position for respawn
    private bool playerInTrigger = false; // Whether the player is in the checkpoint trigger area
    private static int capturedCheckpoints = 0; // Track the number of captured checkpoints

    // Property to access the last checkpoint position
    public static Transform LastCheckpointPosition => lastCheckpointPosition;

    void Start()
    {
        // Make the final checkpoint inactive initially
        if (finalCheckpoint != null)
        {
            finalCheckpoint.SetActive(false); // Ensure it's hidden initially
        }
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
        if (collision.CompareTag("Player") && !isCaptured && isCapturing)
        {
            playerInTrigger = false;
            StopCoroutine(CaptureCheckpoint());
            Debug.Log("Capture process canceled: Player left the area.");
        }
    }

    private IEnumerator CaptureCheckpoint()
    {
        isCapturing = true;
        Debug.Log("Started capturing checkpoint...");

        float captureProgress = 0f;

        while (captureProgress < captureTime && playerInTrigger)  // Continue if the player is still in the area
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

        // Check if all 3 checkpoints are captured
        if (capturedCheckpoints >= 3 && finalCheckpoint != null)
        {
            // Activate the final checkpoint (make it visible and accessible)
            finalCheckpoint.SetActive(true);
            Debug.Log("Final checkpoint is now active!");

            // Transition to the next level if this is the last checkpoint
            TransitionToNextLevel();
        }

        // Reset capturing state for future use
        isCapturing = false;
    }

    private void TransitionToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Property to get how many checkpoints have been captured
    public static int GetCapturedCheckpoints()
    {
        return capturedCheckpoints;
    }
}
