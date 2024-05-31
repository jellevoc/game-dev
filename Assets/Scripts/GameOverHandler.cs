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

    public bool isGameOver = false;

    public static GameOverHandler main;

    [Header("Events")]
    public static UnityEvent onGameOver = new UnityEvent();

    // Set static variable and events
    private void Awake()
    {
        main = this;
        onGameOver.AddListener(GameOver);
    }

    // Show the gameovermenu. Set the wavetext in the menu with the current wave and set the timeScale to 0 so the game is actually paused.
    private void GameOver()
    {
        gameOverMenu.SetActive(true);
        isGameOver = true;

        waveText.text = WaveHandler.main.currentWave.ToString();

        Time.timeScale = 0f;
    }

    // Make sure game is actually running and reload scene.
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Load menu
    public void EnterMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

}
