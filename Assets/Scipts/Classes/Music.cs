using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stellt den Bauplan für die Music dar
/// </summary>
public class Music {

    // Eigenschaften
    private AudioClip audioClip;
    private float currentTime;
    private float volume;

    // Konstructor
    /// <summary>
    /// Erstellt ein Musictrack
    /// </summary>
    /// <param name="audioClipArg">Music Clip</param>
    /// <param name="currentTimeArg">aktuelle Zeit</param>
    /// <param name="volumeArg">Lautstärke</param>
    public Music(AudioClip audioClipArg, float currentTimeArg, float volumeArg)
    {
        audioClip = audioClipArg;
        currentTime = currentTimeArg;
        volume = volumeArg;
    }

    // Fähigkeiten
    public void SetVolume(float value)
    {
        volume = value;
    }
    public void SetCurrentTime(float time)
    {
        currentTime = time;
    }
    
    // Zugriffe
    public AudioClip GetAudioClip()
    {
        return audioClip;
    }
    public float GetCurrentTime()
    {
        return currentTime;
    }
    public float GetVolume()
    {
        return volume;
    }
    public string GetClipName()
    {
        return audioClip.name;
    }

}
