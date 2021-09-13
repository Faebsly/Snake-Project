using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient als Trigger für den Teleport der Schlange auf die Gegeüberliegende Seite.
/// </summary>
public class BoarderTrigger : MonoBehaviour {

    // Felder
    private Boarder thisBoarder;    // Ganau dieser Rahmen
    

    /// <summary>
    /// Sucht den passenden gegenüberliegenden Rahmen
    /// </summary>
    private void Start()
    {
        FindTheBoarder();
    }


    /// <summary>
    /// Der Trigger reagiert auf die Kollision vom Kopf und Körper der Schlange
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GlobalNames.Tags.SnakeHead.ToString() || other.tag == GlobalNames.Tags.SnakeBody.ToString())
        {
            Vector3 headTelPos = new Vector3(); // Speichert die KopfPosition zwischen
            Vector3 bodyTelPos = new Vector3(); // Speichert die KörperPosition zwischen
            switch (thisBoarder.GetAlignment()) // Springt zum passenden Rahmen und Errechnet die Teleportposition an der Gegenseite
            {
                case Boarder.Alignment.north:
                    headTelPos = new Vector3(other.transform.position.x, other.transform.position.y, thisBoarder.GetOppositePosition().z + 1);
                    bodyTelPos = new Vector3(other.transform.position.x, other.transform.position.y, thisBoarder.GetOppositePosition().z + 1);
                    break;
                case Boarder.Alignment.east:
                    headTelPos = new Vector3(thisBoarder.GetOppositePosition().x + 1, other.transform.position.y, other.transform.position.z);
                    bodyTelPos = new Vector3(thisBoarder.GetOppositePosition().x + 1, other.transform.position.y, other.transform.position.z);
                    break;
                case Boarder.Alignment.south:
                    headTelPos = new Vector3(other.transform.position.x, other.transform.position.y, thisBoarder.GetOppositePosition().z - 1);
                    bodyTelPos = new Vector3(other.transform.position.x, other.transform.position.y, thisBoarder.GetOppositePosition().z - 1);
                    break;
                case Boarder.Alignment.west:
                    headTelPos = new Vector3(thisBoarder.GetOppositePosition().x - 1, other.transform.position.y, other.transform.position.z);
                    bodyTelPos = new Vector3(thisBoarder.GetOppositePosition().x - 1, other.transform.position.y, other.transform.position.z);
                    break;
                default:
                    Debug.LogError("Error: Default case reached in OnTriggerEnter -> BoarderTriggers.cs");
                    Debug.Break();
                    break;
            }

            // Speichert und triggert je nach Kopf oder Körper die Position und den Teleport
            if (other.tag == GlobalNames.Tags.SnakeHead.ToString())
            {
                other.GetComponent<SnakeHeadObject>().GetSnake().SetHeadTeleState(true);
                other.GetComponent<SnakeHeadObject>().GetSnake().SetHeadTelePos(headTelPos);
            }
            else
            {
                other.GetComponent<SnakeBodyObject>().GetSnake().SetBodyTeleState(true);
                other.GetComponent<SnakeBodyObject>().GetSnake().SetBodyTelePos(bodyTelPos);
            }
        }
    }
    
    /// <summary>
    /// Sucht den Richtigen Boarder
    /// </summary>
    private void FindTheBoarder()
    {
        
        // Suche in den Boarders den Namen und Weise den gefundenen Boarder zu
        for (int i = 0; i < Level.boarders.Length; i++)
        {
            if (transform.name == Level.boarders[i].GetTag().ToString())
            {
                thisBoarder = Level.boarders[i];
            }
        }
    }

}
