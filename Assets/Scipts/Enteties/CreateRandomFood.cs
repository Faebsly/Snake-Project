using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient zur Steuerung des Erscheinens von Essen auf einer bestimmten Fläsche
/// </summary>
public class CreateRandomFood : MonoBehaviour {

    //Felder
    public GameObject foodPrefab; // Das Prefab für das Essen
    public GameObject spawnArea; // Der Bereich zum Spawnen des Essens

    // Gibt die Warcheinlichkeit an
    public int Cherry = 6;
    public int Banana = 8;
    public int Chocolate = 3;

    // Für die berechnung der Warcheinlichkeit
    private int chanceTotal = 0;
    private float ch_Cherry = 0.0f;
    private float ch_Banana = 0.0f;
    private float ch_Chocolate = 0.0f;
    
    private MeshFilter meshFilter;
    
    private void Start()
    {
        if (StartValidation())
        {
            CalcChance();
        }
    }

    private void Update()
    {
        CheckAppleCount();
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        
        if (foodPrefab == null || spawnArea == null)
        {
            Debug.LogError("Error: Start Validation failed. -> CreateRandomFood.cs ");
            Debug.Break();
        }
        else
        {
            meshFilter = spawnArea.GetComponent<MeshFilter>(); // Versucht den Meshfilter zu holen
            if (meshFilter == null)
            {
                Debug.LogError("Error: Start Validation failed. -> CreateRandomFood.cs : MeshFilter ");
                Debug.Break();
            }
            else
            {
                valid = true;
            }
        }
        return valid;
    }

    /// <summary>
    /// Errechnet die Chancen jedes Essens anhand der Gesamtzahl
    /// </summary>
    private void CalcChance()
    {
        chanceTotal = Cherry + Banana + Chocolate;

        ch_Cherry = (float)Cherry / (float)chanceTotal;
        ch_Banana = (float)Banana / (float)chanceTotal;
        ch_Chocolate = (float)Chocolate / (float)chanceTotal;

    }

    /// <summary>
    /// Prüft ob mindestens ein Apfel im spiel vorhanden ist, wenn nicht wird mindestens einer gespawnt
    /// </summary>
    private void CheckAppleCount()
    {
        int count = 0;
        // Durchlaufe alle food Objecte und zähle die Äpfel
        foreach (GameObject foodItem in GameObject.FindGameObjectsWithTag(GlobalNames.Tags.FoodObject.ToString()))
        {
            if (foodItem.GetComponent<Food>().GetFoodType() == Food.FoodType.apple)
            {
                count++;
            }
        }
        if (count <= 0) // Wenn die Äpfel aus sind dann spawne neue
        {
            SpawnFood(Food.FoodType.apple); // Spawnt die Äpfel
            SpawnOtherFood(); // Spawnt die anderen Essen
        }
    }

    /// <summary>
    /// Gibt wahr zurück, wenn der Random Wert die errechnete Chance trifft
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool CheckFoodSpawn(Food.FoodType type)
    {
        bool valid = false;
        float rand = Random.Range(0.0f, 1.0f); // erstellt eine Zufallszahl
        double randRound = System.Math.Round(rand, 1); // Rundet das Float auf ein Nachkommerstelle
        
        switch (type)
        {
            case Food.FoodType.cherry:
                if (randRound <= ch_Cherry)
                {
                    valid = true;
                }
                break;
            case Food.FoodType.banana:
                if (randRound <= ch_Banana)
                {
                    valid = true;
                }
                break;
            case Food.FoodType.chocolate:
                if (randRound <= ch_Chocolate)
                {
                    valid = true;
                }
                break;
            default:
                Debug.LogError("Error: Default case reached. -> CreateRandomFood.cs : CalcChance ");
                Debug.Break();
                break;
        }

        return valid;
    }

    /// <summary>
    /// Führt den Spawn der Anderen Essen durch, wenn es die chance besagt
    /// </summary>
    private void SpawnOtherFood()
    {
        Food.FoodType[] all = { Food.FoodType.banana, Food.FoodType.cherry, Food.FoodType.chocolate };

        foreach (Food.FoodType type in all) // Geht alls anderen Essen durch
        {
            if (CheckFoodSpawn(type)) // prüft anhand des Randoms und der Warscheinlichkeit ob der Typ spawnen darf
            {
                SpawnFood(type);
            }
        }
    }

    /// <summary>
    /// Spawnt mindestens ein Apfel zufällig auf der SpawnArea
    /// </summary>
    private void SpawnFood(Food.FoodType type)
    {
        int spawnCount = 0;

        // Beim Apfel sollen 1 bis 3 Objecte spwnen können. Alle anderen nur eins.
        if (type == Food.FoodType.apple)
        {
            spawnCount = Random.Range(1, 3);
        }
        else
        {
            spawnCount = 1;
        }

        // Spawnt die Anzahl der Objecte wenn es sein soll und bennent sie demendtsprechend
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject foodOutput = Instantiate(foodPrefab);        // Object errstellen
            foodOutput.tag = GlobalNames.Tags.FoodObject.ToString();// Tag setzten
            foodOutput.transform.position = GetRandomPos();         // Position setzten
            foodOutput.GetComponent<Food>().SetFoodType(type);      // Den Essens Typ setzten
            foodOutput.name = "food_" + type.ToString(); ;          // Für den Debug
        }
    }

    /// <summary>
    /// Gibt eine Zufällige Position berechnet anhand des Mashs und der Transform vom spawnArea Object zurück
    /// </summary>
    /// <returns>Zufallsposition</returns>
    private Vector3 GetRandomPos()
    {
        Vector3 pos = Vector3.zero;
        int maxX;
        int maxZ;

        Mesh mesh = meshFilter.mesh;                    // Den mesh besorgen
        Vector3 size = mesh.bounds.size;                // Die Größen beschaffen 
        Vector3 scale = spawnArea.transform.localScale;
        maxX = ((int)(size.x * scale.x) / 2) - 3;             // Die grenzen errechnen
        maxZ = ((int)(size.z * scale.z) / 2) - 3;

        // erstellt eine zufällige position für X und Z. Pos Y ist vom Prefab
        pos = new Vector3(Random.Range(spawnArea.transform.position.x - maxX, spawnArea.transform.position.x + maxX),
                                       foodPrefab.transform.position.y,
                          Random.Range(spawnArea.transform.position.z - maxZ, spawnArea.transform.position.z + maxZ));
        return pos;
    }
}
