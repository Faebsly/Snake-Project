using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steuert die Scene ChooseLevel
/// </summary>
public class ChooseLevel : MonoBehaviour
{
    public MusicControl mControl; // Dienst als Schnittstelle zur Musik
    public VideoControl vControl; // Dient als Schnittstelle zu den Videos

    private void Start()
    {
        StartValidation();
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        if (mControl == null || vControl == null)
        {
            Debug.LogError("Error: Start Validation failed. -> ChooseLevel.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }



    // Läd das entsprechende Level
    public void OnButtonLevel1Clicked()
    {
        mControl.DoSaveTime();
        vControl.DoStopVideo();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.Level1.ToString());
    }
    public void OnButtonLevel2Clicked()
    {
        mControl.DoSaveTime();
        vControl.DoStopVideo();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.Level2.ToString());
    }
    public void OnButtonLevel3Clicked()
    {
        mControl.DoSaveTime();
        vControl.DoStopVideo();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.Level3.ToString());
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
}
