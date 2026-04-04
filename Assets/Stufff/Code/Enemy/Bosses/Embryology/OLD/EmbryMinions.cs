using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EmbryMinions : MonoBehaviour
{

    public float speed;
    public Health PlayerHealth;
    public float MinDIS;
    public Transform StickmanPOS;
    public Movement PlayerManager;
    public float MeleeDistanceMin;
    public bool isAttacking;
    public Animator EnemyAnim;
    public bool TouchingPlayer;
    public AudioSource AudioP;
    public AudioClip PunchSFX;
    public AudioClip DodgeSFX;
    public float MeleeCD;
    private float LastMeleeAttack;
    public AttackSight HeadCollider;
    public float DMG;
    public bool isOg;
    public float HP;
    private BoxCollider2D Bxcd;
    public BoxCollider2D Stick;

    // Start is called before the first frame update
    void Start()
    {
        Bxcd = GetComponent<BoxCollider2D>();
        EnemyAnim.SetTrigger("Spawn");

    }

    // Update is called once per frame
    void Update()
    {

        if(HP <= 0)
        {
            Destroy(gameObject);
        }

        float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);

        PlayerSpotted();
        if(Time.time >= MeleeCD + LastMeleeAttack && distanceToPlayer <= MeleeDistanceMin)
        {
            EnemyAnim.SetTrigger("Melee");
            EnemyAnim.SetBool("Run",false);
            LastMeleeAttack = Time.time;
        }
        if (Stick.IsTouching(Bxcd) && PlayerManager.isSliding)
        {
            Debug.Log("fALSE COLLISION");
            Physics2D.IgnoreCollision(Bxcd, Stick, true);
        }


        if (PlayerManager.isSliding | PlayerManager.isDashing)
        {
            Debug.Log("fALSE COLLISION");
            Physics2D.IgnoreCollision(Bxcd, Stick, true);
        }
        else if (Stick.IsTouching(Bxcd) && PlayerManager.isSliding)
        {
            Debug.Log("fALSE COLLISION");
            Physics2D.IgnoreCollision(Bxcd, Stick, true);
        }
        else
        {
            Physics2D.IgnoreCollision(Bxcd, Stick, false);
        }
    }

    public void MeleeAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);

        if (!PlayerManager.isDashing && !PlayerManager.isSliding && !PlayerManager.BackFlip && distanceToPlayer <= MeleeDistanceMin)
        {
            PlayerHealth.PlayerHP -= DMG;
            AudioP.PlayOneShot(PunchSFX);
        }
        else
        {
            PlayerHealth.MegaCharge += 1;
            AudioP.PlayOneShot(DodgeSFX);
        }
    }

    public void PlayerSpotted()
    {

        if (PlayerHealth.PlayerHP > 0 && speed > 1 && !isAttacking && !PlayerManager.GustOfBullets && !isOg)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);


            if (distanceToPlayer <= MinDIS | distanceToPlayer <= MeleeDistanceMin)
            {
                TouchingPlayer = true;
                EnemyAnim.SetBool("Run", false);
            }
            else
            {
                TouchingPlayer = false;
                EnemyAnim.SetBool("Run", true);

                Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


                if (targetPosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1.102464f, 1.219102f, 1);

                }
                else if (targetPosition.x > transform.position.x )
                {
                    transform.localScale = new Vector3(-1.102464f, 1.219102f, 1); ;
                }
            }



        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee") && !isOg)
        {
            PlayerHealth.MegaCharge += 0.01f;
            HP -= 1;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerManager.CanCatch && HP <= 1)
        {
            PlayerManager.HasCaught = true;
            Destroy(gameObject);
        }
    }


}
