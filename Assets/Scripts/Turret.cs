using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : TowerBase
{
    [Header("References")]
    [SerializeField] public Transform turretRotationPoint;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected GameObject turretMenu;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button sellButton;

    [Header("Attributes")]
    [SerializeField] protected float targetingRange = 2.5f;
    [SerializeField] protected float rotationSpeed = 200f;
    [SerializeField] protected float bps = 1f; // Bullets per second
    [SerializeField] protected int baseUpgradeCost = 100;

    protected float bpsBase;
    protected float targetingRangeBase;
    protected float rotationSpeedBase;

    protected Transform target;
    protected float timeUntilFire;

    protected int level = 1;

    protected virtual void Start()
    {
        bpsBase = bps;
        targetingRangeBase = targetingRange;
        rotationSpeedBase = rotationSpeed;

        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(() => Sell(BuildManager.main.GetSelectedTower(), level, baseUpgradeCost));

        turretMenu.SetActive(false);
    }



    protected virtual void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

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

    protected void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    protected void FindTarget()
    {
        RaycastHit2D[] nearbyEnemies = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (nearbyEnemies.Length > 0)
        {
            target = nearbyEnemies[0].transform;
        }
    }

    protected bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    protected virtual void RotateTowardsTarget()
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


    public override void OpenTurretMenu()
    {
        turretMenu.SetActive(true);
    }

    public void CloseTurretMenu()
    {
        turretMenu.SetActive(false);
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

        CloseTurretMenu();
        Debug.Log("New BPS: " + bps);
        Debug.Log("New Range: " + targetingRange);
        Debug.Log("New Cost: " + CalculateUpgradeCost());
    }

    protected float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }

    protected float CalculateRotation()
    {
        return rotationSpeedBase * Mathf.Pow(level, 0.5f);
    }

    protected float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    protected int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    // Comment this when exporting
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw a visual circle around the turret to show the range.
        UnityEditor.Handles.color = Color.cyan;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}