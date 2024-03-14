using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Import SceneManager
using FMODUnity;
using FMOD.Studio;
using System;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances = new List<EventInstance>();
    private EventInstance musicEventInstance;
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.music);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopSound();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        eventInstances.Add(musicEventInstance);
        musicEventInstance.start();
    }

    private void StopSound()
    {
        foreach (var instance in eventInstances)
        {
            if (instance.isValid()) { 
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    public void PlayOneShot(EventReference sound)
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public EventInstance CreateEventInstance(EventReference eventreference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventreference);
        return eventInstance;
    }
}
