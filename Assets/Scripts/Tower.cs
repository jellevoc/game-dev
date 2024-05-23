using UnityEngine;
using System;
using Unity.Collections;
using UnityEditor.Rendering.Universal;

[Serializable]
public class Tower
{

    public string name;
    public int cost;
    public GameObject prefab;
    public TowerUpgrades[] upgrades;

    public Tower(string _name, int _cost, GameObject _prefab, TowerUpgrades[] _upgrades)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
        upgrades = _upgrades;
    }

}
