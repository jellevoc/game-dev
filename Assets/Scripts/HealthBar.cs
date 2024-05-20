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
        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
    }

    public bool RemoveHealth(int damage)
    {
        if (healthBarSlider.value > 0)
        {
            healthBarSlider.value -= damage;
            return true;
        }
        else
        {
            Debug.Log("Dead");
            return false;
        }
    }
}
