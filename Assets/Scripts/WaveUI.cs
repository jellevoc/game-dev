using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI waveUI;

    private void OnGUI()
    {
        waveUI.text = "Wave: " + EnemySpawner.main.currentWave;
    }
}
