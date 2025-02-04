using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public delegate void DeathEvent(); // Event to handle death
    public event DeathEvent onDeath;   // Declare the event

    // Take damage method
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Enemy took damage. Health left: " + health);

        // If health drops below or equals 0, call Die method
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle death logic
    void Die()
    {
        Debug.Log("Enemy died!");
        onDeath?.Invoke(); // Trigger the death event
        Destroy(gameObject);  // Destroy the current enemy object
    }
}
