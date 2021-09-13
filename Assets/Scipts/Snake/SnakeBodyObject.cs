using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient zur Steuerung der Bewegung des Körpers
/// </summary>
public class SnakeBodyObject : MonoBehaviour {

    // Felder
    private Snake snake;            
    private WayPoint wayPoint;              // Für die Klasse Snake
    
    /// <summary>
    /// Erstellt den Wegpunkt und das Snake Object
    /// </summary>
    private void Start()
    {
        // Erstellt waypoint und Snake
        wayPoint = new WayPoint(Vector3.zero); 
        snake = new Snake(Level.snakeBodyCount-1, Level.snakeMoveDistance, wayPoint);
    }

    /// <summary>
    /// Steuert die Bewegung des Körpers
    /// </summary>
    public void FollowMovement()
    {
        snake.SetMoveDistance(Level.snakeMoveDistance); // Setzt die Geschwindigkeit
        if (transform.name != GlobalNames.Names.BodyPart_.ToString() + "0") // Wenn dieses Object nicht den Namen BodyPart0 trägt, dann
        {
            // Suche alle Objecte mit den Tag SnakeBody und durchgehe die Objecte
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(GlobalNames.Tags.SnakeBody.ToString());
            foreach (GameObject item in gameObjects)
            {
                // Wenn das gefundene Object die ID Gleich der Aktuellen ID -1 ist, dann ...
                if (item.GetComponent<SnakeBodyObject>().snake.GetID() == snake.GetID() - 1)
                {
                    // Dann nehme den Wegpunkt von dem Item und setzte diese in dieses Object
                    snake.SetWayPoint(new WayPoint(FindAnchor(item.transform)));
                }
            }
        }
        else // Sonst setzte die Anker Position des Kopfes
        {
            snake.SetWayPoint(SnakeHeadObject.snakeHead.GetWayPoint()); // Setzte den Wegpunkt
        }

        snake.Fallow(transform, snake.GetWayPoint()); // folgt dem Wegpunkt
    }

    /// <summary>
    /// Sucht in den CildObjects den TailAnchor für die follge Position
    /// </summary>
    private Vector3 FindAnchor(Transform transObject)
    {
        Vector3 output = new Vector3();
        for (int i = 0; i < transObject.childCount; i++) // Gehe durch die ChildObjecte
        {
            if (transObject.GetChild(i).name == GlobalNames.Names.TailAnchor.ToString()) // Suche das Child mit dem Namen TailAnchor
            {
                output = transObject.GetChild(i).position; // Setzte die Folge Position
            }
        }
        return output;
    }

    /// <summary>
    /// Gibt das SnakeObject zurück
    /// </summary>
    /// <returns></returns>
    public Snake GetSnake()
    {
        return snake;
    }
}
