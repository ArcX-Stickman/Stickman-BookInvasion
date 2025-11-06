using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ThreeD_Sound : MonoBehaviour
{
    public float Distance;
    public float MaxDis;
    public float MinDis;
    public AudioSource AudioP;
    public Transform StickPOS;
    public Light2D Light;
    public Animator Animator;
    public bool WantToCull_Animator;
    public ScientistSpecialEmergency ScientistSpecialEmergency;
    void Update()
    {
        Animator = GetComponent<Animator>();
       
        Distance = Vector2.Distance(transform.position,StickPOS.position);
        if(AudioP == null)
        {
            AudioP = GetComponent<AudioSource>();
        }
        if(Light == null)
        {
            Light = GetComponent<Light2D>();
        }
        if(ScientistSpecialEmergency != null)
        {
            if (ScientistSpecialEmergency.Pushed)
            {
                if (Distance <= MinDis)
                {
                    if (Animator != null)
                    {
                        Animator.enabled = true;
                    }
                    AudioP.enabled = true;
                    if(Light  != null)
                    {
                        Light.enabled = true;

                    }
                }
                else if (Distance >= MaxDis)
                {
                    if (Animator != null)
                    {
                        Animator.enabled = false;
                    }
                    AudioP.enabled = false;
                    if(Light != null)
                    {
                        Light.enabled = false;

                    }
                }
            }
            else
            {
                if (Animator != null)
                {
                    Animator.enabled = false;
                }
                AudioP.enabled = false;
                if(Light != null)
                {
                    Light.enabled = false;

                }
            }
        }
        else
        {
            if (Distance <= MinDis )
            {
                if (Animator != null && WantToCull_Animator)
                {
                    Animator.enabled = true;
                }
                AudioP.enabled = true;
                if(Light != null)
                {
                    Light.enabled = true;

                }
            }
            else if (Distance >= MaxDis)
            {
                if (Animator != null && WantToCull_Animator)
                {
                    Animator.enabled = false;
                }
                AudioP.enabled = false;
                if(Light != null)
                {
                    Light.enabled = false;

                }
            }
        }
       
       
    }
}
