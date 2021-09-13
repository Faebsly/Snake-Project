using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Steuert die Scene Option
/// </summary>
public class Option : MonoBehaviour {

    public Slider masterVol;        // Gesamtlautstärke
    public Slider musicVol;         // Musiklautstärke
    public Slider soundVol;         // Soundlautstärke
    public Button acceptButton;     // Der accept Button
    public MusicControl mControl;   // Dienst als Schnittstelle zur Musik
    public VideoControl vControl; // Dient als Schnittstelle zu den Videos
    

    /// <summary>
    /// Setzt die Startlautstärke
    /// </summary>
    private void Start()
    {
        // Die aktuellen Werte holen und setzten
        masterVol.value = AudioListener.volume; 
        soundVol.value = 1;
        musicVol.value = GlobalNames.musicList[GlobalNames.bGMusicID].GetVolume();

        StartValidation();
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;

        if (masterVol == null || musicVol == null || soundVol == null || acceptButton == null || mControl == null || vControl == null)
        {
            Debug.LogError("Error: Start Validation failed. -> Option.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }

        return valid;
    }


    // Die Buttons ...

    /// <summary>
    ///  Übernimmt die Einstellungen der Slider am Sound
    /// </summary>
    public void OnButtonAcceptClicked()
    {
        AudioListener.volume = masterVol.value; // Gesamtlautstärke anpassen
        DoChangeSoundVolume();                  // Soundlautstärke anpassen
        mControl.ChangeVolume(musicVol.value);  // Musiklautstärke anpassen
        acceptButton.interactable = false; // Deaktiviere die Klickbarkeit für eine visuelle Bestätigung 
    }

    /// <summary>
    /// Läd das Hauptmenü
    /// </summary>
    public void OnButtonBackClicked()
    {
        mControl.DoSaveTime();
        vControl.DoSaveTime();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.MainMenu.ToString());
    }

    /// <summary>
    /// Aktiviere den Button wieder wenn ein Slider bewegt wird
    /// </summary>
    public void OnSliderValueChange()
    {
        acceptButton.interactable = true;
    }

    /// <summary>
    /// Verändert die Lautstärke der Sounds
    /// </summary>
    private void DoChangeSoundVolume()
    {
        foreach (Sound item in GlobalNames.soundList)
        {
            item.SetVolume(soundVol.value);
        }
    }
}
