using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References - Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle repeatToggle;
    [SerializeField] private GameObject settingsPanel;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Music Selector UI")]
    [SerializeField] private TextMeshProUGUI songTitleText;
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";
    private const string RepeatKey = "RepeatOne";

    private List<string> songNames = new List<string>();
    private bool isInitializing = true;

    private void OnEnable()
    {
        AudioManager.OnSongChanged += OnSongChangedHandler;
        if (AudioManager.Instance != null)
        {
            UpdateSongDisplay();
        }
    }

    private void OnDisable()
    {
        AudioManager.OnSongChanged -= OnSongChangedHandler;
    }

    private void Start()
    {
        isInitializing = true;

        float savedMaster = PlayerPrefs.GetFloat(MasterKey, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXKey, 1f);
        bool repeatOn = PlayerPrefs.GetInt(RepeatKey, 0) == 1;

        if (masterSlider != null)
        {
            masterSlider.value = savedMaster;
            masterSlider.onValueChanged.RemoveAllListeners();
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        if (repeatToggle != null)
        {
            repeatToggle.isOn = repeatOn;
            repeatToggle.onValueChanged.RemoveAllListeners();
            repeatToggle.onValueChanged.AddListener(OnRepeatToggleChanged);
        }

        ApplyVolumes(savedMaster, savedMusic, savedSFX);

        if (AudioManager.Instance != null)
        {
            songNames = AudioManager.Instance.GetSongNames();
            UpdateSongDisplay();
        }

        if (prevButton != null)
        {
            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(OnPrevSongPressed);
        }

        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextSongPressed);
        }

        isInitializing = false;
    }

    private void OnSongChangedHandler(int newIndex)
    {
        UpdateSongDisplay();
    }

    public void OnRepeatToggleChanged(bool isLooping)
    {
        if (isInitializing) return;
        PlayerPrefs.SetInt(RepeatKey, isLooping ? 1 : 0);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetLoop(isLooping);
        }
    }

    public void OnMasterVolumeChanged(float value)
    {
        if (isInitializing) return;
        PlayerPrefs.SetFloat(MasterKey, value);
        SetVolume("Master", value);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (isInitializing) return;
        PlayerPrefs.SetFloat(MusicKey, value);
        SetVolume("Music", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (isInitializing) return;
        PlayerPrefs.SetFloat(SFXKey, value);
        SetVolume("SFX", value);
    }

    private void ApplyVolumes(float master, float music, float sfx)
    {
        SetVolume("Master", master);
        SetVolume("Music", music);
        SetVolume("SFX", sfx);
    }

    private void SetVolume(string parameterName, float linearValue)
    {
        if (mainMixer != null)
        {
            float dB = linearValue > 0.0001f ? Mathf.Log10(linearValue) * 20f : -80f;
            mainMixer.SetFloat(parameterName, dB);
        }
    }

    public void OnPrevSongPressed()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPrevSong();
        }
    }

    public void OnNextSongPressed()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayNextSong();
        }
    }

    private void UpdateSongDisplay()
    {
        if (songTitleText != null && AudioManager.Instance != null)
        {
            int index = AudioManager.Instance.GetCurrentSongIndex();
            songNames = AudioManager.Instance.GetSongNames();
            if (songNames.Count > index && index >= 0)
            {
                songTitleText.text = songNames[index];
            }
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