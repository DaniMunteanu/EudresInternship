using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private GameObject settingsPanel;

    [Header("Music Selector UI")]
    [SerializeField] private TextMeshProUGUI songTitleText; // Text in the middle
    [SerializeField] private Button prevButton;            // '<' Button
    [SerializeField] private Button nextButton;            // '>' Button

    private const string VolumeKey = "MasterVolume";
    private const string MusicOnKey = "MusicOn";
    private const string SelectedSongKey = "SelectedSongIndex";

    private List<string> songNames = new List<string>();
    private int currentSongIndex = 0;

    private void Start()
    {
        // 1. Load saved volume & toggle values
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

        // 2. Setup Music Selector UI
        if (MusicSelector.Instance != null)
        {
            songNames = MusicSelector.Instance.GetSongNames();

            if (songNames.Count > 0)
            {
                // Retrieve saved index
                currentSongIndex = PlayerPrefs.GetInt(SelectedSongKey, 0);

                // Ensure the index stays in bounds
                if (currentSongIndex < 0 || currentSongIndex >= songNames.Count)
                {
                    currentSongIndex = 0;
                }

                UpdateSongDisplay();
            }
        }

        // Setup Button Listeners
        if (prevButton != null)
        {
            prevButton.onClick.AddListener(OnPrevSongPressed);
        }

        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextSongPressed);
        }
    }

    public void OnPrevSongPressed()
    {
        if (songNames.Count == 0) return;

        currentSongIndex--;
        // Loop back to the end if going past index 0
        if (currentSongIndex < 0)
        {
            currentSongIndex = songNames.Count - 1;
        }

        ChangeSong(currentSongIndex);
    }

    public void OnNextSongPressed()
    {
        if (songNames.Count == 0) return;

        currentSongIndex++;
        // Loop around to 0 if reaching past the end
        if (currentSongIndex >= songNames.Count)
        {
            currentSongIndex = 0;
        }

        ChangeSong(currentSongIndex);
    }

    private void ChangeSong(int index)
    {
        // Save choice
        PlayerPrefs.SetInt(SelectedSongKey, index);

        // Update UI Text
        UpdateSongDisplay();

        // Switch song audio
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