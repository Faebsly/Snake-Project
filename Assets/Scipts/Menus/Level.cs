using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient als Hauptscript für die Levels, besitzt öffentliche Variablen zur Kommunikation innerhalb des Levels.
/// </summary>
public class Level : MonoBehaviour
{

    // Felder
    public static Boarder[] boarders;               // Für die Ränder im Level
    public static float snakeMoveDistance = 1.1f;   // Steuert den abstand den die Schlange in einem Tackt zurücklegt
    public static int snakeBodyCount = 0;           // Die Anzahl der Body Parts der Schlange
    public static float moveTackt = 0.5f;           // Die zu wartenden Sekunden = Bewegungstackt
    public static float moveTacktLable = 1.0f;       // Dient für die Anzeige der Geschwindigkeit
    public static int lifePoints = 3;               // Die Lebenspunkte des Spielers
    public static bool isDead = false;              // der Todes Status
    public static Vector3 BorderCenter;             // Der Mittelpunkt zur errechnung der Ausrichtung der Ränder

    public Transform Ground;                        // Inspector Variable ( Verknüpft mit BorderCenter )
    public Transform[] boardersTransform;           // Inspector Variable (Für die Ränder im Level)
    public bool sankeMoveAllowed = true;            // Steuert die Bewegung der Schlange

    public Material[] materials;                    // Für den Grund
    public Camera topCam;                           // für die Camera zum ändern der Sykboxcolor
    public MusicControl mControl;                   // Dienst als Schnittstelle zur Musik
    public GameObject manuPanle;

    private Hud hud;
    private float timer = 0;                        // Zwischenspeicher für den Timer
    private float timeScale = 0;
    

    // Farben zum ändern der SykboxColor
    private Color green = new Color(0.2588235f, 0.3333333f, 0.1019608f, 1f);  
    private Color braun = new Color(0.3529412f, 0.2470588f, 0.1019608f, 1f);
    private Color yellow = new Color(0.4745098f, 0.4627451f, 0.2078431f, 1f);


    /// <summary>
    /// Setzt nach der Prüfung die Randpositionen
    /// </summary>
    private void Start()
    {
        if (StartValidation())
        {
            
            // Set the Standart
            lifePoints = 3;
            isDead = false;
            moveTackt = 0.5f;
            moveTacktLable = 1;
            snakeBodyCount = 0;
            hud.DoUpdateBodyCount();
            hud.DoUpdateLifePoints();
            hud.DoUpdateSpeedLable();
            timeScale = Time.timeScale;
            manuPanle.SetActive(false);

            SetBoarderPositions();
            BorderCenter = Ground.position;
            ChangeLevelColor();
        }
    }

