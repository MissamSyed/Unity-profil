using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The enemy prefab to spawn
    public Transform spawnPoint;    // The position where the enemy will spawn
    public float spawnDelay = 2f;   // Time delay between enemy spawns

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemy(); // Spawn the first enemy when the game starts
    }

    public void SpawnEnemy()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy); // Destroy the current enemy if it exists
        }

        // Instantiate the new enemy and spawn it at the designated spawn point
        currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Ensure we only subscribe to the onDeath event once
        Enemy enemyScript = currentEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Remove any existing listeners to avoid duplicates
            enemyScript.onDeath -= OnEnemyDeath;
            enemyScript.onDeath += OnEnemyDeath; // Subscribe to the death event
        }
    }

    private void OnEnemyDeath()
    {
        // After a delay, spawn a new enemy when the current one dies
        Invoke("SpawnEnemy", spawnDelay);
    }
}
