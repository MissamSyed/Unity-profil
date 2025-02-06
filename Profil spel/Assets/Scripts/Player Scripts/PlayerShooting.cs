using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Animator animator;  
    [SerializeField] private string shootAnimationTrigger = "Shoot"; 

    [SerializeField] float fireRate = 0.1f; 
    [SerializeField] float maxShootDistance = 10f; 
    [SerializeField] LayerMask hitLayers; 
    [SerializeField] GameObject gun; 

    [SerializeField] int magazineSize = 10; 
    [SerializeField] int totalAmmo = 30; 
    [SerializeField] float reloadTime = 1.5f; 

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

        //Semi-Auto 
        if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            TryFire();
        }

        //Automatic 
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
        currentAmmo--; //Reduce ammo from the mag

        //Trigger shooting animation and set IsShooting to true to start it
        animator.SetTrigger(shootAnimationTrigger); 
        animator.SetBool("IsShooting", true); 

        // Logic for raycasting (Hit and scan method)
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
                    enemy.TakeDamage(35); //Damage to enemy
                }
            }
        }

        StartCoroutine(ShowDebugRay(gunPosition, endPoint));

       
        StartCoroutine(StopShootingAnimation());  
    }

   
    IEnumerator StopShootingAnimation()
    {
        
        yield return new WaitForSeconds(0.25f); 
        animator.SetBool("IsShooting", false); //Set IsShooting to false to stop the animation
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
            int ammoNeeded = 30 - totalAmmo; 
            int refillAmount = Mathf.Max(0, ammoNeeded); //Checks so it doesnt go above 30

            totalAmmo += refillAmount; 
            Destroy(other.gameObject); //Destroy the ammo box after touching the player

            Debug.Log("Picked up AmmoBox! Total Ammo: " + totalAmmo);
        }
    }

}
