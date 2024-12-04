using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    [Header("Base Wave Settings")]
    float startSpawnInterval = 10f;

    [Header("Difficulty Scaling")]
    float enemyCountScaling = 3f;       //How many enemies to spawn in per wave.
    float enemySpawnTimeScaling = 1.1f;
    int maxEnemiesAllowedAliveIncrease = 1;


    //TODO:
    // Implement the zombies speeding up over time.
    // Implement the zombie damage scaling overtime.

    int currentWaveIndex = 1;
    int enemiesAlive = 0;
    int maxEnemiesAllowedAlive = 3;
    int waveStartDelay = 10;    // The wave delay is the same for every wave

    bool waveSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnWave(currentWaveIndex));
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        waveSpawning = true;

        yield return new WaitForSeconds(waveStartDelay);

        int enemyCount = CalculateEnemyCount(waveIndex);
        float spawnInterval = CalculateSpawnInterval(waveIndex);

        for (int i = 0; i < enemyCount; i++)
        {
            // Wait until there's space for more enemies
            yield return new WaitUntil(() => enemiesAlive < maxEnemiesAllowedAlive);

            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }

        waveSpawning = false;
        maxEnemiesAllowedAlive += maxEnemiesAllowedAliveIncrease;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        EnemyManager enemyManager = spawnedEnemy.GetComponent<EnemyManager>();


        IDamageable damageable = spawnedEnemy.GetComponent<IDamageable>();          // For now this works but I need to adapt it to use objectPools in the future.
        damageable.OnDeath += OnEnemyDeath;                                         // I will also need to grab the enemy manager script and access the reference to IDamageable
                                                                                    // through there. I will need to access other components because I want enemies to scale over time.
        enemiesAlive++;
    }

    void OnEnemyDeath(IDamageable damageable)
    {
        enemiesAlive--;

        damageable.OnDeath -= OnEnemyDeath;

        if (enemiesAlive == 0 && !waveSpawning)
        {
            currentWaveIndex++;
            StartCoroutine(SpawnWave(currentWaveIndex));
        }
    }

    int CalculateEnemyCount(int waveIndex)
    {
        return Mathf.CeilToInt(enemyCountScaling * waveIndex);
    }

    float CalculateSpawnInterval(int waveIndex)
    {
        return Mathf.Max(startSpawnInterval / Mathf.Pow(enemySpawnTimeScaling, waveIndex), 1f);
    }
}
