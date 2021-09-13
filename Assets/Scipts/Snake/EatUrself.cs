using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dinst als Trigger für das Abschneiden des Körpers bis zum Ende der Schlange.
/// </summary>
public class EatUrself : MonoBehaviour {

    private string bodyPart;
    private Hud hud;

    private void Start()
    {
        StartValidation();
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        hud = GameObject.Find("ScriptConteiner").GetComponent<Hud>(); // Beschaffe das Hud

        if ( hud == null)
        {
            Debug.LogError("Error: Start Validation failed. -> EatUrself.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    /// <summary>
    /// Kürtzt die Schlange beim Biss und zieht ein Lebenspunkt ab
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == GlobalNames.Tags.SnakeHead.ToString())
        {
            GlobalNames.activePlayerScore = Level.snakeBodyCount;   // Übergebe die Score
            GlobalNames.playSound = true;                           // Für den Soundeffect
            GlobalNames.soundType = GlobalNames.Sounds.EatYousrself;// Für den Soundeffect
            DoDestroy();                                            // Zerstört die objecte ab dem Biss bis zum Ende der Schlange
            if (Level.lifePoints > 1)                               // Zieht ein LP ab oder löst den tot aus
            {
                Level.lifePoints--;
                hud.DoUpdateLifePoints();
                hud.DoUpdateBodyCount();
            }
            else
            {
                Level.lifePoints = 0;
                hud.DoUpdateLifePoints();
                hud.DoUpdateBodyCount();
                Level.isDead = true;
            }
        }
        
    }

    /// <summary>
    /// Zerstört die objecte ab dem Biss bis zum Ende der Schlange
    /// </summary>
    private void DoDestroy()
    {
        int ID = FindID(); // Finde die ID herraus
        if (ID < Level.snakeBodyCount) // ist die ID kleinder als der Körper lang ist dann schneide den Körper an der ID ab.
        {
            DoCutAt(ID);
        }
    }

    /// <summary>
    /// Findet anhand des Namens die ID des Körperteils raus
    /// </summary>
    /// <returns>ID</returns>
    private int FindID()
    {
        bodyPart = transform.name;
        
        string[] parts = bodyPart.Split('_');

        int id = 0;

        for (int i = 0; i < Level.snakeBodyCount; i++)
        {
            if (parts[1] == i.ToString())
            {
                id = i;
            }
        }
        return id;
    }

    /// <summary>
    /// Schneidet den Körper ab der KörperteilID ab
    /// </summary>
    /// <param name="bodyID"></param>
    private void DoCutAt(int bodyID)
    {
        int length = Level.snakeBodyCount - bodyID; // Errechne die Länge des ab zu schneidenden Körpers

        for (int i = 0; i < length; i++)
        {
            int j = i + bodyID; // errechnet die ID
            GameObject temp = GameObject.Find(GlobalNames.Names.BodyPart_.ToString() + j.ToString()); // Sucht das richtige Körperteil
            if (temp.tag == GlobalNames.Tags.SnakeBody.ToString()) // prüft ob das Körperteil auch ein Bodypart ist
            {
                Destroy(temp); //Löscht das Körperteil
                Level.snakeBodyCount--; // Zieht ein Part ab
            }
        }
    }

}
