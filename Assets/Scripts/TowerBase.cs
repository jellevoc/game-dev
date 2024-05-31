using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    // Function get's overwritten.
    public virtual void OpenTurretMenu()
    {
    }

    public virtual void Sell(Tower _tower, int _level, int _baseUpgradeCost)
    {

        // Calculate how much money the player gets from selling the turret.
        int sellCost = CalculateSellCost(_tower, _level, _baseUpgradeCost);

        LevelManager.main.IncreaseCurrency(sellCost);

        // Close the turret menu
        MenuManager.main.SetHoveringState(false);

        // Destroy the turret.
        Destroy(gameObject);
    }

    public int CalculateSellCost(Tower _tower, int _level, int _baseUpgradeCost)
    {
        // Shorthand if
        int upgrades = (_level == 1) ? 0 : Mathf.RoundToInt(_baseUpgradeCost * _level * 0.6f);

        int towerCost = Mathf.RoundToInt(_tower.cost * 0.84f);

        return upgrades + towerCost;
    }
}
