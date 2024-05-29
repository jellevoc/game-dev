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


    // Set listeners to events
    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
        onWaveStart.AddListener(WaveStart);
        onWaveEnd.AddListener(WaveEnd);
    }

    // Call the startwave function in WaveHandler with a slight timer/delay
    private void Start()
    {
        StartCoroutine(WaveHandler.main.StartWave());
    }


    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        // If conditions are met to spawn enemies.
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

    private void SpawnEnemy()
    {
        // Set random enemy from array
        SetSelectedEnemy(Random.Range(0, enemies.Length));
        Enemy enemyToSpawn = GetSelectedEnemy();

        // Set the health and speed based on the wave. So the enemies will have more health and more speed at round 15 than round 2.
        enemyToSpawn.prefab.GetComponent<EnemyMovement>().moveSpeed = enemyToSpawn.prefab.GetComponent<EnemyMovement>().baseSpeed * (1 + enemySpeedMultiplier * WaveHandler.main.currentWave);
        enemyToSpawn.prefab.GetComponent<Health>().health = enemyToSpawn.prefab.GetComponent<Health>().baseHealth * Mathf.RoundToInt(1 + enemyHealthMultiplier * WaveHandler.main.currentWave);

        // Spawn enemy
        Instantiate(enemyToSpawn.prefab, LevelManager.main.startPoint.position, Quaternion.identity);
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
