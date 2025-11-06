using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    public Movement StickManager;
    public Health StickHP;
    public float PhysicsHP;
    public float speed;
    public Transform PlayerPOS;
    public Transform PhysicsPOS;
    public bool TouchingPlayer;
    public float MinDIS;
    public Animator PhysicsAnim;
    public Vision vision;
    public float MaxSpeed;
    public float PhysicalDMG;
    public PhysicsATK_Zone ATK_Zone;
    public DodgeStreaks StreakManager;
    public GameObject Death;
    public BoxCollider2D SightCollider;
    public BattleV2 Music;
    public AudioSource AudioP;
    public AudioClip ThrowSFX;
    public AttackSight AttackSight;
    public bool SpecialSitting;
    public Animator MainCamAnim;
    public HitMarker HitMarker;
    public EncounterManagerAfterLab EcounterManager;
    public bool StealthKillable;
    public GameObject UIStealth;
    private BoxCollider2D Bx2d;
    public bool WantMusicWhenSpotted;
    public AudioSource BattleTheme;
    public AudioSource HypothesisTheme;
    private bool HasALreadyPlayedJingle;
    public AudioClip Jingle;
    private bool AlrPlayedMusic;


    public float ATKCooldown;
    private float LastATK;

    void Start()
    {
        Bx2d = GetComponent<BoxCollider2D>();
        speed = MaxSpeed;
        if (SpecialSitting)
        {
            PhysicsAnim.SetBool("Sitting",true);
        }
    }


    private bool hasPlayedStealthKill;

    void Update()
    {

        if (StealthKillable && !AttackSight.StickmanIsHERE)
        {
            float distance = Vector2.Distance(transform.position, PlayerPOS.position);
            Debug.Log(distance + "distance PhysicsBook");
            if (distance <= 27 && StickManager.InStealth && UIStealth != null)
            {
                if(UIStealth != null)
                {
                    UIStealth.gameObject.SetActive(true);

                }
            }
            else
            {
                if(UIStealth != null)
                {
                    UIStealth.gameObject.SetActive(false);

                }

            }
        }



        if (StickManager.InStealthKill && StealthKillable)
        {
            Bx2d.isTrigger = false;
        }
        else
        {
            if (StealthKillable)
            {
                Bx2d.isTrigger = true;

            }
        }

        //if(AttackSight.StickmanIsHERE)
        //{
        //    PlayerSpotted();
        //}


        if (StickManager.InStealthKill && !hasPlayedStealthKill)
        {
            WantMusicWhenSpotted = false;
            AlrPlayedMusic = true;
            PhysicsAnim.SetTrigger("StealthKill");
            hasPlayedStealthKill = true;
        }

        if (!StickManager.InStealthKill)
        {
            hasPlayedStealthKill = false;
        }



        if (StickManager.FirstClear)
        {
            PhysicsHP = 0;
        }



        if (ATK_Zone.PlayerInZone == true)
        {
            PhysicalAtk();
        }

        if (PhysicsHP <= 0)
        {


            PhysicsAnim.SetTrigger("Dead");


           
        }

        if (vision.PlayerSpotted)
        {
            if(UIStealth != null)
            {
                Destroy(UIStealth);
            }

            PlayerSpotted();
            if (SpecialSitting)
            {
                PhysicsAnim.SetBool("Sitting", false);
            }
        }
    }

    public void Dead()
    {


        if (SpecialSitting)
        {
            EcounterManager.physics = null;
        }
        StickHP.MegaCharge += 0.2f;
        StickHP.PlayerHP += 2;
        GameObject effect = Instantiate(Death, PhysicsPOS.position, Quaternion.identity);
        StickManager.HeCanMove = true;
        Destroy(gameObject);

        if (WantMusicWhenSpotted)
        {
            if (!HasALreadyPlayedJingle)
            {
                HasALreadyPlayedJingle = true;
                AudioP.PlayOneShot(Jingle);
                BattleTheme.gameObject.SetActive(false);
                HypothesisTheme.gameObject.SetActive(true);
            }

           
        }
            
            
        
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (StickManager.InStealthKill)
            {
                HitMarker.HitMarkerSFX();
                MainCamAnim.SetBool("Zoom", false);
                PhysicsAnim.SetTrigger("Dead");
                //PlayerManager.StickAnim.SetTrigger("StealthPunch");
                StickManager.InStealthKill = false;
                StickHP.PlayerHP += 0.1f;
                GameObject effect = Instantiate(Death, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }

        }
    }

    public void PlayerSpotted()
    {

        if (WantMusicWhenSpotted && !StickManager.InStealthKill)
        {
            if(!BattleTheme.gameObject.activeSelf && PhysicsHP > 0)
            {
                AlrPlayedMusic = true;
                HypothesisTheme.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }



            if (!AlrPlayedMusic)
            {
                AlrPlayedMusic = true;
                HypothesisTheme.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }



        if (AttackSight.StickmanIsHERE && Time.time > LastATK + ATKCooldown)
        {
            LastATK = Time.time;
            PhysicsAnim.SetTrigger("Attack");
        }
        else
        {
            if (StickHP.PlayerHP > 0 )
            {
                float distanceToPlayer = Vector2.Distance(transform.position, PlayerPOS.position);

                if (distanceToPlayer <= MinDIS)
                {
                    TouchingPlayer = true;
                    PhysicsAnim.SetBool("Walk", false);
                }
                else
                {
                    TouchingPlayer = false;
                    PhysicsAnim.SetBool("Walk", true);
                    Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);

                    if (targetPosition.x < transform.position.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (targetPosition.x > transform.position.x)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                }

                if (TouchingPlayer)
                {
                    StickHP.InCombat = true;
                    Debug.Log("PhysicsTouch");
                }
            }
        }


       
    }

    public void PhysicalAtk()
    {
        PhysicsAnim.SetTrigger("Attack");
    }


    public void Accelerate()
    {
        speed += 0.2f;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("ArcBullet"))
        {
            AttackSight.StickmanIsHERE = true;
            PlayerSpotted();
            PhysicsHP -= collision.CompareTag("Bullet") ? 1f : 1.5f;
            if (PhysicsHP <= 0)
            {

                Instantiate(Death, PhysicsPOS.position, Quaternion.identity);
                
            }
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("StickMelee"))
        {
            
            GameObject effect = Instantiate(Death, PhysicsPOS.position, Quaternion.identity);
            PhysicsHP -= 0.3f;
        }

    }

    public void ThrowStickman()
    {
        if (StickManager.Grounded && AttackSight.StickmanIsHERE)
        {
            if (StickHP.PlayerHP > 0)
            {
                StickHP.PlayerHP -= PhysicalDMG;
                AudioP.PlayOneShot(ThrowSFX);
                Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);

                if (targetPosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (targetPosition.x > transform.position.x)
                {

                    //SightCollider.isTrigger = true;
                    
                    KnockPlayer(PlayerPOS);
                    StreakManager.streak = 0;
                }
            }
            else
            {
                StreakManager.StreakThing();
            }
        }
    }


    
    public void KnockPlayer(Transform playerTransform)
    {
        Debug.Log("Yeah");


        Vector2 hitDirection = (playerTransform.position - transform.position).normalized;
        float upwardForce = 1f;
        float horizontalKnockbackForce = 7f;

        if (StickManager != null)
        {
            StartCoroutine(StickFall());
            StartCoroutine(StickManager.KnockBackAction(
                new Vector2(hitDirection.x * horizontalKnockbackForce, upwardForce),
                Vector2.zero,
                0f
            ));
        }

    }



    public IEnumerator StickFall()
    {
        yield return new WaitForSeconds(0.1f);
        StickManager.StickAnim.SetBool("Fall", true);
        StickManager.Grounded = false;
        //yield return new WaitForSeconds(0.2f);
        //StickManager.StickAnim.SetBool("Fall", false);
        //StickManager.Grounded = true;
    }
}
