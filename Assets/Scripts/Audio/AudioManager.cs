using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> musicTracks = new List<AudioClip>();
    public List<AudioClip> soundEffects = new List<AudioClip>();

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("Instance is null!");
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one AudioManager in Scene!");
    }

    private void Start()
    {
        PlayMusic(musicTracks[0]);
    }

    public void PlayMusic(AudioClip musicTrack)
    {
        musicSource.clip = musicTrack;
        musicSource.Play(0);
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.clip = sfx;
        sfxSource.Play(0);
    }

    public IEnumerator StopMusic(float FadeTime)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

}
