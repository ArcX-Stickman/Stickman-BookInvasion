using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBackground : MonoBehaviour
{
    public float DistanceFromPlayer;
    public float MinDisForEvent;
    public AudioSource AudioP;
    public AudioClip SFX;
    public AudioClip SecondSFX;
    public AudioClip Voice;
    public Movement Stickman;
    public Animator Anim;
    public bool isAcid;
    [Header("Acid Boi")]
    public Animator Scien1;
    public Animator Scien2;
    public bool WantParticles;
    public GameObject Particles;
    private bool PlayedScream;
    public AudioClip ScreamSound;
    void Update()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position, Stickman.transform.position);
        if(DistanceFromPlayer <= MinDisForEvent)
        {
            AnimEvent();
        }

    }

    public void AnimEvent()
    {
        Anim = GetComponent<Animator>();
        Anim.SetTrigger("1");
        if (isAcid)
        {
            Scien1.SetTrigger("1");
            Scien2.SetTrigger("1");

        }

    }
    public void ScreamSFX()
    {
        if (!PlayedScream)
        {
            AudioP.PlayOneShot(ScreamSound);
            PlayedScream = true;
        }
    }
    public void VoicePlay()
    {
        AudioP.PlayOneShot(Voice);
    }

    public void SFXPlay()
    {
        AudioP.PlayOneShot(SFX);
    }

    public void Play2ndSFX()
    {
        AudioP.PlayOneShot(SecondSFX);
    }

    public void ParticlesFX()
    {
        GameObject fx = Instantiate(Particles,transform.position,Quaternion.identity);
    }
}
