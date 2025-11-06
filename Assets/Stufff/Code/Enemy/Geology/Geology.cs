using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Geology : MonoBehaviour
{

    public Vision vision;

    public float speed;

    public GameObject Battle1;

    public GameObject Battle2;

    public bool dead;

    public Health PlayerManager;

    public Movement PlayerMovement;

    public Transform StickmanPOS;

    public float Geo_Health;

    public bool TouchingPlayer;

    public float MinDIS;

    public Animator CameraAnimator;

    public BattleV2 Music;


    public Biology BioManager;

    public Transform GEO_POS;


    public bool SpecialListenerForYapping;

    public GameObject Camera;


    public Animator Geo_Anim;

    public float EarthquakeCD;

    public bool InATK;

    public Transform player;
    public Transform ThrowPoint;
    public GameObject Projectile;


    public float lastAttackTimeTHROW;

    public float MeleeCD;

    public EncounterManagerAfterLab EncounterManager;

    public bool IsEarthQuake;


    public float lastAttackTime;


    public DodgeStreaks StreakManager;

    public bool StickEvaded;


    public float StickStunDuration;

    public bool StickStunned;

    private float stunStartTime = 0f;

    public AudioSource AudioP;

    public AudioClip StickDodge;

    public AudioClip EarthQuakeATK;

    public GameObject Death;


    public bool HarderEnemy;


    public bool Roamer;
    public bool StealthKillable;
    public GameObject UIStealth;
    public AudioSource Hypothesis;
    public AudioSource BattleTheme;
    public AudioClip Jingle;
    private bool AlrPlayedJingle;
    private bool AlrPlayedMusic;
    public bool WantMusicWhenSpotted;
    private bool hasPlayedStealthKill;
    public HitMarker HitMarker;
    public Animator MainCamAnim;
    public bool CanMove;
    public Transform currentPOS;
    public int StepsRN;
    public Transform R_Point;
    public Transform L_Point;
    public int HowManyTurnsBeforeBreak;
    private float LastWalkCD;
    public float WalkCD;
    public bool Watcher;
    public bool FacingRight;
    public float CD_For_Turns;
    public bool WanttoCheckPal;
    public Paleontology Paleontology;
    void Start()
    {
        currentPOS = R_Point;
        StickStunned = false;
        InATK = false;
        //vision.PlayerSpotted = true;
    }


    private void Update()
    {

        if (WanttoCheckPal)
        {
            if (Paleontology.AttackSight.StickmanIsHERE)
            {
                PlayerSpotted();
                SpottedPlayer = true;
            }
        }


        if (Watcher && !vision.PlayerSpotted)
        {
            StartCoroutine(Watchout());
        }


        float distance4glitch = Vector2.Distance(transform.position, StickmanPOS.position);


        if (Time.time >= WalkCD + LastWalkCD)
        {
            CanMove = true;
        }
        else
        {
            Geo_Anim.SetBool("Walk", false);
            CanMove = false;

        }

        if (StepsRN == HowManyTurnsBeforeBreak)
        {
            StepsRN = 0;
            CanMove = false;
            LastWalkCD = Time.time;
        }

        if (!PlayerMovement.InStealthKill && Time.time >= WalkCD + LastWalkCD)
        {
            CanMove = true;
        }


        if (Roamer && !PlayerMovement.InStealthKill && CanMove)
        {
            Geo_Anim.SetBool("Walk", true);

            if (!vision.PlayerSpotted && CanMove)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, currentPOS.position, step);

                if (Vector2.Distance(transform.position, currentPOS.position) < 2f)
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
        else if (Roamer && !PlayerMovement.InStealthKill && distance4glitch !<= 35 && CanMove)
        {
            Geo_Anim.SetBool("Walk", true);

            if (!vision.PlayerSpotted && CanMove)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, currentPOS.position, step);

                if (Vector2.Distance(transform.position, currentPOS.position) < 2f)
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



            if (HarderEnemy)
        {
            if(Time.time >= lastAttackTimeTHROW + lastAttackTime && vision.PlayerSpotted) { }
            {
                MeleeATK();
            }
        }

        if (StealthKillable && !vision.PlayerSpotted)
        {
            float distance = Vector2.Distance(transform.position, StickmanPOS.position);
            Debug.Log(distance + "distance GEO");
            if (distance <= 35 && PlayerMovement.InStealth && UIStealth != null)
            {
                if (UIStealth != null)
                {
                    UIStealth.gameObject.SetActive(true);

                }
            }
            else
            {
                if (UIStealth != null)
                {
                    UIStealth.gameObject.SetActive(false);

                }

            }
        }




        if (PlayerMovement.InStealthKill && !hasPlayedStealthKill && distance4glitch <= 35)
        {
            Debug.Log("Geo Broke");
            WantMusicWhenSpotted = false;
            AlrPlayedMusic = true;
            //Geo_Anim.SetTrigger("StealthKill");
            hasPlayedStealthKill = true;
        }

        if (!PlayerMovement.InStealthKill)
        {
            hasPlayedStealthKill = false;
        }

        if (StickStunned == true)
        {
            Music.thing.InCombat = true;
            PlayerMovement.HeCanMove = false;
        }



        if (PlayerMovement.IsStunned == false)
        {

            StickStunned = false;
            PlayerMovement.HeCanMove = false;
            CameraAnimator.SetBool("Shake", false);



        }

        if (StickStunned == false)
        {
            PlayerMovement.HeCanMove = true;

        }

        if (StickStunned == true)
        {
            CameraAnimator.SetBool("Shake", true);
        }

        if (PlayerMovement.IsStunned == true)
        {
            CameraAnimator.SetBool("Shake", true);
            PlayerMovement.HeCanMove = false;
        }


        // He ded


        if (Geo_Health <= 0 && !dead)
        {
            if (SpecialListenerForYapping)
            {
                EncounterManager.geology = null;
            }
            if (WanttoCheckPal)
            {
                if(Paleontology == null)
                {
                    AlrPlayedJingle = true;
                }
            }

            PlayerManager.PlayerHP += 1;
            PlayerManager.MegaCharge += 0.1f;
            Geo_Anim.SetTrigger("Dead");
           
        }

        //if (Geo_Health <= 0)
        //{
        //    Battle1.gameObject.SetActive(false);
        //    Battle2.gameObject.SetActive(false);

        //    if (Music.thing.InCombat)
        //    {
        //        Music.thing.InCombat = false;
        //        Music.NormalCombat = false;
        //        Music.LowHp = false;
        //        Music.NoCombat = true;
        //    }
        //    PlayerManager.InCombat = false;
        //    PlayerManager.PlayerHP = Mathf.Min(PlayerManager.PlayerHP + 20, PlayerManager.MaxHP); 
        //    GameObject effect = Instantiate(Death, GEO_POS.position, Quaternion.identity);
        //    PlayerManager.InCombat = false;
        //    PlayerMovement.IsStunned = false;
        //    PlayerMovement.StickAnim.SetBool("Stun", false);
        //    Destroy(gameObject);
        //}





        if (StickStunned)
        {
            if (Time.time >= stunStartTime + StickStunDuration)
            {
                StickStunned = false;
                PlayerMovement.HeCanMove = true;
            }
        }


        if (StickStunned == true)
        {
            InATK = true;
            MeleeATK();
        }

        if (StealthKillable)
        {
            if (vision.PlayerSpotted == true && !PlayerMovement.InStealth && !PlayerMovement.InStealthKill)
            {
                Music.thing.InCombat = true;
                PlayerSpotted();
            }
        }
        else
        {
            if (vision.PlayerSpotted == true)
            {
                Music.thing.InCombat = true;
                PlayerSpotted();
            }
        }
       
    }

    private float LastFlip;
    private bool SpottedPlayer;
    public IEnumerator Watchout()
    {
        while (!SpottedPlayer)
        {
            SpottedPlayer = vision.PlayerSpotted && !PlayerMovement.InStealth;

            if (SpottedPlayer)
                yield break;

            if (Time.time >= LastFlip + CD_For_Turns)
            {
                LastFlip = Time.time;
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }

            yield return null;
        }
    }


    public void Accelerate()
    {
        speed += 0.2f;

    }
    public void FreezePlayer()
    {
        StreakManager.streak = 0;

        AudioP.PlayOneShot(EarthQuakeATK);

        if (InATK == false)
        {
            if (PlayerMovement.Grounded == true && PlayerMovement.HeCanMove == true)
            {
                if (StickStunned)
                {
                    MeleeATK();
                }
                else
                {

                    StreakManager.streak = 0;
                    PlayerMovement.HeCanMove = false;
                    StickStunned = true;
                    PlayerManager.PlayerHP = PlayerManager.PlayerHP - 25;
                    BioManager.Hurt.gameObject.SetActive(true);


                    PlayerMovement.StickAnim.SetTrigger("Stun");
                    stunStartTime = Time.time;
                    Debug.Log("GEOLOGY ROCKS");
                }



            }
            else
            {
                

                if (PlayerMovement.Grounded == false)
                {
                    if (StreakManager.streak >= 4)
                    {
                        PlayerMovement.StickAnim.SetBool("DodgeHigh", true);
                    }
                    else
                    {
                        PlayerMovement.StickAnim.SetBool("DodgeLow", true);
                    }
                    PlayerMovement.EarthQ_Dodge = true;
                    PlayerMovement.StickAnim.SetBool("Jump", false);
                    StreakManager.streak++;
                    AudioP.PlayOneShot(StickDodge);
                    StreakManager.StreakThing();

                }



            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameObject effect = Instantiate(Death, GEO_POS.position, Quaternion.identity);
            Geo_Health = Geo_Health - 1;
        }

        if (collision.CompareTag("ArcBullet"))
        {
            GameObject effect = Instantiate(Death, GEO_POS.position, Quaternion.identity);
            Geo_Health = Geo_Health - 1.3f;
        }

        if (collision.CompareTag("StickMelee"))
        {
            GameObject effect = Instantiate(Death, GEO_POS.position, Quaternion.identity);
            Geo_Health = Geo_Health - 0.7f;
        }
    }










    public void PlayerSpotted()
    {
        if (WantMusicWhenSpotted && !PlayerMovement.InStealthKill && !PlayerMovement.InStealth)
        {
            if (!AlrPlayedMusic)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }

        Music.thing.InCombat = true;


        if (PlayerManager.PlayerHP > 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);


            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;
                Geo_Anim.SetBool("Walk", false);
            }
            else
            {
                TouchingPlayer = false;
                Geo_Anim.SetBool("Walk", true);

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


            if (TouchingPlayer)
            {
                PlayerManager.InCombat = true;
                Earthquake();
                Debug.Log("GeologyTouchStick");
            }
        }
    }





    public void Earthquake()
    {
        if (InATK == false)
        {
            if (Time.time >= lastAttackTime + EarthquakeCD)
            {
                IsEarthQuake = true;
                lastAttackTime = Time.time;
                Geo_Anim.SetTrigger("Earthquake");

            }
        }



    }


    void EndFight()
    {


        if (WantMusicWhenSpotted)
        {
            if (WanttoCheckPal)
            {
                if(Paleontology != null)
                {
                    AlrPlayedJingle = true;
                }
                else
                {
                    if (!AlrPlayedJingle)
                    {
                        AlrPlayedJingle = true;
                        AudioP.PlayOneShot(Jingle);
                        BattleTheme.gameObject.SetActive(false);
                        Hypothesis.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (!AlrPlayedJingle)
                {
                    AlrPlayedJingle = true;
                    AudioP.PlayOneShot(Jingle);
                    BattleTheme.gameObject.SetActive(false);
                    Hypothesis.gameObject.SetActive(true);
                }
            }


           


        }

        Music.GeoGlitch = true;

        Battle1.gameObject.SetActive(false);
        Battle2.gameObject.SetActive(false);

        
        if (Music.thing.InCombat)
        {
            Music.thing.InCombat = false;  
            Music.NormalCombat = false;
            Music.LowHp = false;
            Music.NoCombat = true;  
        }

        PlayerManager.InCombat = false;  
        

        GameObject effect = Instantiate(Death, GEO_POS.position, Quaternion.identity);  
        Destroy(gameObject);  

        
        PlayerMovement.IsStunned = false;
        PlayerMovement.StickAnim.SetBool("Stun", false);

       
        dead = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerMovement.InStealthKill)
            {
                HitMarker.HitMarkerSFX();
                MainCamAnim.SetBool("Zoom", false);
                Geo_Anim.SetTrigger("Dead");
                //PlayerManager.StickAnim.SetTrigger("StealthPunch");
                PlayerMovement.InStealthKill = false;
                PlayerManager.PlayerHP += 0.1f;
                GameObject effect = Instantiate(Death, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }

        }
    }
    public void MeleeATK()
    {
        if (Time.time >= lastAttackTimeTHROW + MeleeCD)
        {
            InATK = true;
            Geo_Anim.SetTrigger("RockThrow");
            lastAttackTimeTHROW = Time.time;

            StartCoroutine(ThrowRocks());
        }
    }

    private IEnumerator ThrowRocks()
    {
        for (int i = 0; i < 5; i++)
        {
            ThrowRock();
            yield return new WaitForSeconds(0.5f);
        }

        InATKFalse();
    }

    private void ThrowRock()
    {
        Vector2 direction = (player.position - ThrowPoint.position).normalized;
        GameObject projectile = Instantiate(Projectile, ThrowPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        float throwAngle = 9f;
        float angleRad = throwAngle * Mathf.Deg2Rad;
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float velocity = Mathf.Sqrt(10f * gravity / Mathf.Sin(2 * angleRad));

        float velocityX = velocity * Mathf.Cos(angleRad);
        float velocityY = velocity * Mathf.Sin(angleRad);

        rb.velocity = new Vector2(direction.x * velocityX, velocityY);
        rb.AddTorque(570f);
    }



    public void InATKFalse()
    {
        InATK = false;
    }














}
