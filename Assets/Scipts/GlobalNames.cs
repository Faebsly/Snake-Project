using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Dient als Hauptscript für das ganze Spiel. Es hat Öffentliche Steuerungs-Variablen und speichert Dinge Zwischen für andere Scripts.
/// </summary>
public class GlobalNames : MonoBehaviour {

    // Felder
    // Die Globalen Statischen Variablen
    public static bool addNewScorelistItem = false;     // Status für ein neues Scorlist Item
    public static int activePlayerScore = 0;            // Die aktuelle Score des Spielers für die Eintragung in die Hiughscoreliste, wird beim neuen Spiel auf 0 gesetzt
    public static int bGMusicID = 0;                    // Den Namen des Aktuellen Clips
    public static bool bGMusicPlaying = false;          // Status ob Musik gerade Spielt
    public static int bGVideoID = 0;                    // Den Namen des Aktuellen Clips
    public static bool bGVideoPlaying = false;          // Status ob das Video gerade Spielt
    public static AudioClip[] musicClips;               // Die Music Clips
    public static Music[] musicList;
    public static VideoClip[] videoClips;               // Die Video Clips
    public static Video[] videoList;
    public static AudioClip[] soundClips;               // Die Sound Clips
    public static Sound[] soundList;
    public static bool playSound = false;               // Ein trigger fürs abspielen des Sounds
    public static Sounds soundType = Sounds.none;

    // Die Globalen Enumarationen für die Namen

    /// <summary>
    /// Namen der Scenen
    /// </summary>
    public enum SceneNames 
    {
        First,
        MainMenu,
        HighscoreList,
        Option,
        LevelChoose,
        Level1,
        Level2,
        Level3,
        Level4
    }

    /// <summary>
    /// Namen der Tags
    /// </summary>
    public enum Tags
    {
        none,
        HighScoreListItem,
        BoarderTop,
        BoarderBottom,
        BoarderRight,
        BoarderLeft,
        SnakeHead,
        SnakeBody,
        TailAnchor,
        FoodObject,
        HudHeardItem,
        Wall
    }

    /// <summary>
    /// Für gewisse namen im Code
    /// </summary>
    public enum Names
    {
        BodyPart_,
        TailAnchor
    }

    /// <summary>
    /// Für die Typ Namen der Sounds
    /// </summary>
    public enum Sounds
    {
        none,
        Eat,
        EatYousrself,
        WallRun
    }

    private void Start()
    {
        if (StartValidation())
        {
            musicList = new Music[musicClips.Length]; // erstelle das Array anhand der Länge der Clips
            for (int i = 0; i < musicClips.Length; i++) // Fülle das Array
            {
                musicList[i] = new Music(musicClips[i], 0, 0.3f);
            }
            videoList = new Video[videoClips.Length]; // Das Gleiche für die Videos
            for (int i = 0; i < videoClips.Length; i++)
            {
                videoList[i] = new Video(videoClips[i], 0d);
            }
            soundList = new Sound[soundClips.Length]; // erstelle das Array anhand der Länge der Clips
            for (int i = 0; i < soundClips.Length; i++) // Fülle das Array
            {
                soundList[i] = new Sound(soundClips[i], 0.8f, Sounds.none);
            }
            SetSoundTypes();
        }
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        musicClips = Resources.LoadAll<AudioClip>("Audio/Music/"); // Läd alle Musicstücke von dem Ordner und füllt das Array
        videoClips = Resources.LoadAll<VideoClip>("Video/"); // Läd alle Videoclips ins Array
        soundClips = Resources.LoadAll<AudioClip>("Audio/Sounds/"); // Läd alle Soundclips ins Array
        if (musicClips.Length == 0 || videoClips.Length == 0)
        {
            Debug.LogError("Error: Start Validation failed. -> GlobalNames.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }

        return valid;
    }

    /// <summary>
    /// Setzt den Typ des Sounds für die Identifizierung
    /// </summary>
    private void SetSoundTypes()
    {
        foreach (Sound item in soundList)
        {
            switch (item.GetClipName())
            {
                case "Eat":
                    item.SetSoundType(Sounds.Eat);
                    break;
                case "EatYousrself":
                    item.SetSoundType(Sounds.EatYousrself);
                    break;
                case "WallRun":
                    item.SetSoundType(Sounds.WallRun);
                    break;
                default:
                    break;
            }
        }
    }
}