    /// <summary>
    /// Steuert den bewegungstackt der Schlange
    /// </summary>
    private void Update()
    {
        if (sankeMoveAllowed)
        {
            timer = timer + Time.deltaTime; // Timer Hochzählen
            if (timer > moveTackt) // Zeit erreicht?
            {
                MoveSnakeHead();
                MoveSnakeBody();
                timer = 0; // Timer zurücksetzten
            }
        }

        if (isDead)
        {
            LoadHighscore();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (Time.timeScale != 0)
            {
                timeScale = Time.timeScale;
                Time.timeScale = 0;
                manuPanle.SetActive(true);
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

        hud = GameObject.Find("ScriptConteiner").GetComponent<Hud>(); // Beschaffe das Hud
        if (boardersTransform == null || Ground == null || materials == null  || topCam == null || mControl == null || hud == null || manuPanle == null)
        {
            Debug.LogError("Error: Start Validation failed. -> Level.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    /// <summary>
    /// Läd die Highscoreliste
    /// </summary>
    private void LoadHighscore()
    {
        mControl.DoSaveTime();
        // beende level und lade highscore-Liste wenn die score größer 0 ist
        if (GlobalNames.activePlayerScore > 0)
        {
            GlobalNames.addNewScorelistItem = true; // Aktiviere das Eintragen
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(GlobalNames.SceneNames.HighscoreList.ToString());
    }

    /// <summary>
    /// Füllt das boarders Array mit Tag, Position und gegenüberliegender Position
    /// </summary>
    private void SetBoarderPositions()
    {
        boarders = new Boarder[boardersTransform.Length]; // Erstelle die Ränder anhand der Lenge der boardersTransform

        // Durchlaufe die boardersTransform
        foreach (Transform item in boardersTransform)
        {
            // Suche nach den Namen und erstelle dem Tag und der Position die Ränder
            switch (item.name) 
            {
                case "BoarderTop":
                    boarders[0] = new Boarder(GlobalNames.Tags.BoarderTop, /*item.position,*/ Boarder.Alignment.north);
                    break;
                case "BoarderBottom":
                    boarders[1] = new Boarder(GlobalNames.Tags.BoarderBottom, /*item.position,*/ Boarder.Alignment.south);
                    break;
                case "BoarderRight":
                    boarders[2] = new Boarder(GlobalNames.Tags.BoarderRight, /*item.position,*/ Boarder.Alignment.east);
                    break;
                case "BoarderLeft":
                    boarders[3] = new Boarder(GlobalNames.Tags.BoarderLeft, /*item.position,*/ Boarder.Alignment.west);
                    break;
                default:
                    Debug.LogError("Error: Default Case in Level.cs reached.");
                    Debug.Break();
                    break;
            }
        }

        // Durchlaufe nun das erstellte Array boarders
        for (int i = 0; i < boarders.Length; i++)
        {
            // Suche nach dem gegenüberliegenden Rahmen und setzte diese Position in das boarders Array
            switch (boarders[i].GetTag())
            {
                case GlobalNames.Tags.BoarderTop:
                    foreach (Transform temp in boardersTransform) // Durchlaufe das boardersTransform
                    {
                        if (temp.name == GlobalNames.Tags.BoarderBottom.ToString()) // Suche nach den Gegenüberliegenden Rahmen
                        {
                            boarders[i].SetOppositePosition(temp.position); // Setzte die Opposite Position
                            
                        }
                    }
                    break;
                case GlobalNames.Tags.BoarderBottom:
                    foreach (Transform temp in boardersTransform)
                    {
                        if (temp.name == GlobalNames.Tags.BoarderTop.ToString())
                        {
                            boarders[i].SetOppositePosition(temp.position);
                        }
                    }
                    break;
                case GlobalNames.Tags.BoarderRight:
                    foreach (Transform temp in boardersTransform)
                    {
                        if (temp.name == GlobalNames.Tags.BoarderLeft.ToString())
                        {
                            boarders[i].SetOppositePosition(temp.position);
                        }
                    }
                    break;
                case GlobalNames.Tags.BoarderLeft:
                    foreach (Transform temp in boardersTransform)
                    {
                        if (temp.name == GlobalNames.Tags.BoarderRight.ToString())
                        {
                            boarders[i].SetOppositePosition(temp.position);
                        }
                    }
                    break;
                default:
                    Debug.LogError("Error: Default Case 2 in Level.cs reached.");
                    Debug.Break();
                    break;
            }
        }
    }

    /// <summary>
    /// Sucht den Kopf und führt die Bewegung aus
    /// </summary>
    private void MoveSnakeHead()
    {
        GameObject.FindGameObjectWithTag(GlobalNames.Tags.SnakeHead.ToString()).GetComponent<SnakeHeadObject>().IdleMovement();
    }

    /// <summary>
    /// Sucht die Körper und führt die Bewegungen aus
    /// </summary>
    private void MoveSnakeBody()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(GlobalNames.Tags.SnakeBody.ToString());
        foreach (GameObject item in gameObjects)
        {
            item.GetComponent<SnakeBodyObject>().FollowMovement();
        }
    }

    /// <summary>
    /// Ändert das Material(Boden) und die Farbe(topCam) anhand eines Zufallswertes
    /// </summary>
    private void ChangeLevelColor()
    {
        int rand = Random.Range(1, materials.Length); // Erstelle eine Zufalls Zahl
        Ground.GetComponent<MeshRenderer>().material = materials[rand]; // Ändere den Boden vom Ground
        switch (materials[rand].name)
        {
            case "Ground01":
            case "Ground04":
            case "Ground07":
                topCam.backgroundColor = green;
                break;
            case "Ground02":
            case "Ground05":
            case "Ground08":
                topCam.backgroundColor = braun;
                break;
            case "Ground03":
            case "Ground06":
            case "Ground09":
                topCam.backgroundColor = yellow;
                break;
            default:
                Debug.LogError("Error: Default case reached in ChangeLevelColor -> Level.cs");
                Debug.Break();
                break;
        }

    }

    /// <summary>
    /// Speichert die Aktive PlayerScore und leitet den Spieler zur Highscoreliste
    /// </summary>
    public void ButtonBackClicked()
    {
        if (Time.timeScale == 0)
        {
            manuPanle.SetActive(false);
            Time.timeScale = timeScale;
        }
        GlobalNames.activePlayerScore = snakeBodyCount;
        LoadHighscore();
    }
    
    /// <summary>
    /// Lässt das Spiel weiter laufen und deaktiviert das Menü Hud
    /// </summary>
    public void ButtonContinueClicked()
    {
        if (Time.timeScale == 0)
        {
            manuPanle.SetActive(false);
            Time.timeScale = timeScale;
        }
    }
}
