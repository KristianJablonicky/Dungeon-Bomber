using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] private Slider slider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }

        loadVolume();
    }

    public void setVolume()
    {
        AudioListener.volume = slider.value;
        saveVolume();
    }

    private void saveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", slider.value);
    }

    private void loadVolume()
    {
        slider.value = PlayerPrefs.GetFloat("musicVolume");
    }
}
