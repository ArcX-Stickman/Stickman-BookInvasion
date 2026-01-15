using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayASound_AnimationKey : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip Clip;
    public AudioClip SecondClip;

    public float MinRandomPitch;
    public float MaxRandomPitch;

    public AudioSource PitchAudioP;
    public void PLayCLip()
    {
        AudioP.PlayOneShot(Clip);
    }
   
    public void PlaySecondClip()
    {
        AudioP.PlayOneShot(SecondClip);
    }

    public void RandomPitchSFX1()
    {
        PitchAudioP.pitch = UnityEngine.Random.Range(MinRandomPitch,MaxRandomPitch);
        PitchAudioP.PlayOneShot(Clip);

    }

    public void RandomPitchSFX2()
    {
        PitchAudioP.pitch = UnityEngine.Random.Range(MinRandomPitch,MaxRandomPitch);
        PitchAudioP.PlayOneShot(SecondClip);


    }
}
