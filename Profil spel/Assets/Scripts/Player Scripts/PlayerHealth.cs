using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentPlayerHealth = 100;
    public int maxHealth = 100;
    private Vector3 originalSpawnPoint; // Stores the initial spawn position

    void Start()
    {
        currentPlayerHealth = maxHealth;
        originalSpawnPoint = transform.position; // Save the initial spawn position
    }

    // Take damage system
    public void TakeDamage(int damageAmount)
    {
        currentPlayerHealth -= damageAmount;

        if (currentPlayerHealth <= 0)
        {
            currentPlayerHealth = 0;
            StartCoroutine(Respawn());
        }

        Debug.Log("Player's current health: " + currentPlayerHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthItem"))
        {
            Destroy(other.gameObject); // Destroy the healing item after touching the player
            Debug.Log("Picked Up Healing Item!");
        }
    }

    // Respawn at last checkpoint or return to main menu
    private IEnumerator Respawn()
    {
        Debug.Log("Player Died! Respawning...");

        yield return new WaitForSeconds(2f); // Optional delay before respawning

        if (CheckPoint.LastCheckpointPosition != null)
        {
            // Respawn at the last checkpoint position
            transform.position = CheckPoint.LastCheckpointPosition.position;
            Debug.Log("Respawned at last checkpoint!");
        }
        else
        {
            // No checkpoint captured, return to the Main Menu
            Debug.Log("No checkpoint captured! Returning to Main Menu...");
            SceneManager.LoadScene("MainMenu"); // Replace with your actual main menu scene name
            yield break; // Stop execution
        }

        // Reset health after respawning
        currentPlayerHealth = maxHealth;
    }
}
