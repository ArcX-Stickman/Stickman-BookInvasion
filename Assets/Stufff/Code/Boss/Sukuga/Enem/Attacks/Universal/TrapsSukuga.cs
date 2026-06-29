using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsSukuga : MonoBehaviour
{
    public StickCinematicVer_BossFight Stick;
    public float DMG;
    public AudioSource AudioP;
    public AudioClip SFX;
    public bool DMGed;


    private void Start()
    {
        Stick = FindAnyObjectByType<StickCinematicVer_BossFight>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DMGed) return;
        if (Stick.InBackflip) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            DMG_STUFF();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DMGed) return;
        if (Stick.InBackflip) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            DMG_STUFF();
        }
    }


    private void DMG_STUFF()
    {
        DMGed = true;
        Stick.Health -= DMG;
        AudioP.PlayOneShot(SFX);
    }
}
