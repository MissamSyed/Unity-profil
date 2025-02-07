using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damageAmount = 10; 

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the player collided with the knife
        if (other.CompareTag("Player"))
        {
            
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount); //Apply damage to the player
                Debug.Log("Player hit by knife! Damage: " + damageAmount);
            }
        }
    }
}
