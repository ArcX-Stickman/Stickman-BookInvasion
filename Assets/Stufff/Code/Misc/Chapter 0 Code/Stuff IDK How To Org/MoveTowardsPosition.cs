using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTowardsPosition : MonoBehaviour
{
    public Transform PlaceToGO;
    public bool WantToGO;
    public float Speed;
    public bool WantAcceleration;
    public bool StickBasedSpeed;
    public Movement Stick;
    public float Distance;
    public float MinDisForStickMove;
    public bool CanBeStoppedByCollision;
    public bool WantFx;
    public bool WantDestroyOnImpact;
    public GameObject FX;
    public AudioSource AudioP;
    public AudioClip SFX;
    public bool SloMo;
    public bool CanKillPlayer;
    public bool FxHasAnimAndFXNeedTOBeFast;
    public bool WantFollowYAxis;
    public bool SlowDownIFPlayerFar;
    public float OriginalSpeed;
    public bool DestoryOnReach;
    public Transform SecondDes;
    public bool WantSecondDisTrigger;
    public bool SecondDisNow;
    public float MIN_DIS_ATTRIBUTE_FOR_REFERENCE;
    public bool CanBeDodged;
    public bool CanBeDestroyedGround;
    public bool FlipScale;
    private bool Flipped;
    public float IntensityDestruc_CamShake;
    public bool WantShakingIntensity;
    public bool Grounded;
    private void Start()
    {
        OriginalSpeed = Speed;
    }

    void Update()
    {

        if (gameObject.activeSelf)
        {

            if(WantShakingIntensity && Grounded)
            {
                StartCoroutine(ShakeLikeCRazYYY());
            }
            float DistanceDestin = 0;

            if (WantFollowYAxis)
            {
                DistanceDestin = Vector2.Distance(transform.position, PlaceToGO.position);
                            Distance = DistanceDestin;

            }
            else
            {
                Vector2 fakePOS = new Vector2(PlaceToGO.position.x, transform.position.y);

                DistanceDestin = Vector2.Distance(transform.position, fakePOS);
            }
            float MinDisForDestruc = MIN_DIS_ATTRIBUTE_FOR_REFERENCE;
            Stick = FindAnyObjectByType<Movement>();
            if (StickBasedSpeed)
            {
                Distance = Vector2.Distance(transform.position, Stick.transform.position);
                if (Distance <= MinDisForStickMove)
                {
                    Speed = Stick.MaxSpeed - 15.4f;
                }
                else if (Distance >= 65)
                {
                    Speed = Stick.MaxSpeed + 13;
                }
                else
                {
                    Speed = Stick.MaxSpeed - 9.4f; // sum random number i came up with after a lot of attemtps
                }

            }
            if (FlipScale && WantToGO && !Flipped)
            {
                Flipped = true;
                Vector2 fakeFlip = new Vector2(transform.localScale.x * -1,transform.localScale.y);
                transform.localScale = fakeFlip;
            }
            if(DistanceDestin <= MinDisForDestruc && DestoryOnReach)
            {
                Destroy(gameObject);
            }
            if(DistanceDestin <= MinDisForDestruc - 7 && SecondDes != null && SecondDisNow)
            {
                PlaceToGO = SecondDes;
            }
            if (WantToGO && ! WantFollowYAxis)
            {
                Vector2 fakePOS = new Vector2(PlaceToGO.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, fakePOS, Speed * Time.deltaTime);
            }
            else if(WantToGO && WantFollowYAxis)
            {
                transform.position = Vector2.MoveTowards(transform.position, PlaceToGO.position, Speed * Time.deltaTime);

            }

            if (SlowDownIFPlayerFar)
            {
                SpeedManagerSlo();
            }
        }

    }

    IEnumerator ShakeLikeCRazYYY()
    {
        while (Grounded)
        {
            Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(IntensityDestruc_CamShake, 0.1f));
            yield return null;
        }
    }

    // This is for check to see if it should stop go faster or slower
    void SpeedManagerSlo()
    {
        Vector2 directionToStick = (Stick.transform.position - transform.position).normalized;
        float dot = Vector2.Dot(transform.right, directionToStick);

        Speed = Mathf.Lerp(OriginalSpeed - 5f, OriginalSpeed + 80f, (dot + 1f) / 0.3f);
    }
    private bool alrShookCam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spec") && CanBeStoppedByCollision)
        {
            if (WantFx)
            {
                if(AudioP != null && SFX != null)
                {
                    AudioP.PlayOneShot(SFX);
                }
                if (!alrShookCam)
                {
                    alrShookCam = true;
                    Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(IntensityDestruc_CamShake, 0.5f));

                }
                GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                if (WantDestroyOnImpact)
                {
                    if (SloMo)
                    {
                        Stick.TimeManager.SloMO();
                    }
                    Destroy(gameObject);

                }
            }
            DeactivateScriptOnCollision dSC = GetComponent<DeactivateScriptOnCollision>();
            dSC.Deactivate();
        }


        if(collision.CompareTag("Player") && CanKillPlayer)
        {
            if (CanBeDodged && Stick.isDashing || CanBeDodged && Stick.isSliding)
            {
                if (WantFx)
                {
                    if (AudioP != null && SFX != null)
                    {
                        AudioP.PlayOneShot(SFX);
                    }

                    GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                    if (WantDestroyOnImpact)
                    {
                        if (SloMo)
                        {
                            Stick.TimeManager.SloMO();
                        }
                        Destroy(gameObject);

                    }
                }
                DeactivateScriptOnCollision dSC = GetComponent<DeactivateScriptOnCollision>();
                dSC.Deactivate();
            }
            else
            {
                if (WantFx)
                {
                    Stick.StickHP.PlayerHP = 0;
                    if (AudioP != null && SFX != null)
                    {
                        AudioP.PlayOneShot(SFX);
                    }

                    GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                    if (WantDestroyOnImpact)
                    {
                        if (SloMo)
                        {
                            Stick.TimeManager.SloMO();
                        }
                        Destroy(gameObject);

                    }
                }
            }

           
        }
        if (collision.gameObject.CompareTag("Ground") && CanBeDestroyedGround)
        {
            if (WantFx)
            {
                if (AudioP != null && SFX != null)
                {
                    AudioP.PlayOneShot(SFX);
                }

                GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                if (WantDestroyOnImpact)
                {
                    if (SloMo)
                    {
                        Stick.TimeManager.SloMO();
                    }
                    Destroy(gameObject);

                }
            }
            DeactivateScriptOnCollision dSC = GetComponent<DeactivateScriptOnCollision>();
            dSC.Deactivate();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
        if (collision.gameObject.CompareTag("Player") && CanKillPlayer)
        {
            if (WantFx)
            {
                Stick.StickAnim.CrossFade("Dead", 0, 0);
                Stick.StickHP.PlayerHP = 0;
                if (AudioP != null && SFX != null)
                {
                    AudioP.PlayOneShot(SFX);
                }

                GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                if (FxHasAnimAndFXNeedTOBeFast)
                {
                    Animator FXAnim = Boom.GetComponent<Animator>();
                    FXAnim.speed *= 5;
                }
                if (WantDestroyOnImpact)
                {
                    if (SloMo)
                    {
                        Stick.TimeManager.SloMO();
                    }
                    Destroy(gameObject);

                }
            }
        }

        if (collision.gameObject.CompareTag("Ground") && CanBeDestroyedGround)
        {
            if (WantFx)
            {
                if (AudioP != null && SFX != null)
                {
                    AudioP.PlayOneShot(SFX);
                }

                GameObject Boom = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                if (WantDestroyOnImpact)
                {
                    if (SloMo)
                    {
                        Stick.TimeManager.SloMO();
                    }
                    Destroy(gameObject);

                }
            }
            DeactivateScriptOnCollision dSC = GetComponent<DeactivateScriptOnCollision>();
            dSC.Deactivate();
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        Grounded = false;
    //    }
    //}
}
