using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient zur steuerung des Schlangenkopfes, Bewegung, Lenkung
/// </summary>
public class SnakeHeadObject : MonoBehaviour {

    // Felder
    public static Snake snakeHead;      // Der Zugriff von außen muss gegeben sein

    private Transform followPosition;   // Speichert die Position für den folgenden Body Part
    private WayPoint wayPoint;          // Für die Klasse Snake
    
    /// <summary>
    /// Sucht und setzt die Folge Position, Erstellt den Wegpunkt und das Snake Object
    /// </summary>
    private void Start()
    {
        FindAnchor();
        wayPoint = new WayPoint(Vector3.zero);          // Erstelle ein leeren Wegpunkt
        snakeHead = new Snake(0, Level.snakeMoveDistance, wayPoint); // Erstelle die Klasse mit Geschwindigkeit und Wegpunkten
    }

    /// <summary>
    /// Lässt auf Eingaben prüfen
    /// </summary>
    private void Update()
    {
        CheckKeys();
    }

    /// <summary>
    /// Führt die Standartbewegung aus und speichert die Wegpunkte
    /// </summary>
    public void IdleMovement()
    {
        snakeHead.SetMoveDistance(Level.snakeMoveDistance); // Setzt die Geschwindigkeit
        snakeHead.Move(transform); // Die Schlange bewegen
        snakeHead.SetWayPoint(new WayPoint(followPosition.position)); // erstellt ein neuen Wegpunkt
    }

    /// <summary>
    /// Prüfe auf eingaben aller Art
    /// </summary>
    private void CheckKeys()
    {
        float horiInput = Input.GetAxis("Horizontal"); // Speichere den Wert der Horizontalen Achse zwischen
        float vertiInput = Input.GetAxis("Vertical");  // Speichere den Wert der Verticalen Achse zwischen

        // Frage die Achse ab und setzte die Richtung
        if (Input.GetButtonDown("Horizontal"))
        {
            if (horiInput != 0)
            {
                if (horiInput > 0) // 1 Rechts
                {
                    // Nur wenn die aktuelle bewegung nicht engegengesetzt ist dann darf bewegt werden
                    if (snakeHead.GetMoveDirection() != Snake.Direction.left) 
                    {
                        snakeHead.SetDirection(Snake.Direction.right);
                    }
                }
                else // -1 Links
                {
                    if (snakeHead.GetMoveDirection() != Snake.Direction.right)
                    {
                        snakeHead.SetDirection(Snake.Direction.left);
                    }
                }
            }
        }

        // Frage die Achse ab und setzte die Richtung
        if (Input.GetButtonDown("Vertical"))
        {
            if (vertiInput != 0)
            {
                if (vertiInput > 0) // 1 Hoch
                {
                    if (snakeHead.GetMoveDirection() != Snake.Direction.down)
                    {
                        snakeHead.SetDirection(Snake.Direction.up);
                    }
                    
                }
                else // -1 Runter
                {
                    if (snakeHead.GetMoveDirection() != Snake.Direction.up)
                    {
                        snakeHead.SetDirection(Snake.Direction.down);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sucht in den CildObjects den TailAnchor für die Follge Position
    /// </summary>
    private void FindAnchor()
    {
        for (int i = 0; i < transform.childCount; i++) // Gehe durch die ChildObjecte
        {
            if (transform.GetChild(i).name == GlobalNames.Names.TailAnchor.ToString()) // Suche das Child mit dem Namen TailAnchor
            {
                followPosition = transform.GetChild(i); // Setzte die Folge Position
            }
        }
    }

    /// <summary>
    /// Gibt das SnakeObject zurück
    /// </summary>
    /// <returns></returns>
    public Snake GetSnake()
    {
        return snakeHead;
    }

}
