using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] public int health = 2;
    [SerializeField] public int currencyWorth = 20;

    private bool isDestroyed = false;

    public int baseHealth;

    private void Start()
    {
        baseHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Call the onEnemyDestroy event and destroy the enemy.
        if (health <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
