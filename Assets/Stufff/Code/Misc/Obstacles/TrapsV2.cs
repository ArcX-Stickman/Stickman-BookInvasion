using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsV2 : MonoBehaviour
{
    public Health StickHP;
    public AudioSource AudioP;
    public AudioClip AudioClip;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickHP.PlayerHP = 0;
            AudioP.PlayOneShot(AudioClip);
        }
        this.enabled = false;
    }
}
