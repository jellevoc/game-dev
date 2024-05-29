using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int baseIncome = 200;

    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = baseIncome;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    // Return bool so we can check if player has enough money to buy (true: player has enough money) | (false: player doesnt have enough money)
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

}
