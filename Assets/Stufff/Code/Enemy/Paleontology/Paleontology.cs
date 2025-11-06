using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Paleontology : MonoBehaviour
{

    public bool CanGo;
    public Health StickHP;
    public bool TouchingPlayer;
    public Transform PlayerPOS;
    public float MinDIS;
    public AttackSight AttackSight;
    public Animator PalenAnim;
    public float speed;
    public Movement PlayerManager;
    public Transform ThrowPoint;
    public Rigidbody2D rb;
    public GameObject Fossil1;
    public GameObject Fossil2;
    public float FossilSpeed;
    public float FossilCD;
    private float LastAtk;
    public GameObject[] FossilList;
    public Transform PalenHPPOS;
    public float PalenHP;
    public GameObject Dead;
    public RangeSight RangeSight;

    public AudioClip MeleePierce;
    public AudioClip MeleeMissed;
    public AudioSource AudioP;
    public bool Falling;
    public bool TriggeredFall;
    public bool FreeRoamer;
    public Transform currentPOS;
    public Transform R_Point;
    public Transform L_Point;
    public bool AlrPlayedMusic;
    public bool AlrPlayedJingle;
    public AudioSource Hypothesis;
    public AudioSource BattleTheme;
    public bool WantMusicWhenSpotted;
    public AudioClip Jingle;
    public Animator MainCamAnim;
    public HitMarker HitMarker;
    private bool hasPlayedStealthKill;
    public bool StealthKillable;
    public bool AlreadySeen;
    public GameObject UIStealth;
    public float WalkCD;
    public int HowManyTurnsBeforeBreak;
    public int StepsRN;
    public float LastWalkCD;
    public bool CanWalk;
    public BoxCollider2D BX2d;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //if (PlayerManager.InStealthKill)
        //{
        //    BX2d.isTrigger = false;
        //}
        //else
        //{
        //    BX2d.isTrigger = true;
        //}

        if (StealthKillable && !AlreadySeen)
        {
            float distance = Vector2.Distance(transform.position, PlayerPOS.position);
            Debug.Log(distance + "distance Pal");
            if (distance <= 41 && PlayerManager.InStealth)
            {
                UIStealth.gameObject.SetActive(true);
            }
            else
            {
                UIStealth.gameObject.SetActive(false);

            }
        }



        if (PlayerManager.InStealthKill && !hasPlayedStealthKill)
        {
            WantMusicWhenSpotted = false;
            AlrPlayedMusic = true;
            AlrPlayedJingle = true;
            PalenAnim.SetTrigger("StealthKill");
            hasPlayedStealthKill = true;
        }


        if (Time.time >= WalkCD + LastWalkCD)
        {
            CanWalk = true;
        }
        else
        {
            PalenAnim.SetBool("Walk", false);
            CanWalk = false;

        }

        if (StepsRN == HowManyTurnsBeforeBreak)
        {
            StepsRN = 0;
            CanWalk = false;
            LastWalkCD = Time.time;
        }



        if (FreeRoamer && !PlayerManager.InStealthKill && CanWalk)
        {
            PalenAnim.SetBool("Walk", true);

            if (!AttackSight.StickmanIsHERE && CanWalk)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, currentPOS.position, step);

                if (Vector2.Distance(transform.position, currentPOS.position) < 1f)
                {
                    currentPOS = currentPOS == R_Point.transform ? L_Point.transform : R_Point.transform;

                    if (currentPOS == R_Point.transform)
                        transform.localScale = new Vector3(-1, 1, 1);
                    else
                        transform.localScale = new Vector3(1, 1, 1);

                    StepsRN++;
                }
            }
        }



        if (RangeSight.StickHere)
        {
            MeleeATK();
        }


        if (AttackSight.StickmanIsHERE)
        {
            CanGo = true;
        }



        if (CanGo)
        {
            PlayerSpotted();
        }

        if (PlayerManager.FirstClear)
        {
            PalenHP = 0;
        }


        if (PalenHP <= 0)
        {
            PalenAnim.SetTrigger("Dead");
        }


        if (Falling)
        {
            PalenAnim.SetBool("Fall", true);

            speed = 0;
            FossilCD = 999;
            StunCD = 999;
            if (!TriggeredFall)
            {
                TriggeredFall = true;
                StartCoroutine(TriggerFallWait());
            }
        }


    }

    public void Accelerate()
    {
        speed += 0.1f;

    }

    public void Death()
    {
        if (WantMusicWhenSpotted && !PlayerManager.InStealthKill)
        {
            if (!AlrPlayedJingle)
            {
                AlrPlayedJingle = true;
                AudioP.PlayOneShot(Jingle);
                BattleTheme.gameObject.SetActive(false);
                Hypothesis.gameObject.SetActive(true);
            }


        }
        GameObject effect = Instantiate(Dead, PalenHPPOS.position, Quaternion.identity);
        StickHP.PlayerHP += 10;
        StickHP.MegaCharge += 4;
        Destroy(gameObject);
    }

    public void PlayerSpotted()
    {

        if (WantMusicWhenSpotted && !PlayerManager.InStealthKill)
        {
            if (!AlrPlayedMusic)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }


        if (AttackSight.StickmanIsHERE && Time.time >= LastAtk + FossilCD)
        {
            PalenAnim.SetTrigger("ATK");
        }
        else
        {
            if (StickHP.PlayerHP > 0 && !PlayerManager.Hiding && !Falling)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, PlayerPOS.position);

                if (distanceToPlayer <= MinDIS)
                {
                    TouchingPlayer = true;
                    PalenAnim.SetBool("Walk", false);
                }
                else
                {
                    PalenAnim.SetBool("Walk", true);
                    Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
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

                if (TouchingPlayer)
                {
                    StickHP.InCombat = true;
                    Debug.Log("PhysicsTouch");
                }
            }
        }



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.InStealthKill)
            {
                HitMarker.HitMarkerSFX();
                MainCamAnim.SetBool("Zoom", false);
                PalenAnim.SetTrigger("Dead");
                //PlayerManager.StickAnim.SetTrigger("StealthPunch");
                PlayerManager.InStealthKill = false;
                StickHP.PlayerHP += 0.1f;
                GameObject effect = Instantiate(Dead, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }

        }
    }
    public IEnumerator TriggerFallWait()
    {
        Debug.Log("Waiting For Astro to die");
        yield return new WaitForSecondsRealtime(2.4f);
        Destroy(gameObject);
        GameObject effect = Instantiate(Dead, transform.position, Quaternion.identity);

    }
    public void MeleeATK()
    {

        CanGo = true;

        int RandInt = UnityEngine.Random.Range(1, 2);

        Debug.Log(RandInt + "Ya THIS DMG");

        if (RandInt == 1)
        {
            if (RangeSight.StickHere)
            {
                PalenAnim.SetTrigger("Melee1");
            }

        }
        else
        {
            if (RangeSight.StickHere)
            {
                PalenAnim.SetTrigger("Melee2");
            }
        }


    }



    public void DMG()
    {
        if (RangeSight.StickHere)
        {
            //AudioClip RandSFX = MeleePierce[UnityEngine.Random.Range(0, MeleePierce.Length)];
            AudioP.PlayOneShot(MeleePierce);

            float DMG = UnityEngine.Random.Range(18, 25);
            PlayerManager.StickAnim.SetTrigger("Hurt");
            StickHP.PlayerHP -= DMG;
        }
        else
        {
            AudioP.PlayOneShot(MeleeMissed);
        }

    }

    public void Combo1()
    {
        if (RangeSight.StickHere)
        {
            PalenAnim.SetTrigger("Melee2");
        }

    }
    public void Combo2()
    {
        if (RangeSight.StickHere)
        {
            PalenAnim.SetTrigger("Melee1");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            StickHP.PlayerHP += 0.2f;
            GameObject effect = Instantiate(Dead, PalenHPPOS.position, Quaternion.identity);
            PalenHP -= 0.7f;
        }
        if (collision.CompareTag("ArcBullet"))
        {
            StickHP.PlayerHP += 0.5f;
            GameObject effect = Instantiate(Dead, PalenHPPOS.position, Quaternion.identity);
            PalenHP -= 2f;
        }
        if (collision.CompareTag("Void"))
        {
            Falling = true;
        }
        if (collision.CompareTag("StickMelee"))
        {
            StickHP.PlayerHP += 0.1f;
            GameObject effect = Instantiate(Dead, PalenHPPOS.position, Quaternion.identity);
            PalenHP -= 0.3f;

            Stun();
        }
    }


    private int StunCount;
    private float LastStun;
    private float StunCD = 1.6f;
    public void Stun()
    {
        if (Time.time >= LastStun + StunCD && StunCount < 8)
        {
            PalenAnim.SetTrigger("Stun");
            StunCount += 1;

            if (StunCount == 4)
            {
                LastStun = Time.time;
            }
        }
        else
        {
            MeleeATK();
        }



    }




    public void FossilATK()
    {

        float Chance = UnityEngine.Random.Range(0f, 10f);


        if (StickHP.PlayerHP > 0)
        {


            GameObject Fossil = FossilList[UnityEngine.Random.Range(0, FossilList.Length)];






            Debug.Log("Fossil ATK");

            Vector2 direction = (PlayerPOS.position - ThrowPoint.position).normalized;


            GameObject projectile = Instantiate(Fossil, ThrowPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();


            // ig it works dont change this PLS future Arjun ik ur stupid

            float enemySpeed = rb.velocity.x;


            float distance = Vector2.Distance(ThrowPoint.position, PlayerPOS.position);


            float baseThrowSpeed = FossilSpeed;


            float velocityX = direction.x * baseThrowSpeed + enemySpeed;


            float arcHeight = 0.01f;
            float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));


            rb.velocity = new Vector2(velocityX, velocityY);


            rb.AddTorque(570f);
        }

    }
}