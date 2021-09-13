using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Steuert die Videos des Spiels
/// </summary>
public class VideoControl : MonoBehaviour {

    // Felder
    private VideoPlayer videoPlayer;

    private void Start()
    {
        if (StartValidation())
        {
            if (GlobalNames.bGVideoPlaying)
            {
                int ID = GlobalNames.bGVideoID;
                videoPlayer.clip = GlobalNames.videoList[ID].GetVideoClip();
                videoPlayer.time = GlobalNames.videoList[ID].GetCurrentTime();
                videoPlayer.Play();
            }
            else
            {
                PlayANewVideo();
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
        videoPlayer = GetComponent<VideoPlayer>(); // Beschaffe die Komponente

        if (GlobalNames.videoClips.Length == 0 || videoPlayer == null)
        {
            Debug.LogError("Error: Start Validation failed. -> VideoControl.cs || Oder die MusicClips sind leer siehe GlobalNames.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }


        return valid;
    }

    /// <summary>
    /// Prüft ob ein Video zuende ist und spielt dann das Nächste ab
    /// </summary>
    private void Update()
    {
        if (videoPlayer.time == videoPlayer.clip.length) // Wenn das Video zuende ist, spiele ein neues ab
        {
            PlayANewVideo();
        }
    }

    /// <summary>
    /// Spielt ein neuen Zufälligen audioClip ab.
    /// </summary>
    private void PlayANewVideo()
    {
        int counter = 1000; // zum Abfangen einer Endlosschleife
        int currentVideoID = 0; // die aktuelle ID des Songs
        string oldVideoName = GlobalNames.videoList[GlobalNames.bGVideoID].GetClipName();
        string newVideoName = "";
        do
        {
            currentVideoID = Random.Range(0, GlobalNames.videoList.Length); // Erstelle eine Zufällige ID
            videoPlayer.clip = GlobalNames.videoList[currentVideoID].GetVideoClip(); // setzte den Clip
            newVideoName = videoPlayer.clip.name;

            // Fänngt eine Eventuelle Endlosschleife ab
            counter--;
            if (counter <= 0)
            {
                currentVideoID = 0;
                videoPlayer.clip = GlobalNames.videoList[currentVideoID].GetVideoClip();
                Debug.LogError("Error: Endless loop stopped -> VideoControl.cs");
                Debug.Break();
                break;
            }
            // ---------------------------------------------

        } while (oldVideoName == newVideoName);

        GlobalNames.bGVideoID = currentVideoID; // Setzte den ID für das aktuelle Video
        videoPlayer.Play(); // Spiele die Video ab
        GlobalNames.bGVideoPlaying = true;
    }

    /// <summary>
    /// Speichert die Zeit des aktuellen Videos ab
    /// </summary>
    public void DoSaveTime()
    {
        GlobalNames.videoList[GlobalNames.bGVideoID].SetCurrentTime(videoPlayer.time);
    }

    /// <summary>
    /// Stoppt das Video
    /// </summary>
    public void DoStopVideo()
    {
        videoPlayer.Stop();
        GlobalNames.bGVideoPlaying = false;
    }

    

}
