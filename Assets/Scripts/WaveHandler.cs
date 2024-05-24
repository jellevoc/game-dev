using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveHandler : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private int cashAfterRound = 100;
    [SerializeField] private float cashAfterRoundMultiplier = 0.5f;
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
        if (currentWave == maxWaves)
        {
            VictoryHandler.onGameWon.Invoke();
            return;
        }

        LevelManager.main.IncreaseCurrency(Mathf.RoundToInt(cashAfterRound * cashAfterRoundMultiplier * currentWave));

        StartCoroutine(StartWave());
    }
}
