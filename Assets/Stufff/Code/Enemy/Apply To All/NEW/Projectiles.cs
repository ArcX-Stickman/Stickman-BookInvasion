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
    public AudioClip HitSFX;
    public AudioClip Miss;
    public GameObject FX;

    [Header("booleans ")]

    public bool isPhysicsEnemyWindBlade;




    private void Awake()
    {
        scales = new Vector2(transform.localScale.x, transform.localScale.y);

    }

    private void OnEnable()
    {
        
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

        if (collision.gameObject.CompareTag("Player") && !Hit)
        {
            Hit = true;
            if (PlayerIsVulnerable())
            {
                StickHP.PlayerHP -= Damage;
                KnockBackPlayer();
                StickManager.StickAnim.SetTrigger("Hurt");
                AudioP.PlayOneShot(HitSFX);
                //StickManager.TimeManager.FreezeTime(FreezeTimeOnHit);
                if(FX != null)
                {
                    ObjectPooler.SpawnObj(FX,transform.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);
                }
                Destroy(gameObject);
            }
            else
            {
                //StickManager.TimeManager.FreezeTime(0.1f);
                AudioP.PlayOneShot(Miss);
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
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
