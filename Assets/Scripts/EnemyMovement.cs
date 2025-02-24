using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip enemyDeathSound;

    [Header("Attributes")]
    [SerializeField] public float moveSpeed = 2f;

    [Header("Events")]
    public static UnityEvent onEnemyDie = new UnityEvent();

    private Enemy enemy;

    private Transform target;
    private int pathIndex = 0;

    public float baseSpeed;

    void Start()
    {
        onEnemyDie.AddListener(PlayDeathSound);

        src.volume = 0.3f;
        src.clip = enemyDeathSound;

        enemy = EnemySpawner.main.GetSelectedEnemy();
        baseSpeed = moveSpeed;
        // Set the target to the first point in array (Not the startPoint!)
        target = LevelManager.main.path[pathIndex];
    }

    private void PlayDeathSound()
    {
        src.Play();
    }

    void Update()
    {
        // If the distance between the enemy and the position of the point is less than 0.1f set target to the next point
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // If the enemy reaches the last point
            if (pathIndex == LevelManager.main.path.Length)
            {
                // Call the onEnemyDestroy event
                EnemySpawner.onEnemyDestroy.Invoke();

                // Remove health from player
                HealthBar.main.RemoveHealth(enemy.damage);

                Destroy(gameObject);
                return;
            }
            else
            {
                target = LevelManager.main.path[pathIndex];
            }
        }
    }

    // Add velocity to the enemy in the direction of the next point
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    public void UpdateSpeed(float _newSpeed)
    {
        moveSpeed = _newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }
}
