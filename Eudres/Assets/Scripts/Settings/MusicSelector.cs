using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    public static MusicSelector Instance;
    public static event Action<int> OnSongChanged;

    [Header("Playlist")]
    public List<AudioClip> musicTracks;
    private int currentIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedSongIndex", 0);
        PlaySong(currentIndex);
    }

    private void Update()
    {
        // Când melodia s-a terminat și NU e pe loop nativ, trecem automat la următoarea
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            AudioSource source = AudioManager.Instance.musicSource;
            if (!source.isPlaying && !source.loop && musicTracks.Count > 0)
            {
                PlayNextSong();
            }
        }
    }

    public void SetLoop(bool isLooping)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            AudioManager.Instance.musicSource.loop = isLooping;
        }
    }

    public List<string> GetSongNames()
    {
        List<string> names = new List<string>();
        foreach (var clip in musicTracks)
        {
            if (clip != null) names.Add(clip.name);
        }
        return names;
    }

    public void SelectSongByIndex(int index)
    {
        PlaySong(index);
    }

    public void PlayNextSong()
    {
        if (musicTracks.Count == 0) return;
        currentIndex = (currentIndex + 1) % musicTracks.Count;
        PlaySong(currentIndex);
    }

    public void PlayPrevSong()
    {
        if (musicTracks.Count == 0) return;
        currentIndex--;
        if (currentIndex < 0) currentIndex = musicTracks.Count - 1;
        PlaySong(currentIndex);
    }

    private void PlaySong(int index)
    {
        if (index >= 0 && index < musicTracks.Count && musicTracks[index] != null)
        {
            currentIndex = index;
            PlayerPrefs.SetInt("SelectedSongIndex", currentIndex);

            AudioManager.Instance.PlayMusic(musicTracks[currentIndex]);
            OnSongChanged?.Invoke(currentIndex);
        }
    }
}