using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string shootAnimationTrigger = "Shoot";
    [SerializeField] private string reloadAnimationTrigger = "Reload";

    [SerializeField] float fireRate = 0.6f;  // Default fire rate (semi-automatic)
    [SerializeField] float maxShootDistance = 10f;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] GameObject gun;

    [SerializeField] int magazineSize = 30;
    [SerializeField] int totalAmmo = 60;
    [SerializeField] float reloadTime = 1.5f;

    // Bullet casing effect
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private float casingEjectionForce = 5f;

    // Line Renderer Tracer Effect
    [SerializeField] private LineRenderer tracerPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float tracerSpeed = 50f;
    [SerializeField] private float tracerFadeTime = 0.05f;

    // Recoil System
    [SerializeField] private CrossHair crosshair; // Reference to the CrossHair script

    // Muzzle Flash Sprite
    [SerializeField] private GameObject muzzleFlashPrefab;  // Muzzle flash sprite prefab
    [SerializeField] private Transform gunBarrel;  // Reference to the gun barrel position

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private bool isAutomatic = false;

    private HUD hud;

    void Start()
    {
        GameObject camerasGameObject = GameObject.FindWithTag("Cameras");
        if (camerasGameObject != null)
        {
            hud = camerasGameObject.GetComponent<HUD>();
        }

        if (hud != null)
        {
            hud.SetAmoCount(currentAmmo);
        }
        else
        {
            Debug.LogError("HUD script not found on the Cameras GameObject!");
        }

        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            isAutomatic = !isAutomatic;
            Debug.Log(isAutomatic ? "Fire Mode: Automatic" : "Fire Mode: Semi-Auto");

            // Adjust the fire rate based on the mode
            if (isAutomatic)
            {
                fireRate = 0.1f;  // Faster fire rate for automatic mode
            }
            else
            {
                fireRate = 0.6f;  // Default fire rate for semi-automatic mode
            }
        }

        if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }

        if (isAutomatic && Input.GetButton("Fire1"))
        {
            TryFire();
            fireRate = 0.1f;
        }

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

    void Fire()
    {
        currentAmmo--;
        animator.SetTrigger(shootAnimationTrigger);
        animator.SetBool("IsShooting", true);

        if (hud != null)
        {
            hud.SetAmoCount(currentAmmo);
        }

        Vector2 gunPosition = bulletSpawnPoint.position;
        Vector2 gunDirection = bulletSpawnPoint.up;
        Vector2 endPoint = gunPosition + (gunDirection * maxShootDistance);

        RaycastHit2D hit = Physics2D.Raycast(gunPosition, gunDirection, maxShootDistance, hitLayers);

        if (hit.collider != null)
        {
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

        // Create tracer effect using Line Renderer
        CreateTracerEffect(gunPosition, endPoint);

        // Emit muzzle flash at the gun barrel
        EmitMuzzleFlash();

        // Emit casing
        EmitCasing();

        // Apply recoil effect to crosshair
        if (crosshair != null)
        {
            crosshair.ApplyRecoil();
        }

        StartCoroutine(StopShootingAnimation());
    }

    // Create the Tracer effect with Line Renderer
    private void CreateTracerEffect(Vector2 start, Vector2 end)
    {
        if (tracerPrefab != null)
        {
            LineRenderer tracer = Instantiate(tracerPrefab, start, Quaternion.identity);
            tracer.positionCount = 2;
            tracer.SetPosition(0, start);
            tracer.SetPosition(1, start); // Start from the gun

            StartCoroutine(AnimateTracer(tracer, start, end));

            Destroy(tracer.gameObject, tracerFadeTime);
        }
    }

    // Animate the tracer movement
    private IEnumerator AnimateTracer(LineRenderer tracer, Vector2 start, Vector2 end)
    {
        float time = 0f;
        while (time < tracerFadeTime)
        {
            time += Time.deltaTime * tracerSpeed;
            tracer.SetPosition(1, Vector2.Lerp(start, end, time / tracerFadeTime));
            yield return null;
        }
    }

    private void EmitCasing()
    {
        if (casingPrefab != null && casingSpawnPoint != null)
        {
            GameObject casing = Instantiate(casingPrefab, casingSpawnPoint.position, casingSpawnPoint.rotation);
            Rigidbody2D rb = casing.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 ejectDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.AddForce(ejectDirection * casingEjectionForce, ForceMode2D.Impulse);
            }
        }
    }

    private void EmitMuzzleFlash()
    {
        if (muzzleFlashPrefab != null && gunBarrel != null)
        {
            // Instantiate the muzzle flash sprite at the gun barrel's position and rotation
            GameObject muzzleFlashObj = Instantiate(muzzleFlashPrefab, gunBarrel.position, gunBarrel.rotation);

            // Ensure the muzzle flash rotates along with the barrel (already handled, but here we refine the positioning)
            muzzleFlashObj.transform.rotation = gunBarrel.rotation;

            // Slightly offset the muzzle flash to appear ahead of the gun barrel
            float muzzleFlashOffset = 0.2f; // Adjust this value to better align with your gun
            muzzleFlashObj.transform.position = gunBarrel.position + (gunBarrel.up * muzzleFlashOffset);

            // Optionally adjust position vertically (up or down) if the gun barrel is not perfectly centered
            // muzzleFlashObj.transform.position += new Vector3(0, 0.1f, 0); // adjust as needed

            // Destroy the muzzle flash sprite after a short duration (e.g., 0.1 seconds) to prevent it from lingering
            Destroy(muzzleFlashObj, 0.1f);  // Adjust this duration to control how long the flash lasts
        }
        else
        {
            Debug.LogError("Muzzle flash prefab or gun barrel is not assigned!");  // Error check
        }
    }

    IEnumerator StopShootingAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("IsShooting", false);
    }

    IEnumerator ShowDebugRay(Vector2 start, Vector2 end)
    {
        float duration = 0.08f;
        Debug.DrawLine(start, end, Color.red, duration);
        yield return new WaitForSeconds(duration);
    }

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
        animator.SetBool("IsReloading", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int ammoNeeded = 60 - totalAmmo;
            int refillAmount = Mathf.Max(0, ammoNeeded);
            totalAmmo += refillAmount;
            Destroy(other.gameObject);

            Debug.Log("Picked up AmmoBox! Total Ammo: " + totalAmmo);
        }
    }
}
