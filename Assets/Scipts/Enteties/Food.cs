using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient zur Steuerung der Nahrung und führt die Effecte aus.
/// </summary>
public class Food : MonoBehaviour {

    // Die Typen an Essen
    public enum FoodType
    {
        none,
        apple,
        cherry,
        banana,
        chocolate
    }

    private FoodType foodType;              // Für die Auswahl des Typs im inspector
    private float changeMoveTackt = 0.04f;  // Für die Veränderung der Geschwindigkeit
    private GameObject snakeBodyPrefab;     // Für das Prefab der Körper

    private Vector3 spawnPosition;          // Die Position für den Spawn

    private Material[] materials;           // Die Materials die für das Essen eingesetzt werden
    private Hud hud;                        // Für die Updates des Huds

    private void Start()
    {
        if (StartValidation())
        {
            // Änderung des Materials auslösen
            foreach (Material item in materials)
            {
                if (foodType.ToString() == item.name)
                {
                    transform.GetComponent<MeshRenderer>().material = item;
                }
            }
        }
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;

        snakeBodyPrefab = Resources.Load<GameObject>("Prefabs/SnakeBody");
        materials = Resources.LoadAll<Material>("Materials/Food/");
        hud = GameObject.Find("ScriptConteiner").GetComponent<Hud>(); // Beschaffe das Hud

        if (snakeBodyPrefab == null || materials == null || hud == null)
        {
            Debug.LogError("Error: Start Validation failed. -> Food.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }

    /// <summary>
    /// Vergleiche den Tag mit SnakeHead, wenn wahr dann Führe DoTypeEffect aus und zerstöre das GameObject
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == GlobalNames.Tags.SnakeHead.ToString())
        {
            GlobalNames.playSound = true;
            GlobalNames.soundType = GlobalNames.Sounds.Eat;
            DoTypeEffect(other);
            Destroy(gameObject);
        }

        // Löscht die Blöcke die in ein ander Stehen, da es sonst zu bugs führen kann.
        if (other.tag == GlobalNames.Tags.FoodObject.ToString())
        {
            Destroy(other.gameObject);
        }
        if (other.tag == GlobalNames.Tags.SnakeBody.ToString() || other.tag == GlobalNames.Tags.Wall.ToString())
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary>
    /// Führt den Typisierten effect aus
    /// </summary>
    private void DoTypeEffect(Collider target)
    {
        switch (foodType)
        {
            case FoodType.apple: // Apfel zum Wachsen der Schlange
                DoAppleEffect(target);
                break;
            case FoodType.cherry: // Kirsche zum Verlangsamen der Bewegungsgeschwindigkeit für kurze Zeit
                DoCherryEffect(target);
                break;
            case FoodType.banana: // Banane zum Erhöhen der Bewegungsgeschwindigkeit für kurze Zeit
                DoBananaEffect(target);
                break;
            case FoodType.chocolate: // Schokolade zum Schrumpfen der Schlange
                DoChocolateEffect(target);
                break;
            default:
                Debug.LogError("Error: Default case reached. -> Food.cs ");
                Debug.Break();
                break;
        }
    }

    /// <summary>
    /// Intaniate ein neuen Schlangenbody
    /// </summary>
    /// <param name="target"></param>
    private void DoAppleEffect(Collider target)
    {

        spawnPosition = new Vector3(); // erstelle die Variable
        FindSpawnPosition(); // Finde die Coordinaten

        GameObject output = Instantiate(snakeBodyPrefab);   // erstellen
        output.tag = GlobalNames.Tags.SnakeBody.ToString(); // Taggen
        output.name = GlobalNames.Names.BodyPart_.ToString() + Level.snakeBodyCount.ToString(); // Setzte den Variablen namen des Objects zum Finden
        
        Level.snakeBodyCount++;                             // Erhöhe die Anzahl des Boddys
        output.transform.position = spawnPosition;          // Positionieren

        hud.DoUpdateBodyCount();                            // Updatet den BodyCount
    }

    /// <summary>
    /// Verlangsamt die Geschwindigkeit
    /// </summary>
    /// <param name="target"></param>
    private void DoCherryEffect(Collider target)
    {
        if (Level.moveTackt < 3f)
        {
            Level.moveTackt = Level.moveTackt + changeMoveTackt;
            Level.moveTacktLable = Level.moveTacktLable - (changeMoveTackt * 10);
            hud.DoUpdateSpeedLable(); // Updatet die Geschwindigkeit
        }
    }

    /// <summary>
    /// Verschnellert die Geschwindigkeit
    /// </summary>
    /// <param name="target"></param>
    private void DoBananaEffect(Collider target)
    {
        if (Level.moveTackt > 0.059f)
        {
            Level.moveTackt = Level.moveTackt - changeMoveTackt;
            Level.moveTacktLable = Level.moveTacktLable + (changeMoveTackt * 10);
            hud.DoUpdateSpeedLable(); // Updatet die Geschwindigkeit
        }
    }

    /// <summary>
    /// Schrumpft die Schlange, Löscht das letzte Element der Schlange
    /// </summary>
    /// <param name="target"></param>
    private void DoChocolateEffect(Collider target)
    {
        string temp = GlobalNames.Names.BodyPart_.ToString() + (Level.snakeBodyCount - 1).ToString();
        GameObject[] snakeBodys = GameObject.FindGameObjectsWithTag(GlobalNames.Tags.SnakeBody.ToString());
        foreach (GameObject bodyItem in snakeBodys)
        {
            if (bodyItem.name == temp)
            {
                Destroy(bodyItem);
                Level.snakeBodyCount--;
                hud.DoUpdateBodyCount();    // Updatet den BodyCount
            }
        }
    }

    /// <summary>
    /// Sucht nach der richtigen Spawn Position
    /// </summary>
    private void FindSpawnPosition()
    {
        if (Level.snakeBodyCount == 0)
        {
            // Suche den TailAnchor und nehme die Position dessen.
            spawnPosition = GameObject.FindGameObjectWithTag(GlobalNames.Tags.TailAnchor.ToString()).transform.position;
        }
        else
        {
            string temp = GlobalNames.Names.BodyPart_.ToString() + (Level.snakeBodyCount -1).ToString(); // Setzte den String für den Namen zusammen
            GameObject tempObject = GameObject.Find(temp); // Suche nach den Object

            for (int i = 0; i < tempObject.transform.childCount; i++) // Gehe durch die ChildObjecte
            {
                if (tempObject.transform.GetChild(i).name == GlobalNames.Names.TailAnchor.ToString()) // Suche das Child mit dem Namen TailAnchor
                {
                    spawnPosition = tempObject.transform.GetChild(i).position; // Setzte die Position
                }
            }
        }
        if (spawnPosition == Vector3.zero)
        {
            Debug.LogError("Error: Spawnposition null in FindSpawnPosition -> Food.cs");
            Debug.Break();
        }
        
    }

    /// <summary>
    /// Setzt den FoodType
    /// </summary>
    /// <param name="type"></param>
    public void SetFoodType(FoodType type)
    {
        foodType = type;
    }

    /// <summary>
    /// Holt den FoodType
    /// </summary>
    /// <param name="type"></param>
    public FoodType GetFoodType()
    {
        return foodType;
    }

}
