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
        int upgrades;
        if (_level == 1)
        {
            upgrades = 0;
        }
        else
        {
            upgrades = Mathf.RoundToInt((_baseUpgradeCost * _level) * 0.6f);
        }
        int towerCost = Mathf.RoundToInt(_tower.cost * 0.84f);
        LevelManager.main.IncreaseCurrency(upgrades + towerCost);
        MenuManager.main.SetHoveringState(false);
        Destroy(gameObject);
    }
}
