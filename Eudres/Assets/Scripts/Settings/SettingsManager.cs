using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Adăugat pentru AudioMixer
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References - Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private GameObject settingsPanel;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer; // Tragi AudioMixer-ul aici dacă folosiți unul

    [Header("Music Selector UI")]
    [SerializeField] private TextMeshProUGUI songTitleText;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";
    private const string MusicOnKey = "MusicOn";
    private const string SelectedSongKey = "SelectedSongIndex";

    private List<string> songNames = new List<string>();
    private int currentSongIndex = 0;

    private void Start()
    {
        // 1. Încarcă valorile salvate
        float savedMaster = PlayerPrefs.GetFloat(MasterKey, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXKey, 1f);
        bool musicOn = PlayerPrefs.GetInt(MusicOnKey, 1) == 1;

        if (masterSlider != null)
        {
            masterSlider.value = savedMaster;
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = musicOn;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        ApplyVolumes(savedMaster, savedMusic, savedSFX, musicOn);

        // 2. Setup Music Selector UI
        if (MusicSelector.Instance != null)
        {
            songNames = MusicSelector.Instance.GetSongNames();

            if (songNames.Count > 0)
            {
                currentSongIndex = PlayerPrefs.GetInt(SelectedSongKey, 0);
                if (currentSongIndex < 0 || currentSongIndex >= songNames.Count)
                {
                    currentSongIndex = 0;
                }
                UpdateSongDisplay();
            }
        }

        if (prevButton != null) prevButton.onClick.AddListener(OnPrevSongPressed);
        if (nextButton != null) nextButton.onClick.AddListener(OnNextSongPressed);
    }

    public void OnMasterVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(MasterKey, value);
        SetVolume("Master", value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(MusicKey, value);
        bool musicOn = musicToggle == null || musicToggle.isOn;
        if (musicOn) SetVolume("Music", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(SFXKey, value);
        SetVolume("SFX", value);
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(MusicOnKey, isOn ? 1 : 0);
        float musicVol = musicSlider != null ? musicSlider.value : 1f;
        SetVolume("Music", isOn ? musicVol : 0.0001f);
    }

    private void ApplyVolumes(float master, float music, float sfx, bool musicOn)
    {
        SetVolume("Master", master);
        SetVolume("Music", musicOn ? music : 0.0001f);
        SetVolume("SFX", sfx);
    }

    private void SetVolume(string parameterName, float linearValue)
    {
        if (mainMixer != null)
        {
            // Convertește valoarea slider-ului (0.0001 - 1) în decibeli (-80dB la 0dB)
            float dB = linearValue > 0.0001f ? Mathf.Log10(linearValue) * 20f : -80f;
            mainMixer.SetFloat(parameterName, dB);
        }
        else if (parameterName == "Master")
        {
            AudioListener.volume = linearValue;
        }
    }

    public void OnPrevSongPressed()
    {
        if (songNames.Count == 0) return;
        currentSongIndex--;
        if (currentSongIndex < 0) currentSongIndex = songNames.Count - 1;
        ChangeSong(currentSongIndex);
    }

    public void OnNextSongPressed()
    {
        if (songNames.Count == 0) return;
        currentSongIndex++;
        if (currentSongIndex >= songNames.Count) currentSongIndex = 0;
        ChangeSong(currentSongIndex);
    }

    private void ChangeSong(int index)
    {
        PlayerPrefs.SetInt(SelectedSongKey, index);
        UpdateSongDisplay();
        if (MusicSelector.Instance != null)
        {
            MusicSelector.Instance.SelectSongByIndex(index);
        }
    }

    private void UpdateSongDisplay()
    {
        if (songTitleText != null && songNames.Count > currentSongIndex)
        {
            songTitleText.text = songNames[currentSongIndex];
        }
    }

    public void OnCloseSettingsPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}