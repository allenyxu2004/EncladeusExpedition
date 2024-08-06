using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public static int maxEnemies = 20;

    public float spawnTime = 3f;

    public float xMin = -25f;
    public float xMax = 25f;
    public float yMin = 1f;
    public float yMax = 5f;
    public float zMin = -25f;
    public float zMax = 25f;

    bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
        isSpawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawners that spawn at the same time will bypass the maxEnemies check once
        if (!isSpawning && maxEnemies > EnemyNavAi.enemiesAlive)
        {
            InvokeRepeating("SpawnEnemies", spawnTime, spawnTime);
        }
        else
        {
            CancelInvoke("SpawnEnemies");
            isSpawning = false;
        }
    }

    void SpawnEnemies()
    {
        Vector3 enemyPosition;

        enemyPosition.x = gameObject.transform.position.x + Random.Range(xMin, xMax);
        enemyPosition.y = gameObject.transform.position.y + Random.Range(yMin, yMax);
        enemyPosition.z = gameObject.transform.position.z + Random.Range(zMin, zMax);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, transform.rotation)
            as GameObject;

        spawnedEnemy.transform.parent = gameObject.transform;
    }
}
