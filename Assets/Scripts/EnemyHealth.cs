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

        if (health <= 0 && !isDestroyed)
        {
            EnemyMovement.onEnemyDie.Invoke();
            // Call the onEnemyDestroy event and add money to the player for killing an enemy.
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);

            isDestroyed = true;

            Destroy(gameObject);


        }
    }
}
