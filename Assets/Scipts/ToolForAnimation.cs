using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolForAnimation : MonoBehaviour {

    // Felder
    public Transform Mouth;
    public Transform UpperMouth;

    public float MouthXAxis;
    public float UpperMouthXAxis;

    
    private float timer = 0.0f;

    
    private void Start () {
        if (UpperMouth == null || Mouth == null )
        {
            Debug.LogError("Error: Start Validation failed. -> ToolForAnimation.cs ");
            Debug.Break();
        }
        MouthXAxis = Mouth.rotation.eulerAngles.x;
        UpperMouthXAxis = UpperMouth.eulerAngles.x;
    }

    
    private void Update () {
        
        timer = timer + Time.deltaTime; // Timer Hochzählen
        if (timer > 0.1) // Zeit erreicht?
        {
            Change();
            timer = 0; // Timer zurücksetzten
        }
    }

    private void Change()
    {
        
        Mouth.rotation = Quaternion.Euler(MouthXAxis, 0, 0);
        UpperMouth.rotation = Quaternion.Euler(UpperMouthXAxis, 0, 0);
    }

}
