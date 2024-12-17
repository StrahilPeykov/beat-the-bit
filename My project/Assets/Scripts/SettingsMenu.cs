using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    public static SettingsMenu Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer; // The Audio Mixer for managing volume
    [SerializeField] private Slider volumeSlider;   // Slider for adjusting volume

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize volume slider with saved value
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
    }

    // Open the settings menu
    public void Open()
    {
        gameObject.SetActive(true);
    }

    // Close the settings menu
    public void Close()
    {
        gameObject.SetActive(false);
    }

    // Adjust the volume through the slider
    public void SetVolume(float volume)
    {
        // Convert slider value to logarithmic scale for AudioMixer
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
