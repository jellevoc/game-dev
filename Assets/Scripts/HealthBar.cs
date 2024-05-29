using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject healthBar;

    [Header("Attributes")]
    [SerializeField] private int maxHealth = 100;

    public static HealthBar main;

    private Slider healthBarSlider;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        // Set slider values
        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = 100;
        healthBarSlider.value = maxHealth;
    }

    public bool RemoveHealth(int damage)
    {
        if (healthBarSlider.value > 0)
        {
            healthBarSlider.value -= damage;

            // Double check if enemy health is lower than 0 then call the GameOverHandler event.
            if (healthBarSlider.value <= 0)
            {
                GameOverHandler.onGameOver.Invoke();
            }
            return true;
        }
        else
        {
            GameOverHandler.onGameOver.Invoke();
            return false;
        }
    }
}
