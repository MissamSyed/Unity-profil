using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;

    //Take damage system
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; 

        //Check if health reaches 0 or below, die if it is 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Respawn();
        }

        Debug.Log("Player's current health: " + currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealthItem"))
        {
            
            HealPlayer();

            Destroy(other.gameObject); //Destroy the healing item after touching player
            Debug.Log("Picked Up Healing Item");
        }
    }

    // Player death system
    private void Respawn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name); //Reload the current scene (respawn the player)
    }

    //Heal the player by 33% of their max health
    private void HealPlayer()
    {
        int healingAmount = Mathf.FloorToInt(maxHealth * 0.33f);  
        currentHealth += healingAmount;  

        if (currentHealth > maxHealth) //Ensure health does not exceed max health
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Player healed by " + healingAmount + " health");
        Debug.Log("Player's current health: " + currentHealth);
    }
}
