using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 100;  
    public int maxHealth = 100;      

    //Take damage system
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

    //Player death system
    private void Die()
    {
        Debug.Log("Player has died.");
        Destroy(gameObject);
    }


}
