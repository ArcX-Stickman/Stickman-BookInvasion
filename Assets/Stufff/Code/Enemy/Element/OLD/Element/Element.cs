using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Element : MonoBehaviour
{
   
    public Animator ElementAnim;
    public float attackCD = 4f;
    public int DMG;
    public Transform player;
    public GameObject Projectile;
    public AudioSource AudioPlayer;
    public AudioClip PlayerRestore;
    public Health PlayerHP;
    public Transform ElementPOS;
    public GameObject DeadFX;
    public GameObject FX;
    public float MinDis;
    public float ElementHP = 6.5f;
    public float speed;
    public float force;
    public Transform ThrowPoint;
    public float cooldowntimer = Mathf.Infinity;
    public Vision vision;
    public bool CanATK;
    public Biology FlashRed;
    public BattleV2 Music;

    void Start()
    {
        ElementAnim = GetComponent<Animator>();
        vision.PlayerSpotted = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (PlayerHP.PlayerHP > 0)
        {
            if (vision.PlayerSpotted)

            {

                Music.thing.InCombat = true;

                Attack();

              
                if (Vector2.Distance(transform.position, player.position) > MinDis)
                {
                    Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    
                    if (targetPosition.x < transform.position.x)
                    {
                        
                        transform.localScale = new Vector3(1, 1, 1);

                    }
                    else if (targetPosition.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        
                    }

                    transform.localScale = new Vector3(Mathf.Sign(player.position.x - transform.position.x) * -1, 1, 1);
                }
            }

            
            if (ElementHP <= 0)
            {
                Music.thing.InCombat = false;
                GameObject effect = Instantiate(DeadFX, ElementPOS.position, Quaternion.identity);
                PlayerHP.PlayerHP = 100; 
                AudioPlayer.PlayOneShot(PlayerRestore);
                Destroy(gameObject);
                PlayerHP.InCombat = false;
            }
        }
    }

    void Attack()
    {
        if (Time.time >= cooldowntimer)
        {
            ElementAnim.SetTrigger("Throw");
            cooldowntimer = Time.time + attackCD; 
        }
    }


    public void PublicFollowPlayer()
    {
        if (PlayerHP.PlayerHP > 0)
        {
            if (vision.PlayerSpotted)

            {

                Music.thing.InCombat = true;

                Attack();


                if (Vector2.Distance(transform.position, player.position) > MinDis)
                {
                    Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                    if (targetPosition.x < transform.position.x)
                    {

                        transform.localScale = new Vector3(1, 1, 1);

                    }
                    else if (targetPosition.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);

                    }

                    transform.localScale = new Vector3(Mathf.Sign(player.position.x - transform.position.x) * -1, 1, 1);
                }
            }


            if (ElementHP <= 0)
            {
                Music.thing.InCombat = false;
                GameObject effect = Instantiate(DeadFX, ElementPOS.position, Quaternion.identity);
                PlayerHP.PlayerHP = 100;
                AudioPlayer.PlayOneShot(PlayerRestore);
                Destroy(gameObject);
                PlayerHP.InCombat = false;
            }
        }
    }

    void ThrowBomb()
    {
        Vector2 direction = (player.position - ThrowPoint.position).normalized;
        GameObject projectile = Instantiate(Projectile, ThrowPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // i Mean if it works it works Dont change it >:(
        float distance = Vector2.Distance(ThrowPoint.position, player.position);
        float heightDifference = player.position.y - ThrowPoint.position.y;
        float throwAngle = 26f; 
        float angleRad = throwAngle * Mathf.Deg2Rad;
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float velocity = Mathf.Sqrt(distance * gravity / Mathf.Sin(2 * angleRad));

        float velocityX = velocity * Mathf.Cos(angleRad);
        float velocityY = velocity * Mathf.Sin(angleRad);
        rb.velocity = new Vector2(direction.x * velocityX, velocityY);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && PlayerHP.InCombat)
        {
            Instantiate(FX, ElementPOS.position, Quaternion.identity);
            ElementHP -= 0.5f;
            PlayerHP.MegaCharge += 0.1f;
        }
        if (collision.CompareTag("Bullet") && PlayerHP.InCombat)
        {
            Instantiate(FX, ElementPOS.position, Quaternion.identity);
            ElementHP -= 0.5f;
            PlayerHP.MegaCharge += 0.1f;
        }
        else if (collision.CompareTag("StickMelee") && PlayerHP.InCombat)
        {
            Instantiate(FX, ElementPOS.position, Quaternion.identity);
            ElementHP -= 0.45f;
        }

        


    }
}
