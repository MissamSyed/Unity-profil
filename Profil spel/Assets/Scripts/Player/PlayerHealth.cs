using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;  // Starting health of the player
    public int maxHealth = 100;      // Maximum health of the player

    // Method to take damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduce health by the damage amount

        // Check if health reaches 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); // Call the Die method if health reaches 0
        }

        Debug.Log("Player's current health: " + currentHealth);
    }

    // Method to handle the player's death
    private void Die()
    {
        Debug.Log("Player has died.");
        // You can add your death logic here (e.g., disable the player, restart the scene, etc.)
        Destroy(gameObject); // Example: Destroy the player GameObject
    }
}
