using System.Collections;
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

    [SerializeField] int magazineSize = 30;
    [SerializeField] int totalAmmo = 60;
    [SerializeField] float reloadTime = 1.5f;

    // Bullet Casing Related Fields
    [SerializeField] private GameObject casingPrefab;  // Bullet casing particle system prefab
    [SerializeField] private Transform casingSpawnPoint;  // Where the casing will spawn (near gun barrel)
    [SerializeField] private float casingEjectionForce = 5f; // Force applied to casing ejection

    // Add reference to CrossHair script
    [SerializeField] private CrossHair crosshair; // Reference to the CrossHair script

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAutomatic = false; // Fire mode: false = Semi, true = Auto

    private HUD hud; // Reference to the HUD script

    void Start()
    {
        // Find the Cameras GameObject by tag
        GameObject camerasGameObject = GameObject.FindWithTag("Cameras");

        // Ensure the Cameras GameObject was found
        if (camerasGameObject != null)
        {
            // Get the HUD component from the Cameras GameObject
            hud = camerasGameObject.GetComponent<HUD>();
        }

        // Optionally check if the HUD script was found
        if (hud != null)
        {
            hud.SetAmoCount(currentAmmo); // Set the initial ammo count on HUD
        }
        else
        {
            Debug.LogError("HUD script not found on the Cameras GameObject!");
        }

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

    // Hit and scan firing and ammo mechanic
    void Fire()
    {
        currentAmmo--; // Reduce ammo from the mag

        // Trigger shooting animation and set IsShooting to true to start it
        animator.SetTrigger(shootAnimationTrigger);
        animator.SetBool("IsShooting", true);

        // Update ammo counter UI
        if (hud != null)
        {
            hud.SetAmoCount(currentAmmo);
        }

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

        // Emit the casing after shooting
        EmitCasing();

        // Call ApplyRecoil on the CrossHair script
        if (crosshair != null)
        {
            crosshair.ApplyRecoil(); // Apply recoil to inner crosshair and rotate player
        }

        StartCoroutine(StopShootingAnimation());
    }

    private void EmitCasing()
    {
        if (casingPrefab != null && casingSpawnPoint != null)
        {
            // Instantiate the casing particle system at the spawn point
            GameObject casing = Instantiate(casingPrefab, casingSpawnPoint.position, casingSpawnPoint.rotation);

            // Add force to the casing to simulate ejection
            Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Randomize the direction a bit to make it feel more natural
                Vector2 ejectDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.AddForce(ejectDirection * casingEjectionForce, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator StopShootingAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("IsShooting", false); // Stop shooting animation
    }

    // Debug Hit and scan system
    IEnumerator ShowDebugRay(Vector2 start, Vector2 end)
    {
        float duration = 0.08f;
        Debug.DrawLine(start, end, Color.red, duration);
        yield return new WaitForSeconds(duration);
    }

    // Reload mechanic
    IEnumerator Reload()
    {
        animator.SetTrigger(reloadAnimationTrigger);
        animator.SetBool("IsReloading", true);

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

            // Update ammo count in the HUD after reloading
            if (hud != null)
            {
                hud.SetAmoCount(currentAmmo);
            }
        }
        else
        {
            Debug.Log("No more ammo left!");
        }

        StartCoroutine(StopReloadingAnimation());
    }

    IEnumerator StopReloadingAnimation()
    {
        yield return new WaitForSeconds(0f);
        animator.SetBool("IsReloading", false); // Stop reloading animation
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int ammoNeeded = 60 - totalAmmo;
            int refillAmount = Mathf.Max(0, ammoNeeded); //Checks so it doesn't go above 60

            totalAmmo += refillAmount;
            Destroy(other.gameObject); //Destroy the ammo box after touching the player

            Debug.Log("Picked up AmmoBox! Total Ammo: " + totalAmmo);
        }
    }

}
