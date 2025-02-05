using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float fireRate = 0.1f; //Fire rate for automatic mode
    [SerializeField] float maxShootDistance = 10f; //Max shooting range
    [SerializeField] LayerMask hitLayers; // Layers to check for hits
    [SerializeField] GameObject gun; // Reference to the gun object

    [SerializeField] int magazineSize = 10; // Bullets per magazine
    [SerializeField] int totalAmmo = 30; // Total bullets available (reserves)
    [SerializeField] float reloadTime = 1.5f; // Time taken to reload

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAutomatic = false; // Fire mode: false = single, true = auto

    void Start()
    {
        currentAmmo = magazineSize; // Start with a full magazine
    }

    void Update()
    {
        if (isReloading) return; // Prevent shooting while reloading

        //Mode selection with "V"
        if (Input.GetKeyDown(KeyCode.V))
        {
            isAutomatic = !isAutomatic;
            Debug.Log(isAutomatic ? "Fire Mode: Automatic" : "Fire Mode: Semi-Auto");
        }

        //Semi-Auto (One shot per click)
        if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }

        //Automatic (Hold for continuous fire)
        if (isAutomatic && Input.GetButton("Fire1"))
        {
            TryFire();
            fireRate = 0.1f;
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R))
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

    //Hit and scan firing and ammo mechanic
    void Fire()
    {
        currentAmmo--; //Reduce ammo

        Vector2 gunPosition = gun.transform.position;
        Vector2 gunDirection = gun.transform.up;
        Vector2 endPoint = gunPosition + (gunDirection * maxShootDistance);

        //Ignore the knife because of the Ignore Raycast tag
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
                    enemy.TakeDamage(35);
                    
                }
            }
        }

        StartCoroutine(ShowDebugRay(gunPosition, endPoint));
    }


    //Debug Hit and scan system
    IEnumerator ShowDebugRay(Vector2 start, Vector2 end)
    {
        float duration = 0.1f;
        Debug.DrawLine(start, end, Color.red, duration);
        yield return new WaitForSeconds(duration);
    }

    //Reload mechanic
    IEnumerator Reload()
    {
        if (totalAmmo > 0 && currentAmmo < magazineSize)
        {
            isReloading = true;
            Debug.Log("Reloading...");

            yield return new WaitForSeconds(reloadTime);

            int ammoNeeded = magazineSize - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

            currentAmmo += ammoToReload;
            totalAmmo -= ammoToReload;

            isReloading = false;
            Debug.Log("Reloaded! Ammo: " + currentAmmo + "/" + totalAmmo);
        }
        else
        {
            Debug.Log("No more ammo left!");
        }
    }
}
