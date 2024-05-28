using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public AudioSource src;
    [SerializeField] public AudioClip sfxCratePickup;
    [SerializeField] public AudioClip sfxCrossbowShoot;

    public static SFXHandler main;

    private void Awake()
    {
        main = this;
    }

}
