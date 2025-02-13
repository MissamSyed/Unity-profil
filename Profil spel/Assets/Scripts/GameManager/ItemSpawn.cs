using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    [SerializeField] GameObject healingPrefab;
    [SerializeField] GameObject ammoboxPrefab;
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 2f;
    [SerializeField] float spawnDistanceMin = 1f;  // Minimum distance from the screen edge
    [SerializeField] float spawnDistanceMax = 5f;  // Maximum distance from the screen edge
    [SerializeField] float initialDelay = 1f;

    private Vector2 screenBounds;
    

    void Start()
    {
       

        // Calculate screen bounds for 2D (assuming the camera is orthographic)
        Camera camera = Camera.main;
        screenBounds = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

        // Start spawning enemies after the initial delay
        StartCoroutine(SpawnHealingCoroutine());
        StartCoroutine(SpawnAmmoCoroutine());
    }

    IEnumerator SpawnHealingCoroutine()
    {
        // Wait for the initial delay before the first spawn
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Get a random spawn position
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject healingItem = Instantiate(healingPrefab, spawnPosition, Quaternion.identity);  // Spawn the enemy prefab

            // Wait for a random amount of time before spawning the next enemy
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator SpawnAmmoCoroutine()
    {
        // Wait for the initial delay before the first spawn
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // Get a random spawn position
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject ammoBox = Instantiate(ammoboxPrefab, spawnPosition, Quaternion.identity);  // Spawn the enemy prefab

            // Wait for a random amount of time before spawning the next enemy
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        // Randomly choose a side (top, bottom, left, right)
        int side = Random.Range(0, 4);

        // Randomly pick spawn distance within the given range
        float spawnDistance = Random.Range(spawnDistanceMin, spawnDistanceMax);

        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0:  // Top side
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spawnDistance);
                break;
            case 1:  // Bottom side
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spawnDistance);
                break;
            case 2:  // Right side
                spawnPosition = new Vector2(screenBounds.x + spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 3:  // Left side
                spawnPosition = new Vector2(-screenBounds.x - spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
        }

        return spawnPosition;
    }
}
