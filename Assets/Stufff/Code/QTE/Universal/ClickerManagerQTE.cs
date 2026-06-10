using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickerManagerQTE : MonoBehaviour
{
    public AudioClip[] SFX;



    public AudioClip ClickSFX()
    {
        AudioClip rnd = SFX[UnityEngine.Random.Range(0, SFX.Length)];
        return rnd;

    }
}
