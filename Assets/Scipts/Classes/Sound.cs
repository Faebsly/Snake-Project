using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stellt den Bauplan für die Sounds dar
/// </summary>
public class Sound {

    // Eigenschaften
    private AudioClip audioClip;
    private float volume;
    private GlobalNames.Sounds soundType;

    // Konstructor
    /// <summary>
    /// Erstellt ein Soundtrack
    /// </summary>
    /// <param name="audioClipArg">Sound Clip</param>
    /// <param name="volumeArg">Lautstärke</param>
    /// <param name="soundTypeArg">Art des Soundtracks</param>
    public Sound(AudioClip audioClipArg, float volumeArg, GlobalNames.Sounds soundTypeArg)
    {
        audioClip = audioClipArg;
        volume = volumeArg;
        soundType = soundTypeArg;
    }

    // Fähigkeiten
    public void SetVolume(float value)
    {
        volume = value;
    }
    public void SetSoundType(GlobalNames.Sounds type)
    {
        soundType = type;
    }
    
    // Zugriffe
    public AudioClip GetSoundClip()
    {
        return audioClip;
    }
    public float GetVolume()
    {
        return volume;
    }
    public string GetClipName()
    {
        return audioClip.name;
    }
    public GlobalNames.Sounds GetSoundType()
    {
        return soundType;
    }
}
