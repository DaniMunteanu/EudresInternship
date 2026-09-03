using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public static event Action<int> OnSongChanged;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips & Playlist")]
    public AudioClip ingredientDrop;
    public List<AudioClip> musicTracks;

    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";
    private const string RepeatKey = "RepeatOne";

    private int currentSongIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        float savedMaster = PlayerPrefs.GetFloat(MasterKey, 1f);
        float savedMusic = PlayerPrefs.GetFloat(MusicKey, 1f);
        float savedSFX = PlayerPrefs.GetFloat(SFXKey, 1f);
        bool repeatOn = PlayerPrefs.GetInt(RepeatKey, 0) == 1;

        ApplyVolumes(savedMaster, savedMusic, savedSFX);

        if (musicSource != null && musicTracks != null && musicTracks.Count > 0)
        {
            // Resetăm seed-ul aleatoriu folosind timpul curent (Ticks)
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

            // Alege un index cu adevărat random
            currentSongIndex = UnityEngine.Random.Range(0, musicTracks.Count);

            SetLoop(repeatOn);

            PlaySong(currentSongIndex);
        }
    }

    private void Update()
    {
        if (musicSource != null && !musicSource.isPlaying && !musicSource.loop && musicTracks.Count > 0)
        {
            PlayNextSong();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySong(int index)
    {
        if (musicTracks != null && index >= 0 && index < musicTracks.Count)
        {
            currentSongIndex = index;
            PlayerPrefs.SetInt("SelectedSongIndex", currentSongIndex);

            PlayMusic(musicTracks[currentSongIndex]);
            OnSongChanged?.Invoke(currentSongIndex);
        }
    }

    public void PlayNextSong()
    {
        if (musicTracks.Count == 0) return;
        currentSongIndex = (currentSongIndex + 1) % musicTracks.Count;
        PlaySong(currentSongIndex);
    }

    public void PlayPrevSong()
    {
        if (musicTracks.Count == 0) return;
        currentSongIndex--;
        if (currentSongIndex < 0) currentSongIndex = musicTracks.Count - 1;
        PlaySong(currentSongIndex);
    }

    public void SetLoop(bool isLooping)
    {
        if (musicSource != null && musicSource.loop != isLooping)
        {
            musicSource.loop = isLooping;
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

    public int GetCurrentSongIndex() => currentSongIndex;

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
}