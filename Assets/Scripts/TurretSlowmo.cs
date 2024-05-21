using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TurretSlowmo : TowerBase
{

    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] protected GameObject turretMenu;
    [SerializeField] protected Button upgradeButton;
    [SerializeField] protected Button sellButton;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 2.5f;
    [SerializeField] private float aps = 4f; // Attacks per second
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] protected int baseUpgradeCost = 150;

    protected float apsBase;
    protected float targetingRangeBase;
    protected float freezeTimeBase;

    protected int level = 1;

    private float timeUntilFire;

    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;
        freezeTimeBase = freezeTime;

        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(() => Sell(BuildManager.main.GetSelectedTower(), level, baseUpgradeCost));

        turretMenu.SetActive(false);
    }

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
        if (CalculateUpgradeCost() > LevelManager.main.currency)
        {
            MessageHandler.main.ShowMessage();
            return;
        }

        LevelManager.main.SpendCurrency(CalculateUpgradeCost());

        level++;

        aps = CalculateAPS();
        targetingRange = CalculateRange();
        // rotationSpeed = CalculateRotation();

        CloseTurretMenu();
        Debug.Log("New APS: " + aps);
        Debug.Log("New Range: " + targetingRange);
        Debug.Log("New Cost: " + CalculateUpgradeCost());
    }

    // public void Sell()
    // {
    //     Tower tower = BuildManager.main.GetSelectedTower();
    //     int upgrades = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.4f));
    //     int towerCost = Mathf.RoundToInt(tower.cost * 0.84f);
    //     LevelManager.main.IncreaseCurrency(upgrades + towerCost);
    // }

    protected int CalculateUpgradeCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    protected float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    protected float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.6f);
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }

    // Comment this when exporting
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw a visual circle around the turret to show the range.
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
#endif
}
