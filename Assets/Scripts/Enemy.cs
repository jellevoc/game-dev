using UnityEngine;
using System;

[Serializable]
public class Enemy
{

    public string name;
    public int damage;
    public GameObject prefab;
    public float moveSpeed;
    public int health;

    public Enemy(string _name, int _damage, GameObject _prefab, float _moveSpeed, int _health)
    {
        name = _name;
        damage = _damage;
        prefab = _prefab;
        moveSpeed = _moveSpeed;
        health = _health;
    }

}
