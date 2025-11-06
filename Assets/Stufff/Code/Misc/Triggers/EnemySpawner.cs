using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; 
    public float spawnDelay = 2f;
    public int maxEnemies = 5;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        isSpawning = true;
        while (isSpawning)
        {
            activeEnemies.RemoveAll(enemy => enemy == null);
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }   
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}
