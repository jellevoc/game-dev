using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;

    public static VolumeSlider main;

    public float sliderValue;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        slider.value = MusicHandler.main.src.volume;
        sliderValue = slider.value;

        slider.onValueChanged.AddListener(delegate { OnSliderValueChange(); });
    }

    private void OnSliderValueChange()
    {
        sliderValue = slider.value;
        MusicHandler.onSliderChanged.Invoke();
    }
}
