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
    public AudioSource LowHP_BG;
    public AudioClip HypothesizingBeforeMayhem;
    public AudioSource self;
    private PauseManager PM;
    void Update()
    {
        if (PM == null)
        {
            PM = FindAnyObjectByType<PauseManager>();
        }
        if (PlayerManager.BreakIn)
        {
            self.gameObject.SetActive(false);
            LowHP_BG.gameObject.SetActive(false);
        }
        ManageMusic();
    }
    public void ManageMusic()
    {
        if(AudioP != null)
        {
            if (PlayerManager.Checkpoint <= 0)
            {
                if(PlayerManager.StickHP.PlayerHP == 0 || Time.timeScale == 0 && !PM.NoHUD) 
                {
                    self.mute = true;
                    LowHP_BG.mute = true;
                }
                else
                {
                    if (PlayerManager.StickHP.PlayerHP >= 60 && !PlayerManager.BreakIn)
                    {
                        self.mute = false;
                        LowHP_BG.mute = true;
                        if (AudioP.clip != IntroTheme)
                        {
                            AudioP.clip = IntroTheme;
                            AudioP.Play();
                        }
                    }
                     
                    else
                    {
                        self.mute = true;
                        LowHP_BG.mute = false;
                    }
                }
               
               
            }
            else if(PlayerManager.Checkpoint == 726438 && !PlayerManager.BreakIn)
            {
                LowHP_BG.gameObject.SetActive(false);
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
