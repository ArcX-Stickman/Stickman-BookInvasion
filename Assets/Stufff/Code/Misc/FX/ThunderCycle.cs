using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCycle : MonoBehaviour
{
    public GameObject Flash;
    public float intervals;
    public float LastStrike;
    public AudioSource AudioP;
    public AudioClip Strike;

    void Update()
    {
        if(Time.time >= intervals + LastStrike)
        {
            LastStrike = Time.time;
            Flash.gameObject.SetActive(true);
            AudioP.PlayOneShot(Strike);
        }
    }
}
