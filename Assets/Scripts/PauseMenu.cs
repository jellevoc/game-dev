using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Resume if game is already paused, else pause the game
            if (isPaused)
            {
                Resume();
                return;
            }
            Pause();
        }
    }

    // Disable menu and set timescale so game actually runs again.
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;

        Time.timeScale = 1f;
    }

    // Set pause menu active and set timescale to 0 so the game actually pauses
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;

        Time.timeScale = 0f;
    }

    // Load menu scene
    public void LoadMenu()
    {
        pauseMenuUI.SetActive(false);
        isPaused = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
