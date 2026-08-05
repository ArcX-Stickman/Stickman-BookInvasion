using System.Collections;
using UnityEngine;

public class TrapsSukuga : MonoBehaviour
{
    public StickCinematicVer_BossFight Stick;
    public float DMG;
    public AudioSource AudioP;
    public AudioClip SFX;
    public bool DMGed;
    public bool NotAvoidBackflip;
    public GameObject FX;
    public bool Destroyy;
    public bool MustMatchPlayerZ;
    public bool GapAndDestroy;

    public bool DestoryOnGround;

    private void EnsureStick()
    {
        if (Stick == null)
        {
            Stick = FindAnyObjectByType<StickCinematicVer_BossFight>();
        }
    }

    private bool SamePosAsPlayer()
    {
        EnsureStick();

        if (Stick == null) return false;

        // Only compare fake depth now.
        // Physics already checks X/Y.
        float zDifference = Mathf.Abs(
            Stick.transform.position.z - transform.position.z
        );

        return zDifference < 0.5f;
    }

    private void Start()
    {
        EnsureStick();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnsureStick();

        if (DMGed) return;
        if (Stick == null) return;
        if (Stick.InBackflip && !NotAvoidBackflip) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (MustMatchPlayerZ)
            {
                if (SamePosAsPlayer())
                    DMG_STUFF();
            }
            else
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
        EnsureStick();

        if (DMGed) return;
        if (Stick == null) return;
        if (Stick.InBackflip && !NotAvoidBackflip) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("collided with player");

            if (MustMatchPlayerZ)
            {
                if (SamePosAsPlayer())
                    DMG_STUFF();
            }
            else
            {
                DMG_STUFF();
            }
        }

        if (collision.gameObject.CompareTag("Ground") && DestoryOnGround)
        {
            NONDMG_STUFF();
        }
    }


    public bool CanDMG_STUFF()
    {
        EnsureStick();

        if (DMGed) return false;
        if (Stick == null) return false;
        if (Stick.InBackflip && !NotAvoidBackflip) return false;

        return true;
    }

    public bool TryDMG_STUFF()
    {
        if (!CanDMG_STUFF()) return false;

        DMG_STUFF();
        return true;
    }

    public bool TryNONDMG_STUFF()
    {
        if (DMGed) return false;

        NONDMG_STUFF();
        return true;
    }

    public void DMG_STUFF()
    {
        EnsureStick();

        if (DMGed) return;
        if (Stick == null) return;

        DMGed = true;

        Stick.Health -= DMG;

        if (AudioP != null && SFX != null)
        {
            AudioP.PlayOneShot(SFX);
        }

        if (FX != null)
        {
            Instantiate(FX, Stick.transform.position, Quaternion.identity);
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
        EnsureStick();

        if (DMGed) return;

        DMGed = true;

        if (AudioP != null && SFX != null)
        {
            AudioP.PlayOneShot(SFX);
        }

        if (FX != null)
        {
            Vector3 Fake = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );

            if (Stick != null)
            {
                Fake.z = Stick.transform.position.z;
            }

            Instantiate(FX, Fake, Quaternion.identity);
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

        if (MP2 != null)
        {
            MP2.enabled = false;
        }

        yield return new WaitForSecondsRealtime(GapWait);

        Destroy(gameObject);
    }
}
