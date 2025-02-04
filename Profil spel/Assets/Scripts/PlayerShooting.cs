using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] float playerBulletSpeed = 10f;
    [SerializeField] GameObject playerBullet;
    [SerializeField] GameObject playerGun;
    [SerializeField] float fireRate = 0.5f; // Cooldown duration
    private float nextFireTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) //Fire 1 is mapped to shooting
        {
            TryFire();
        }
    }

    void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(playerBullet, playerGun.transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * playerBulletSpeed, ForceMode2D.Impulse);
    }
}
