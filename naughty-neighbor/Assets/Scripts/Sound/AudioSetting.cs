using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume")) LoadMusicVolume();
        else SetMusicVolume();
        if (PlayerPrefs.HasKey("sfxVolume")) LoadSFXVolume();
        else SetSFXVolume();

        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        if (volume <= 0) mixer.SetFloat("Music", -80f);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        if (volume <= 0) mixer.SetFloat("SFX", -80f);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }

    void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        SetSFXVolume();
    }
}
