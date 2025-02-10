using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 2f;
    [SerializeField] float spawnDistanceMin = 0f;
    [SerializeField] float spawnDistanceMax = 5f;
    [SerializeField] float initialDelay = 1f;
    private Vector2 screenBounds;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));  // Get the camera bounds to get the spawn locations

        // Start the spawning 
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        // Wait for the initial delay before the first spawn so it doesn't spawn directly when the game starts
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Set spawn position and spawn enemy
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);  // Spawn the enemy prefab

            if (enemy != null)
            {
                EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();
                if (enemyFollow != null && player != null)
                {
                    enemyFollow.player = player.transform;
                }
            }

            // Wait for a random amount of time before spawning the next enemy
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        // Randomize spawn position around the screen bounds
        int side = Random.Range(0, 4);
        float spawnDistance = Random.Range(spawnDistanceMin, spawnDistanceMax);
        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0:  // Top
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spawnDistance);
                break;
            case 1:  // Bottom
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spawnDistance);
                break;
            case 2:  // Right
                spawnPosition = new Vector2(screenBounds.x + spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 3:  // Left
                spawnPosition = new Vector2(-screenBounds.x - spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
        }

        return spawnPosition;
    }
}
