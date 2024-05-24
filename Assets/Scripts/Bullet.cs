using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private Transform target;

    private bool hasCollided = false;

    // Public function so it can be called in different scripts.
    public void SetTarget(Transform _target)
    {
        target = _target;
    }


    // Follow the direction of the enemy.
    void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;
    }

    // If the bullet collides with the enemy, Call the TakeDamage function in the health script and destroy the bullet.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hasCollided) return;
        hasCollided = true;
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }
}
