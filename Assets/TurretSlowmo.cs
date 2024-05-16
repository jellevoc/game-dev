using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretSlowmo : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 2.5f;
    [SerializeField] private float aps = 4f; // Attacks per second
    [SerializeField] private float freezeTime = 1f;

    private float timeUntilFire;

    private void Update()
    {
        // Double check if target is in turret range, shoot and reset the timer.
        // Else, set target to null so it finds a new target.
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            Freeze();
            timeUntilFire = 0;
        }
    }

    private void Freeze()
    {
        RaycastHit2D[] nearbyEnemies = Physics2D.CircleCastAll(transform.position, targetingRange,
        (Vector2)transform.position, 0f, enemyMask);

        if (nearbyEnemies.Length > 0)
        {
            for (int i = 0; i < nearbyEnemies.Length; i++)
            {
                RaycastHit2D enemy = nearbyEnemies[i];

                EnemyMovement em = enemy.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a visual circle around the turret to show the range.
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

}
