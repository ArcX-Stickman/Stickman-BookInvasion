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

    public bool TurnOffMoveScript;

    public string animName;

    public bool isAnimBool;

    public string AnimBool;

    public bool DisableRB;

    public void Update()
    {
        if (Triggered && !Reached)
        {
            StickMove();
        }

    }
    public void StickMove()
    {
        if (ForcePlayerLookDirection)
        {
            StickPOS.transform.localScale = new Vector3(Direc, StickPOS.transform.localScale.y, StickPOS.transform.localScale.z);
        }
        if (DisableRB)
        {
            StickScript.rb.simulated = false;
        }
        Distance = Vector2.Distance(TargetPOS.position, StickPOS.position);
        if (Distance > MINDIS)
        {
            if(animName != "")
            {
                StickScript.StickAnim.CrossFade(animName, 0, 0);

                //StickScript.StickAnim.enabled = false;
            }
            if (isAnimBool)
            {
                if(AnimBool == "")
                {
                    StickScript.StickAnim.SetBool(animName, true);
                }
                else
                {
                    StickScript.StickAnim.SetBool(AnimBool, true);

                }
            }
            if (TurnOffMoveScript)
            {
                StickScript.enabled = false;
            }
            StickPOS.position = Vector2.MoveTowards(StickPOS.position, TargetPOS.position, Speed * Time.deltaTime);
        }
        if(Distance < MINDIS)
        {
            StickScript.rb.simulated = true;
            if (TurnOffMoveScript)
            {
                StickScript.enabled = true;

            }
            StickScript.StickAnim.enabled = true;
            Reached = true;
            if (AnimBool == "")
            {
                StickScript.StickAnim.SetBool(animName, false);
            }
            else
            {
                StickScript.StickAnim.SetBool(AnimBool, false);

            }
        }
    }

    /// Make the OnTrigger Enter 2d Function so It can trigger the function
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Triggered = true;

        }
    }

}
