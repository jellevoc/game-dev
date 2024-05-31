using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VictoryHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gameWonMenu;

    [Header("Events")]
    public static UnityEvent onGameWon = new UnityEvent();

    private void Awake()
    {
        onGameWon.AddListener(GameWon);
    }

    // Show gameWonMenu and pause game
    private void GameWon()
    {
        gameWonMenu.SetActive(true);
        GameOverHandler.main.isGameOver = true;

        Time.timeScale = 0f;
    }

    // Reload current scene.
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnterMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
