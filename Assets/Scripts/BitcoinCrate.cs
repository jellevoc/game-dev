using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitcoinCrate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource src;
    [SerializeField] private AudioClip cratePickupSound;

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

        StartCoroutine(DestroyAfterSound());
    }

    void PlaySFX()
    {
        src.volume = 0.5f;
        src.clip = cratePickupSound;
        src.Play();

    }

    IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(cratePickupSound.length / 1.5f);

        Destroy(gameObject);
    }
}
