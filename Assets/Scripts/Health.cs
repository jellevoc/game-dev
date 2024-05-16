using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int health = 2;

    public void TakeDamage(int damage)
    {
        health -= damage;

        // Call the onEnemyDestroy event and destroy the enemy.
        if (health <= 0)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}
