using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentPlayerHealth = 100;
    public int maxHealth = 100;
    private Vector3 originalSpawnPoint; //Stores spawn position

    void Start()
    {
        currentPlayerHealth = maxHealth;
        originalSpawnPoint = transform.position; //Save the original spawn position
    }

    //Take damage system
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

    //HealingItem
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthItem"))
        {
            Destroy(other.gameObject); 
            Debug.Log("Picked Up Healing Item!");
        }
    }

    //Respawn at last checkpoint captured or return to main menu
    private IEnumerator Respawn()
    {
        Debug.Log("Player Died! Respawning...");

        yield return new WaitForSeconds(2f); //Delay Before spawn

        if (CheckPoint.LastCheckpointPosition != null)
        {
            //Respawn at the last checkpoint 
            transform.position = CheckPoint.LastCheckpointPosition.position;
            Debug.Log("Respawned at last checkpoint!");
        }
        else
        {
            //No checkpoint captured, return to the Main Menu
            Debug.Log("No checkpoint captured! Returning to Main Menu...");
            SceneManager.LoadScene("Main menu"); 
            yield break; 
        }

        //Reset health after respawn
        currentPlayerHealth = maxHealth;
    }
}
