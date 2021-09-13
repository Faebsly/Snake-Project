using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Steuert die Scene MainMenu
/// </summary>
public class MainMenu : MonoBehaviour {

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
            Debug.LogError("Error: Start Validation failed. -> MainMenu.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }



    // Die einzelnen Buttons, laden die einzelnen Scenen bis auf dem Quit Button. Er beendet das Spiel.

    public void OnButtonNewGameClicked()
    {
        mControl.DoSaveTime(); // Speichert die aktuelle Zeit des Clips
        vControl.DoSaveTime(); // Speichert die aktuelle Zeit des Videos
        SceneManager.LoadScene(GlobalNames.SceneNames.LevelChoose.ToString());
    }

    public void OnButtonHighScoreListClicked()
    {
        mControl.DoSaveTime();
        vControl.DoSaveTime();
        SceneManager.LoadScene(GlobalNames.SceneNames.HighscoreList.ToString());
    }

    public void OnButtonOptionClicked()
    {
        mControl.DoSaveTime();
        vControl.DoSaveTime();
        SceneManager.LoadScene(GlobalNames.SceneNames.Option.ToString());
    }

    public void OnButtonQuitClicked()
    {
        mControl.DoStopMusic(); // Stoppt die Music und das Video
        vControl.DoStopVideo();
        Debug.Break();
        Application.Quit();
    }
}
