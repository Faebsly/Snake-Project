using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dient zur Steuerung der Animation des Kopfes
/// </summary>
public class Animation : MonoBehaviour {

    // Felder
    public Animator animator;       // Der zu steuernde Animator

    private RaycastHit raycastHit;  // Für die Suche nach den Essen, zur Steuerung der Animation
    private float range = 4.1f;

    private void Start()
    {
        if (StartValidation())
        {
            range = 4f * Level.moveTackt;
        }
    }

    /// <summary>
    /// Fragt den Raycast ab, wenn er trifft spielt der die Animation ab
    /// </summary>
    private void Update()
    {
        animator.SetBool("PlayEatAnim", CheckFoodRaycast()); // Triggert das abspielen der Animation
    }

    /// <summary>
    /// Prüft anhand der Range ob es ein Hit mit einem FoodObject gibt, Wenn ja gebe wahr zurück
    /// </summary>
    /// <returns></returns>
    private bool CheckFoodRaycast()
    {
        bool valid = false;

        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, range))
        {
            if (raycastHit.collider.gameObject.tag == GlobalNames.Tags.FoodObject.ToString())
            {
                valid = true;
            }
        }
        return valid;
    }

    /// <summary>
    /// Prüft die zum Start benötigten Variablen
    /// </summary>
    /// <returns></returns>
    private bool StartValidation()
    {
        bool valid = false;
        if (animator == null)
        {
            Debug.LogError("Error: Start Validation failed. -> Animation.cs ");
            Debug.Break();
        }
        else
        {
            valid = true;
        }
        return valid;
    }



}
