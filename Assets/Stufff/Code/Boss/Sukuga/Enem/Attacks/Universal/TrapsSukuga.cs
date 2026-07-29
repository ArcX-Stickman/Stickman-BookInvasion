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
    public bool  NotAvoidBackflip;
    public GameObject FX;
    public bool Destroyy;
    public bool MustMatchPlayerZ;
    public bool GapAndDestroy;

    public bool DestoryOnGround;

    private bool SamePosAsPlayer()
    {
        Vector3 Trans = Stick.transform.position;
        Vector3 Pos = transform.position;
        return Trans == Pos;
    }

    private void Start()
    {
        Stick = FindAnyObjectByType<StickCinematicVer_BossFight>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (DMGed) return;
        if (Stick.InBackflip && !NotAvoidBackflip) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SamePosAsPlayer() && MustMatchPlayerZ)
            {
                DMG_STUFF();

            }
            else if (!MustMatchPlayerZ)
            {
                DMG_STUFF();
            }
        }

        if (collision.gameObject.CompareTag("Ground") && DestoryOnGround)
        {
            NONDMG_STUFF();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (DMGed) return;
        if (Stick.InBackflip && !NotAvoidBackflip) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (SamePosAsPlayer() && MustMatchPlayerZ)
            {
                DMG_STUFF();

            }
            else if (!MustMatchPlayerZ)
            {
                DMG_STUFF();
            }
        }

        if (collision.gameObject.CompareTag("Ground") && DestoryOnGround)
        {
            NONDMG_STUFF();
        }
    }


    public void DMG_STUFF()
    {
        DMGed = true;
        Stick.Health -= DMG;
        AudioP.PlayOneShot(SFX);
        if(FX != null)
        { 
            GameObject FX_ = Instantiate(FX,Stick.transform.position,Quaternion.identity);
        }

        if (Destroyy && !GapAndDestroy)
        {
            Destroy(gameObject);
        }
        else if (GapAndDestroy)
        {
            StartCoroutine(GapHoldDes());
        }

    }

    public void NONDMG_STUFF()
    {
        DMGed = true;
        AudioP.PlayOneShot(SFX);
        if (FX != null)
        {
            Vector3 Fake = new Vector3(transform.position.x,transform.position.y,Stick.transform.position.z);
            GameObject FX_ = Instantiate(FX, Fake, Quaternion.identity);
        }

        if (Destroyy && !GapAndDestroy)
        {
            Destroy(gameObject);
        }
        else if (GapAndDestroy)
        {
            StartCoroutine(GapHoldDes());
        }

    }

    public float GapWait;
    public IEnumerator GapHoldDes()
    {
        MoveToPositionV2 MP2 = GetComponent<MoveToPositionV2>();
        if(MP2 != null)
        {
            MP2.enabled = false;
        }
        yield return new WaitForSecondsRealtime(GapWait);
        Destroy(gameObject);
        yield break;
    }
}
