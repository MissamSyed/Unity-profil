using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;        
    public LineRenderer lineRenderer; 
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

        
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, hit.collider ? hit.point : transform.position + direction * shootingRange);
        lineRenderer.enabled = true;

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage); 
        }

        
        Invoke(nameof(HideShot), 0.1f);
    }

    void HideShot()
    {
        lineRenderer.enabled = false;
    }
}
