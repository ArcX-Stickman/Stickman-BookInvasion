using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicShifter : MonoBehaviour
{
    public AudioSource AudioWantToChange;
    public AudioClip ClipWantToPlay;
    public bool Stealth;
    public AudioSource FootSteps;
    public Movement Stickman;
    public bool WantCheckPointCode;
    public int Code;

    private void Start()
    {
        if(Stickman.Checkpoint == Code && WantCheckPointCode)
        {
            AudioWantToChange.clip = ClipWantToPlay;
            AudioWantToChange.Play();
            if (Stealth)
            {
                FootSteps.volume = 0.3f;
            }
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Stickman.Checkpoint == Code && WantCheckPointCode)
        {
            AudioWantToChange.clip = ClipWantToPlay;
            AudioWantToChange.Play();
            if (Stealth)
            {
                FootSteps.volume = 0.3f;
            }
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioWantToChange.clip = ClipWantToPlay;
            AudioWantToChange.Play();
            if (Stealth)
            {
                FootSteps.volume = 0.3f;
            }
            Destroy(gameObject);
        }
    }
}
