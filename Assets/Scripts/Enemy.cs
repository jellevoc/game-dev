using UnityEngine;
using System;

[Serializable]
public class Enemy
{

    public string name;
    public int damage;
    public GameObject prefab;

    public Enemy(string _name, int _damage, GameObject _prefab)
    {
        name = _name;
        damage = _damage;
        prefab = _prefab;
    }

}
