using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int maxWaves = 25;

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
    }

    public void EndWave()
    {
        EnemySpawner.onWaveEnd.Invoke();

        currentWave++;
        // If player reached max waves
        if (currentWave == maxWaves)
        {
            // Call onGameWon event
            VictoryHandler.onGameWon.Invoke();
            return;
        }

        // Call onWaveEnd event 
        Atlas.onWaveEnd.Invoke();

        // Wait ... (5) seconds before starting wave.
        StartCoroutine(StartWave());
    }
}
