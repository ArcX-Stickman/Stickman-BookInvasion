using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateCinematicEnemyTrigger : MonoBehaviour
{
    public EnemyAI Enem;
   
    public void DestroyThisThing()
    {
        Destroy(gameObject);
    }
}
