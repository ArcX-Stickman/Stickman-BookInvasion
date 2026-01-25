using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger_After_Seconds : MonoBehaviour
{
    public string AnimTrigger;
    public float Interval;
    public float LastTrigger;
    public Animator Anim;

    void Update()
    {
        if(Anim == null)
        {
            Anim = GetComponent<Animator>();
        }
        if(Time.time >= Interval + LastTrigger)
        {
            LastTrigger = Time.time;
            Anim.SetTrigger(AnimTrigger);

        }
    }
}
