using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_MUSIC_SWAP : MonoBehaviour
{
    public Movement CheckpointChecker;
    public AudioSource AudioP;
    public AudioClip NightAmbience;
    public AudioClip Hypothesis;


    // Update is called once per frame
    void Update()
    {
        if(CheckpointChecker.Checkpoint < -1)
        {
            AudioP.clip = NightAmbience;
        }
        else if(CheckpointChecker.Checkpoint > -1)
        {
            AudioP.clip = Hypothesis;

        }

    }
}
