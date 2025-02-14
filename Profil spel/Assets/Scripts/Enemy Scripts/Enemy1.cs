using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy1 : MonoBehaviour
{
    public GameObject weaponDrop; 
    public GameObject enemyPrefab; 
    public Transform spawnPoint; 
    public Transform escapeTarget;  
    public Transform teleportTarget; 
    public int maxEnemies = 10;
    private int enemiesKilled = 0;
    private bool isDead = false;
    private static int totalEnemiesSpawned = 0;

    void Start()
    {
        if (totalEnemiesSpawned < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        DropWeapon();
        enemiesKilled++;
        totalEnemiesSpawned--;

        if (enemiesKilled >= maxEnemies)
        {
            EscapeRemainingEnemies();
        }

        Destroy(gameObject); 
    }

    void DropWeapon()
    {
        if (weaponDrop != null)
        {
            Instantiate(weaponDrop, transform.position, Quaternion.identity);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        totalEnemiesSpawned++;
    }

    void EscapeRemainingEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.SetEscapeTarget(escapeTarget.position, teleportTarget.position);
            }
        }
    }
}

public class EnemyAI : MonoBehaviour
{
    private Vector3 escapePosition;
    private Vector3 teleportPosition;
    private bool escaping = false;
    private float speed = 3f;
    private float teleportThreshold = 1f; 

    public void SetEscapeTarget(Vector3 target, Vector3 teleportTarget)
    {
        escapePosition = target;
        this.teleportPosition = teleportTarget;
        escaping = true;
    }

    void Update()
    {
        if (escaping)
        {
            transform.position = Vector3.MoveTowards(transform.position, escapePosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, escapePosition) <= teleportThreshold)
            {
                transform.position = teleportPosition; 
                escaping = false;
            }
        }
    }
}
