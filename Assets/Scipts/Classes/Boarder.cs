using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stellt den Bauplan für die Ränder dar
/// </summary>
public class Boarder  {

    public enum Alignment
    {
        none,
        north, // oben
        east,  // rechts
        south, // unten
        west   // links
    }

    // Eigenschaften
    private GlobalNames.Tags tag;
    //private Vector3 position;
    private Vector3 oppositePosition;

    private Alignment alignment;

    // Konstructor
    public Boarder(GlobalNames.Tags tagArg, /*Vector3 positionArg,*/ Alignment alignmentArg)
    {
        tag = tagArg;
        //position = positionArg;
        oppositePosition = Vector3.zero;
        alignment = alignmentArg;
    }

    // Fähigkeiten
    /// <summary>
    /// Setzt die Position der Gegenseite
    /// </summary>
    /// <param name="pos"></param>
    public void SetOppositePosition( Vector3 pos)
    {
        oppositePosition = pos;
    }

    /*
    /// <summary>
    /// Findet die Ausrichtung der mitgegebenen Coordinaten herraus
    /// </summary>
    /// <param name="center"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public Alignment DoDetectAlignment(Vector3 center, Vector3 target)
    {
        Alignment ali = Alignment.none;

        if (center.x != target.x)
        {
            if (center.x < target.x)
            {
                ali = Alignment.east;
            }
            else
            {
                ali = Alignment.west;
            }
        }
        else
        {
            if (center.z < target.z)
            {
                ali = Alignment.north;
            }
            else
            {
                ali = Alignment.south;
            }
        }
        return ali;
    }
    */

    // Zugriffe
    public GlobalNames.Tags GetTag()
    {
        return tag;
    }
    public Vector3 GetOppositePosition()
    {
        return oppositePosition;
    }
    public Alignment GetAlignment()
    {
        return alignment;
    }
}
