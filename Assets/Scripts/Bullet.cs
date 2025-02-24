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
    [SerializeField] private float waitDelayBeforeDestroying = 0.1f;
    [SerializeField] private float destroyBulletTime = 0.5f;

    private Transform target;

    private bool hasCollided = false;

    // Public function so it can be called in different scripts.
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        // Makes sure bullet get's destroyed after ... (7) seconds.
        StartCoroutine(DestroyIfNotCollided());
    }


    // Add velocity to the direction of the enemy
    void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;

    }

    // If the bullet collides with the enemy, wait for slight delay else the arrow wouldn't visible if enemy is close to the turret.
    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (hasCollided) return;
        hasCollided = true;

        StartCoroutine(DestroyAfterDelay(_other));
    }

    private IEnumerator DestroyAfterDelay(Collision2D _other)
    {
        yield return new WaitForSeconds(waitDelayBeforeDestroying);

        // Call the TakeDamage function in the health script of the enemy.
        _other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);

        Destroy(gameObject);
    }

    private IEnumerator DestroyIfNotCollided()
    {
        yield return new WaitForSeconds(destroyBulletTime);
        Destroy(gameObject);
    }
}
