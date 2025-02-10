using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string shootAnimationTrigger = "Shoot";
    [SerializeField] private string reloadAnimationTrigger = "Reload";

    [SerializeField] float fireRate = 0.1f;
    [SerializeField] float maxShootDistance = 10f;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] GameObject gun;

    [SerializeField] int magazineSize = 10;
    [SerializeField] int totalAmmo = 30;
    [SerializeField] float reloadTime = 1.2f;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAutomatic = false; // Fire mode: false = Semi, true = Auto

    void Start()
    {
        currentAmmo = magazineSize; // Start with a full magazine
    }

    void Update()
    {
        if (isReloading) return; // No shooting while reloading

        // Mode selection with "V"
        if (Input.GetKeyDown(KeyCode.V))
        {
            isAutomatic = !isAutomatic;
            Debug.Log(isAutomatic ? "Fire Mode: Automatic" : "Fire Mode: Semi-Auto");
        }

        // Semi-Auto
        if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }

        // Automatic
        if (isAutomatic && Input.GetButton("Fire1"))
        {
            TryFire();
            fireRate = 0.1f;
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && !isReloading) // Ensure reload can only happen once at a time
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

    // Hit and scan firing and ammo mechanic
    void Fire()
    {
        currentAmmo--; // Reduce ammo from the mag

        // Trigger shooting animation and set IsShooting to true to start it
        animator.SetTrigger(shootAnimationTrigger);
        animator.SetBool("IsShooting", true);

        // Raycasting system (Hit and scan method)
        Vector2 gunPosition = gun.transform.position;
        Vector2 gunDirection = gun.transform.up;
        Vector2 endPoint = gunPosition + (gunDirection * maxShootDistance);

        int layerMask = hitLayers & ~LayerMask.GetMask("Ignore Raycast");

        RaycastHit2D hit = Physics2D.Raycast(gunPosition, gunDirection, maxShootDistance, layerMask);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            endPoint = hit.point;

            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(35); // Damage to enemy
                }
            }
        }

        StartCoroutine(ShowDebugRay(gunPosition, endPoint));

        StartCoroutine(StopShootingAnimation());
    }

    IEnumerator StopShootingAnimation()
    {
        yield return new WaitForSeconds(0.25f); // Wait for animation to finish
        animator.SetBool("IsShooting", false); // Set IsShooting to false to stop the animation
    }

    // Debug Hit and scan system
    IEnumerator ShowDebugRay(Vector2 start, Vector2 end)
    {
        float duration = 0.08f;
        Debug.DrawLine(start, end, Color.red, duration);
        yield return new WaitForSeconds(duration);
    }

    // Reload mechanic (Exact same flow as shooting)
    IEnumerator Reload()
    {
        isReloading = true; // Prevent multiple reloads at the same time

        animator.SetTrigger(reloadAnimationTrigger); // Trigger the reload animation
        animator.SetBool("IsReloading", true); // Set reload state in the animator

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime); // Wait for the reload animation to finish

        // Calculate how much ammo we can reload
        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        Debug.Log("Reloaded! Ammo: " + currentAmmo + "/" + totalAmmo);

        // Stop reload animation after the time
        yield return new WaitForSeconds(0.2f); // Small buffer to ensure the reload animation finishes
        animator.SetBool("IsReloading", false); // Stop reload animation state
        isReloading = false; // Allow reload again in the future
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int ammoNeeded = 30 - totalAmmo;
            int refillAmount = Mathf.Max(0, ammoNeeded); // Ensures ammo doesn’t go over the max limit

            totalAmmo += refillAmount;
            Destroy(other.gameObject); // Destroy the ammo box after picking it up

            Debug.Log("Picked up AmmoBox! Total Ammo: " + totalAmmo);
        }
    }
}
