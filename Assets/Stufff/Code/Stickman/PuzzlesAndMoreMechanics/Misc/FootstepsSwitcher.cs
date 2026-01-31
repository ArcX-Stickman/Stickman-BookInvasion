using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSwitcher : MonoBehaviour
{
    public AudioClip Footsteps;
    public AudioSource FootstepsSource;
    public AudioClip GroundImpact;
    public Movement StickMan;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FootstepsSource.clip = Footsteps;
            if(GroundImpact != null)
            {
                StickMan.LandSFX = GroundImpact;
            }
        }
    }
}
