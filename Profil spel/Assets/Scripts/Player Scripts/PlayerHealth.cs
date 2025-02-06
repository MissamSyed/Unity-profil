using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;  //Starting health of the player
    public int maxHealth = 100;      //Maximum health of the player

    //Take damage mechanic
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; //Reduce health by the damage amount

        //Check if health reaches 0 or below die if it is 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); 
        }

        Debug.Log("Player's current health: " + currentHealth);
    }

    //Player death mechanic
    private void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject); // Example: Destroy the player GameObject
    }


}
