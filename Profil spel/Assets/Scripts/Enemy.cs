using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100; // Example health value

    // Method to deal damage to the enemy
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Enemy took damage. Health left: " + health);

        // Check if the enemy has died
        if (health <= 0)
        {
            Die();
        }
    }

    // Method for enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
