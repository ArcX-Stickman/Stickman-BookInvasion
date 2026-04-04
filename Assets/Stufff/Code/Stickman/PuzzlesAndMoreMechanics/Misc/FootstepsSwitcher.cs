using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSwitcher : MonoBehaviour
{
    public AudioClip Footsteps;
    public AudioSource FootstepsSource;
    public AudioClip GroundImpact;
    public Movement StickMan;
    private AudioClip CurrentSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CurrentSource = FootstepsSource.clip;
            FootstepsSource.clip = Footsteps;
            Switched(Footsteps);

            if (GroundImpact != null)
            {
                StickMan.LandSFX = GroundImpact;
            }
        }
    }


    public void Switched(AudioClip switchroo)
    {
        if(switchroo ==  CurrentSource)
        {
            return;
        }

        FootstepsSource.enabled = false;
        FootstepsSource.enabled = true;

    }
}
