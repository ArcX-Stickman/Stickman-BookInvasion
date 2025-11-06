using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickMoveGlitch : MonoBehaviour
{
    public Movement movementScript; // Reference to the movement script

    void Start()
    {
        // Check if the movement script is assigned
        if (movementScript == null)
        {
            movementScript = GetComponent<Movement>(); // Automatically find the script on the same GameObject
        }
    }

    void Update()
    {
        // Always ensure the movement script is enabled
        if (movementScript != null && !movementScript.enabled)
        {
            movementScript.enabled = true;
        }
    }
}
