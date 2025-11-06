using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTouchActivateAudio : MonoBehaviour
{

    public AudioSource AudioP;
    private bool StartedLoop;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Ground") && !StartedLoop)
        {
            AudioP.Play();
            StartedLoop = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !StartedLoop && !AudioP.isPlaying)
        {
            AudioP.Play();
            StartedLoop = true;
        }
    }


}
