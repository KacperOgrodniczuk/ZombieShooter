using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;

    [Header("Base Wave Settings")]
    [SerializeField]
    int startingEnemyCount = 3;
    [SerializeField]
    float startSpawnInterval = 10f;

    [Header("Difficulty Scaling")]
    [SerializeField]
    float difficultyScaling = 1.1f;


    int currentWaveIndex = 0;
    int enemiesAlive = 0;
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
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        waveSpawning = false;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        IDamageable damageable = spawnedEnemy.GetComponent<IDamageable>();          // For now this works but I need to adapt it to use objectPools in the future.
        damageable.OnDeath += OnEnemyDeath;                                         // I will also need to grab the enemy manager script and access the reference to IDamageable
                                                                                    // through there. I will need to access other components because I want enemies to scale over time.
        enemiesAlive++;
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive == 0 && !waveSpawning)
        {
            currentWaveIndex++;

            StartCoroutine(SpawnWave(currentWaveIndex));
        }
    }

    int CalculateEnemyCount(int waveIndex)
    {
        return Mathf.CeilToInt(startingEnemyCount * Mathf.Pow(difficultyScaling, waveIndex));
    }

    float CalculateSpawnInterval(int waveIndex)
    {
        return Mathf.Max(startSpawnInterval / Mathf.Pow(difficultyScaling, waveIndex), 1f);
    }
}
