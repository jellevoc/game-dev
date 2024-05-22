using UnityEngine;
using System;
using Unity.Collections;
using UnityEditor.Rendering.Universal;

[Serializable]
public class TowerUpgrades
{

    public GameObject prefab;

    public TowerUpgrades(GameObject _prefab)
    {
        prefab = _prefab;
    }

}
