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
        currentHealth -= damageAmount; //Reduce health by the damage amount

        //Check if health reaches 0 or below die if it is 0 or below
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
            
            Destroy(other.gameObject); //Destroy the ammo box after touching the player

            Debug.Log("Picked Up Healing Item: ");
        }
    }

    //Player death system
    private void Respawn()
    {
            
     Scene currentScene = SceneManager.GetActiveScene();
     SceneManager.LoadScene(currentScene.name);


    }


}
