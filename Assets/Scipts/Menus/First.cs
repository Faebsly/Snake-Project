using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Steuert die Scene First
/// Sie dient nur für das Laden der GlobalNames.cs und für ein Startbildschirm mit Credits
/// </summary>
public class First : MonoBehaviour {

    public MusicControl mControl; // Dient als Schnittstelle zur Musik
    public VideoControl vControl; // Dient als Schnittstelle zu den Videos

    private void Start ()
    {
        if (StartValidation())
        {
            StartCoroutine("Timer", 10);
        }
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
            Debug.LogError("Error: Start Validation failed. -> First.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }


    // Timer für den Startbildshirm
    IEnumerator Timer(int time)
    {
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1);
        }

        mControl.DoSaveTime(); // Speichert die aktuelle Zeit des Clips
        vControl.DoSaveTime(); // Speichert die aktuelle Zeit des Videos
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.MainMenu.ToString()); // Danach Lade das Hauptmenü
    }

}
