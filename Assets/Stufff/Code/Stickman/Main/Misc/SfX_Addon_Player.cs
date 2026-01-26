using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfX_Addon_Player : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioSource AudioP_PitchModication;
    public AudioClip[] PunchWhoosh;
    public AudioClip KickWhoosh;


    public void PunchWhooshSFX_Play()
    {
        AudioP_PitchModication.pitch = UnityEngine.Random.Range(0.8f, 1f);
        AudioClip rndSound = PunchWhoosh[UnityEngine.Random.Range(0, PunchWhoosh.Length)];

        AudioP_PitchModication.PlayOneShot(rndSound);
    }

    public void KickWhooshSFX_Play()
    {
        AudioP_PitchModication.pitch = UnityEngine.Random.Range(0.6f, 1.3f);
        AudioP_PitchModication.PlayOneShot(KickWhoosh);
    }
}
