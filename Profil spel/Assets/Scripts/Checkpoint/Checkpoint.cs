using UnityEngine;
using TMPro;  // Ensure you import the TMP namespace
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private float captureTime = 20f;  
    [SerializeField] private int checkpointIndex; 
    private bool isCaptured = false;  
    public bool isCapturing = false;  
    private static Transform lastCheckpointPosition;  
    private bool playerInTrigger = false; 
    private static int capturedCheckpoints = 0; 
    private static int totalCheckpoints = 3;

   
    [SerializeField] private TextMeshProUGUI checkpointStatusText;

   
    public static Transform LastCheckpointPosition => lastCheckpointPosition;

    void Start()
    {
        
        if (checkpointStatusText != null)
        {
            checkpointStatusText.text = "";  
        }
    }

    //Capturing mechanic
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCaptured && !isCapturing)
        {
            
            playerInTrigger = true;
            StartCoroutine(CaptureCheckpoint());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Stop capturing if the player exits the trigger area
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
            //If the player leaves the area during capture, stop the capture process
            if (isCapturing)
            {
                StopCoroutine(CaptureCheckpoint());
                isCapturing = false; //Reset so player can capture again
                UpdateCheckpointStatusText("Capture process canceled.");
                StartCoroutine(ClearCheckpointStatusText(2f)); 
            }
        }
    }

    private IEnumerator CaptureCheckpoint()
    {
        isCapturing = true;
        Debug.Log("Started capturing checkpoint...");

        //Update the text to notify the player
        UpdateCheckpointStatusText("Started capturing checkpoint...");

        float captureProgress = 0f;

        //Continue if the player is still in the area
        while (captureProgress < captureTime && playerInTrigger)
        {
            captureProgress += Time.deltaTime;
            yield return null;
        }

        //If capture is successful (captureProgress >= captureTime)
        if (captureProgress >= captureTime)
        {
            CompleteCapture();
        }
    }

    private void CompleteCapture()
    {
        //Mark checkpoint as captured
        isCaptured = true;

        //Save this checkpoint position for respawn
        lastCheckpointPosition = transform;

       
        Debug.Log("Checkpoint captured!");

        //Update the text to notify the player
        UpdateCheckpointStatusText("Checkpoint captured!");

       
        StartCoroutine(ClearCheckpointStatusText(2f));

       
        capturedCheckpoints++;

        //After capturing, check if all checkpoints are captured
        if (capturedCheckpoints >= totalCheckpoints)
        {
           
            TransitionToNextLevel();
        }

      
        isCapturing = false;
    }

    private void TransitionToNextLevel()
    {
        //Check if the scene is in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    //Property to get how many checkpoints have been captured
    public static int GetCapturedCheckpoints()
    {
        return capturedCheckpoints;
    }

    //Method to update the checkpoint status text
    private void UpdateCheckpointStatusText(string message)
    {
        if (checkpointStatusText != null)
        {
            checkpointStatusText.text = message;
        }
    }

    //Clear checkpoint status
    private IEnumerator ClearCheckpointStatusText(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (checkpointStatusText != null)
        {
            checkpointStatusText.text = ""; 
        }
    }
}
