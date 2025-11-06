using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Music : MonoBehaviour
{
    public Movement PlayerManager;
    public AudioSource AudioP;
    //Clips
    public AudioClip IntroTheme;
    public AudioClip Hypothesis;
    public AudioClip HypothesizingBeforeMayhem;
    void Update()
    {
        
        ManageMusic();
    }
    public void ManageMusic()
    {
        if(AudioP != null)
        {
            if (PlayerManager.Checkpoint <= 0)
            {
                if (AudioP.clip != IntroTheme)
                {
                    AudioP.clip = IntroTheme;
                    AudioP.Play();
                }
            }
            else if(PlayerManager.Checkpoint == 726438 && !PlayerManager.BreakIn)
            {
                AudioP.clip = HypothesizingBeforeMayhem;
                if (!AudioP.isPlaying && Time.timeScale > 0)
                {
                    AudioP.Play();

                }
            }
            else
            {
                if (AudioP.clip != Hypothesis)
                {
                    AudioP.clip = Hypothesis;
                    AudioP.Play();
                }
            }
        }
        
    }

}
