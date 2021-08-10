using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider volumeSlider2;
    private float sliderValue;
    private float sliderValue2;

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SoundFXVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", .5f);
            PlayerPrefs.SetFloat("SoundFXVolume", .5f);
        }
        sliderValue = PlayerPrefs.GetFloat("MusicVolume");
        sliderValue2 = PlayerPrefs.GetFloat("SoundFXVolume");
        volumeSlider.value = sliderValue;
        volumeSlider2.value = sliderValue2;
    }

    public void VolumeController()
    {
        sliderValue = volumeSlider.value;
        FindObjectOfType<AudioManager>().masterVolume = sliderValue;
        FindObjectOfType<AudioManager>().VolumeChange();
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void VolumeController2()
    {
        sliderValue2 = volumeSlider2.value;
        FindObjectOfType<AudioManager>().soundVolume = sliderValue2;
        FindObjectOfType<AudioManager>().VolumeChange2();
        PlayerPrefs.SetFloat("SoundFXVolume", sliderValue2);
    }
}
