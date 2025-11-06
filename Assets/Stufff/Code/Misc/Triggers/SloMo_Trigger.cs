using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloMo_Trigger : MonoBehaviour
{
    public Movement Stickman;
    private bool Alr_Did;
    public AudioSource AudioP;
    public AudioClip SFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !Alr_Did)
        {
            Alr_Did = true;
            Stickman.TimeManager.SloMO();
        }
    }
}
