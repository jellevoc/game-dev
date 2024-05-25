using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy[] enemies;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [SerializeField] private float enemySpeedMultiplier = 0.07f;
    [SerializeField] private float enemyHealthMultiplier = 0.1f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    public static UnityEvent onWaveStart = new UnityEvent();
    public static UnityEvent onWaveEnd = new UnityEvent();

    public static EnemySpawner main;

    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; //Enemies per second
    private bool isSpawning = false;

    private int selectedEnemy = 0;


    // If the onEnemyDestroy event is called, run the EnemyDestroyed function.
    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
        onWaveStart.AddListener(WaveStart);
        onWaveEnd.AddListener(WaveEnd);
    }

    // Start the wave with a timer.
    private void Start()
    {
        StartCoroutine(WaveHandler.main.StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        // If timeSinceLastSpawn >= 2 and there are enemies still left to spawn.
        // Spawn enemy and update variables.
        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        // If all enemies are destroyed and there aren't any left to spawn.
        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            WaveHandler.main.EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void WaveStart()
    {
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void WaveEnd()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
    }

    // // Wait for timeBetweenWaves (5 seconds), before starting new wave.
    // private IEnumerator StartWave()
    // {
    //     yield return new WaitForSeconds(timeBetweenWaves);

    // }

    // Reset variables, add to current wave and start the StartWave timer.
    // private void EndWave()
    // {

    //     currentWave++;
    //     LevelManager.main.IncreaseCurrency(Mathf.RoundToInt(cashAfterRound * cashAfterRoundMultiplier * currentWave));
    //     StartCoroutine(StartWave());
    // }

    // Spawn enemy
    private void SpawnEnemy()
    {
        SetSelectedEnemy(Random.Range(0, enemies.Length));
        Enemy enemyToSpawn = GetSelectedEnemy();
        enemyToSpawn.prefab.GetComponent<EnemyMovement>().moveSpeed = enemyToSpawn.prefab.GetComponent<EnemyMovement>().baseSpeed * (1 + enemySpeedMultiplier * WaveHandler.main.currentWave);
        enemyToSpawn.prefab.GetComponent<Health>().health = enemyToSpawn.prefab.GetComponent<Health>().baseHealth * Mathf.RoundToInt((1 + enemyHealthMultiplier * WaveHandler.main.currentWave));
        Instantiate(enemyToSpawn.prefab, LevelManager.main.startPoint.position, Quaternion.identity);
        // Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        // baseEnemies (8) * currentWave (1) ^ 0.75 = 8
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(WaveHandler.main.currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        // baseEnemies (8) * currentWave (1) ^ 0.75 = 8
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(WaveHandler.main.currentWave, difficultyScalingFactor)
        , 0f, enemiesPerSecondCap);
    }

    public Enemy GetSelectedEnemy()
    {
        return enemies[selectedEnemy];
    }

    public void SetSelectedEnemy(int _selectedEnemy)
    {
        selectedEnemy = _selectedEnemy;
    }
}
