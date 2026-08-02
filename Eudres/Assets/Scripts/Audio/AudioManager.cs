using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip ingredientDrop;
    public List<AudioClip> musicTracks;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Încarcă melodia selectată din PlayerPrefs
        int selectedSongIndex = PlayerPrefs.GetInt("SelectedSongIndex", 0);
        if (selectedSongIndex >= 0 && selectedSongIndex < musicTracks.Count)
        {
            PlayMusic(musicTracks[selectedSongIndex]);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
}