using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : TowerBase
{
    [Header("References")]
    [SerializeField] protected Transform turretRotationPoint;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected GameObject turretMenu;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button sellButton;
    [SerializeField] protected GameObject crossbow;
    [SerializeField] protected GameObject maxLevelUI;

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

        // Because rotating isn't instant, double check if target is in range.
        if (CheckTargetIsInRange())
        {
            RotateTowardsTarget();
        }
        else
        {
            target = null;
        }

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

        // Calculate direction from firing point to target
        Vector3 direction = (target.position - firingPoint.position).normalized;

        // Calculate the angle in degrees needed to rotate on the Z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Since the bullet's initial orientation is upwards, add 90 degrees to the angle
        angle -= 90;

        // Create a rotation around the Z-axis
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Instantiate the bullet with the calculated rotation
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, rotation);

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
        if (target == null) return false;
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
        if (level == 3)
        {
            StartCoroutine(ShowAndHideMessage());
            return;
        }

        if (CalculateUpgradeCost() > LevelManager.main.currency)
        {
            MessageHandler.main.ShowMessage();
            return;
        }

        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;

        // Set current tower components to the new prefab
        TowerUpgrades towerToUpgradeTo = BuildManager.main.GetSelectedTower().upgrades[level - 2];

        gameObject.GetComponent<SpriteRenderer>().sprite = towerToUpgradeTo.prefab.GetComponent<SpriteRenderer>().sprite;
        bulletPrefab = towerToUpgradeTo.prefab.GetComponent<Turret>().bulletPrefab;
        crossbow.GetComponent<SpriteRenderer>().sprite = towerToUpgradeTo.prefab.GetComponent<Turret>().crossbow.GetComponent<SpriteRenderer>().sprite;



        bps = CalculateBPS();
        targetingRange = CalculateRange();
        // rotationSpeed = CalculateRotation();

        CloseTurretMenu();

        if (level == 3)
        {
            upgradeButton.image.color = Color.gray;
        }
        // Debug.Log("New BPS: " + bps);
        // Debug.Log("New Range: " + targetingRange);
        // Debug.Log("New Cost: " + CalculateUpgradeCost());
    }

    private IEnumerator ShowAndHideMessage()
    {
        maxLevelUI.SetActive(true);
        yield return new WaitForSeconds(1);
        maxLevelUI.SetActive(false);
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