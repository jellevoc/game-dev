using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class MusicHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public AudioSource src;
    [SerializeField] public AudioClip music;

    [Header("Events")]
    public static UnityEvent onSliderChanged = new UnityEvent();

    public static MusicHandler main;

    private void Awake()
    {

        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;

        DontDestroyOnLoad(gameObject);

        onSliderChanged.AddListener(ChangeVolume);
    }

    private void Start()
    {
        src.clip = music;
        src.Play();
    }

    private void ChangeVolume()
    {
        src.volume = VolumeSlider.main.sliderValue;
    }

}
