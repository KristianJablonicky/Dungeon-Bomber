using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private GameObject mutedVoxSymbol;
    private int voxEnabled = 0;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1.0f);
        }
        if (!PlayerPrefs.HasKey("vox"))
        {
            PlayerPrefs.SetInt("vox", 0);
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
        voxEnabled = PlayerPrefs.GetInt("vox");
        updateVoxVisuals();
    }

    private void updateVoxVisuals()
    {
        if (voxEnabled == 1)
        {
            mutedVoxSymbol.SetActive(false);
        }
        else
        {
            mutedVoxSymbol.SetActive(true);
        }
    }

    public void switchVocals()
    {
        if (voxEnabled == 1)
        {
            voxEnabled = 0;
        }
        else
        {
            voxEnabled = 1;
        }
        updateVoxVisuals();
        PlayerPrefs.SetInt("vox", voxEnabled);
    }
}
