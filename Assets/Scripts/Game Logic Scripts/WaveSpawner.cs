using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    Transform[] SpawnPoints;

    [Header("Base Wave Settings")]
    float startSpawnInterval = 10f;

    [Header("Difficulty Scaling")]
    float enemyCountScaling = 3f;       //How many enemies to spawn in per wave.
    float enemySpawnTimeScaling = 1.1f;
    int maxEnemiesAllowedAliveIncrease = 1;

    public Transform SpawnPointsRoot;  //the script will automatically grab all the transforms attached to this object.

    float currentHealthMultiplier;
    float currentDamageMultiplier;
    float currentSpeedMultiplier;

    int currentWaveIndex = 1;
    int enemiesAlive = 0;
    int maxEnemiesAllowedAlive = 3;
    public int waveStartDelay = 10;    // The wave delay is the same for every wave

    bool waveSpawning = false;

    public event Action<int> OnWaveChange;

    //Temporary before an event manager script is implemented.
    [SerializeField] UIManager UIManager;

    private void Start()
    {
        StartCoroutine(SpawnWave(currentWaveIndex));
        GetSpawnPoints();
    }

    private void OnEnable()
    {
        if (UIManager != null)
            OnWaveChange += UIManager.UpdateWaveNumberUI;

        TriggerWaveChangeEvent();
    }

    void GetSpawnPoints() {
        Transform[] allTransforms = SpawnPointsRoot.GetComponentsInChildren<Transform>();

        int childCount = allTransforms.Length - 1;
        SpawnPoints = new Transform[childCount];
        int index = 0;

        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i] != SpawnPointsRoot)
            {
                SpawnPoints[index] = allTransforms[i];
                index++;
            }
        }
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        TriggerWaveChangeEvent();
        CalculateEnemyScaling();

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
        Transform spawnPoint = SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)];
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        EnemyManager enemyManager = spawnedEnemy.GetComponent<EnemyManager>();
        enemyManager.ScaleEnemy(currentHealthMultiplier, currentDamageMultiplier, currentSpeedMultiplier);

        IDamageable damageable = spawnedEnemy.GetComponent<IDamageable>();          // For now this works but I need to adapt it to use objectPools in the future.
        damageable.OnDeath += OnEnemyDeath;                                         // I will also need to grab the enemy manager script and access the reference to IDamageable
                                                                                    // through there. I will need to access other components because I want enemies to scale over time.
        enemyManager.Spawn();
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

    void CalculateEnemyScaling()
    {
        currentHealthMultiplier = Mathf.Pow(1 + (0.1f * (currentWaveIndex - 1)), 2);
        currentDamageMultiplier = 1;    //todo later
        currentSpeedMultiplier = Mathf.Min(Mathf.Pow(1 + (0.05f * (currentWaveIndex - 1)), 2),  3);    //yuck
    }

    private void TriggerWaveChangeEvent()
    {
        OnWaveChange?.Invoke(currentWaveIndex);
    }

    private void OnDisable()
    {
        if (UIManager != null)
            OnWaveChange -= UIManager.UpdateWaveNumberUI;
    }
}
