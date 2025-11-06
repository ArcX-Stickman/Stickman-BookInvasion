using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Astros_Star : MonoBehaviour
{

    public float DMG;
    public Movement PlayerManager;
    public Health StickHP;
    public DodgeStreaks StreakManager;
    public AudioSource AudioP;
    public AudioClip Hit;
    public AudioClip Dodge;
    public Animator MainCamAnim;
    public Transform Stickman;
    public float speed;
    public Vector2 direction;
    public float turnSpeed;
    public bool OG;
    public bool Touched;
    public GameObject FX;
    public Rigidbody2D rb;
    public EnemyAI AstrologyReciver;


    private void Start()
    {
        Touched = false;
        rb.simulated = true;
    }
    public void SetDirection(Vector2 newDirection, float angle)
    {
        direction = newDirection.normalized;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        if(AstrologyReciver != null)
        {
            if(AstrologyReciver.globaldistance <= AstrologyReciver.MeleeDistanceMin)
            {
                Destroy(gameObject);   
            }
        }
        rb = GetComponent<Rigidbody2D>();
        float Distance = Vector2.Distance(transform.position,Stickman.position);
        if(Distance >= 150)
        {
            rb.simulated = false;
        }
        else
        {
            rb.simulated = true;
        }

        if (!OG )
        {
            float angle = Mathf.Atan2(Stickman.position.y - transform.position.y, Stickman.position.x - transform.position.x);
            float VelocityX = Mathf.Cos(angle) * speed;
            float VelocityY = Mathf.Sin(angle) * speed;
            Vector3 NewThing = new Vector3(VelocityX, VelocityY, 0f) * Time.deltaTime;
            transform.position += NewThing;


        }

    }

    public void HomeToPlayer()
    {
        //float angle = Mathf.Atan2(Stickman.position.y - transform.position.y, Stickman.position.x - transform.position.x) * Mathf.Rad2Deg;
        //direction = (Stickman.position - transform.position).normalized;
        //float VelocityX = Mathf.Cos(angle) * speed;
        //float VelocityY = Mathf.Sin(angle) * speed;
        //transform.Transla


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            if (PlayerManager.Grounded && !PlayerManager.BackFlip && !PlayerManager.isDashing && !Touched)
            {
                AstrologyReciver.Meteo_Stun();
                Touched = true;
                MainCamAnim.ResetTrigger("ShakeMelee");
                MainCamAnim.SetTrigger("ShakeMelee");
                StreakManager.streak = 0;
                PlayerManager.HeCanMove = false;
                StickHP.PlayerHP = StickHP.PlayerHP - DMG;
                GameObject effect = Instantiate(FX, transform.position, Quaternion.identity);
                AudioP.PlayOneShot(Hit);
                PlayerManager.StickAnim.SetTrigger("Stun");
                Destroy(gameObject);


            }

            else
            {
                Touched = true;
                GameObject effect = Instantiate(FX, transform.position, Quaternion.identity);
                Destroy(gameObject);


            }


        }

        if (collision.CompareTag("Ground"))
        {
            Destroy (gameObject);
            AudioP.PlayOneShot(Hit);
            GameObject effect = Instantiate(FX, transform.position, Quaternion.identity);


        }



    }



}
