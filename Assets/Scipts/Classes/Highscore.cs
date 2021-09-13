using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stellt den Bauplan für ein HighScoreItem dar
/// </summary>
[System.Serializable]
public  class Highscore : System.IComparable
{

    // Eigenschaften
    
    private string name;
    private int score;
    
    // Konstructor
    /// <summary>
    /// Erstelle ein neues HighScoreItem
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    public Highscore(string nameArg, int scoreArg)
    {
        
        name = nameArg;
        score = scoreArg;
    }

    // Fähigkeiten
    
    // Zugriffe
    public string GetName()
    {
        return name;
    }
    public int GetScore()
    {
        return score;
    }

    // Schnittstelle für System.IComparable
    public int CompareTo(object compareObject)
    {
        Highscore tempItem = compareObject as Highscore;
        if (this.score < tempItem.score)
        {
            return 1;
        }
        if (this.score > tempItem.score)
        {
            return -1;
        }
        return 0;

    }
}
