using UnityEngine;

/// <summary>
/// Stellt den Bauplan für die Schlangen dar
/// </summary>
public class Snake {

    // für die Richtungangabe
    public enum Direction
    {
        up,
        down,
        right,
        left
    }

    // Eigenschaften ........................................
    private float moveDistance;         // Die Distanz die die Objecte in einem tackt zurücklegen
    private int id;                     // Die ID für die Körperteile
    private Direction moveDirection;    // Die Bewegungsrichtung
    private WayPoint wayPoint;          // Für den Wegpunkt
    private bool bodyRunState;          // Status: Wenn der Vordere Teil teleportiert wurde Greift dieser Status und Lässt das Object Gerade aus weiter gehen. Der Status wird erst beim teleportieren wieder geändert.
    private bool bodyTeleState;         // Status: Der Status aktiviert den Teleport in der Bewegung und hällt an bis der Teleport durchgeführt wurde.
    private bool headTeleState;         // Status:  der Gleiche wie beim Körper nur für den Kopf
    private Vector3 headTelPos;         // Speichert die Teleportationsposition 
    private Vector3 bodyTelPos;         // Speichert die Teleportationsposition 


    // Konstructor ........................................
    /// <summary>
    /// Erstellt eine neue Schlange, Kopf oder Körper
    /// </summary>
    /// <param name="idArg">Relevent für den Körper</param>
    /// <param name="moveDistanceArg">Distanz</param>
    /// <param name="wayPointArg">Waypoint</param>
    public Snake(int idArg, float moveDistanceArg, WayPoint wayPointArg)
    {
        id = idArg;
        moveDistance = moveDistanceArg;
        moveDirection = Direction.up;
        wayPoint = wayPointArg;
        bodyRunState = false;
        bodyTeleState = false;
        headTeleState = false;
        headTelPos = Vector3.zero;
        bodyTelPos = Vector3.zero;
    }

    // Fähigkeiten ........................................
    /// <summary>
    /// Bewegt anhand der Richtung und geschwindigkeit die Schlange.
    /// </summary>
    /// <param name="moveObject">Das zu bewegende Object.</param>
    public void Move(Transform moveObject)
    {
        switch (moveDirection) // choose the direction
        {
            case Direction.up:
                moveObject.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.down:
                moveObject.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case Direction.right:
                moveObject.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case Direction.left:
                moveObject.rotation = Quaternion.Euler(0, 270, 0);
                break;
            default:
                Debug.LogError("Error: Default Case reached -> Snake.cs");
                Debug.Break();
                break;
        }
        Snake tempHead = moveObject.GetComponent<SnakeHeadObject>().GetSnake(); // Hole das Snake Object

        if (tempHead.GetHeadTeleState()) // Wenn der Teleportationsstatus active ist dann...
        {

            moveObject.position = tempHead.GetHeadTelePos() + (moveObject.forward * moveDistance); // Teleporte Das Schlangen-Element auf die gegenüberliegende Seite
            tempHead.SetHeadTeleState(false); // Deaktiviere die Teleportation
            
            // Sucht das erste BoddyPart und setzt den Laufstatus auf wahr
            GameObject item = GameObject.Find(GlobalNames.Names.BodyPart_.ToString() + "0");
            if (item != null) // Sollte die Schlange noch kein Körper haben so wird das item null sein
            {
                item.GetComponent<SnakeBodyObject>().GetSnake().SetRunState(true);
            }
        }
        else // Sonst führe die Standartbewegung aus
        {
            moveObject.position = moveObject.position + (moveObject.forward * moveDistance);
        }
    }

    /// <summary>
    /// Bewegt das Object in die richtung in der es schaut (Vorne)
    /// </summary>
    /// <param name="moveObject">Das zu bewegende Object.</param>
    public void Fallow(Transform moveObject)
    {
        Snake tempBody = moveObject.GetComponent<SnakeBodyObject>().GetSnake(); // Hole das Snake Object

        if (tempBody.GetBodyTeleState()) // Wenn der Teleportationsstatus active ist dann..
        {
            moveObject.position = moveObject.GetComponent<SnakeBodyObject>().GetSnake().GetBodyTelePos() + (moveObject.forward * moveDistance); // Teleporte Das Schlangen-Element auf die gegenüberliegende Seite
            moveObject.GetComponent<SnakeBodyObject>().GetSnake().SetBodyTeleState(false); // deaktiviert beide Stati
            moveObject.GetComponent<SnakeBodyObject>().GetSnake().SetRunState(false);
            
            int id = moveObject.GetComponent<SnakeBodyObject>().GetSnake().GetID() + 1; // Holt sich die ID des nachvolgers
            if (id < Level.snakeBodyCount)
            {
                GameObject item = GameObject.Find(GlobalNames.Names.BodyPart_.ToString() + id.ToString()); // Holt sich den Nachvolger und aktiviert den LAufStatus
                item.GetComponent<SnakeBodyObject>().GetSnake().SetRunState(true);
            }
        }
        else // Sonst führe die Standartbewegung aus
        {
            moveObject.position = moveObject.position + (moveObject.forward * moveDistance);
        }
        
    }

