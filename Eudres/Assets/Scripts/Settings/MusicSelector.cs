using UnityEngine;
using System.Collections.Generic;

public class MusicSelector : MonoBehaviour
{
    public static MusicSelector Instance;

    public List<AudioClip> musicTracks;
    private int currentIndex = 0;

    private void Awake()
    {
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
        currentIndex = PlayerPrefs.GetInt("SelectedSongIndex", 0);
        PlaySong(currentIndex);
    }

    private void Update()
    {
        if (AudioManager.Instance != null && !AudioManager.Instance.musicSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    // Metoda nouă cerută de SettingsManager
    public List<string> GetSongNames()
    {
        List<string> names = new List<string>();
        foreach (var clip in musicTracks)
        {
            if (clip != null)
                names.Add(clip.name);
        }
        return names;
    }

    public void SelectSongByIndex(int index)
    {
        PlaySong(index);
    }

    private void PlaySong(int index)
    {
        if (index >= 0 && index < musicTracks.Count && musicTracks[index] != null)
        {
            currentIndex = index;
            AudioManager.Instance.PlayMusic(musicTracks[currentIndex]);
            PlayerPrefs.SetInt("SelectedSongIndex", currentIndex);
        }
    }

    private void PlayNextSong()
    {
        if (musicTracks.Count == 0) return;

        currentIndex = (currentIndex + 1) % musicTracks.Count;
        PlaySong(currentIndex);
    }
}