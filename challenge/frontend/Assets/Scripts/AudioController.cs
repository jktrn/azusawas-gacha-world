using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    private UIManager uiManager;
    public Sound[] musicSounds,
        sfxSounds;
    public AudioSource musicSource,
        sfxSource;
    public float fadeTime = 1.0f;

    private bool isFading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Duplicate {gameObject.name} found, destroying...");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Gacha");
    }

    public void PlayMusic(string name)
    {
        Sound sound = System.Array.Find(musicSounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} not found!");
            return;
        }

        musicSource.clip = sound.clip;
        musicSource.Play();
    }

    public void FadeMusic(string name)
    {
        if (isFading)
        {
            StopAllCoroutines();
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource != musicSource && audioSource != sfxSource)
                {
                    audioSource.Stop();
                    Destroy(audioSource);
                }
            }
            isFading = false;
        }

        StartCoroutine(Fade(name));
    }

    IEnumerator Fade(string name)
    {
        isFading = true;

        Sound sound = System.Array.Find(musicSounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} not found!");
            yield break;
        }

        AudioSource newMusicSource = gameObject.AddComponent<AudioSource>();
        newMusicSource.clip = sound.clip;
        newMusicSource.loop = true;
        newMusicSource.Play();
        newMusicSource.volume = 0.0f;

        float startVolume1 = musicSource.volume;
        float startVolume2 = newMusicSource.volume;
        float currentTime = 0.0f;

        uiManager = FindObjectOfType<UIManager>();
        float endVolume = uiManager.musicSlider.value / 100.0f;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume1, 0.0f, currentTime / fadeTime);
            newMusicSource.volume = Mathf.Lerp(startVolume2, endVolume, currentTime / fadeTime);
            yield return null;
        }

        musicSource.volume = 0.0f;
        newMusicSource.volume = endVolume;

        musicSource.Stop();
        Destroy(musicSource);
        musicSource = newMusicSource;

        isFading = false;
    }

    public void PlaySFX(string name)
    {
        Sound sound = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound {name} not found!");
            return;
        }

        sfxSource.PlayOneShot(sound.clip);
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume / 100.0f;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume / 100.0f;
    }
}
