using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }
    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
}
