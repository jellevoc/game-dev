using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int cashAfterRound = 100;
    [SerializeField] private float cashAfterRoundMultiplier = 0.5f;

    public int currentWave = 1;

    public static WaveHandler main;

    private void Awake()
    {
        main = this;
    }

    public IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        EnemySpawner.onWaveStart.Invoke();
        // isSpawning = true;
        // enemiesLeftToSpawn = EnemiesPerWave();
        // eps = EnemiesPerSecond();
    }

    public void EndWave()
    {
        // isSpawning = false;
        // timeSinceLastSpawn = 0f;
        EnemySpawner.onWaveEnd.Invoke();
        currentWave++;
        LevelManager.main.IncreaseCurrency(Mathf.RoundToInt(cashAfterRound * cashAfterRoundMultiplier * currentWave));
        StartCoroutine(StartWave());
    }
}
