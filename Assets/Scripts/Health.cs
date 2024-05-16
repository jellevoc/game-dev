using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int health = 2;
    [SerializeField] private int currencyWorth = 20;

    private bool isDestroyed = false;

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
