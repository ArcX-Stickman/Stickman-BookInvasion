using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistLazerPush : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip LazerUp;
    public LazerPole LazerManager;
    public Movement Stickman;
    public void LazersGoingUp()
    {
        //Stickman.TimeManager.SloMO();
        AudioP.PlayOneShot(LazerUp);
        LazerManager.LazersUp = true;
    }
}
