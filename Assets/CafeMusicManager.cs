using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CafeMusicManager : MonoBehaviour
{
    // Use EventReference instead of EventRefAttribute
    [SerializeField] private EventReference musicEvent;

    private FMOD.Studio.EventInstance musicInstance;

    void Start()
    {
        // Create an instance of the music event
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        // Start playing the music
        musicInstance.start();
    }

    void OnDestroy()
    {
        // Stop the music and release the instance
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}
