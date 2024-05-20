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
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    public static EnemySpawner main;

    private int currentWave = 1;
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
    }

    // Start the wave with a timer.
    private void Start()
    {
        StartCoroutine(StartWave());
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
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    // Wait for timeBetweenWaves (5 seconds), before starting new wave.
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    // Reset variables, add to current wave and start the StartWave timer.
    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    // Spawn enemy
    private void SpawnEnemy()
    {
        SetSelectedEnemy(Random.Range(0, enemies.Length));
        Enemy enemyToSpawn = GetSelectedEnemy();
        Vector3 startPointPosition = LevelManager.main.startPoint.position;
        startPointPosition.y -= 0.4f;
        Instantiate(enemyToSpawn.prefab, startPointPosition, Quaternion.identity);
        // Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        // baseEnemies (8) * currentWave (1) ^ 0.75 = 8
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private float EnemiesPerSecond()
    {
        // baseEnemies (8) * currentWave (1) ^ 0.75 = 8
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor)
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
