using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private Button upgradeButton;


    [Header("Attribute")]
    [SerializeField] private float targetingRange = 2.5f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bps = 1f; //Bullets per second
    [SerializeField] private int baseUpgradeCost = 100;

    private float bpsBase;
    private float targetingRangeBase;
    private float rotationSpeedBase;

    private Transform target;
    private float timeUntilFire;

    private int level = 1;

    private void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;
        rotationSpeedBase = rotationSpeed;

        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        // Find target if there isn't one.
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        // Double check if target is in turret range, shoot and reset the timer.
        // Else, set target to null so it finds a new target.
        if (CheckTargetIsInRange())
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0;
            }
        }
        else
        {
            target = null;
        }
    }

    private void Shoot()
    {
        // Spawn new bullet, get the bullet script, call the SetTarget function in that script and pass it our target.
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        //Get all colliders with the enemy layer within a circular area.
        RaycastHit2D[] nearbyEnemies = Physics2D.CircleCastAll(transform.position, targetingRange,
        (Vector2)transform.position, 0f, enemyMask);


        // If there is an enemy nearby, set the target to the first one found in the array.
        if (nearbyEnemies.Length > 0)
        {
            target = nearbyEnemies[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        // Check if the target position based is less than targetingRange based on the position of the turret.
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        // Get the angle the turret needs to turn to based on the positions of the turret and target.
        float angle = Mathf.Atan2(target.position.y - transform.position.y,
        target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;


        // Create a rotation on the Z axis with the angle that was just calculated.
        // Rotate the turrent towards the point that was just made in a smooth manner with a rotation speed so it doesn't snap.
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation,
        rotationSpeed * Time.deltaTime);
    }

    public void OpenUpgradeMenu()
    {
        upgradeMenu.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        MenuManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateUpgradeCost() > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;

        bps = CalculateBPS();
        targetingRange = CalculateRange();
        // rotationSpeed = CalculateRotation();

        CloseUpgradeMenu();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New Range: " + targetingRange);
        Debug.Log("New Cost: " + CalculateUpgradeCost());
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRotation()
    {
        return rotationSpeedBase * Mathf.Pow(level, 0.5f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }


    private int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a visual circle around the turret to show the range.
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
