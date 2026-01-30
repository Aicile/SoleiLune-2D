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
        // Ensure the bank containing the event is loaded
        RuntimeManager.LoadBank("Master", true);
        RuntimeManager.LoadBank("SOLU_MX", true);

        // Create an instance of the music event
        musicInstance = RuntimeManager.CreateInstance(musicEvent);
        // Set the volume (1.0f is the default volume level)
        musicInstance.setVolume(1.0f);
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
