using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 2f;
    [SerializeField] float spawnDistanceMin = 1f;  
    [SerializeField] float spawnDistanceMax = 5f;  
    [SerializeField] float initialDelay = 1f;

    private Vector2 screenBounds;
    private GameObject player;

   
    private CheckPoint checkpoint;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        //Get the checkpoint script from the checkpoint object 
        checkpoint = GameObject.FindWithTag("Checkpoint").GetComponent<CheckPoint>();

        //Calculate screen bounds 
        Camera camera = Camera.main;
        screenBounds = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);

        //Start spawning enemies after the delay
        StartCoroutine(SpawnEnemyCoroutine());
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        //Wait for the delay before the first spawn
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            //Adjust spawn parameters if the checkpoint is being captured
            if (checkpoint != null && checkpoint.isCapturing)
            {
                minSpawnTime = 3f;  //Fast spawn rate when capturing checkpoint
                maxSpawnTime = 6f;
                spawnDistanceMin = 0.5f;  //Spawn closer to player
                spawnDistanceMax = 1f;
            }
            else
            {
                minSpawnTime = 6f;  //Normal spawn rate
                maxSpawnTime = 11f;
                spawnDistanceMin = 6f;  //Default spawn distance
                spawnDistanceMax = 12f;
            }

            //Get a random spawn position
            Vector2 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);  

            if (enemy != null)
            {
                EnemyFollow enemyFollow = enemy.GetComponent<EnemyFollow>();
                if (enemyFollow != null && player != null)
                {
                    enemyFollow.player = player.transform;
                }
            }

            //Wait for a random amount of time before spawning the next enemy
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        //Randomly choose a side (top, bottom, left, right)
        int side = Random.Range(0, 4);

        //Randomly pick spawn distance within the given range
        float spawnDistance = Random.Range(spawnDistanceMin, spawnDistanceMax);

        Vector2 spawnPosition = Vector2.zero;

        switch (side)
        {
            case 0:  //Top side
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spawnDistance);
                break;
            case 1:  //Bottom side
                spawnPosition = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spawnDistance);
                break;
            case 2:  //Right side
                spawnPosition = new Vector2(screenBounds.x + spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 3:  //Left side
                spawnPosition = new Vector2(-screenBounds.x - spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
        }

        return spawnPosition;
    }
}
