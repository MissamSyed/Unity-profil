using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float fireRate = 0.5f; // Cooldown duration between shots
    [SerializeField] float maxShootDistance = 10f; // Maximum range for the hit scan
    [SerializeField] LayerMask hitLayers; // Layers to check for hits (e.g., Enemy, Obstacles)
    [SerializeField] GameObject gun; // Reference to the gun object

    private float nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Fire1 is mapped to shooting
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
        if (gun == null) return; // Ensure the gun object is assigned

        // Get the position and rotation of the gun
        Vector2 gunPosition = gun.transform.position;
        Vector2 gunDirection = gun.transform.up; // Direction the gun is facing

        // Raycast from the gun's position in the direction it's facing
        RaycastHit2D hit = Physics2D.Raycast(gunPosition, gunDirection, maxShootDistance, hitLayers);

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
                    enemy.TakeDamage(35); // Example damage value
                }
            }
        }

        // Visualize the ray in the editor (for debugging purposes)
        Debug.DrawRay(gunPosition, gunDirection * maxShootDistance, Color.red, 0.1f);
    }
}
