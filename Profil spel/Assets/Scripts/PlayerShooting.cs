using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float fireRate = 0.5f; // Cooldown duration between shots
    [SerializeField] float maxShootDistance = 10f; // Maximum range for the hit scan
    [SerializeField] LayerMask hitLayers; // Layers to check for hits (e.g., Enemy, Obstacles)

    private float nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Fire1 is mapped to shooting (e.g., Left Mouse Button)
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        // Raycast from the player's gun position in the direction they are facing
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxShootDistance, hitLayers);

        if (hit.collider != null)
        {
            // If the ray hits something, handle it (e.g., deal damage to enemy)
            Debug.Log("Hit: " + hit.collider.name);

            // Example: If the hit object has an Enemy script, deal damage
            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10); // Example damage value
                }
            }
        }

        // Visualize the ray in the editor (for debugging purposes)
        Debug.DrawRay(transform.position, transform.up * maxShootDistance, Color.red, 0.1f);
    }
}
