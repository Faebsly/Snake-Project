using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dient zur Anzeige und aktualisierung der Anzeige des Huds in den Leveln.
/// </summary>
public class Hud : MonoBehaviour {

    public Text speedLable;
    public Text bodyCountLable;
    public Image heartPrefab;

    public Transform heartPanel;


    private void Start()
    {
        if (StartValidation())
        {
            DoUpdateSpeedLable();
            DoUpdateBodyCount();
            DoUpdateLifePoints();
        }
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        

        if (speedLable == null || bodyCountLable == null || heartPrefab == null || heartPanel == null)
        {
            Debug.LogError("Error: Start Validation failed. -> Hud.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    /// <summary>
    /// Updatet das SpeedLable
    /// </summary>
    public void DoUpdateSpeedLable()
    {
        speedLable.text = "Speed: " + Level.moveTacktLable.ToString();
    }

    /// <summary>
    /// Updatet das BodyCountLable
    /// </summary>
    public void DoUpdateBodyCount()
    {
        bodyCountLable.text = "Length: " + Level.snakeBodyCount.ToString();
    }

    /// <summary>
    /// Updatet die LifePoints
    /// </summary>
    public void DoUpdateLifePoints()
    {
        RefreshHeartPanel();
    }

    /// <summary>
    /// Aktualisiert die Herzenanzahl
    /// </summary>
    private void RefreshHeartPanel()
    {
        int lp = Level.lifePoints;

        // Alte Herzen löschen
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(GlobalNames.Tags.HudHeardItem.ToString()))
        {
            Destroy(item);
        }

        // Neue Herzen Erstellen
        for (int i = 0; i < lp; i++)
        {
            Image outputHI = Instantiate(heartPrefab); // Erstellen
            outputHI.tag = GlobalNames.Tags.HudHeardItem.ToString(); // Taggen
            outputHI.transform.SetParent(heartPanel); // Positionieren
        }
    }

   
}
