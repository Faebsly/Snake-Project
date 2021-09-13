using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Löst den Teleport des Kopfes und die löschung des Körpers aus und berechnet die LP neu
/// </summary>
public class WallRun : MonoBehaviour {

    //private Snake snakeTemp;        // Ein Temporäres Snake Object

    private Vector3 spawnpoint;
    private Hud hud;

    /// <summary>
    /// Erstellt das Snake TempObject
    /// </summary>
    private void Start()
    {
        if (StartValidation())
        {
            //snakeTemp = new Snake(0, 0, new WayPoint(Vector3.zero));
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

        if (hud == null)
        {
            Debug.LogError("Error: Start Validation failed. -> WallRun.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    /// <summary>
    /// Lösst den Teleport des Kopfes und die löschung des Körpers aus und berechnet die LP neu
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GlobalNames.Tags.SnakeHead.ToString())
        {
            GlobalNames.activePlayerScore = Level.snakeBodyCount; // Übergebe die Score
            if (Level.lifePoints > 1) // Berechnet das Leben neu
            {
                GlobalNames.playSound = true;
                GlobalNames.soundType = GlobalNames.Sounds.WallRun;
                Level.lifePoints--;
                hud.DoUpdateLifePoints();
            }
            else
            {
                Level.lifePoints = 0;
                hud.DoUpdateLifePoints();
                hud.DoUpdateBodyCount();
                Level.isDead = true;
            }
            DelBody(); // Löscht alle Body Parts
            spawnpoint = new Vector3(0, other.transform.position.y, 0); // Schreibt die Spawnposition
            other.GetComponent<SnakeHeadObject>().GetSnake().SetHeadTeleState(true); // Startet die Teleportation
            other.GetComponent<SnakeHeadObject>().GetSnake().SetHeadTelePos(spawnpoint); // Teleportiert die Schlange
        }
    }

    /// <summary>
    /// Löscht alle Body Parts
    /// </summary>
    private void DelBody()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(GlobalNames.Tags.SnakeBody.ToString()))
        {
            Destroy(item);
            Level.snakeBodyCount = 0;
            hud.DoUpdateBodyCount();
        }
    }
}
