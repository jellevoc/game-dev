using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitcoinCrate : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private int cashAfterRound = 100;
    [SerializeField] private float cashAfterRoundMultiplier = 0.5f;

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isColliding = boxCollider.OverlapPoint(mousePosition);

        if (isColliding)
        {
            LevelManager.main.IncreaseCurrency(Mathf.RoundToInt(cashAfterRound * cashAfterRoundMultiplier * WaveHandler.main.currentWave));
            Destroy(gameObject);
        }
    }
}
