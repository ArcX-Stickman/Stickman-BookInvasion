using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Embryology : MonoBehaviour
{
    public PieceOfCrap it_pmo;
    public AttackSight AttackSight;
    public float speed;
    public Movement PlayerManager;
    public Health StickHP;
    public Transform PlayerPOS;
    public bool TouchingPlayer;
    public float MinDIS;
    public Slider HpSlider;
    public float MeleeDMG;
    public Transform AimPoint;
    public GameObject Projectile;
    public float ProjectileSpeed;
    public RangeSight MeleeSight;
    public RangeSight ThrowSight;
    public AudioSource AudioP;
    public AudioClip PunchSFX;
    public DodgeStreaks SteakManager;
    public AudioClip Miss;
    public float HP;
    public GameObject FX;
    public Transform EmbryPOS;
    public AudioSource EmbryBossTheme;
    public Animator Book_Anim;
    public GameObject Hypothesis;
    public bool InAttack;
    public AudioSource SecondAudioP;
    public AudioClip StickSound;
    public GameObject FakeProjectile;
    public AudioClip PunchMiss;
    public AudioClip Throw;
    public float LastMelee;
    public float MeleeCD;
    public Image HPBAR;
    public GameObject UI_New;
    public float ThrowCD;
    public float LastThrow;

    public float ShieldCD;
    public float LastShielded;
    public Transform GelSac;
    public GameObject GelSacGameobject;
    public float StunCD;
    public float LastStunATK;
    public float WallCD;
    public float LastWall;
    public GameObject Wall;
    public bool FacingRight;
    public Transform WallOffset;
    public bool StickStunned;
    public float stunStarttime;
    public AudioClip DeathSound;
    public AudioSource EndAudioP;
    public bool InTips;
    public Collider2D enemyCollider;
    public Collider2D playerCollider;
    //Phases
    public bool Phase1;
    public bool Phase2;
    public bool CanSummonWalls;
    public bool Phase3;
    public GameObject TipsHUD;
    public TextMeshProUGUI TextTips;
    public Animator TipsAnim;
    public PauseManager PauseManager;
    public int ThrowCount4Phase3;
    
    // Start is called before the first frame update
    void Start()
    {

        CanSummonWalls = false;
        HpSlider.maxValue = HP;
        ApplyPhaseOne();
        Hypothesis.gameObject.SetActive(false);
        EmbryBossTheme.gameObject.SetActive(true);
        PlayerSpotted();
        PlayerManager.InEmbryBoss = true;
    }




    private bool PopupTipFirstTime;

    // Update is called once per frame
    void Update()
    {



        // ignore colision 
        if (PlayerManager.isSliding | PlayerManager.isDashing)
        {
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
        }
        else
        {
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
        }


        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (InTips)
            {

                it_pmo.CanBe = false;
                StopCoroutine(BossFight());
                SkipTip();
            }
            
        }

        if (Phase2 && !CanSummonWalls && HP <= 70)
        {
            CanSummonWalls = true;
        }


        if (!PopupTipFirstTime)
        {
            StartCoroutine(WaitABit());
            
        }




        UpdatePhase();

        


        if(HP  <= 0)
        {
            PlayerManager.HasKilledEmbry = true;
            PlayerManager.SaveProgress();
            Book_Anim.SetTrigger("Dead");
        }
        
        




        PlayerSpotted();

        if (PlayerManager.HeCanMove)
        {
            GelSacGameobject.gameObject.SetActive(false);

        }

        HpSlider.value = HP;

        



        if (MeleeSight.StickHere)
        {
            if(Time.time >= MeleeCD + LastMelee)
            {
                LastMelee = Time.time;
                InAttack = true;
                Book_Anim.SetTrigger("Melee");
            }

        }
        else if (Time.time >= ThrowCD + LastThrow && !InAttack && !NoThrow)
        {
            InAttack = true;
            LastThrow = Time.time;
            Book_Anim.SetTrigger("Throw");
        }

        else if (Time.time >= StunCD + LastStunATK && !InAttack && !NoThrow)
        {
            if(Phase2 | Phase3)
            {
                InAttack = true;
                LastStunATK = Time.time;
                Book_Anim.SetTrigger("StunATK");
            }
            


        }
        else if (Time.time >= WallCD + LastWall && !InAttack && !NoThrow && CanSummonWalls)
        {

            if (Phase2 | Phase3)
            {
                InAttack = true;
                
                Book_Anim.SetTrigger("Wall");
            }

        }


    }

    public IEnumerator WaitABit()
    {
        yield return new WaitForSecondsRealtime(1f);
        PauseManager.NoHUD = true;
        PopupTipFirstTime = true;
        UI_New.SetActive(true);
        PauseManager.Pause();
        
    }
    
    public void WallSummon()
    {
        if (FacingRight)
        {
            WallOffset.position = new Vector3(PlayerPOS.position.x - 5, WallOffset.position.y, WallOffset.position.z);

        }
        else if (!FacingRight)
        {
            WallOffset.position = new Vector3(PlayerPOS.position.x + 5, WallOffset.position.y, WallOffset.position.z);
        }


        Vector2 spawnPosition = WallOffset.position;


        GameObject wall = Instantiate(Wall, spawnPosition, Quaternion.identity);
        GameObject SpawnFX = Instantiate(FX, spawnPosition, Quaternion.identity);


        Destroy(wall, 2f);


        LastWall = Time.time;
    }



    public IEnumerator BossFight()
    {
        InTips = true;
        
        yield return new WaitForSecondsRealtime(3);
        PauseManager.NoHUD = true;
        it_pmo.CanBe = true;

        PauseManager.Pause();
        InTips = true;
        PopupTipFirstTime = true;
        TipsHUD.gameObject.SetActive(true);
        TextTips.text = "Woah, This Is A Boss Fight";
        yield return new WaitForSecondsRealtime(5);
        TextTips.text = "These Guys Have More Health";
        yield return new WaitForSecondsRealtime(5);
        TextTips.text = "Instead Of Punching";
        yield return new WaitForSecondsRealtime(3);
        TextTips.text = "Counter Attack By Pressing [R]";
        yield return new WaitForSecondsRealtime(3);
        TextTips.text = "You Should Be Ready Now";
        yield return new WaitForSecondsRealtime(3);
        PauseManager.NoHUD = false;
        PauseManager.Resume();
        PlayerManager.HasAlreadySeenEmbryTips = true;
        TipsAnim.SetTrigger("GoBack");
        it_pmo.CanBe = false;
        InTips = false;


    }

    public void SkipTip()
    {
        InTips = false;
        TipsAnim.SetTrigger("GoBack");
        TextTips.text = "";
        PauseManager.NoHUD = false;
        PauseManager.Resume();
    }

    public void UpdatePhase()
    {
        if (Phase1 && HP <= 90)
        {
            Phase1 = false;
            Phase2 = true;
            ApplyPhaseTwo();
        }
        else if (Phase2 && HP <= 50)
        {
            Phase2 = false;
            Phase3 = true;
            ApplyPhaseThree();
        }
    }

    public void ApplyPhaseOne()
    {
        Phase1 = true;
        Debug.Log("We In Phase 1");
        ProjectileSpeed = 72.8f;
        MeleeCD = 1.3f;
        ThrowCD = 4.37f;
        speed = 33.07f;
    }

    public void ApplyPhaseTwo()
    {
        Debug.Log("Now In Phase 2");
        ProjectileSpeed = 80.3f;
        MeleeCD = 0.5f;
        ThrowCD = 2.6f;
        speed = 40f;
        StunCD = 3;
        WallCD = 2.3f;

    }

    public void ApplyPhaseThree()
    {
        Debug.Log("We In Phase 3");


        if (NoThrow)
        {
            Book_Anim.speed = 1f;

        }
        else
        {
            Book_Anim.speed = 1.6f;
        }


        
        ProjectileSpeed = 90.3f;
        MeleeCD = 0.1f;
        ThrowCD = 0.4f;
        StunCD = 1;
        WallCD = 1.3f;
        if (NoThrow)
        {
            speed = 109.7f;
        }
        else
        {
            speed = 55.7f;
        }

        

    }




    public void StunAttack()
    {
        if (PlayerManager.Grounded == true && PlayerManager.HeCanMove == true && !PlayerManager.IsInThrowAnim)
        {
            GelSacGameobject.gameObject.SetActive(true);
            //GelSac.position = PlayerPOS.position;
            SteakManager.streak = 0;
            PlayerManager.HeCanMove = false;
            StickStunned = true;
            StickHP.PlayerHP = StickHP.PlayerHP - 9.5f;

            AudioP.PlayOneShot(StickSound);
            PlayerManager.StickAnim.SetTrigger("Stun");
            stunStarttime = Time.time;
            Debug.Log("Embry got him");



        }
        else
        {


            if (PlayerManager.Grounded == false)
            {
               
                PlayerManager.EarthQ_Dodge = true;
                PlayerManager.StickAnim.SetBool("Jump", false);
                SteakManager.streak++;
                AudioP.PlayOneShot(Miss);
                SteakManager.StreakThing();

            }



        }
    }





    


   

    public void PlayFanfare()
    {
        Destroy(EmbryBossTheme);
        EndAudioP.PlayOneShot(DeathSound);
    }

    public void ByeBye()
    {
        StartCoroutine(EndOfFight());
    }

    public IEnumerator EndOfFight()
    {
        InTips = true;
        yield return new WaitForSecondsRealtime(3);
        PauseManager.NoHUD = true;
        PauseManager.Pause();
        InTips = true;
        PopupTipFirstTime = true;
        TipsHUD.gameObject.SetActive(true);
        TextTips.text = "And Thats How You Do It";
        yield return new WaitForSecondsRealtime(5);
        PauseManager.NoHUD = false;
        PauseManager.Resume();
        PauseManager.Hypothesis.gameObject.SetActive(true);
        TipsAnim.SetTrigger("GoBack");
        Destroy(gameObject);

        InTips = false;
    }



    public bool NoThrow;

    public void ThrowATK()
    {
        if (StickHP.PlayerHP > 0 && !NoThrow)
        {


            if (Phase3)
            {
                ThrowCount4Phase3 += 1;
                if(ThrowCount4Phase3 >= 2)
                {
                    NoThrow = true;
                    PlayerSpotted();
                }
            }

            if (Phase2 | Phase3 && !NoThrow)
            {
                InAttack = true;
                LastStunATK = Time.time;
                Book_Anim.SetTrigger("StunATK");
            }

            if (Phase2 | Phase3 && !NoThrow && CanSummonWalls)
            {
                InAttack = true;

                Book_Anim.SetTrigger("Wall");
            }


            if (MeleeSight.StickHere)
            {
                if (Time.time >= MeleeCD + LastMelee)
                {
                    Book_Anim.ResetTrigger("Throw");
                    Book_Anim.SetTrigger("Melee");
                }

            }


            Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
            
            
            
            // look at him (stick)
            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }


            AudioP.pitch = UnityEngine.Random.Range(1, 1.3f);
            AudioP.PlayOneShot(Throw);

            Vector2 direction = (PlayerPOS.position - AimPoint.position).normalized;
            GameObject projectile = Instantiate(Projectile, AimPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(AimPoint.position, PlayerPOS.position);
            float baseThrowSpeed = ProjectileSpeed;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = Random.Range(0.4f, 0.9f);
            float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            rb.AddTorque(570f);


                        // Old Code Was Good But top is better
            //Vector2 direction = (PlayerPOS.position - AimPoint.position).normalized;
            //GameObject projectile = Instantiate(Projectile, AimPoint.position, Quaternion.identity);
            //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //float angle = Mathf.Atan2(direction.y, direction.x);
            //float velocityX = Mathf.Cos(angle) * speed * 2;
            //float velocityY = Mathf.Sin(angle) * speed / 1.4f;
            //rb.velocity = new Vector2(velocityX, velocityY);
            //rb.AddTorque(570f);
        }
    }

    public void FakeThrowATK()
    {
        if (StickHP.PlayerHP > 0)
        {




            if (MeleeSight.StickHere)
            {
                if (Time.time >= MeleeCD + LastMelee && !NoThrow)
                {
                    Book_Anim.ResetTrigger("Throw");
                    Book_Anim.SetTrigger("Melee");
                }

            }


            Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }


            AudioP.pitch = UnityEngine.Random.Range(1, 1.3f);
            AudioP.PlayOneShot(Throw);

            Vector2 direction = (PlayerPOS.position - AimPoint.position).normalized;


            GameObject projectile = Instantiate(FakeProjectile, AimPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();


            float enemySpeed = rb.velocity.x;


            float distance = Vector2.Distance(AimPoint.position, PlayerPOS.position);


            float baseThrowSpeed = ProjectileSpeed - 14;


            float velocityX = direction.x * baseThrowSpeed + enemySpeed;


            float arcHeight = 0.5f;
            // u = sqrt(2 * as)
            // formula gives you the velocity u needed for 
            // sum to reach a height s when it's moving under accceleration
            float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));


            rb.velocity = new Vector2(velocityX, velocityY);


            rb.AddTorque(570f);
        }
    }

    public void MeleeAttack()
    {
        if (MeleeSight.StickHere)
        {
            NoThrow = false;
            ThrowCount4Phase3 = 0;
            if (PlayerManager.BackFlip)
            {
                StickHP.PlayerHP +=  1.1f;
                AudioP.PlayOneShot(PunchMiss);
                SecondAudioP.PlayOneShot(Miss);
                SteakManager.StreakThing();
            }
            else if (PlayerManager.isDashing | PlayerManager.isSliding | PlayerManager.Grounded)
            {
                StickHP.PlayerHP += 0.1f;
                AudioP.PlayOneShot(PunchMiss);
                
                
            }
            else
            {
                StickHP.PlayerHP -= 10.5f;
                AudioP.pitch = UnityEngine.Random.Range(1, 1.3f);
                AudioP.PlayOneShot(PunchSFX);
                PlayerManager.StickAnim.SetTrigger("Hurt");
            }

            
        }
        else
        {
            ThrowCount4Phase3 = 0;

            NoThrow = false;
            StickHP.PlayerHP += 3;
            AudioP.PlayOneShot(PunchMiss);
            SteakManager.StreakThing();
        }

    }







    public void PlayerSpotted()
    {
        Hypothesis.gameObject.SetActive(false);


        if (StickHP.PlayerHP > 0 && !PlayerManager.Hiding && !InAttack)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, PlayerPOS.position);
            Debug.Log("Distance from Player:" + distanceToPlayer);

            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;
                Book_Anim.SetBool("Walk", false);
            }
            else
            {
                Book_Anim.SetBool("Walk", true);
                
                
                Vector2 targetPosition = new Vector2(PlayerPOS.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                if (targetPosition.x < transform.position.x)
                {
                    FacingRight = true;

                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (targetPosition.x > transform.position.x)
                {
                    FacingRight = false;

                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (TouchingPlayer)
            {
                StickHP.InCombat = true;

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee"))
        {
            if (StickHP.Testing)
            {
                HP -= 10;
            }
            else
            {
                HP -= 1f;
            }
               
            GameObject effect = Instantiate(FX, EmbryPOS.position, Quaternion.identity);

        }
    }




    public void AttackDecider()
    {
         
        
    }



    public void InAttackANIM_BOOL()
    {
        InAttack = false;

    }

}
