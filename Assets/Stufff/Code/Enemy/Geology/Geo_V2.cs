using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geo_V2 : MonoBehaviour
{
    public Movement PlayerMovement; // Reference to the player's movement script
    public float speed = 4f;
    public AttackSight AttackSight;
    public Health PlayerManager;
    public float biologyHealth = 200f;
    public VisionB1 visionThing;
    public Animator Geo_Anim;
    public Transform BiologyPosition;
    public Transform StickmanPOS;
    public BiologyBookObject BioManager;
    public bool TouchingPlayer;
    public bool PlayerOnTopOfBook;
    public bool IsAttacking;
    public BoxCollider2D BiologyCollider;
    public Biology_AttackZone AttackZone;
    public bool PhysicalKB;
    public GameObject Hurt;
    public GameObject Death;
    public Transform ThrowPoint;
    public GameObject Bacteria;
    public GameObject BacteriaProjectile;
    public float BacteriaCD;
    public Rigidbody2D BiologyEnemyGameobject;
    public float ThrowSpeedBAC;
    public GameObject BeatedGame;

    public bool LowerSpeed;
    public bool StopLowerSpeed;

    public float loopcount = 0f;
    public float delay = 1f;


    // Minimum distance 
    public float MinDIS;
    private Vector3 StepBackFunc;



    // SFX
    public AudioClip ThrowSFX;
    public AudioClip PlayerHurt;
    public AudioSource AudioS;

    // Cooldown variables
    public float attackCooldown = 1f; // Cooldown duration in seconds
    private float lastAttackTime = 0f; // Time when the last attack was performed

    private float lastAttackTimeBAC = 0f;

    void Start()
    {
        BeatedGame.gameObject.SetActive(false);
        Hurt.gameObject.SetActive(false);
        PhysicalKB = false;
        PlayerMovement = FindObjectOfType<Movement>();
        Geo_Anim = GetComponent<Animator>();
        BioManager = GetComponentInChildren<BiologyBookObject>(); // Use GetComponentInChildren
    }

    void Update()
    {
        if (PlayerManager.PlayerHP > 0)
        {

            if (PlayerMovement.Grounded == true)
            {
                BiologyCollider.isTrigger = false;
            }

            if (visionThing.PlayerSpotted == true)
            {
                PlayerSpotted();
            }

            if (PhysicalKB == true)
            {
                PlayerMovement.knockbackTime = 0.4f;
                PlayerMovement.hitdirectionforce = 5f;
                PlayerMovement.constantForce = 3.6f;
            }

            if (biologyHealth <= 0)
            {
                GameObject effect = Instantiate(Death, BiologyPosition.position, Quaternion.identity);
                PlayerManager.InCombat = false;
                PlayerManager.BiologyBoss = false;
                PlayerManager.PlayerHP = 100;
                BeatedGame.gameObject.SetActive(true);
                Destroy(gameObject);
            }

            // Handle cooldown timer
            if (IsAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                IsAttacking = false; // Reset attack state when cooldown is done
            }
        }






    }

    public void PlayerSpotted()
    {
        if (PlayerManager.PlayerHP > 0)
        {
            if (TouchingPlayer == true)
            {
                
                AttackStickman();
                Debug.Log("TouchingStick");
            }
            else
            {
                if (Vector2.Distance(transform.position, StickmanPOS.position) > MinDIS)
                {




                    TouchingPlayer = false;
                    Geo_Anim.SetBool("Walk", true);
                    

                    Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                    // Flip the sprite based on direction of movement
                    if (targetPosition.x < transform.position.x) // Moving left
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (targetPosition.x > transform.position.x) // Moving right
                    {
                        transform.localScale = new Vector3(-1, 1, 1); // Face right
                    }
                }
                else
                {
                    TouchingPlayer = true;
                    Debug.Log("its trippin");
                }
            }

            if (TouchingPlayer == true)
            {
                Geo_Anim.SetBool("Walk", false);
                if (Vector2.Distance(transform.position, StickmanPOS.position) != MinDIS)
                {
                    TouchingPlayer = false;
                }
            }
        }
    }

    public void AttackStickman()
    {
        if (PlayerManager.PlayerHP > 0)
        {
            // Check if the cooldown period has passed
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                IsAttacking = true;
                lastAttackTime = Time.time; // Record the time of the attack
                Geo_Anim.SetTrigger("Earthquake");
            }
        }

    }

    public void RevertKB()
    {
        PhysicalKB = false;
        PlayerMovement.knockbackTime = 0.62f;
        PlayerMovement.hitdirectionforce = 13.07f;
        PlayerMovement.constantForce = 8.6f;
    }

    public void AttackFalsing()
    {
        IsAttacking = false;
    }

    public void ThrowStickman()
    {
        if (PlayerManager.PlayerHP > 0)
        {

            AudioS.PlayOneShot(ThrowSFX);
            BiologyCollider.isTrigger = true;
            // Find the player transform directly
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            BioManager.KnockPlayer(playerTransform);




            if (AttackZone.StickDodged == true && AttackZone.streak >= 4)
            {
                PlayerMovement.StickAnim.SetBool("DodgeHigh", true);
            }
            if (AttackZone.StickDodged == true && AttackZone.streak < 4)
            {
                PlayerMovement.StickAnim.SetBool("DodgeLow", true);
            }
            else
            {
                PlayerMovement.StickAnim.SetBool("Fall", true);
            }



        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (PlayerManager.InCombat == true)
            {





                GameObject effect = Instantiate(Death, BiologyPosition.position, Quaternion.identity);
                biologyHealth -= 0.5f;
                PlayerManager.PlayerHP += 0.05f;
                PlayerManager.MegaCharge += 0.1f;
            }
        }



        if (collision.CompareTag("ArcBullet"))
        {
            if (PlayerManager.InCombat == true)
            {
                GameObject effect = Instantiate(Death, ThrowPoint.position, Quaternion.identity);
                biologyHealth -= 1f;
                PlayerManager.PlayerHP += 1f;
                PlayerManager.MegaCharge += 0.1f;
            }
        }

        if (collision.CompareTag("SuperB"))
        {
            if (PlayerManager.InCombat == true)
            {
                biologyHealth -= 10;
            }
        }

        if (collision.CompareTag("Player"))
        {
            // Additional logic for player collision can go here
        }
    }



    public void RangeATKCDAdd()
    {
        BacteriaCD = BacteriaCD + 2.5f;
    }

   

   


    

    

    
}
