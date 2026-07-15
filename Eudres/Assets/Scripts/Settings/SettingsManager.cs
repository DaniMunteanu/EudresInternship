using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References (assign in Inspector)")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private GameObject settingsPanel;

    private const string VolumeKey = "MasterVolume";
    private const string MusicOnKey = "MusicOn";

    private void Start()
    {
        // Load saved values (default: full volume, music on)
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        bool musicOn = PlayerPrefs.GetInt(MusicOnKey, 1) == 1;

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

    private void ApplyVolume(float volume, bool musicOn)
    {
        // Simple global volume control.
        // If music later gets its own AudioSource, swap this for that source's volume/mute instead.
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
