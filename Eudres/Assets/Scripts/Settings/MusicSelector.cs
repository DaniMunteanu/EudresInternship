using System.Collections.Generic;
using UnityEngine;

public class MusicSelector : MonoBehaviour
{
    public static MusicSelector Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Tracks")]
    [SerializeField] private List<AudioClip> musicTracks = new List<AudioClip>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        int savedSongIndex = PlayerPrefs.GetInt("SelectedSongIndex", 0);
        PlaySong(savedSongIndex);
    }

    public void SelectSongByIndex(int index)
    {
        PlaySong(index);
    }

    private void PlaySong(int index)
    {
        if (index >= 0 && index < musicTracks.Count && musicTracks[index] != null)
        {
            audioSource.Stop();
            audioSource.clip = musicTracks[index];
            audioSource.Play();
        }
    }

    public List<string> GetSongNames()
    {
        List<string> names = new List<string>();
        foreach (AudioClip clip in musicTracks)
        {
            if (clip != null) names.Add(clip.name);
        }
        return names;
    }
}