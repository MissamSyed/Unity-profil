using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float fireRate = 0.5f; // Cooldown between shots
    [SerializeField] float maxShootDistance = 10f; // Max shooting range
    [SerializeField] LayerMask hitLayers; // Layers to check for hits
    [SerializeField] GameObject gun; // Reference to the gun object

    [SerializeField] int magazineSize = 2; // Bullets per magazine
    [SerializeField] int totalAmmo = 30; // Total bullets available (reserves)
    [SerializeField] float reloadTime = 4f; // Time taken to reload

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = magazineSize; // Start with a full magazine
    }

    void Update()
    {
        if (isReloading) return; // Prevent firing while reloading

        if (Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to reload
        {
            StartCoroutine(Reload());
        }
    }

    void TryFire()
    {
        if (Time.time >= nextFireTime && currentAmmo > 0)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
        else if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Reload needed.");
        }
    }

    void Fire()
    {
        currentAmmo--; // Reduce ammo

        Vector2 gunPosition = gun.transform.position;
        Vector2 gunDirection = gun.transform.up; // Direction the gun is facing
        Vector2 endPoint = gunPosition + (gunDirection * maxShootDistance);

        RaycastHit2D hit = Physics2D.Raycast(gunPosition, gunDirection, maxShootDistance, hitLayers);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            endPoint = hit.point; // Stop the ray at the hit point

            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(35);
                }
            }
        }

     
    }

    IEnumerator Reload()
    {
        if (totalAmmo > 0 && currentAmmo < magazineSize)
        {
            isReloading = true;
            Debug.Log("Reloading...");

            yield return new WaitForSeconds(reloadTime); // Wait for reload time

            int ammoNeeded = magazineSize - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;

            isReloading = false;
            Debug.Log("Reloaded! Ammo: " + currentAmmo + "/" + totalAmmo);
        }
    }
}
