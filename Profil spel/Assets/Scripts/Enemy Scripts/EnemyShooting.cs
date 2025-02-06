using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;           
    public GameObject bulletPrefab;    
    public Transform shootingPoint;    
    public float fireRate = 1f;        
    private float nextFireTime;        

    void Update()
    {
        if (player != null)
        {
            AimAtPlayer();  

            
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;  
            }
        }
    }

    void AimAtPlayer()
    {
        Vector3 direction = player.position - transform.position; 
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 

        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = bullet.transform.right * 10f; 
        }
    }
}
