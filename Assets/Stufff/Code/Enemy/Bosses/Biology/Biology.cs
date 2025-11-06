using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biology : MonoBehaviour
{

    // Unused Boss Ngl Pretty Garbage its just the boss running behind you and ypu just shoot and he throws bacteria and that it 
    // Even tho I did spent like 10 hrs on this boss pretty important for this games development as it made me create fundamental stuff
    // like sliding and dashing and Crounching which you still can do but ngl pretty uselewss and pretty bad.
    public Movement PlayerMovement;
    public float speed = 4f;
    public AttackSight AttackSight;
    public Health PlayerManager;
    public float biologyHealth = 200f;
    public VisionB1 visionThing;
    public Animator BioAnim;
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
    public GameObject Vine;
    public float NormalSpeed;
    public Animator VineAnim;
    public bool Vinewhipping;
    public VineWhip VineManager;
    public DodgeStreaks StreakManager;

    public float LeafAttackCount;

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
    public float attackCooldown = 1f;
    public float lastAttackTime = 0f; 
    public float VineTime;
    public float VineCD;
    public float LastVineWhip;

    public float lastAttackTimeBAC = 0f;

    void Start()
    {
        speed = NormalSpeed;
        BeatedGame.gameObject.SetActive(false);
        Hurt.gameObject.SetActive(false);
        PhysicalKB = false;
        PlayerMovement = FindObjectOfType<Movement>();
        BioAnim = GetComponent<Animator>();
        BioManager = GetComponentInChildren<BiologyBookObject>(); // Use GetComponentInChildren
    }

    void Update()
    {


        if(LeafAttackCount >= 4)
        {
            BioAnim.SetTrigger("Throw");
        }




        if(PlayerManager.PlayerHP > 0 )
        {

            if(Vinewhipping == true)
            {
                VineWhip();
            }

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
                
                Destroy(gameObject);
            }

            // Handle da cd timer
            if (IsAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                IsAttacking = false;
            }
        }

        

        


    }

    public void PlayerSpotted()
    {
        if(PlayerManager.PlayerHP > 0 && !Vinewhipping)
        {
            if (TouchingPlayer == true)
            {
                PlayerManager.BiologyBoss = true;
                AttackStickman();
                Debug.Log("TouchingStick");
            }
            else
            {
                if (Vector2.Distance(transform.position, StickmanPOS.position) > MinDIS)
                {




                    TouchingPlayer = false;
                    BioAnim.SetBool("Walk", true);
                    if (Time.time >= BacteriaCD)
                    {
                        BacteriaATK();
                    }

                    Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                    if (targetPosition.x < transform.position.x) 
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (targetPosition.x > transform.position.x) 
                    {
                        transform.localScale = new Vector3(-1, 1, 1); 
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
                BioAnim.SetBool("Walk", false);
                if (Vector2.Distance(transform.position, StickmanPOS.position) != MinDIS)
                {
                    TouchingPlayer = false;
                }
            }
        }
    }

    public void AttackStickman()
    {
        if(PlayerManager.PlayerHP > 0 && Vinewhipping == false)
        {
            
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                IsAttacking = true;
                lastAttackTime = Time.time; 
                BioAnim.SetTrigger("Leaf");
                if(StreakManager.streak >= 4)
                {
                    VineWhip();
                }
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
        if(PlayerManager.PlayerHP > 0 && !Vinewhipping)
        {
            
            AudioS.PlayOneShot(ThrowSFX);
            BiologyCollider.isTrigger = true;
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            BioManager.KnockPlayer(playerTransform);




            if(AttackZone.StickDodged == true && AttackZone.streak >= 4)
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
            if (PlayerManager.BiologyBoss == true)
            {


                PlayerManager.MegaCharge += 0.1f;


                GameObject effect = Instantiate(Death, BiologyPosition.position, Quaternion.identity);
                biologyHealth -= 0.2f;
                PlayerManager.PlayerHP += 0.8f;
                
            }
        }



        if (collision.CompareTag("ArcBullet"))
        {
            if (PlayerManager.BiologyBoss == true)
            {
                PlayerManager.MegaCharge += 0.1f;
                Debug.Log("VineWhip called From Bullet");
                
                Vinewhipping = true;
                GameObject effect = Instantiate(Death, ThrowPoint.position, Quaternion.identity);
                biologyHealth -= 2f;
                PlayerManager.PlayerHP += 0.2f;
                

                if(Time.time >= LastVineWhip + VineCD)
                {
                    LastVineWhip = Time.time;
                    VineWhip();
                }


            }
        }

        if (collision.CompareTag("SuperB") && PlayerManager.BiologyBoss == true)
        {
            biologyHealth -= 20;

        }

        if (collision.CompareTag("Player"))
        {
           //idk 
        }
    }

    public void VineWhip()
    {
        StreakManager.streak = 0;
        Vinewhipping = true;
        Debug.Log("VineWhip called");
        BioAnim.SetTrigger("VineWhip");
        VineWhipSummon();
        speed = 0;
        
        
    }


    public void VineWhipSummon()
    {
        BioAnim.SetTrigger("FalseVWhip");
        Vinewhipping = false;
        PlayerMovement.Speed = PlayerMovement.Speed/5;
        VineManager.VineAnim.SetTrigger("Whip");
        Vine.SetActive(true); 
        
    }


    public void WhipDone()
    {
        PlayerMovement.Speed = PlayerMovement.MaxSpeed;
        Vinewhipping = false;
        speed = NormalSpeed;
    }



    public void RangeATKCDAdd()
    {
        //BacteriaCD = BacteriaCD + 2.5f;
    }

    public void Attack()
    {
        BioAnim.SetTrigger("Attack");
    }

    public void DamageStickmanLEAF()
    {
        AttackZone.DamageStickmanLEAF();
    }


    void BacteriaATK()
    {


        if (Time.time >= lastAttackTimeBAC + BacteriaCD && IsAttacking == false && TouchingPlayer == false)
        {
            BioAnim.SetTrigger("Bacteria");

            lastAttackTimeBAC = Time.time;


           
        }

       
    }

    public void AddLeafCount()
    {
        LeafAttackCount = LeafAttackCount + 1;
    }

    public void LeafZero()
    {
        LeafAttackCount = 0;
    }


    public void ThrowTheBacteria()
    {
        if(PlayerManager.PlayerHP > 0)
        {
            
            Vector2 direction = (StickmanPOS.position - ThrowPoint.position).normalized;

            
            GameObject projectile = Instantiate(BacteriaProjectile, ThrowPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            
            float enemySpeed = BiologyEnemyGameobject.velocity.x;

           
            float distance = Vector2.Distance(ThrowPoint.position, StickmanPOS.position);

            
            float baseThrowSpeed = ThrowSpeedBAC;

            
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;  

            
            float arcHeight = 5f;  
            float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));

           
            rb.velocity = new Vector2(velocityX, velocityY);

           
            rb.AddTorque(50f); 
        } 
       
    }







}