    /// <summary>
    /// Bewegt das Object in die Richtung des Wegpunkts
    /// </summary>
    /// <param name="moveObject">Das zu bewegende Object.</param>
    /// <param name="wayPoint">Die an zu peilende Richtung.</param>
    public void Fallow(Transform moveObject, WayPoint wayPoint)
    {
        // Wenn der Laufstatus aktiviert ist laufe nur gerade aus
        if (moveObject.GetComponent<SnakeBodyObject>().GetSnake().GetBodyRunState())
        {
            Fallow(moveObject);
            
        }
        else // Sonst schaue in die richtung des Verherigen Object Akers
        {
            moveObject.LookAt(wayPoint.GetPosition());
            Fallow(moveObject);
        }
        
    }
    
    
    // Zugriffe ........................................
    // Setter --------------------

    /// <summary>
    /// Setzt den neuen Wegpunkt
    /// </summary>
    /// <param name="oneWayPoint">Neuer Wegpunkt</param>
    public void SetWayPoint(WayPoint oneWayPoint)
    {
        wayPoint = oneWayPoint;
    }

    /// <summary>
    /// Setzt die Bewegungsgeschrindigkeit
    /// </summary>
    /// <param name="value">Geschwindigkeit</param>
    public void SetMoveDistance(float value)
    {
        moveDistance = value;
    }

    /// <summary>
    /// Setzt die Richtung
    /// </summary>
    /// <param name="newDirection">Richtung</param>
    public void SetDirection(Direction newDirection)
    {
        moveDirection = newDirection;
    }

    /// <summary>
    /// Setzt den Lauf Status 
    /// </summary>
    /// <param name="state"></param>
    public void SetRunState(bool state)
    {
        bodyRunState = state;
    }

    /// <summary>
    /// Setzt den Teleportstatus Körper
    /// </summary>
    /// <param name="state"></param>
    public void SetBodyTeleState(bool state)
    {
        bodyTeleState = state;
    }

    /// <summary>
    /// Setzt den Teleportstatus Kopf
    /// </summary>
    /// <param name="state"></param>
    public void SetHeadTeleState(bool state)
    {
        headTeleState = state;
    }

    /// <summary>
    /// Setzt die Teleport-Position Kopf
    /// </summary>
    /// <param name="pos"></param>
    public void SetHeadTelePos(Vector3 pos)
    {
        headTelPos = pos;
    }

    /// <summary>
    /// Setzt die Teleport-Position Körper
    /// </summary>
    /// <param name="pos"></param>
    public void SetBodyTelePos(Vector3 pos)
    {
        bodyTelPos = pos;
    }


    // Getter -------------------

    /// <summary>
    /// Gibt den Wegpunkt zurück
    /// </summary>
    /// <returns>Wegpunkt</returns>
    public WayPoint GetWayPoint()
    {
        return wayPoint;
    }

    /// <summary>
    /// Gibt die Richtung zurück
    /// </summary>
    /// <returns></returns>
    public Direction GetMoveDirection()
    {
        return moveDirection;
    }

    /// <summary>
    /// Gibt die ID zurück
    /// </summary>
    /// <returns></returns>
    public int GetID()
    {
        return id;
    }

    /// <summary>
    /// Gibt die Bewegungsentefernung für ein Takt zurück
    /// </summary>
    /// <returns></returns>
    public float GetMoveDistance()
    {
        return moveDistance;
    }

    /// <summary>
    /// Gibt den Lauf-Status zurück
    /// </summary>
    /// <returns></returns>
    public bool GetBodyRunState()
    {
        return bodyRunState;
    }

    /// <summary>
    /// Gibt den Teleportation-Status Körper zurück
    /// </summary>
    /// <returns></returns>
    public bool GetBodyTeleState()
    {
        return bodyTeleState;
    }

    /// <summary>
    /// Gibt den Teleportation-Status Kopf zurück
    /// </summary>
    /// <returns></returns>
    public bool GetHeadTeleState()
    {
        return headTeleState;
    }

    /// <summary>
    ///  Gibt die Teleportation-Position Kopf zurück
    /// </summary>
    /// <returns></returns>
    public Vector3 GetHeadTelePos()
    {
        return headTelPos;
    }

    /// <summary>
    /// Gibt die Teleportation-Position Körper zurück
    /// </summary>
    /// <returns></returns>
    public Vector3 GetBodyTelePos()
    {
        return bodyTelPos;
    }

}
