using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public bool Accelerating;
    public float Speed;
    public Vector2 Acceleration;
    public Transform PlayerPOS;
    public Vector2 direction;
    public Vector2 scales;
    public Rigidbody2D rb;
    public float Damage;
    public float KnockBackValueForPlayerX;
    public float KnockBackValueForPlayerY;
    public float DurationOfKnockBackForPlayer;
    public Movement StickManager;
    public Health StickHP;
    public bool Hit;
    public float FreezeTimeOnHit;

    public AudioSource AudioP;
    public AudioSource NonPitchAudioP;
    public AudioClip HitSFX;
    public AudioClip Miss;
    public GameObject FX;
    public bool DontDestory;


    public float ShakeAmount;
    public float ShakeDuration;

    [Header("booleans ")]

    public bool isPhysicsEnemyWindBlade;
    public bool WantCameraShake;

    public bool isOG;

    public bool DestroysAfterCertainTime;

    public float DestoyAfterDuration;



    private void Awake()
    {
        scales = new Vector2(transform.localScale.x, transform.localScale.y);

    }

    private void OnEnable()
    {
        if(!isOG)
        {
            StartCoroutine(DestoryAfterThisManySeconds());
        }
    }

    public IEnumerator DestoryAfterThisManySeconds()
    {
        float timer = 0;
        while(timer < DestoyAfterDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);

    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void Start()
    {
        //IsGone = false;
        if (transform.localScale.x >= 0)
        {
            // Thats jus the scale i used 
            transform.localScale = new Vector3(scales.x, scales.y, 1);

        }
        else
        {
            transform.localScale = new Vector3(-scales.x, scales.y, 1);

        }
    }

    private void Update()
    {
        if(PlayerPOS == null)
        {
            Movement stickMove = FindAnyObjectByType<Movement>();
            PlayerPOS = stickMove.transform;
            StickHP = FindAnyObjectByType<Health>();
            StickManager = stickMove;
        }
        Vector2 movement = direction * Speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, movement.magnitude);
        // i put negative cuz ts was going backwards and im too lazy to use my brain i alr used all my brain power for impact frames >:(
        transform.Translate(-movement);

        if (Accelerating)
        {
            AccelerationThing();
        }
    }


    public void AccelerationThing()
    {
        rb.AddForce((Acceleration * direction) * rb.mass);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        HandleCollision(collision.collider);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }


    public void HandleCollision(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !Hit)
        {
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
            Hit = true;
            if (PlayerIsVulnerable())
            {
                StickHP.PlayerHP -= Damage;
                KnockBackPlayer();
                StickManager.StickAnim.SetTrigger("Hurt");
                AudioP.PlayOneShot(HitSFX);
                //StickManager.TimeManager.FreezeTime(FreezeTimeOnHit);
                if (FX != null)
                {
                    ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                }
                if (!DontDestory)
                {
                    Destroy(gameObject);

                }
                if(WantCameraShake)
                {
                    StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(ShakeAmount,ShakeDuration));
                }
            }
            else
            {
                //StickManager.TimeManager.FreezeTime(0.1f);
                NonPitchAudioP.PlayOneShot(Miss);
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);

            AudioP.PlayOneShot(HitSFX);

            Hit = true;

            if (FX != null)
            {
                ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
            }


            Destroy(gameObject);
        }
    }

    public bool PlayerIsVulnerable()
    {
        if (!StickManager.BackFlip && !StickManager.isSliding && !StickManager.isDashing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void KnockBackPlayer()
    {
        //Debug.Log("Tried To Knockback Player From Projectile Script");
        //StartCoroutine(StickManager.PlayerKnockBack(KnockBackValueForPlayerX, KnockBackValueForPlayerY, DurationOfKnockBackForPlayer));
        //Destroy(gameObject);
    }
}
