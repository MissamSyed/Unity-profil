using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private LayerMask hitLayers;

    [Header("Recoil Settings")]
    [SerializeField] private CrossHair crosshair;

    [Header("Bullet Casing and Tracer Effects")]
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private float casingEjectionForce = 5f;
    [SerializeField] private LineRenderer tracerPrefab;
    [SerializeField] private float tracerSpeed = 50f;
    [SerializeField] private float tracerFadeTime = 0.05f;

    private bool isReloading = false;
    private float nextFireTime = 0f;

    private void Start()
    {
        if (playerWeapon.GetCurrentWeapon() != null)
        {
            // Initial setup: use max ammo from the weapon data
            Debug.Log("Weapon Initialized: " + playerWeapon.GetCurrentWeapon().weaponName);
        }
    }

    private void Update()
    {
        if (isReloading || playerWeapon.GetCurrentWeapon() == null) return;

        Weapon currentWeapon = playerWeapon.GetCurrentWeapon();  // Changed from WeaponData to Weapon

        // Handle shooting
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot(currentWeapon);
            nextFireTime = Time.time + currentWeapon.fireRate;
        }

        // Handle reloading
        if (Input.GetKeyDown(KeyCode.R) && playerWeapon.CanReload())
        {
            StartCoroutine(Reload(currentWeapon));
        }
    }

    private void Shoot(Weapon weapon)
    {
        if (playerWeapon.GetCurrentAmmo() > 0)
        {
            playerWeapon.UseAmmo(1);  // Decrease ammo by 1 when firing

            animator.SetTrigger(weapon.shootTrigger);

            Vector2 startPosition = bulletSpawnPoint.position;
            Vector2 direction = bulletSpawnPoint.up;
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, weapon.fireRate, hitLayers);
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(weapon.damage);
                }
            }

            CreateTracerEffect(startPosition, direction);
            EmitCasing();

            if (crosshair != null)
            {
                crosshair.ApplyRecoil();
            }
        }
        else
        {
            Debug.Log("Out of ammo! Reload needed.");
        }
    }

    private void CreateTracerEffect(Vector2 start, Vector2 direction)
    {
        if (tracerPrefab != null)
        {
            LineRenderer tracer = Instantiate(tracerPrefab, start, Quaternion.identity);
            tracer.positionCount = 2;
            tracer.SetPosition(0, start);
            tracer.SetPosition(1, start);

            StartCoroutine(AnimateTracer(tracer, start, direction));

            Destroy(tracer.gameObject, tracerFadeTime);
        }
    }

    private IEnumerator AnimateTracer(LineRenderer tracer, Vector2 start, Vector2 direction)
    {
        float time = 0f;
        Vector2 end = start + direction * 50f;
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

    private IEnumerator Reload(Weapon weapon)
    {
        isReloading = true;
        animator.SetTrigger(weapon.reloadTrigger);
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(weapon.reloadTime);

        playerWeapon.Reload();  // Reload to maxAmmo
        isReloading = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoBox"))
        {
            int ammoNeeded = playerWeapon.GetCurrentWeapon().maxAmmo - playerWeapon.GetCurrentAmmo();
            int ammoToReload = Mathf.Min(ammoNeeded, 60);  // Assuming 60 is the max refill ammo available

            playerWeapon.GetCurrentAmmo() += ammoToReload;
            Destroy(other.gameObject);

            Debug.Log("Picked up AmmoBox! Ammo now: " + playerWeapon.GetCurrentAmmo());
        }
    }
}
