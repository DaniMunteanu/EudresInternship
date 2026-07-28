using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro Dropdown support

public class SettingsManager : MonoBehaviour
{
    [Header("UI References (assign in Inspector)")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private TMP_Dropdown musicDropdown; // <-- Added Dropdown reference
    [SerializeField] private GameObject settingsPanel;

    private const string VolumeKey = "MasterVolume";
    private const string MusicOnKey = "MusicOn";
    private const string SelectedSongKey = "SelectedSongIndex"; // <-- Added key to save song choice

    private void Start()
    {
        // 1. Load saved volume & toggle values
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        bool musicOn = PlayerPrefs.GetInt(MusicOnKey, 1) == 1;

        AudioListener.volume = musicOn ? savedVolume : 0f;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = musicOn;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }

        ApplyVolume(savedVolume, musicOn);

        // 2. Setup Music Dropdown if MusicSelector exists in the scene
        if (musicDropdown != null && MusicSelector.Instance != null)
        {
            // Populate options from the PersistentMusicManager
            musicDropdown.ClearOptions();
            musicDropdown.AddOptions(MusicSelector.Instance.GetSongNames());

            // Set the saved option (default to 0)
            int savedSongIndex = PlayerPrefs.GetInt(SelectedSongKey, 0);
            musicDropdown.value = savedSongIndex;

            // Listen for user changes
            musicDropdown.onValueChanged.AddListener(OnSongChanged);
        }
    }

    public void OnVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(VolumeKey, value);
        bool musicOn = musicToggle == null || musicToggle.isOn;
        ApplyVolume(value, musicOn);
    }

    public void OnMusicToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(MusicOnKey, isOn ? 1 : 0);
        float volume = volumeSlider != null ? volumeSlider.value : 1f;
        ApplyVolume(volume, isOn);
    }

    public void OnSongChanged(int songIndex)
    {
        // Save the chosen song index
        PlayerPrefs.SetInt(SelectedSongKey, songIndex);

        // Tell the persistent manager to switch tracks
        if (MusicSelector.Instance != null)
        {
            MusicSelector.Instance.SelectSongByIndex(songIndex);
        }
    }

    private void ApplyVolume(float volume, bool musicOn)
    {
        AudioListener.volume = musicOn ? volume : 0f;
    }

    public void OnCloseSettingsPressed()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}