using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Transform respawnPoint; 
    public float respawnDelay = 2f;

    private void Start()
    {
        currentHealth = maxHealth;

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took damage. Health left: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
        Invoke("Respawn", respawnDelay);

    }

    void Respawn()
    {
        transform.position = respawnPoint.position; 
        currentHealth = maxHealth;
        gameObject.SetActive(true); 
        Debug.Log("Player respawned!");
    }
}

