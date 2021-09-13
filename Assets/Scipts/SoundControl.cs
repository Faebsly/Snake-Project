using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steuert die Sounds des Spiels
/// </summary>
public class SoundControl : MonoBehaviour {

    // Felder
    private AudioSource soundPlayer;
    
    private void Start()
    {
        StartValidation();
    }

    /// <summary>
    /// fragt die Variable PlaySound ab und spielt dann den sound ab
    /// </summary>
    private void Update()
    {
        if (GlobalNames.playSound == true)
        {
            foreach (Sound item in GlobalNames.soundList)
            {
                if (item.GetSoundType() == GlobalNames.soundType)
                {
                    soundPlayer.clip = item.GetSoundClip();
                    soundPlayer.volume = item.GetVolume();
                }
            }

            GlobalNames.playSound = false;
            GlobalNames.soundType = GlobalNames.Sounds.none;
            soundPlayer.Play();
        }
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        soundPlayer = GetComponent<AudioSource>(); // Beschaffe die Komponente

        if (soundPlayer == null)
        {
            Debug.LogError("Error: Start Validation failed. -> SoundControl.cs || Oder die MusicClips sind leer siehe GlobalNames.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }


        return valid;
    }
    
}
