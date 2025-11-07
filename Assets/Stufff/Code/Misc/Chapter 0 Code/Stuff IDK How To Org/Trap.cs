using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{

    public Health PlayerHP;
    public AudioSource AudioP;
    public AudioClip WaterSplash;
    public bool isWater;
    public bool AlrDid;
    public bool DoesntOneShot;
    public float DMG;
    public bool WantToBeDestoryedOnTouch;
    public GameObject FX;
    public Animator StickAnim;
    public bool CanBeGone_W_Bullet;
    public AudioClip DestroySFX;
    public Movement PlayerManager;
    public bool isInEcology;
    public EnemyAI Ecology;
    public AudioClip Fall;
    public bool isVoid;
    [Header(" Chapter 0 Demo Stuff ")]
    private bool ALR_PLAYED_VOID;
    public bool EndingDemo1;
    public AudioSource Music;
    public GameObject FadeToWhite;

    public bool Teleportable;

    private float LastHit;
    private float HitCD = 2.2f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!EndingDemo1)
            {
                Debug.Log("VoidKill");
                if (Teleportable)
                {
                    TeleportPlayerToSafety();
                    PlayerHP.PlayerHP -= DMG;
                }
                if (isVoid && !ALR_PLAYED_VOID)
                {
                    PlayerManager.isVoid = true;
                    ALR_PLAYED_VOID = true;
                    PlayerManager.StickAnim.CrossFade("Dead", 0, 0);
                    // AudioP.PlayOneShot(Fall);
                }
                AlrDid = true;
                if (isWater && !AlrDid)
                {
                    AlrDid = true;
                    AudioP.PlayOneShot(WaterSplash);
                }
                if (!DoesntOneShot)
                {
                    PlayerHP.PlayerHP = 0;

                }
                else if(!Teleportable)
                {
                    if(Time.time >= LastHit + HitCD)
                    {
                        LastHit = Time.time;    
                        PlayerHP.PlayerHP -= DMG;

                    }
                    if (AudioP != null)
                    {
                        if (WantToBeDestoryedOnTouch && !PlayerManager.isDashing)
                        {
                            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
                            Destroy(gameObject);
                            AudioP.PlayOneShot(WaterSplash);
                            StickAnim.SetTrigger("Hurt");

                        }
                        if (PlayerManager.isDashing)
                        {
                            Destroy(gameObject);
                        }
                    }

                }
            }
            else
            {
                AudioP.PlayOneShot(WaterSplash);
                FadeToWhite.gameObject.SetActive(true);
                StartCoroutine(QuitTitle());
            }

        }

        if (collision.gameObject.CompareTag("Bullet") && CanBeGone_W_Bullet)
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            AudioP.PlayOneShot(DestroySFX);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Book") && !AlrDid && isInEcology && Ecology != null)
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            AudioP.PlayOneShot(WaterSplash);
            Ecology.Health -= 5;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("SunRay"))
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
 
   
    IEnumerator QuitTitle()
    {
        while(Music.volume > 0.01f)
        {
            Music.volume -= 2f * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2);
        LoadingSystem load = FindAnyObjectByType<LoadingSystem>();
        load.QuitToTitle();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!EndingDemo1)
            {
                Debug.Log("VoidKill");
                if (Teleportable)
                {
                    TeleportPlayerToSafety();
                    PlayerHP.PlayerHP -= DMG;
                }
                if (isVoid && !ALR_PLAYED_VOID)
                {
                    PlayerManager.isVoid = true;
                    ALR_PLAYED_VOID = true;
                    PlayerManager.StickAnim.CrossFade("Dead", 0, 0);
                    // AudioP.PlayOneShot(Fall);
                }
                AlrDid = true;
                if (isWater && !AlrDid)
                {
                    AlrDid = true;
                    AudioP.PlayOneShot(WaterSplash);
                }
                if (!DoesntOneShot)
                {
                    PlayerHP.PlayerHP = 0;

                }
                else if(!Teleportable)
                {
                    PlayerHP.PlayerHP -= DMG;
                    if (AudioP != null)
                    {
                        if (WantToBeDestoryedOnTouch && !PlayerManager.isDashing)
                        {
                            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
                            Destroy(gameObject);
                            AudioP.PlayOneShot(WaterSplash);
                            StickAnim.SetTrigger("Hurt");

                        }
                        if (PlayerManager.isDashing)
                        {
                            Destroy(gameObject);
                        }
                    }

                }
            }
            else
            {
                AudioP.PlayOneShot(WaterSplash);
                FadeToWhite.gameObject.SetActive(true);
                StartCoroutine(QuitTitle());
            }

        }

        if (collision.gameObject.CompareTag("Bullet") && CanBeGone_W_Bullet)
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            AudioP.PlayOneShot(DestroySFX);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Book") && !AlrDid && isInEcology && Ecology != null)
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            AudioP.PlayOneShot(WaterSplash);
            Ecology.Health -= 5;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("SunRay"))
        {
            AlrDid = true;
            GameObject FX1 = Instantiate(FX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    

    public void TeleportPlayerToSafety()
    {
        if (PlayerManager.SafePosition != Vector3.zero)
        {
            PlayerManager.transform.position = PlayerManager.SafePosition;
        }
    }

}
