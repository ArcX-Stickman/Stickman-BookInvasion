using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick_TriggerEvent : MonoBehaviour
{
    public bool Trigger;
    public bool Trigger2;
    public Movement Stick;
    public void AnimTrigger()
    {
       Trigger = true;
    }
    public void AnimTrigger2()
    {
        Trigger2 = true;
    }
    private void Update()
    {
        if(Stick == null)
        {
            Stick = FindAnyObjectByType<Movement>();
            
        }
    }

    public void StickCutsceneCancel()
    {
        Stick.Anim_Locked = false;
        Stick.StickAnim.SetTrigger("CinematicDone");
    }

}
