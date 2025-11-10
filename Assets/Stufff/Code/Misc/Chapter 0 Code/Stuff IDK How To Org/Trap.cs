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

    public Animator RespawnUI_BlackFade;


    public RespawnUI_ForTraps respawnUI_ForTraps;

    public GameObject WaterSplashFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!EndingDemo1)
            {
                Debug.Log("VoidKill");
                if (Teleportable)
                {
                    if (isWater)
                    {
                        AudioP.PlayOneShot(WaterSplash);
                        if (WaterSplashFX != null)
                        {
                            GameObject FX = ObjectPooler.SpawnObj(WaterSplashFX, PlayerManager.transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

                        }

                    }
                    if(PlayerHP.PlayerHP - DMG > 0)
                    {
                        RespawnUI_BlackFade.CrossFade("Respawn_FadeIn_Out_TrapBased", 0, 0);
                        TeleportPlayerToSafety();
                    }
                  
                    PlayerHP.PlayerHP -= DMG;
                    return;
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
                if (!DoesntOneShot && !Teleportable)
                {
                    PlayerHP.PlayerHP = 0;

                }
                else if (!Teleportable)
                {
                    if (Time.time >= LastHit + HitCD)
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
    private bool WaterSplashSFX_Played;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!EndingDemo1)
            {
                Debug.Log("VoidKill");
                if (Teleportable)
                {
                   
                    if (isWater)
                    {
                        AudioP.PlayOneShot(WaterSplash);

                    }
                    PlayerHP.PlayerHP -= DMG;
                    if(Time.timeScale == 0)
                    {
                        TeleportPlayerToSafety();
                        return;
                    }
                    
                   
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
                if (!DoesntOneShot && !Teleportable)
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
            PlayerManager.rb.velocity = Vector2.zero;
            StopCoroutine(PlayerManager.Slide());
            SendRespawnToUI_Script(PlayerManager.SafePosition);
            //PlayerManager.transform.position = PlayerManager.SafePosition;
        }
    }
    
    public void SendRespawnToUI_Script(Vector3 Pos)
    {
        if(respawnUI_ForTraps == null)
        {
            respawnUI_ForTraps = FindAnyObjectByType<RespawnUI_ForTraps>();
        }
        respawnUI_ForTraps.SafePosition = Pos;
    }

}
