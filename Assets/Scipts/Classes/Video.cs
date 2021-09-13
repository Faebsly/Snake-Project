using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Stellt den Bauplan für die Vodeos dar
/// </summary>
public class Video {

    // Eigenschaften
    private VideoClip videoClip;
    private double currentTime;

    // Konstructor
    /// <summary>
    /// Erstellt ein Videotrack
    /// </summary>
    /// <param name="videoClipArg">Video Clip</param>
    /// <param name="currentTimeArg">aktuelle Zeit</param>
    public Video(VideoClip videoClipArg, double currentTimeArg)
    {
        videoClip = videoClipArg;
        currentTime = currentTimeArg;
    }

    // Fähigkeiten
    public void SetCurrentTime(double time)
    {
        currentTime = time;
    }

    // Zugriffe
    public VideoClip GetVideoClip()
    {
        return videoClip;
    }
    public double GetCurrentTime()
    {
        return currentTime;
    }
    public string GetClipName()
    {
        return videoClip.name;
    }
}
