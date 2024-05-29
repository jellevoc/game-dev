using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitcoinCrate : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private int cashAfterRound = 100;
    [SerializeField] private float cashAfterRoundMultiplier = 0.5f;


    // If mouse hovers over crate
    private void OnMouseEnter()
    {
        PlaySFX();

        int amount = Mathf.RoundToInt(cashAfterRound * cashAfterRoundMultiplier * WaveHandler.main.currentWave);
        LevelManager.main.IncreaseCurrency(amount);

        Destroy(gameObject);
    }

    void PlaySFX()
    {
        SFXHandler sfx = SFXHandler.main;
        sfx.src.clip = sfx.sfxCratePickup;
        sfx.src.Play();
    }
}
