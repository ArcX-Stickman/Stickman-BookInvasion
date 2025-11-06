using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASound_AnimationKey : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Clip;
    public AudioClip SecondClip;
    public void PLayCLip()
    {
        AudioP.PlayOneShot(Clip);
    }
   
    public void PlaySecondClip()
    {
        AudioP.PlayOneShot(SecondClip);
    }
}
