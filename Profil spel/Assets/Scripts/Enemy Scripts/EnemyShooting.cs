using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;           
    public float fireRate = 1f;        
    public float shootingRange = 10f;   
    private float nextFireTime;        
    public int damage = 20;            

    public LineRenderer lineRenderer; 

    void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false; 
        }
    }

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
        
        if (lineRenderer != null)
        {
            
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position); 
            lineRenderer.SetPosition(1, player.position); 

            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, shootingRange);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                
                hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }

            
            Invoke("DisableLineRenderer", 0.1f);
        }
    }

    void DisableLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false; 
        }
    }
}

