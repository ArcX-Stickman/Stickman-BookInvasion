using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToPosition_Mark : MonoBehaviour
{
    public Transform TargetPOS;
    public Transform StickPOS;
    public Movement StickScript;
    public float Speed;
    public float MINDIS;
    private float Distance;
    private bool Reached;

    private bool Triggered;

    public bool ForcePlayerLookDirection;

    public bool DisableMovementScript;

    public float Direc;

    public void Update()
    {
        if (Triggered && !Reached)
        {
            StickMove();
        }

    }
    public void StickMove()
    {
        while (Distance > MINDIS)
        {
            Vector2.MoveTowards(TargetPOS.position, StickPOS.position, Speed * Time.deltaTime);
        }
    }

    /// Make the OnTrigger Enter 2d Function so It can trigger the function
    
    
}
