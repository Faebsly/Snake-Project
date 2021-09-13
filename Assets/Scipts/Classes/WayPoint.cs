using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stellt den Bauplan für die WayPoints dar
/// </summary>
public class WayPoint {

    // Eigenschaften
    private Vector3 position;
    private bool canDelete;
    
    // Konstructor
    /// <summary>
    /// Erstellt ein neuen Wegpunkt.
    /// Position und Löschstatus (= false)
    /// </summary>
    /// <param name="positionArg">Position</param>
    public WayPoint(Vector3 positionArg)
    {
        position = positionArg;
        canDelete = false;
    }

    // Fähigkeiten
    public void SetDeleteState(bool state)
    {
        canDelete = state;
    }

    // Zugriffe
    public Vector3 GetPosition()
    {
        return position;
    }
    public bool GetDeleteState()
    {
        return canDelete;
    }

}
