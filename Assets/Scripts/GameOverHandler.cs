using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] TextMeshProUGUI waveText;


    public static GameOverHandler main;

    [Header("Events")]
    public static UnityEvent onGameOver = new UnityEvent();

    private void Awake()
    {
        main = this;
        onGameOver.AddListener(GameOver);
    }

    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        waveText.text = EnemySpawner.main.currentWave.ToString();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void EnterMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
