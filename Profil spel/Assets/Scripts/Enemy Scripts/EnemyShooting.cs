using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;
    public Transform firePoint; // A fire point for shooting
    public float fireRate = 1f;
    public float shootingRange = 10f;
    public int damage = 20;
    public LayerMask playerLayer;

    private float nextFireTime = 0f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform; // Auto-find player
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (firePoint == null) firePoint = transform; // Fallback if firePoint is not set

        Vector2 direction = (player.position - firePoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, shootingRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Debug.Log("Enemy hit the player!");
        }
    }
}
