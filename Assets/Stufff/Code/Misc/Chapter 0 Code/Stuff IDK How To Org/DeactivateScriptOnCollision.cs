using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateScriptOnCollision : MonoBehaviour
{
    public bool MoveTowardScript;
    
    public void Deactivate()
    {
        if (MoveTowardScript)
        {
            MoveTowardsPosition moveTowardsPosition = GetComponent<MoveTowardsPosition>();
            moveTowardsPosition.enabled = false;
        }
    }
}
