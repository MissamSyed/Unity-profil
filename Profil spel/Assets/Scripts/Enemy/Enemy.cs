using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHealth = 100;
    public delegate void DeathEvent(); 
    public event DeathEvent onDeath; //Declare the event

    //Damage mechanic
    public void TakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;
        Debug.Log("Enemy took damage. Health left: " + enemyHealth);

        //If health drops below or is 0, die
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    //Death mechanic 
    void Die()
    {
        Debug.Log("Enemy died!");
        onDeath?.Invoke(); 
        Destroy(gameObject); // Destroy the current enemy object
    }
}
