using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  wehy the abosolute hell did i make this abomination of a script
/// </summary>
public class StickMoveGlitch : MonoBehaviour
{
    public Movement movementScript; 

    void Start()
    {
       if (movementScript == null)
        {
            movementScript = GetComponent<Movement>();
        }
    }

    void Update()
    {
        this.enabled = false;
      //if (movementScript != null && !movementScript.enabled)
      //  {
      //      movementScript.enabled = true;
      //  }
    }
}
