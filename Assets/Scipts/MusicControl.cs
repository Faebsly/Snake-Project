using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steuert die Musik des Spiels
/// </summary>
public class MusicControl : MonoBehaviour {

    // Felder
    private AudioSource audioPlayer;
    
    private void Start()
    {
        if (StartValidation())
        {
            if (GlobalNames.bGMusicPlaying) // Wenn musik schon Spielt...
            {
                int ID = GlobalNames.bGMusicID;                                 // ID holen
                audioPlayer.clip = GlobalNames.musicList[ID].GetAudioClip();    // Clip setzten
                audioPlayer.time = GlobalNames.musicList[ID].GetCurrentTime();  // Zeit setzten
                audioPlayer.volume = GlobalNames.musicList[ID].GetVolume();     // Lautstärke setzten
                audioPlayer.Play();                                             // Musik abspielen
            }
            else // sonst spiele ein neuen Song ab
            {
                PlayANewSong();
            }
            
        }        
    }
    
    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        audioPlayer = GetComponent<AudioSource>(); // Beschaffe die Komponente

        if (GlobalNames.musicClips.Length == 0 || audioPlayer == null)
        {
            Debug.LogError("Error: Start Validation failed. -> MusicControl.cs || Oder die MusicClips sind leer siehe GlobalNames.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        

        return valid;
    }

    /// <summary>
    /// Prüft ob ein Lied zuende ist und spielt dann das Nächste ab
    /// </summary>
    private void Update()
    {
        if (audioPlayer.time == audioPlayer.clip.length) // Wenn das Lied zuende ist, spiele ein neues Lied ab
        {
            PlayANewSong();
        }
    }

    /// <summary>
    /// Spielt ein neuen Zufälligen audioClip ab.
    /// </summary>
    private void PlayANewSong()
    {
        int counter = 1000; // zum Abfangen einer Endlosschleife
        int currentSongID = 0; // die aktuelle ID des Songs
        string oldClipName = GlobalNames.musicList[GlobalNames.bGMusicID].GetClipName(); 
        string newClipName = "";
        do
        {
            currentSongID = Random.Range(0, GlobalNames.musicList.Length); // Erstelle eine Zufällige ID
            audioPlayer.clip = GlobalNames.musicList[currentSongID].GetAudioClip(); // setzte den Clip
            audioPlayer.time = 0;
            newClipName = audioPlayer.clip.name;

            // Fänngt eine Eventuelle Endlosschleife ab
            counter--;
            if (counter <= 0)
            {
                currentSongID = 0;
                audioPlayer.clip = GlobalNames.musicList[currentSongID].GetAudioClip();
                Debug.LogError("Error: Endless loop stopped -> MusicControl.cs");
                Debug.Break();
                break;
            }
            // ---------------------------------------------

        } while (oldClipName == newClipName);

        GlobalNames.bGMusicID = currentSongID; // Setzte den ID für den aktuellen Tittle
        audioPlayer.volume = GlobalNames.musicList[currentSongID].GetVolume(); // Stellt die aktuelle Lautstärke ein
        audioPlayer.Play(); // Spiele die musik ab
        GlobalNames.bGMusicPlaying = true;
    }

    /// <summary>
    /// Speichert die Zeit des aktuellen Clips ab
    /// </summary>
    public void DoSaveTime()
    {
        GlobalNames.musicList[GlobalNames.bGMusicID].SetCurrentTime(audioPlayer.time);
    }

    /// <summary>
    /// Stoppt die Musik
    /// </summary>
    public void DoStopMusic()
    {
        audioPlayer.Stop();
        GlobalNames.bGMusicPlaying = false;
    }

    /// <summary>
    /// Ändert die lautstärke der Musik
    /// </summary>
    /// <param name="musicVolume"></param>
    public void ChangeVolume(float musicVolume)
    {
        GlobalNames.musicList[GlobalNames.bGMusicID].SetVolume(musicVolume);
        audioPlayer.volume = GlobalNames.musicList[GlobalNames.bGMusicID].GetVolume();
    }
}
