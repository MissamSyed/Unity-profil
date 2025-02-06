using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;  //Reference to the Animator
    [SerializeField] private string shootAnimationTrigger = "Shoot"; //The trigger for the shooting animation

    [SerializeField] float fireRate = 0.1f; //Fire rate for automatic mode
    [SerializeField] float maxShootDistance = 10f; //Max shooting range
    [SerializeField] LayerMask hitLayers; //Layers to check for hits so only this takes damage
    [SerializeField] GameObject gun; //Reference to the gun object

    [SerializeField] int magazineSize = 10; //Bullets in the magazine 
    [SerializeField] int totalAmmo = 30; //Total bullets available in whole (reserves) (about 3 magazines)
    [SerializeField] float reloadTime = 1.5f; //Time to reload

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAutomatic = false; //Fire mode: false = Semi, true = Auto

    void Start()
    {
        currentAmmo = magazineSize; //Start with a full magazine
    }

    void Update()
    {
        if (isReloading) return; //No shooting while reload

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

        //Reloading mechanic
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
        currentAmmo--; // Reduce ammo

        // Trigger shooting animation and set IsShooting to true
        animator.SetTrigger(shootAnimationTrigger); // Trigger shoot animation
        animator.SetBool("IsShooting", true); // Set IsShooting to true to transition to shooting state

        // Logic for raycasting
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
                    enemy.TakeDamage(35);  // Damage to enemy
                }
            }
        }

        StartCoroutine(ShowDebugRay(gunPosition, endPoint));

        // Make sure IsShooting is set back to false when shooting finishes
        StartCoroutine(StopShootingAnimation());  // Add a delay to set IsShooting back to false
    }

    // Stop shooting after the animation is done (you can adjust time if necessary)
    IEnumerator StopShootingAnimation()
    {
        // Wait for the animation to finish (this time should match your animation duration)
        yield return new WaitForSeconds(0.25f); // Adjust this to match the length of your shooting animation
        animator.SetBool("IsShooting", false); // Set IsShooting to false
    }


    //Debug Hit and scan system
    IEnumerator ShowDebugRay(Vector2 start, Vector2 end)
    {
        float duration = 0.08f;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int ammoNeeded = 30 - totalAmmo; //How much ammo can be refilled
            int refillAmount = Mathf.Max(0, ammoNeeded); //Checks so it doesnt go above 30

            totalAmmo += refillAmount; 
            Destroy(other.gameObject); //Destroy the ammo box after touching the player

            Debug.Log("Picked up AmmoBox! Total Ammo: " + totalAmmo);
        }
    }

}
