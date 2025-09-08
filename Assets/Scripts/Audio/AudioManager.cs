using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class AudioManager : MonoBehaviour
{
    //Music
    [SerializeField] private List<AudioClip> timeOfDayMusicTracks = new List<AudioClip>();
    private int currentTimeOfDayTrack;
    public AudioClip titleScreenTrack;
    public AudioClip orientationTrack;
    public AudioClip dateTrack;
    public AudioClip lifeEventTrack;
    //SFX
    public AudioClip clickSFX;
    public AudioClip dialogueNextSFX;
    public AudioClip hoverOverSFX;
    [SerializeField] private float trackFadeOutTime;


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
        DontDestroyOnLoad(gameObject);
        currentTimeOfDayTrack = 0;
        //StartCoroutine(Test());
        //PlayMusic(musicTracks[0]);
    }

    public void PlayTrack(AudioClip musicTrack)
    {
        musicSource.clip = musicTrack;
        musicSource.Play(0);
        Debug.Log(musicSource.clip.name);
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.clip = sfx;
        sfxSource.Play(0);
    }

    public void SwitchTimeOfDayTrack()
    {
        currentTimeOfDayTrack += 1;
        StartCoroutine(SwitchTracks(timeOfDayMusicTracks[currentTimeOfDayTrack]));
    }

    public IEnumerator StopTracks()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / trackFadeOutTime;

            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    public IEnumerator SwitchTracks(AudioClip newMusicTrack)
    {
        yield return StartCoroutine(StopTracks());
        PlayTrack(newMusicTrack);
    }

    /*
    private IEnumerator Test()
    {
        StartCoroutine(SwitchTracks(titleScreenTrack));
        yield return new WaitForSeconds(3);
        StartCoroutine(SwitchTracks(dateTrack));
        yield return new WaitForSeconds(3);
        StartCoroutine(SwitchTracks(orientationTrack));
        yield return new WaitForSeconds(3);
        StartCoroutine(StopMusic());
        yield return new WaitForSeconds(3);
        PlaySFX(dialogueNextSFX);
        StartCoroutine(SwitchTracks(timeOfDayMusicTracks[0]));

        for (int i = 0; i < timeOfDayMusicTracks.Count - 1; i++)
        {
            yield return new WaitForSeconds(3);
            SwitchTimeOfDayTrack();
        }
        
    }
    */

}
