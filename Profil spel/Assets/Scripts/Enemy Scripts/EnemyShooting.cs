using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;        
 
    public float fireRate = 1f;     
    public float shootingRange = 10f; 
    public int damage = 20;         

    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate; 
        }
    }

    void Shoot()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootingRange);

        
        

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage); 
        }

        
        Invoke(nameof(HideShot), 0.1f);
    }

    void HideShot()
    {
       
    }
}
