using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public virtual void OpenTurretMenu()
    {
    }

    public virtual void Sell(Tower _tower, int _level, int _baseUpgradeCost)
    {
        int sellCost = CalculateSellCost(_tower, _level, _baseUpgradeCost);
        LevelManager.main.IncreaseCurrency(sellCost);
        MenuManager.main.SetHoveringState(false);
        Destroy(gameObject);
    }

    public int CalculateSellCost(Tower _tower, int _level, int _baseUpgradeCost)
    {
        int upgrades;
        if (_level == 1)
        {
            upgrades = 0;
        }
        else
        {
            upgrades = Mathf.RoundToInt(_baseUpgradeCost * _level * 0.6f);
        }
        int towerCost = Mathf.RoundToInt(_tower.cost * 0.84f);
        int total = upgrades + towerCost;
        return total;
    }
}
