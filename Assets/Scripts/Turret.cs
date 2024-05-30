using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
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
    [SerializeField] protected TextMeshProUGUI upgradeText;
    [SerializeField] protected TextMeshProUGUI sellText;

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
        // Set base variables
        bpsBase = bps;
        targetingRangeBase = targetingRange;
        rotationSpeedBase = rotationSpeed;

        // Set events
        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(() => Sell(BuildManager.main.GetSelectedTower(), level, baseUpgradeCost));

        turretMenu.SetActive(false);
    }



    protected virtual void Update()
    {

        if (target == null)
        {
            FindTarget();
        }

        // Double check if target is in range because turret rotation isn't instant.
        if (target != null && CheckTargetIsInRange())
        {
            RotateTowardsTarget();

            // If turret can shoot.
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

        // Calculate angle in degrees for Z-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Add 90 degrees to angle because bullet is facing upwards.
        angle -= 90;

        // New rotation with angle we calculated
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Instantiate the bullet with the calculated rotation
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, rotation);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        ShootSFX();
    }

    protected void ShootSFX()
    {
        SFXHandler sfx = SFXHandler.main;
        sfx.src.clip = sfx.sfxCrossbowShoot;
        sfx.src.Play();
    }

    protected void FindTarget()
    {
        // Make circle around the player with the targetingRange as radius and the enemymask as layer
        RaycastHit2D[] nearbyEnemies = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        // Set target to the first enemy
        if (nearbyEnemies.Length > 0)
        {
            target = nearbyEnemies[0].transform;
        }
    }

    // Return if the enemy position based on the turret position is less than the targetingRange
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

        SetUpgradeText();
        SetSellText();

    }

    public void CloseTurretMenu()
    {
        MenuManager.main.SetHoveringState(false);
    }

    private void SetUpgradeText()
    {
        upgradeText.text = "Upgrade: " + CalculateUpgradeCost().ToString();
    }

    private void SetSellText()
    {
        int SellProfit = CalculateSellCost(BuildManager.main.GetSelectedTower(), level, baseUpgradeCost);
        sellText.text = "Sell: " + SellProfit.ToString();
    }

    public void Upgrade()
    {
        // If turret is max level
        if (level == 3)
        {
            StartCoroutine(ShowAndHideMessage());
            return;
        }

        // If player doesn't have enough money.
        if (CalculateUpgradeCost() > LevelManager.main.currency)
        {
            MessageHandler.main.ShowMessage();
            return;
        }

        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;

        SetUpgradeText();
        SetSellText();

        // Set current tower components to the new prefab
        TowerUpgrades towerToUpgradeTo = BuildManager.main.GetSelectedTower().upgrades[level - 2];

        // Set the turret sprite, bulletprefab and crossbow sprite to the new tower.
        gameObject.GetComponent<SpriteRenderer>().sprite = towerToUpgradeTo.prefab.GetComponent<SpriteRenderer>().sprite;
        bulletPrefab = towerToUpgradeTo.prefab.GetComponent<Turret>().bulletPrefab;
        crossbow.GetComponent<SpriteRenderer>().sprite = towerToUpgradeTo.prefab.GetComponent<Turret>().crossbow.GetComponent<SpriteRenderer>().sprite;



        bps = CalculateBPS();
        targetingRange = CalculateRange();

        CloseTurretMenu();

        if (level == 3)
        {
            upgradeButton.image.color = Color.gray;
        }
    }

    // Show message, wait 1 second and hide message.
    private IEnumerator ShowAndHideMessage()
    {
        maxLevelUI.SetActive(true);
        yield return new WaitForSeconds(1);
        maxLevelUI.SetActive(false);
    }


    // Calculate upgrades and cost based on the level
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
        float newRange = targetingRangeBase * Mathf.Pow(level, 0.4f);
        return newRange;
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