using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Astrology : MonoBehaviour
{
    public Transform AstroPOS;
    public GameObject AstroBook;
    public GameObject Death;
    public float speed;
    public Transform StickmanPOS;
    public GameObject BlackHoleObj;
    public float BlackHoleCD;
    private float LastATK;
    public float MaxSpeed;
    public float HP;
    public Vision RangeManager;
    public Health PlayerHealth;
    public bool TouchingPlayer;
    public float MinDIS;
    public Animator AstroAnim;
    public Transform AttackPoint;
    public Blackhole BlackholeScript;
    public AudioClip SummonSound;
    public AudioSource AudioP;
    public Transform FXpoint;
    public Movement PlayerManager;
    public Transform BookFix;
    public RangeSight MeleeSight;
    public bool MeleeATK_Bool;
    public DodgeStreaks StreakManager;
    public int HurtRndInt;
    private bool FirstHit = false;
    // SFX Melee
    public AudioClip Melee1;
    public AudioClip Melee2;
    public bool Falling;

    public GameObject Dead;

    private bool TriggeredFall;



    [Header("Patrol/Roam")]

    public bool FreeRoamer;
    public bool StealthKillable;
    public Transform currentPOS;
    public Transform R_Point;
    public Transform L_Point;
    public Rigidbody2D rb;
    public GameObject UIStealth;
    public Animator MainCamAnim;
    public HitMarker HitMarker;
    public Roaming roaming;

    private bool AlreadySeen;
    private bool hasPlayedStealthKill;


    public AudioSource Hypothesis;
    public AudioSource BattleTheme;
    public AudioClip Jingle;
    public bool AlrPlayedMusic;
    public bool WantMusicWhenSpotted;
    public bool AlrPlayedJingle;
    // Start is called before the first frame update
    void Start()
    {
        speed = MaxSpeed;
        LastATK = 0;
    }

    // Update is called once per frame
    void Update()
    {


        if (StealthKillable && !AlreadySeen)
        {
            float distance = Vector2.Distance(transform.position, StickmanPOS.position);
            Debug.Log(distance + "distance Astro");
            if (distance <= 25 && PlayerManager.InStealth)
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
            AstroAnim.SetTrigger("StealthKill");
            hasPlayedStealthKill = true;
        }

        if (!PlayerManager.InStealthKill)
        {
            hasPlayedStealthKill = false;
        }


        if (PlayerManager.InStealth && RangeManager.PlayerSpotted)
        {
            RangeManager.PlayerSpotted = false;
        }


        if (FreeRoamer && !PlayerManager.InStealthKill)
        {


            AstroAnim.SetBool("Run", true);


            if (!RangeManager.PlayerSpotted)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, currentPOS.position, step);

                if (currentPOS == R_Point.transform)
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                    rb.velocity = new Vector2(speed, 0);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);

                    rb.velocity = new Vector2(-speed, 0);
                }

                if (Vector2.Distance(transform.position, currentPOS.position) < 1f)
                {
                    currentPOS = currentPOS == R_Point.transform ? L_Point.transform : R_Point.transform;
                }
            }





        }




        if (Falling)
        {
            speed = 0;
            BlackHoleCD = 999;
            StunCD = 999;
            if (!TriggeredFall)
            {
                TriggeredFall = true;
                StartCoroutine(TriggerFallWait());
            }
        }

        if (MeleeSight.StickHere)
        {
            MeleeATK_Bool = true;
            ComboTime();

        }
        else
        {
            MeleeATK_Bool = false;
        }


        if(speed <= 0)
        {
            AstroAnim.SetBool("Run", false);
        }

        if (PlayerManager.FirstClear)
        {
            HP = 0;
        }


        if (RangeManager.PlayerSpotted)
        {
            PlayerSpotted();
        }


        

        if(HP <= 0 && !AlreadyGavePlayerBoost)
        {
            AlreadyGavePlayerBoost = true;
            PlayerHealth.PlayerHP += 10;
            PlayerHealth.MegaCharge += 4;
            AstroAnim.SetTrigger("Dead");
            

        }
    }

    private bool AlreadyGavePlayerBoost;


    private bool InFirstPunch;
    public void ComboTime()
    {
        InFirstPunch = true;
        AstroAnim.SetTrigger("Melee");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.InStealthKill)
            {
                HitMarker.HitMarkerSFX();
                MainCamAnim.SetBool("Zoom", false);
                AstroAnim.SetTrigger("Dead");
                //PlayerManager.StickAnim.SetTrigger("StealthPunch");
                PlayerManager.InStealthKill = false;
                PlayerHealth.PlayerHP += 0.1f;
                GameObject effect = Instantiate(Dead, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }

        }
    }
    public void MeleeDMG()
    {

        float DMG = Random.Range(15f, 25f);


        if (MeleeSight.StickHere)
        {
            PlayerHealth.PlayerHP -= DMG;
            if (InFirstPunch)
            {
                PlayerManager.StickAnim.SetTrigger("Hurt");
                AudioP.PlayOneShot(Melee1);

            }
            else
            {
                PlayerManager.StickAnim.SetTrigger("Hurt");

                AudioP.PlayOneShot(Melee2);
            }
        }
        else
        {
            PlayerHealth.PlayerHP += 0.4f;
            PlayerHealth.MegaCharge += 0.01f;
            StreakManager.StreakThing();
        }
    }


    public void SecondHit()
    {
        InFirstPunch = false;
        AstroAnim.SetTrigger("Melee2");
    }
    public void DeathDestroy()
    {


        if (WantMusicWhenSpotted)
        {
            if (BattleTheme.isPlaying)
            {
                if (!AlrPlayedJingle)
                {
                    AlrPlayedJingle = true;

                    AudioP.PlayOneShot(Jingle);

                }
                BattleTheme.gameObject.SetActive(false);
                Hypothesis.gameObject.SetActive(true);
            }
        }




        if (WantMusicWhenSpotted && !PlayerManager.InStealthKill && !PlayerManager.InStealth)
        {
            if (!AlrPlayedJingle)
            {
                AlrPlayedJingle = true;
                AudioP.PlayOneShot(Jingle);
                BattleTheme.gameObject.SetActive(false);
                Hypothesis.gameObject.SetActive(true);
            }


        }
        GameObject effect = Instantiate(Dead, FXpoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator TriggerFallWait()
    {
        Debug.Log("Waiting For Astro to die");
        yield return new WaitForSecondsRealtime(2.4f);
        Destroy(gameObject);
        GameObject effect = Instantiate(Dead, FXpoint.position, Quaternion.identity);

    }

    public void MeleeATK_BoolFalse()
    {
        MeleeATK_Bool = false;
    }

    public void PlayerSpotted()
    {


        if (WantMusicWhenSpotted && !PlayerManager.InStealthKill && !PlayerManager.InStealth)
        {
            if (!AlrPlayedMusic)
            {
                //AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }


        if (RangeManager.PlayerSpotted && !PlayerManager.InStealth)
        {
            if (Time.time >= LastATK + BlackHoleCD)
            {
                BlackholeATK();
            }
        }

        if (PlayerHealth.PlayerHP > 0 && speed > 1 && !PlayerManager.Hiding && !Falling && !PlayerManager.InStealth)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);

            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;
                AstroAnim.SetBool("Run", false);
            }
            else
            {
                TouchingPlayer = false;
                AstroAnim.SetBool("Run", true);

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
        }
    }

    public void BlackholeATK()
    {
        if (!MeleeATK_Bool)
        {
            Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            Transform BookPos = BookFix.transform;
            BookFix.transform.position = BookPos.position;
            LastATK = Time.time;
            AstroAnim.SetTrigger("ATK");
            
        }

    }


    public void Accelerate()
    {
        speed += 0.1f;

    }


    public void PlaySoundSummon()
    {
        AudioP.PlayOneShot(SummonSound);
    }

    public void SummonBlackHole()
    { 

        Vector2 direction = (StickmanPOS.position - AttackPoint.position).normalized;

        GameObject blackhole = Instantiate(BlackHoleObj, AttackPoint.position, Quaternion.identity);

        Blackhole blackholeScript = blackhole.GetComponent<Blackhole>();
        if (blackholeScript != null)
        {
            float directionValue = transform.localScale.x > 0 ? -1 : 1;
            blackholeScript.SettingDirection(directionValue);
        }
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerHealth.PlayerHP += 0.2f;
            GameObject effect = Instantiate(Dead, FXpoint.position, Quaternion.identity);
            HP -= 0.7f;
        }
        if (collision.CompareTag("ArcBullet"))
        {
            PlayerHealth.PlayerHP += 0.5f;
            GameObject effect = Instantiate(Dead, FXpoint.position, Quaternion.identity);
            HP -= 2f;
        }

        if (collision.CompareTag("StickMelee"))
        {
            PlayerHealth.PlayerHP += 0.1f;
            GameObject effect = Instantiate(Dead, FXpoint.position, Quaternion.identity);
            HP -= 0.3f;



            Stun();



        }
        if (collision.CompareTag("Void"))
        {

            Falling = true;
            AstroAnim.SetBool("Fall", true);


        }

    }


    private int StunCount;
    private float LastStun;
    private float StunCD = 1.2f;
    public void Stun()
    {
        if(Time.time >= LastStun + StunCD && StunCount < 1)
        {
            AstroAnim.SetTrigger("Hurt");
            StunCount += 1;

            if(StunCount == 1)
            {
                LastStun = Time.time;
            }
        }
        


    }
}
