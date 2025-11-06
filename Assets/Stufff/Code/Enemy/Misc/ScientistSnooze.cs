using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistSnooze : MonoBehaviour
{
    public BattleManager BattleManager;
    public AudioSource AudioP;
    public AudioClip AudioClip;
    public HitMarker HM;
    public Animator anim;
    public AudioClip Scream;

    public void Alert()
    {
        BattleManager.AlertAllEnemies();
    }

    public void SoundMeme()
    {
        AudioP.PlayOneShot(AudioClip);
       
    }

    public void SCREAMMMM()
    {
        AudioP.PlayOneShot(Scream);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) 
        {
            HM.HitMarkerSFX();
            SoundMeme();
            anim.SetTrigger("Fall");
        
        }
    }

    public void BYEEE()
    {
        Destroy(gameObject);
    }
}
