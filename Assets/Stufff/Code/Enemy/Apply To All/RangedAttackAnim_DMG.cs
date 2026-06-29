using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackAnim_DMG : MonoBehaviour
{
    public float DMG;

    public StickCinematicVer_BossFight StickSuk;
    public Movement StickNorm;

    public AudioSource AudioP;
    public AudioClip SFX;

    // Start is called before the first frame update
    void Start()
    {
        if(StickSuk != null && StickNorm != null)
        {
            Debug.LogError(" Yo Bruh Logic Failed we turning ts off asap  - you got both sukuga format and normal format ion know what to choose");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(StickNorm != null)
        {
            NormManager();
        }

        if (StickSuk != null)
        {
            SukugaManager();
        }
    }


    private void NormManager()
    {
        return ; // i got nothing here yet
    }

    private bool AlrDMG;
    private void SukugaManager()
    {

        if (Touch && StickSuk.InBackflip && !AlrDMG)
        {
            AlrDMG = true;
        } 
        else if (Touch && !StickSuk.InBackflip && !AlrDMG)
        {
            AlrDMG = true;
            StickSuk.Health -= DMG;
            AudioP.PlayOneShot(SFX);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Touch = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Touch = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Touch = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Touch = false;
        }


    }

    private bool Touch;
}
