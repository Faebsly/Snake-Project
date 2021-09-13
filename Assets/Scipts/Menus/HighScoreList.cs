using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Steuert die Scene Highscoreliste
/// </summary>
public class HighScoreList : MonoBehaviour {

    // Felder
    public Canvas addListCanvas;                        // Das Addlist Canvas, was aus und eingeblendet wird 
    public GameObject highScoreListItemPrefab;          // Das Prefab für das Highscorelist-Item
    public GameObject targetListObject;                 // Das Parent für die Highscorelist-Items

    public UnityEngine.UI.InputField playerNameInput;   // der Name des Spielers
    public UnityEngine.UI.Button signMeUpButton;        // Der button zum eintragen des Spielernamens in die Liste

    public MusicControl mControl;                       // Dienst als Schnittstelle zur Musik
    public VideoControl vControl;                       // Dient als Schnittstelle zu den Videos

    private List<Highscore> highscoreList;              // Für die Liste mit den Werten
    
    private string pathFilename;                        // Für den Pfad und Name in einem String
    private FileStream fileStream;                      // Für den Filestream zum Lesen und Speichern
    private BinaryFormatter binFormatter;               // Zum Formatieren der Daten in die Datei und aus der Datei

    /// <summary>
    /// führt Validierung aus, setzt den Pfand, läd die Liste und prüft ob ein neuer Eintrag in die Liste erstellt werden soll, wenn ja blende Das Canvas ein, sonst aus
    /// </summary>
    private void Start()
    {
        if (StartValidation())
        {
            pathFilename = Path.Combine(Application.dataPath, "highscore.bin"); // erstelle den Pfad
            LoadList(); // Lade die liste

            // Soll ein neuer Eintrag in der Liste vorgenommen werden?
            if (GlobalNames.addNewScorelistItem) // JA
            {
                addListCanvas.enabled = true; // Canvas aktivieren
                GlobalNames.addNewScorelistItem = false; // Die Statische Variable zurücksetzten
            }
            else // Nein
            {
                addListCanvas.enabled = false; // Canvas deaktivieren
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

        if (addListCanvas == null || highScoreListItemPrefab == null || targetListObject == null || playerNameInput == null || signMeUpButton == null || mControl == null || vControl == null)
        {
            Debug.LogError("Error: Start Validation failed. -> HighScoreList.cs");
            Debug.Break();
        }
        else
        {
            valid = true;
        }

        return valid;
    }

    /// <summary>
    /// Sortiert die Liste neu, löscht die alten Einträge und erstellt die neuen Einträge
    /// </summary>
    private void RefreshList()
    {
        highscoreList.Sort(); // Liste Sortieren
        // Alte einträge löschen
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(GlobalNames.Tags.HighScoreListItem.ToString()))
        {
            Destroy(item);
        }

        GameObject output;
        int rank = 1;
        // Liste neu erzeugen und anzeigen
        foreach (Highscore item in highscoreList)
        {
            output = Instantiate(highScoreListItemPrefab); // Erstellen
            output.transform.SetParent(targetListObject.transform); // Parent setzten
            output.tag = GlobalNames.Tags.HighScoreListItem.ToString(); // Tag Setzten
            output.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = rank.ToString();            // Dem Cild 1 den Rang zuweisen
            output.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = item.GetName();             // Dem Cild 2 dem Namen zuweisen
            output.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>().text = item.GetScore().ToString(); // Dem Cild 3 die Score zuweisen
            rank++; // Den Rang erhöhen
        }

    }

    /// <summary>
    /// Läd entweder die Liste aus der Datei oder Erstellt eine neue Liste und führt anschließend RefreshList() aus
    /// </summary>
    private void LoadList()
    {
        if (File.Exists(pathFilename))
        {
            fileStream = new FileStream(pathFilename, FileMode.Open, FileAccess.Read); // erstelle den Stream
            binFormatter = new BinaryFormatter(); // neuen Formatter erstellen
            highscoreList = binFormatter.Deserialize(fileStream) as List<Highscore>; // Lade und Formatiere die Daten in die Variable
            fileStream.Close(); // Schließe den Stream
        }
        else // falls die Datei nicht vorhanden ist dann erstelle eine leere Liste
        {
            CreateEmptyList();
        }
        
        RefreshList(); // Liste anzeigen

    }

    /// <summary>
    /// Sortiert, speichert und überschreibt die aktuelle Liste als Datei 
    /// </summary>
    private void SaveList()
    {
        highscoreList.Sort(); // Liste vor dem Speichern Sortieren
        fileStream = new FileStream(pathFilename, FileMode.Create); // Neue Datei erstellen
        binFormatter = new BinaryFormatter(); // neuen Formatter erstellen
        binFormatter.Serialize(fileStream, highscoreList); // Dateien Formatieren und Schreiben
        fileStream.Close(); // Stream schließen
    }

    /// <summary>
    /// Erstellt nur eine Leere Liste ins highscoreList List Object
    /// </summary>
    private void CreateEmptyList()
    {
        highscoreList = new List<Highscore>();
        for (int i = 0; i < 10; i++)
        {
            highscoreList.Add(new Highscore("empty", 0));
        }
    }

    /// <summary>
    /// Fügt ein neuen Eintrag hunzu, sortiert die Liste neu und löscht die Einträge am Ende der Liste wenn es mehr als 10 Einträge sind.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    private void AddNewNameToList(string name, int score)
    {
        highscoreList.Add(new Highscore(name, score)); // füge neuen Eintrag hinzu
        highscoreList.Sort(); // Sortiere die Liste

        // Löscht die letzten Elemente
        while (highscoreList.Count > 10)
        {
            highscoreList.RemoveAt(highscoreList.Count - 1);
        }
    }

    /// <summary>
    /// Läd die Scene für das Hauptmenü
    /// </summary>
    public void OnButtonBackClicked()
    {
        mControl.DoSaveTime();
        vControl.DoSaveTime();
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.MainMenu.ToString());
    }

    /// <summary>
    /// Führt AddNewNameToList() mit parameter aus, Deaktiviert den Button und das Canvas, Aktualisiert die Lisste und speichert sie ab.
    /// </summary>
    public void OnButtonSignMeUpClicked()
    {
        AddNewNameToList(playerNameInput.text, GlobalNames.activePlayerScore); // Eintrag hinzufügen
        addListCanvas.enabled = false;          // Canvas deaktiviern
        signMeUpButton.interactable = false;    // Button deaktiviern
        RefreshList();  // Liste aktualisieren
        SaveList();     // Liste speichern
    }

    /// <summary>
    /// Wenn eine Eingabe getätigt wird dann activiere den signMeUp Button
    /// </summary>
    public void OnInputFieldValueChanged()
    {
        signMeUpButton.interactable = true;
    }
}
