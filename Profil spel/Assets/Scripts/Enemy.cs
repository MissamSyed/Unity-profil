using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enenmy : MonoBehaviour
{
    [SerializeField] private int health = 3; // Fienden har 3 HP
    private bool isHit = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isHit) // Om fienden träffas av en kula
        {
            isHit = true;
            TakeDamage(1);
            Destroy(collision.gameObject); // Förstör kulan vid träff
            StartCoroutine(ResetHit());
        }
    }

    IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.8f); // Förhindrar flera träffar samtidigt
        isHit = false;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject); // Fienden förstörs
    }
}
