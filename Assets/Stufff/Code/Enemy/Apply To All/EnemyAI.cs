using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// this code is for all futurte enemies after the lab chase makes so much easier to implement new mechanics
public class EnemyAI : MonoBehaviour
{
    public float speed;
    public Movement PlayerManager;
    public Health PlayerHealth;
    public Transform StickmanPOS;
    public float MinDIS;
    public bool TouchingPlayer;
    public Animator EnemyAnim;
    public Vision RangeManager;
    public float Health;
    public bool WantMusic;
    public AudioSource BattleTheme;
    public AudioSource Hypothesis;
    public AudioClip Jingle;
    private bool AlrPlayedJingle;
    private bool AlrPlayedMusic = false;
    public AudioSource AudioP;
    public GameObject Dead;
    public AttackSight MeleeAttackSight;
    public float RangedAttackRange;
    public float globaldistance;
    public float DMG_Ranged;
    public Transform AimPoint;
    public GameObject RangedProjectile;
    public float DMG_Melee;
    public float MeleeDistanceMin;
    public BoxCollider2D enemyCollider;
    public BoxCollider2D playerCollider;
    public AttackSight HeadCollider;
    public float LastKnockback;
    public bool LowStunDMG;
    public float CounterBulletSpeed;
    public GameObject CounterBullet;
    public Transform AimPointCounter;
    public AudioClip Throw;
    public Transform FX_POS;
    public AudioClip DodgeSFX;
    public float LastStunATK;
    public bool isAttacking;
    public AudioClip PunchSFX;
    public float DodgeForce;
    public AudioClip StarSpawn;
    public Rigidbody2D rb;
    public float LastRangedATK;
    public Animator StickAnim;
    public float OGSpeed;
    public float MaxSpeed;
    public float DistanceThatsTooFar;
    public Animator MainCamAnim;
    public AudioClip DodgeBookSfx;
  
    public DodgeStreaks StreakManager;
    [Header("Enemy Declare Values")]
    public bool isAstro;
    public bool isGeo;
    public bool isMeteo;
    public int EnemyID;
    public bool AbleToParry;
    public GameObject AttackIndicator;
    private float LastParryTime;
    public Slider HPSlide;
    public float StunMinDis;
    private float LastInvisiFrame;
    public HitMarker HitMarker;
    public float StunDMG;
    public bool HasHeavyAttack;
    public bool RushEventEnemy;
    public bool isScientist;
    public bool NoRanged;
    

    [Header("Meteorology")]
    public Transform cloudOffset;
    public GameObject Cloud;
    public GameObject StormCloud;

    [Header("Geology")]
    public GameObject EarthQuakeVFX;
    public Transform GeologyLegPOS;


    [Header("Astrology")]
    public GameObject Astros_Star;

    [Header("Archaeology")]
    public GameObject[] FossilList;
    public float FossilSpeed;
    public GameObject Soil_Stun;
    public AudioClip DigSFX;

    [Header("NeuroScience")]
    public GameObject BrainFX;
    public AudioClip BrainTouchSound;
    
    [Header("Physics")]
    public AudioClip AppleSfx;
    public AudioClip DeathPhy;
    public float AppleAcceleration;
    public TextMeshProUGUI PhysicsText;
    public GameObject PhysicsUI_HUD;

    [Header("Scientist")]
    public GameObject[] EyesList;
    private int RndEye = 0;
    public bool AlrPickedEyes;



    [Header("Enemy CD Values")]
    public int PlayerShootCount;
    public bool CantMeleeATK;
    public float Cooldown_For_Melee_Break;
    public float LastMeleeBreak;
    public int PlayerPunchCount;
    public float RangedCD_Dominant;
    public float MeleeCD_Dominant;
    public float RangedCD_Recesive;
    public float MeleeCD_Recesive;
    public float LastStaggered;
    public float StaggeredCD;
    public float DodgeCD;
    public float LastDodgeCDTime;
    public float CounterCD;
    public float LastCounterCDTime;
    public float MeleeCD;
    private float LastMeleeCDTime;
    public float KnockbackCD;
    public bool isDodging;
    public AudioClip StickSound;
    public float StunCD;
    public float RangedATK_CD;
    public float ParryCD;
    public float InvisiFrameCD;
    private float Tick = 0.2f;
    private float lastTickCalled;
    public bool isOneShot;
    private CameraScript CamScript;
    
    [Header("Stuff dat makes da AI better")]
    public float Threshold4RangedAfterMelee;
    private float LastMeleeThresholdCalled;
    public Transform BookDetectorRayCast;
    private EnemyAI[] enemyAIs;
    public bool AbleToStrike;
    public LayerMask WutsABook;
    public float DistanceOfRaycast;
    private float BookDirection;
    public bool isStunnable;
    

    [Header("Boss Values")]
    public bool isBoss;
    public int PhaseNum;
    private bool AlrRegenPlayer;
    public bool isEmbryology;
    public bool isEcology;
    public TimeManager TimeManager;
    private bool AlrDid_Slo_MO;
    public GameObject UI_Ripped;
    public PauseManager PauseManager;
    public String Name;
    public TextMeshProUGUI RippedName;
    public int CurrentPunches;
    public int MaxPunchesBeforeReset;
    public GameObject BossHP_UI;

    [Header("Embryology")]
    public int TimesCaught_Embry;
    public GameObject Zygote;
    public GameObject[] Zygotes;
    public GameObject StickyCell;
    public float CellWallCD;
    private float LastCellWall;
    public GameObject Wall;
    public Transform WallOffset;
    public bool NoThrow;
    public int CurrentThrowsLastPhase;
    public int MaxThrowsBeforeCharge;
    public GameObject Minion;
    public float MegaAttackCD;
    private float LastMegaAttack;
    public GameObject ShockWaveOBJ;
    public Transform ShockPoint;
    public AudioClip ShockSound;
    public float DashCD;
    public float LastDash;
    [Header("Ecology")]
    public GameObject Spore;
    public SunLogic SunManager;
    public AudioClip SolarSFX;
    public AudioSource SolarAudioP;
    public FoxSpawner FoxSpawner;
    public GameObject WaterTeleGraph;
    public Animator WaterAnimEcology;
    private float LastDebuffCloud;
    public float DebuffCloudCD;


    [Header("Tutorial Values")]

    public bool isTutorial;
    public bool isCombat;
    public bool AlrAttacked;
    public GameObject TutText;
    public string TutTXT;
    public TutorialKeyInput TutKeyInput;
    private bool AlrTaught_Tutorial_Bool;
    public bool DodgeTutorial;
    private bool AlrTaughtDodge;
    public GameObject DodgeTutVideo;

    private float IFrameTime = 0.5f;
    private float LastIFrame;

    //public int CheckPointBased;

    void Start()
    {

        if (PlayerManager.HasKilledEcology == true && isEcology)
        {
            Destroy(gameObject);
        }
        if (PhysicsUI_HUD != null)
        {
            PhysicsUI_HUD.gameObject.SetActive(false);

        }
        if ( Minion != null)
        {
            Minion.gameObject.SetActive(false);
        }
        HPSlide.maxValue = Health;
        OGSpeed = speed;
        PlayerManager.GustOfBullets = false;

        CamScript = FindObjectOfType<CameraScript>();
    }

    float nextCheckTime = 0f;
    float checkGap = 0.3f;
    private bool PlayerisCloseActivateAI;
    void FixedUpdate()
    {
        if (isBoss) return; 

        if (PlayerManager == null) return; 

        if (Time.time < nextCheckTime) return;

        float distance = (transform.position - PlayerManager.transform.position).sqrMagnitude;
        
        float minDIS = 225f;
        
        float closeDIS = 100f;
        float farDIS = 250f;

        if (distance <= closeDIS * closeDIS)
        {
            checkGap = 0.02f;

        }
        else if (distance <= minDIS * minDIS)
        {
            checkGap = 0.3f;

        }
        else if (distance <= farDIS * farDIS)
        {
            checkGap = 0.67f;

        }
        else
        {
            checkGap = 3f;

        }
        nextCheckTime = Time.time + checkGap;

        if (distance <= minDIS * minDIS)
        {
            HPSlide.gameObject.SetActive(true);
            PlayerisCloseActivateAI = true;

            if (rb.bodyType != RigidbodyType2D.Static)
            {
                rb.bodyType = RigidbodyType2D.Static;

            }

            if (EnemyAnim.cullingMode != AnimatorCullingMode.AlwaysAnimate)
            {
                EnemyAnim.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            }
        }
        else
        {
            HPSlide.gameObject.SetActive(false);
            PlayerisCloseActivateAI = false;

            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;

            }

            if (EnemyAnim.cullingMode != AnimatorCullingMode.CullCompletely)
            {
                EnemyAnim.cullingMode = AnimatorCullingMode.CullCompletely;

            }
        }


       
    }


    private bool PlayedHitRushEvent;
    public void Update()
    {

        if (!isBoss && !PlayerisCloseActivateAI) return;
        EnemyAnim.SetBool("PlayerSpotted", RangeManager.PlayerSpotted);
        
        if(isCombat && Health <= 0 || isTutorial && Health <= 0)
        {
            TutText.gameObject.SetActive(false);
        }
        if (HPSlide != null)
        {
            HPSlide.transform.rotation = Quaternion.identity;

            Vector3 scale = HPSlide.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            HPSlide.transform.localScale = scale;
        }
        if(RangeManager.PlayerSpotted)
        {
            LookAtPlayer();

        }

        if (LastRangedATK <= 0)
        {
            LastRangedATK = Time.time;
        }
        //pick eyes for scientists
        if(!AlrPickedEyes && isScientist)
        {
            AlrPickedEyes = true;
            PickEyes();
        }


        if(enemyCollider == null)
        {
            enemyCollider = GetComponent<BoxCollider2D>();
        }
        if (enemyCollider.IsTouching(playerCollider) && PlayerManager.inRush && PlayerManager.isSliding && RushEventEnemy)
        {
            Health = 0;
            if (!PlayedHitRushEvent)
            {
                PlayedHitRushEvent = true;
                HitMarker.HitMarkerSFX();

            }
        }

        if (HitMarker == null)
        {
            HitMarker = gameObject.GetComponent<HitMarker>();
        }

       
        if(PlayerHealth.PlayerHP <= 0)
        {
            Destroy(BattleTheme);
            Destroy(HPSlide);
        }
        
        
        //if(Time.time > LastDash + DashCD && isEmbryology )
        //{
        //    PlayerPastPosition = StickmanPOS.position;
        //    StartCoroutine(EmbryDash());
        //}

        if(isBoss && isEcology)
        {
           
            if(PhaseNum >= 2 && Time.time >= LastDebuffCloud + DebuffCloudCD)
            {
                LastDebuffCloud = Time.time;
                StartCoroutine(SpawnDebuffCloud_Ecology());
            }
            EcologyPhaseManager();
        }
        if(isBoss && isEmbryology)
        {

            if(globaldistance <= 10 && !TutText.gameObject.activeSelf && !AlrSlidThroughEnemy_TUT)
            {
                TutorialKeyInput tutorialKeyInput = TutText.gameObject.GetComponent<TutorialKeyInput>();
                tutorialKeyInput.InputS = KeyCode.LeftShift;
                AlrTaught_Tutorial_Bool =true;
                TutTXT = "Dash Or Slide Through Enemies To Reposition";
                TextMeshProUGUI ugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
                ugi.text = TutTXT;
                TutText.gameObject.gameObject.SetActive(true);
            }
            if (TutText.gameObject.activeSelf && PlayerManager.isSliding || TutText.gameObject.activeSelf && PlayerManager.isDashing)
            {
                Animator anim = TutText.gameObject.GetComponent<Animator>();
                anim.CrossFade("Out", 0, 0);
            }

            if (PlayerManager.isDashing)
            {
                StickyCell.gameObject.SetActive(false);
            }
            EmbryPhaseManager();
            PlayerManager.InEmbryBoss = true;
            if(PhaseNum >= 2 && Time.time >= LastCellWall + CellWallCD)
            {
                LastCellWall = Time.time;
                EmbryCellWall();
            }
            if(PhaseNum >= 3 && Time.time >= LastMegaAttack + MegaAttackCD)
            {
                isAttacking = true;
                LastMegaAttack = Time.time;
                LastStunATK = Time.time;
                LastMeleeCDTime = Time.time;
                LastRangedATK = Time.time;
                EnemyAnim.ResetTrigger("Melee");
                EnemyAnim.ResetTrigger("Ranged");
                EnemyAnim.ResetTrigger("Stun");
                EnemyAnim.SetTrigger("MegaATK");




            }

        }
        if(RangeManager.PlayerSpotted && !AlrRegenPlayer && isBoss)
        {
            if (isEcology)
            {
                FoxSpawner.CanSpawn = true;
            }
            PlayerHealth.PlayerHP = 100;
            PlayerHealth.MegaCharge = 30;
            AlrRegenPlayer = true;
            PlayerHealth.changerate = 0.05f;

        }
        AnimatorStateInfo stateInfo = EnemyAnim.GetCurrentAnimatorStateInfo(0);
        if (PlayerManager.HeCanMove == false)
        {
            speed = MaxSpeed;
        }
        HPSlide.value = Health;
        if (PlayerManager.CanCatch && AbleToParry && globaldistance <= MeleeDistanceMin)
        {
            Debug.Log("Parried");
            EnemyGotStun();
            StickAnim.SetTrigger("Caught");
            LastParryTime = Time.time;
        }
        else if (globaldistance <= MeleeDistanceMin && !PlayerManager.GustOfBullets && !CantMeleeATK && AbleToStrike)
        {
            if (Time.time >= LastMeleeCDTime + MeleeCD)
            {
               
                LastMeleeCDTime = Time.time;
                EnemyAnim.SetBool("Run", false);
                EnemyAnim.SetTrigger("Melee");
            }
        }
        if (globaldistance > MeleeDistanceMin)
        {
            EnemyAnim.SetBool("WantATK", false);

        }

        if (stateInfo.IsName("AstroDodge"))
        {
            isDodging = true;
        }
        else
        {
            isDodging = false;
        }

        if (PlayerManager.GustOfBullets && RangeManager.PlayerSpotted)
        {
            StartCoroutine(HangleStickmanUlt());
        }

    
        globaldistance = Vector2.Distance(transform.position, StickmanPOS.position);

        if (PlayerManager.isSliding && !RushEventEnemy || PlayerManager.isDashing && !PlayerManager.inRush)
        {
            //Debug.Log("fALSE COLLISION");
            AlrSlidThroughEnemy_TUT = true;
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);
        }
        else if (PlayerManager.isUppercut)
        {
            StickUpperCutManager();
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, true);

        }
        else
        {
            Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (PlayerManager.inRush && Health > 0 && RushEventEnemy)
        {
            
            rb.isKinematic = true;
            enemyCollider.isTrigger = true; 
        }
        else if(Health <= 0)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;

        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.isKinematic = false;
            enemyCollider.isTrigger = false;
        }
       
        if (RangeManager.PlayerSpotted)
        {
            if (!inDelay)
            {
                inDelay = true;
                StartCoroutine(DelayForWhenSpotted());
            }
            PlayerSpotted();
        }

        if (HeadCollider.StickmanIsHERE && !PlayerManager.inRush )
        {
            if (Time.time >= LastKnockback + KnockbackCD)
            {
                LastKnockback = Time.time;
                if (PlayerManager.Grounded)
                {
                    HeadCollider.StickmanIsHERE = false;
                    PlayerManager.StartCoroutine(PlayerManager.BackFlipMove());
                }
            }
        }

        // Make Enemy Kinda Smart

        if (PlayerShootCount > PlayerPunchCount)
        {
            RangedATK_CD = RangedCD_Dominant;
            MeleeCD = MeleeCD_Recesive;
        }
        else if (PlayerShootCount < PlayerPunchCount)
        {
            RangedATK_CD = RangedCD_Recesive;
            MeleeCD = MeleeCD_Dominant;
        }
       
        if(Time.time >= LastFairBoolEcology + FairBoolCD && !isAttacking)
        {
            EcologyFairBool = false;
        }

        if (Health <= 0)
        {
            if (isBoss && !AlrDid_Slo_MO)
            {
                AlrDid_Slo_MO = true;
                TimeManager.WantAudioSame = true;
                if(UI_Ripped != null && !UI_Ripped.gameObject.activeSelf)
                {
                    UI_Ripped.gameObject.SetActive(true);
                    RippedName.text = Name;
                    speed = 0;
                    MaxSpeed = 0;
                    //PauseManager.NoHUD = true;
                    //PauseManager.Pause();


                }
               

            }
            EnemyAnim.SetTrigger("Dead");
            EnemyAnim.CrossFade("Dead", 0, 0);
        }
        //if(PhysicsText != null)
        //{
        //    Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        //    if (targetPosition.x < transform.position.x)
        //    {
        //        //transform.localScale = new Vector3(1, 1, 1);
        //        PhysicsText.transform.localScale = new Vector3(1, 1, 1);

        //    }
        //    else if (targetPosition.x > transform.position.x)
        //    {
        //      //  transform.localScale = new Vector3(-1, 1, 1);
        //        PhysicsText.transform.localScale = new Vector3(-1, 1, 1);
        //    }
        //}
       if(globaldistance <= MeleeDistanceMin)
        {
            EnemyAnim.ResetTrigger("Ranged");
        }

        if (globaldistance >= DistanceThatsTooFar)
        {
            speed = MaxSpeed;
        }
        else
        {
            if (PlayerManager.HeCanMove)
            {
                speed = OGSpeed;


            }
        }

        // New Functions Here 
        WhatsTheDirectionBoy();
        if(Time.time >= Tick + lastTickCalled)
        {
            CheckIfOtherBooksExist();
            lastTickCalled = Time.time;

        }

    }


    public void LookAtPlayer()
    {
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        if (targetPosition.x < transform.position.x && HPSlide != null)
        {
            transform.localScale = new Vector3(1, 1, 1);
           // HPSlide.direction = Slider.Direction.LeftToRight;

        }
        else if (targetPosition.x > transform.position.x && HPSlide != null)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //HPSlide.direction = Slider.Direction.RightToLeft;
        }
        Vector3 barScale = HPSlide.transform.localScale;
        barScale.x = Mathf.Abs(barScale.x);
        HPSlide.transform.localScale = barScale;

    }
    public void CheckIfOtherBooksExist()
    {
        //Vector2 rayDirection = new Vector2(BookDirection, 0f);
        //RaycastHit2D[] BookRay = Physics2D.RaycastAll(BookDetectorRayCast.position, rayDirection, DistanceOfRaycast, WutsABook);
        //Debug.DrawRay(BookDetectorRayCast.position, rayDirection * DistanceOfRaycast, Color.gray);

        //if (BookRay.Length > 0)
        //{
        //    List<EnemyAI> BookAIs = new List<EnemyAI>();
        //    List<float> AtkTimes = new List<float>();
        //    float LongestIdleTime = Mathf.Infinity;
        //    int DatBookWhoAttacks = -1;
        //    foreach (RaycastHit2D hit in BookRay)
        //    {
        //        EnemyAI ai = hit.collider.gameObject.GetComponent<EnemyAI>();
        //        if (ai != null)
        //        {
        //            BookAIs.Add(ai);

        //            float lastMelee = ai.LastMeleeCDTime;
        //            float lastRanged = ai.LastRangedATK;
        //            float oldestTime = Mathf.Min(lastMelee, lastRanged);

        //            AtkTimes.Add(oldestTime);

        //            if (oldestTime < LongestIdleTime)
        //            {
        //                LongestIdleTime = oldestTime;
        //                DatBookWhoAttacks = BookAIs.Count - 1;
        //            }
        //        }
        //    }
        //    for (int i = 0; i < BookAIs.Count; i++)
        //    {
        //        BookAIs[i].AbleToStrike = false;
        //    }

        //    if (DatBookWhoAttacks >= 0)
        //    {
        //        BookAIs[DatBookWhoAttacks].AbleToStrike = true;
        //    }
        //    AtkTimes.Clear();
        //    BookAIs.Clear();
        //}
        //else
        //{
        //    AbleToStrike = true;
        //}
    }

    public void PickEyes()
    {
        GameObject EyePicked = EyesList[UnityEngine.Random.Range(0, EyesList.Length)];
        EyePicked.gameObject.SetActive(true);

    }

    public void WhatsTheDirectionBoy()
    {
        BookDirection = StickmanPOS.position.x < transform.position.x ? -1 : 1;

    }


    public void StickUpperCutManager()
    {
        if (Time.time >= LastInvisiFrame + InvisiFrameCD)
        {
            LastInvisiFrame = Time.time;
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            CamScript.StartCoroutine(CamScript.ShakeCam(3.5f, 0.1f));
            Health -= 0.3f;

        }
    }

    public float dashTime;
    public float dashPower;
    public float dashCD;
    public Vector2 PlayerPastPosition;
    public IEnumerator EmbryDash()
    {
        isAttacking = true;
        EnemyAnim.SetBool("Dash", true);
        float timer = 0;
        float DashTime = 0.5f;
        Vector2 targetPos = new Vector2(PlayerPastPosition.x - 10, transform.position.y);
        //if (globaldistance <= MeleeDistanceMin)
        //{
        //    MeleeAttack();
        //}
        while (timer < DashTime && Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, dashPower * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        EnemyAnim.SetBool("Dash", false);
        EnemyAnim.SetTrigger("Melee");
        LastDash = Time.time;
        isAttacking = false;    


    }
    public void FossilATK()
    {
        isAttacking = true;
        float Chance = UnityEngine.Random.Range(0f, 10f);
       
        if (PlayerHealth.PlayerHP > 0)
        {


            GameObject Fossil = FossilList[UnityEngine.Random.Range(0, FossilList.Length)];
            Debug.Log("Fossil ATK");
            Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
            GameObject projectile = Instantiate(Fossil, AimPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(AimPoint.position, StickmanPOS.position);
            float baseThrowSpeed = FossilSpeed;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = 0.01f;
            float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            rb.AddTorque(570f);
        }

    }
    private float Phase2StartHP = 70;
    private float Phase3StartHP = 45;
    public void EmbryPhaseManager()
    {
        if (Health <= Phase3StartHP)
        {
            // Phase 3
            RangedATK_CD = 0.9f;
            MeleeCD = 0.88f;
            StunCD = 534453434575676;
            PhaseNum = 3;
            //FossilSpeed = 90;
            CellWallCD = 6.5f;
            FossilSpeed = 105;

            if (NoThrow)
            {
                speed = 32;
                MaxSpeed = 32;
            }
            else
            {
                speed = 30;
                MaxSpeed = 30;
            }
        }
        else if (Health <= Phase2StartHP)
        {
            // Phase 2
            speed = 30;
            MaxSpeed = 30;
            RangedATK_CD = 3.4f;
            MeleeCD = 0.89f;
            StunCD = 534453434575676;
            PhaseNum = 2;
            FossilSpeed = 100;
            CellWallCD = 10;
            FossilSpeed = 100;
        }
        else
        {
            // Phase 1
            speed = 30;
            MaxSpeed = 30;
            RangedATK_CD = 2.5f;
            MeleeCD = 0.9f;
            StunCD = 534453434575676;
            PhaseNum = 1;
            FossilSpeed = 80;
            CellWallCD = 99999;
            FossilSpeed = 90;
        }

        if(CurrentThrowsLastPhase >= MaxThrowsBeforeCharge)
        {
            NoThrow = true;
            PlayerSpotted();
        }
    }

    public void ShockWaveATK()
    {
        EnemyAnim.ResetTrigger("Ranged");
        EnemyAnim.ResetTrigger("Stun");
        EnemyAnim.ResetTrigger("Melee");
        AudioP.PlayOneShot(ShockSound);
        GameObject ShockL = Instantiate(ShockWaveOBJ, ShockPoint.position, Quaternion.identity);
        GameObject ShockR = Instantiate(ShockWaveOBJ, ShockPoint.position, Quaternion.identity);
        Shockwave ShockLScript = ShockL.GetComponent<Shockwave>(); 
        Shockwave ShockRScript = ShockR.GetComponent<Shockwave>();
        ShockLScript.isOG = false;
        ShockRScript.isOG = false;
        ShockLScript.isRight = false;
        ShockRScript.isRight = true;


    }
    public void EmbryCellWall()
    {
        if (BookDirection > 0)
        {
            WallOffset.position = new Vector3(StickmanPOS.position.x - 5, WallOffset.position.y, WallOffset.position.z);

        }
        else if (BookDirection <= 0)
        {
            WallOffset.position = new Vector3(StickmanPOS.position.x + 5, WallOffset.position.y, WallOffset.position.z);
        }


        Vector2 spawnPosition = WallOffset.position;


        GameObject wall = Instantiate(Wall, spawnPosition, Quaternion.identity);
        GameObject SpawnFX = Instantiate(Dead, spawnPosition, Quaternion.identity);
        CellWallSummon SummonManager = wall.GetComponent<CellWallSummon>();
        SummonManager.isOg = false;

        Destroy(wall, 3f);

    }
    public void EcologyPhaseManager()
    {
        if(Health > Phase2StartHP)
        {
            speed = 30;
            MaxSpeed = 50;
            RangedATK_CD = 8;
            MeleeCD = 2;
            StunCD = 4545464563;
            PhaseNum = 1;
        }
        else if (Health <= Phase2StartHP)
        {
            if(Health <= Phase2StartHP)
            {
                speed = 50;
                MaxSpeed = 60;
                RangedATK_CD = 5.6f;
                MeleeCD = 1;
                StunCD = 10;
                PhaseNum = 2;

            }
            else if(Health <= Phase3StartHP)
            {
                speed = 60;
                MaxSpeed = 65.1f;
                RangedATK_CD = 2.3f;
                MeleeCD = 0.5f;
                StunCD = 7.5f;
                PhaseNum = 3;

            }


        }

    }
    public void EcologySpore()
    {
        LastRangedATK = Time.time;
        isAttacking = true;
        
        if (PlayerHealth.PlayerHP > 0)
        {
            Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
            GameObject projectile = Instantiate(Spore, AimPoint.position, Quaternion.identity);
            BeakerProjectile SporeManager = projectile.GetComponent<BeakerProjectile>();
            SporeManager.isOG = false;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            SporeManager.WantNewVersion = true;
            float angle = Mathf.Atan2(AimPoint.position.y - StickmanPOS.position.y, AimPoint.position.x - StickmanPOS.position.x);
            float VelocityX = Mathf.Cos(angle) * FossilSpeed;
            float VelocityY = Mathf.Sin(angle) * FossilSpeed;
            Vector3 Destination = new Vector3 (VelocityX, VelocityY);
            SporeManager.Destination = Destination;
            SporeManager.Angle = angle;
            SporeManager.Speed = FossilSpeed;
        }
        isAttacking = false;


    }

    private bool AlrTaughtEmbry;
    private string txtteaachEmbryZygoteCounter = "Counter RED Projectiles [R]";

    public void Embry_Zygote()
    {
        if (TimesCaught_Embry < 5)
        {
            TutorialKeyInput tutorialKeyInput = TutText.gameObject.GetComponent<TutorialKeyInput>();
            tutorialKeyInput.InputS = KeyCode.R;
            AlrTaughtEmbry = true;
            TextMeshProUGUI ugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
            ugi.text = txtteaachEmbryZygoteCounter;
            TutText.gameObject.SetActive(true);
           
        }
        if(PhaseNum == 3)
        {
            CurrentThrowsLastPhase += 1;
        }
        LastRangedATK = Time.time;
        
        if (PlayerHealth.PlayerHP > 0)
        {
            GameObject projectile = null;

            if (PhaseNum >= 2)
            {
                GameObject Rndprojectile = Zygotes[UnityEngine.Random.Range(0, Zygotes.Length)];
                Debug.Log("RND PROJECTILE CALC" + Rndprojectile.gameObject.name);
                projectile = ObjectPooler.SpawnObj(Rndprojectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);

            }
            else
            {
                projectile = ObjectPooler.SpawnObj(Zygote, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);

            }
            BeakerProjectile Script = projectile.GetComponent<BeakerProjectile>();
            Script.isOG = false;
            Script.PerfectDodge = false;    
            Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(AimPoint.position, StickmanPOS.position);
            float baseThrowSpeed = FossilSpeed;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = UnityEngine.Random.Range(0.4f, 0.9f);
            float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            rb.AddTorque(570f);

        }
        isAttacking = false;


    }

    public IEnumerator SpawnDebuffCloud_Ecology()
    {
        WaterTeleGraph.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.2f);
        WaterTeleGraph.gameObject.SetActive(false);
        //Forces Player To Move to make the fight less stale
        if (transform.localScale == new Vector3(1, 1, 1))
        {
            cloudOffset.position = new Vector3(StickmanPOS.position.x - 5, cloudOffset.position.y, cloudOffset.position.z);
        }
        else
        {
            cloudOffset.position = new Vector3(StickmanPOS.position.x + 5, cloudOffset.position.y, cloudOffset.position.z);
        }
        WaterAnimEcology.Play("1");
        Vector2 spawnPosition = cloudOffset.position;
        GameObject cloud = ObjectPooler.SpawnObj(Cloud, spawnPosition, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        DebuffCloud CloudScript = cloud.GetComponent<DebuffCloud>();
        CloudScript.AlrPickedRnd = false;
        CloudScript.PickIt();
        CloudScript.isOG = false;
        LastDebuffCloud = Time.time;
    }

    public void Scientist_Throw()
    {
        if (PhaseNum == 3)
        {
            CurrentThrowsLastPhase += 1;
        }
        LastRangedATK = Time.time;

        if (PlayerHealth.PlayerHP > 0)
        {

            GameObject projectile;
            projectile = ObjectPooler.SpawnObj(RangedProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
            BeakerProjectile Script = projectile.GetComponent<BeakerProjectile>();
            Script.isOG = false;
            Script.PerfectDodge = false;
            Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(AimPoint.position, StickmanPOS.position);
            float baseThrowSpeed = FossilSpeed;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = UnityEngine.Random.Range(0.4f, 0.9f);
            float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            rb.AddTorque(570f);

        }
        isAttacking = false;


    }

    public void PlayThrowSfx()
    {
        AudioP.PlayOneShot(Throw);
    }
    public void PlayAppleSfx()
    {
        AudioP.PlayOneShot(AppleSfx);
    }
    public void PlayerSpotted()
    {

        //EnemyAnim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        if (WantMusic && !PlayerManager.InStealthKill && !PlayerManager.InStealth && !PlayerManager.GustOfBullets && PlayerHealth.PlayerHP > 0)
        {
            if (!BattleTheme.gameObject.activeSelf && Health > 0 && !PlayerManager.GustOfBullets && !PlayerManager.isInPerfectDodge && !PlayerManager.BreakIn)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }



            if (!AlrPlayedMusic && !PlayerManager.GustOfBullets && !PlayerManager.BreakIn)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }


        if (globaldistance <= MeleeDistanceMin && !PlayerManager.GustOfBullets && !CantMeleeATK && AbleToStrike)
        {
            if (Time.time >= LastMeleeCDTime + MeleeCD)
            {
                
                isAttacking = true;
                LastMeleeCDTime = Time.time;
                EnemyAnim.SetTrigger("Melee");
            }
        }
        if (Time.time >= LastRangedATK + RangedATK_CD && !PlayerManager.GustOfBullets  && AbleToStrike && !NoThrow || Time.time >= LastRangedATK + RangedATK_CD && PlayerManager.Checkpoint == PlayerManager.ExamHeliID_Checkpoint)
        {
            isAttacking = true;
            RangedAttack();
        }
        if (Time.time >= StunCD + LastStunATK && !PlayerManager.GustOfBullets && globaldistance <= StunMinDis && PlayerHealth.MegaCharge >= 3.2f && AbleToStrike)
        {
            if (isAstro && !PlayerManager.GustOfBullets)
            {
                LastStunATK = Time.time;
                EnemyAnim.SetTrigger("Stun");
            }
            else if (isGeo && !PlayerManager.GustOfBullets)
            {
                LastStunATK = Time.time;
                EnemyAnim.SetTrigger("Stun");
            }
            else if (isMeteo && !PlayerManager.GustOfBullets && Time.time >= StunCD + LastStunATK && !PlayerManager.GustOfBullets && globaldistance <= StunMinDis)
            {
                LastStunATK = Time.time;
                EnemyAnim.SetTrigger("Stun");

            }
            else
            {
                
                LastStunATK = Time.time;
                EnemyAnim.SetTrigger("Stun");
            }
        }





        if (PlayerHealth.PlayerHP > 0 && speed > 1 && !isAttacking && !PlayerManager.GustOfBullets)
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


               
            }



        }
    }

    public void AnimationKey_isNotAbleToBeStunned()
    {
        isStunnable = false;
    }

    public void AnimationKey_isAbleToBeStunned()
    {
        isStunnable = true;
    }
    public void AnimationKey_PreventGettingStunAgain()
    {
        EnemyAnim.ResetTrigger("Hurt");

    }
    private bool StopSloMeAgain;
    public void OnDeath()
    {
        EnemyAnim.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.isKinematic = true;
        enemyCollider.enabled = false;
        if (!PlayerManager.inRush && !RushEventEnemy && PlayerManager.Checkpoint != PlayerManager.ExamHeliID_Checkpoint)
        {
            PlayerManager.TimeManager.FreezeTime(0.09f);

        }
        if (TimeManager != null)
        {
            TimeManager.WantAudioSame = false;
        }
        if (isEmbryology)
        {
            HitMarker.HitMarkerSFX();
            PlayerManager.HasKilledEmbry = true;
            PlayerManager.SaveProgress();
        }
        PlayerManager.InEmbryBoss = false;
        
        // Remove Player current Debuffs
        PlayerManager.AbleToJump = true;
        PlayerManager.HeCanMove = true;
        speed = 0;
        MaxSpeed = 0;
        float rndHPValue = UnityEngine.Random.Range(4.2f, 7.5f);
        float rndStaminaValue = UnityEngine.Random.Range(1.2f, 2.5f);

        PlayerHealth.PlayerHP += rndHPValue;
        PlayerHealth.MegaCharge += rndStaminaValue;

        if (WantMusic && !PlayerManager.GustOfBullets && !isBoss && !PlayerManager.BreakIn)
        {
            BattleTheme.gameObject.SetActive(false);

            if (!AlrPlayedJingle && !isBoss)
            {
                AlrPlayedJingle = true;
                AudioP.PlayOneShot(Jingle);

            }

        }
        if (isEcology)
        {
            
            FoxSpawner.CanSpawn = false;
        }
        if (!PlayerManager.BreakIn && WantMusic)
        {
            Hypothesis.gameObject.SetActive(true);

        }
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        enemyAI.enabled = false;
        if (!RushEventEnemy)
        {
            Destroy(gameObject);

        }
        else
        {
            EnemyAnim.enabled = false;
        }
        GameObject effect = Instantiate(Dead, FX_POS.position, Quaternion.identity);

    }


    public void AnimationKey_ParryableAttack()
    {
        if (Time.time >= LastParryTime + ParryCD)
        {
            AttackIndicator.gameObject.SetActive(true);
            AbleToParry = true;
            Debug.Log("Parryable Attack");
        }
        else
        {
            return;
        }
    }

    private float delayforAttack = 2.5f;
    private bool inDelay;
    public IEnumerator DelayForWhenSpotted()
    {
        float currentDelayCounter = 0;
        while(currentDelayCounter < delayforAttack)
        {
            AbleToStrike = false;
            currentDelayCounter += Time.deltaTime;
            yield return null;
        }
        AbleToStrike = true;
    }
    public void AnimationKey_IncomingAttack()
    {
        AttackIndicator.gameObject.SetActive(true);

    }

    public void AnimationKey_HideIndicater()
    {
        AttackIndicator.gameObject.SetActive(false);
    }


    private int StunCount;
    private float LastEnemStun;
    private float EnemyStunCD = 1.2f;
    public void EnemyGotStun()
    {
        EnemyAnim.SetTrigger("Hurt");




    }
    public void AnimationKey_FalseParry()
    {
        AttackIndicator.gameObject.SetActive(false);
        AbleToParry = false;

    }

    private bool PhysicsGlitch;
    private float EmbryGlitchCD = 1.2f;
    private float LastEmbryGlitch;
    public void AnimationKey_NeuroScience_BrainTouch_SFX()
    {
        AudioP.PlayOneShot(StickSound);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
       
        if (collision.CompareTag("Boom") || (collision.CompareTag("ObjectOfDestruction")))
        {
            Health = 0;
        }
        if (collision.CompareTag("Bullet") )
        {
            if (!RangeManager.PlayerSpotted)
            {
                RangeManager.PlayerSpotted = true;
            }
            PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(58.5f, 0.2f));
            Bullet bulletScript = collision.GetComponent<Bullet>();
            GameObject effect = Instantiate(Dead, transform.position, Quaternion.identity);
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            CamScript.StartCoroutine(CamScript.ShakeCam(3.5f, 0.1f));
            int rndNum = UnityEngine.Random.Range(0, 3);
            PlayerShootCount += 1;
            if (Time.time >= LastCounterCDTime + CounterCD && !bulletScript.isEnemyCounter)
            {
                if (isOneShot)
                {
                    Health = 0;
                }
                if (!isBoss)
                {

                    Health -= 0.9f;

                }
                else
                {
                    Health -= 1f;
                }
                Counter();


            }
            else
            {
                if (!isBoss)
                {
                    Health -= 1f;

                }
                else
                {
                    if (bulletScript.isEnemyCounter)
                    {
                        PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.2f));
                        Health -= 3.5f;

                    }
                    else
                    {
                        Health -= 2f;

                    }
                }
                PlayerHealth.MegaCharge += 0.1f;

            }
            Destroy(collision.gameObject);
            HitMarker.HitMarkerSFX();
            
        }
        if (collision.CompareTag("CounterBullet"))
        {
            PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.2f));

            GameObject effect = Instantiate(Dead, transform.position, Quaternion.identity);
            CamScript.StartCoroutine(CamScript.ShakeCam(3.5f, 0.1f));
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            if (!isBoss)
            {
                Health -= 1.8f;

            }
            else
            {
                Health -= 3.5f;

            }
            Destroy(collision.gameObject);
            HitMarker.HitMarkerSFX();
            if (PlayerHealth.Testing)
            {
                Health = 0;
            }
        }
        //if (collision.CompareTag("Player"))
        //{
        //    if (RushEventEnemy)
        //    {
        //        Health = 0;
        //        HitMarker.HitMarkerSFX();
        //    }
        //}
        if (collision.CompareTag("Spec") && isEmbryology && Time.time >= LastEmbryGlitch + EmbryGlitchCD)
        {
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            LastEmbryGlitch = Time.time;
            Health -= 3.98f;
        }
        if (collision.CompareTag("StickMelee") && !PlayerManager.isUppercut)
        {
            if(DodgeTutorial && !AlrTaughtDodge)
            {
                AlrTaughtDodge = true;
                DodgeTutVideo.gameObject.SetActive(true);
            }
            if (!RangeManager.PlayerSpotted)
            {
                RangeManager.PlayerSpotted = true;
            }
            
            PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(48.5f, 0.2f));
            
            if (PlayerManager.isInPerfectDodge)
            {
                PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(78.5f, 0.2f));
                PlayerManager.PowerfulPunch = false;
                PlayerManager.PerfectDodgeSloMO.gameObject.SetActive(false);
                PlayerManager.TimeManager.RevertSpeed();
                if(Time.time >= LastIFrame + IFrameTime)
                {
                    Health -= 17;
                    LastIFrame = Time.time;

                }
            }
            PlayerPunchCount += 1;
            PlayerManager.CurrentPunches += 1;
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            LastRangedATK = Time.time;
            int rndNum = UnityEngine.Random.Range(0, 2);
            PlayerManager.EnemyRecivedHit = true;
            if (Time.time >= Cooldown_For_Melee_Break + LastMeleeBreak && globaldistance <= MeleeDistanceMin && isStunnable)
            {
                EnemyAnim.SetTrigger("Flinch");
                LastMeleeBreak = Time.time;
                Health -= 0.12f;
                PlayerHealth.MegaCharge += 0.23f;
                //LastMeleeCDTime = Time.time;
                //LastRangedATK = Time.time;
                //LastStunATK = Time.time;
                //LastCounterCDTime = Time.time;
            }
            else
            {
                if (isBoss && !isEmbryology)
                {
                    Health -= 1.3f;
                }
                else if (isEmbryology)
                {
                    Health -= 0.3f;
                }
                else
                {
                    Health -= 0.34f;

                }
                if (Health - 0.34f <= 0 && !isBoss)
                {
                    StickAnim.SetTrigger("Finish");
                    Debug.Log("Finish Him or her ig");
                }
                if (isBoss)
                {
                    PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(49.5f, 0.2f));
                    PlayerHealth.MegaCharge += 2.3f;

                }
                else
                {
                    PlayerHealth.MegaCharge += 3.2f;

                }

            }

        }

    }

    public void AcidRain()
    {
        if (transform.localScale == new Vector3(1, 1, 1))
        {
            cloudOffset.position = new Vector3(StickmanPOS.position.x - 30, cloudOffset.position.y, cloudOffset.position.z);
        }
        else
        {
            cloudOffset.position = new Vector3(StickmanPOS.position.x + 30, cloudOffset.position.y, cloudOffset.position.z);
        }
        Vector2 spawnPosition = cloudOffset.position;
        GameObject cloud = ObjectPooler.SpawnObj(Cloud, spawnPosition, Quaternion.identity);
        AcidRain CloudScript = cloud.GetComponent<AcidRain>();
        CloudScript.isOG = false;
        LastRangedATK = Time.time;
    }


    private float StormCloudDelay = 1;
    private float LastCloudSpawn;

    public void StormCloudStun()
    {
        if(Time.time >= StormCloudDelay + LastCloudSpawn)
        {
            LastCloudSpawn = Time.time;
            if (transform.localScale == new Vector3(1, 1, 1))
            {
                cloudOffset.position = new Vector3(StickmanPOS.position.x - 5, cloudOffset.position.y, cloudOffset.position.z);
            }
            else
            {
                cloudOffset.position = new Vector3(StickmanPOS.position.x + 5, cloudOffset.position.y, cloudOffset.position.z);
            }
            cloudOffset.position = new Vector3(StickmanPOS.position.x, cloudOffset.position.y, cloudOffset.position.z);
            LastStunATK = Time.time;
            Vector2 spawnPosition = cloudOffset.position;
            GameObject cloud = ObjectPooler.SpawnObj(StormCloud, spawnPosition, Quaternion.identity);
            StormCloud CloudScript = cloud.GetComponent<StormCloud>();
            EnemyAI ScriptToAssign = GetComponent<EnemyAI>();
            CloudScript.IsOG = false;
            CloudScript.EnemyReciever = ScriptToAssign;
            LastRangedATK = Time.time;
        }
       
    }

    private bool TuffDodge;
    public void YOOO_Tuff_Dodge()
    {
        if(PlayerManager.BackFlip && globaldistance <= MeleeDistanceMin + 4.3f && !isBoss)
        {
            TuffDodge = true;
            AudioP.PlayOneShot(PlayerManager.PerfectDodge);
            PlayerHealth.MegaCharge += 1.5f;
            PlayerManager.PerfectDodgeCount += 2;
            TuffDodge = false;

        }
        else if (PlayerManager.BackFlip && globaldistance <= MeleeDistanceMin + 5.1f && isBoss)
        {
            TuffDodge = true;
            AudioP.PlayOneShot(PlayerManager.PerfectDodge);
            PlayerManager.TimeManager.FreezeTime(0.1f);
            PlayerHealth.MegaCharge += 3.5f;
            PlayerManager.PerfectDodgeCount += 1;
            TuffDodge = false;


        }
        else if(PlayerManager.BackFlip && globaldistance > MeleeDistanceMin + 4.3f)
        {
            AudioP.PlayOneShot(DodgeSFX);

        }
        else if(PlayerManager.BackFlip && globaldistance > MeleeDistanceMin + 5.1f && isBoss)
        {
            AudioP.PlayOneShot(DodgeSFX);

        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ObjectOfDestruction") && !RushEventEnemy)
        {
            Health = 0;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.isDashing)
            {
                Health -= 0.1f;
                HitMarker.HitMarkerSFX();
            }
            if (RushEventEnemy && PlayerManager.isSliding)
            {
                Health = 0;
                HitMarker.HitMarkerSFX();

            }



        }
        
        if (collision.gameObject.CompareTag("Cart") && !isBoss)
        {
            Health = -1;
            OnDeath();
        }
    }

    
    


    public void HandleDashDMG()
    {
        Health -= 0.1f;
        HitMarker.HitMarkerSFX();
    }
    public IEnumerator HangleStickmanUlt()
    {
        CounterCD = 434;
        RangedATK_CD = 434;
        MeleeCD = 434;
        DodgeCD = 434;
        speed = 0;
        KnockbackCD = 434;
        StunCD = 434;

        while (PlayerManager.GustOfBullets)
        {
            Health = 100;
            yield return null;
        }

        Health = 0;
        BattleTheme.gameObject.SetActive(false);
        PlayerManager.GustOfBullets = false;
        AlrPlayedJingle = true;
    }

  
    public void RangedAttack()
    {
        if(isEmbryology && !NoThrow)
        {
            EnemyAnim.SetTrigger("Ranged");

        }
        else if(isEmbryology && NoThrow)
        {
            // Embryology Dash
            PlayerSpotted();
        }
        else if(NoRanged)
        {

            isAttacking = false;
            return;
        }
        else
        {
            EnemyAnim.SetTrigger("Ranged");

        }
    }


    public void KnockPlayer()
    {
        if (Time.time >= LastMeleeCDTime + MeleeCD)
        {
            LastMeleeCDTime = Time.time;

            PlayerManager.StartCoroutine(PlayerManager.BackFlipMove());
        }

    }
    public void Astro_Stun()
    {

        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
       
        AudioP.pitch = UnityEngine.Random.Range(1, 1.3f);
        AudioP.PlayOneShot(Throw);
        AudioP.PlayOneShot(StarSpawn);
        Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
        GameObject starObj = Instantiate(Astros_Star, AimPoint.position, Quaternion.identity);
        Astros_Star star = starObj.GetComponent<Astros_Star>();
        EnemyAI Astro = GetComponent<EnemyAI>();
        star.AstrologyReciver = Astro;
        float angle = Mathf.Atan2(direction.y, direction.x);
        star.SetDirection(direction, angle);
        star.HomeToPlayer();
        star.OG = false;
        
    }
    public void Meteo_Stun()
    {
        StickAnim.ResetTrigger("Shoot");
        StickAnim.SetBool("Walking ", false);
        StickAnim.SetBool("Jump ", false);
        Debug.Log("Stun Function Being Called");
        StickAnim.SetTrigger("Stun");
        StreakManager.streak = 0;
        PlayerManager.HeCanMove = false;
        PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;
        LastStunATK = Time.time;
        AudioP.PlayOneShot(StickSound);
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

        // look at him (stick)
       

    }
    public void Geo_Stun()
    {
        GameObject effect = ObjectPooler.SpawnObj(EarthQuakeVFX, GeologyLegPOS.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

        // look at him (stick)
        


        if (PlayerManager.Grounded)
        {
            StreakManager.streak = 0;
            PlayerManager.HeCanMove = false;
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;

            AudioP.PlayOneShot(StickSound);
            StickAnim.SetTrigger("Stun");
            Debug.Log("Astro got him");
        }
        else
        {
            if (PlayerManager.Grounded == false)
            {

                PlayerManager.EarthQ_Dodge = true;
                StickAnim.SetBool("Jump", false);
                StreakManager.streak++;
                AudioP.PlayOneShot(DodgeSFX);
                StreakManager.StreakThing();

            }
        }
    }

    public void Embry_Stun()
    {

        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        // look at him (stick)
        
        if (PlayerManager.Grounded)
        {
            StreakManager.streak = 0;
            StickyCell.gameObject.SetActive(true);
            PlayerManager.HeCanMove = false;
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;
            AudioP.PlayOneShot(StickSound);
            StickAnim.SetTrigger("Stun");
        }
        else
        {
            if (PlayerManager.Grounded == false)
            {
                PlayerManager.EarthQ_Dodge = true;
                StickAnim.SetBool("Jump", false);
                StreakManager.streak++;
                AudioP.PlayOneShot(DodgeSFX);
                StreakManager.StreakThing();

            }
        }
    }
    public void NeuroStun()
    {


        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

        // look at him (stick)
       


        if (PlayerManager.Grounded)
        {
            BrainFX.gameObject.SetActive(true);
            StreakManager.streak = 0;
            PlayerManager.HeCanMove = false;
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;

            AudioP.PlayOneShot(StickSound);
            StickAnim.SetTrigger("Stun");
        }
        else
        {
            if (PlayerManager.Grounded == false)
            {

                PlayerManager.EarthQ_Dodge = true;
                StickAnim.SetBool("Jump", false);
                StreakManager.streak++;
                AudioP.PlayOneShot(DodgeSFX);
                StreakManager.StreakThing();

            }
        }
    }
    //Used To Be Paleontology But who the heck knows what that is Even I Didnt even know it was a thing
    public void Archaeology_Stun()
    {
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        float ProjectileSpeed = 150;
        GameObject Soil = Instantiate(Soil_Stun, AimPoint.position, Quaternion.identity);
        float angle = Mathf.Atan2(AimPoint.position.y - StickmanPOS.position.y, AimPoint.position.x - StickmanPOS.position.x);
        float X = Mathf.Cos(angle) * ProjectileSpeed;
        float Y = Mathf.Sin(angle) * ProjectileSpeed;
        Soil_Projectile SoilScript = Soil.GetComponent<Soil_Projectile>();
        SoilScript.GoToPlayer(angle, X, Y);
        EnemyAI enemyAI = GetComponent<EnemyAI>();
        SoilScript.EnemyReciver = enemyAI;
        SoilScript.isOG = false;
    }

    private bool EcologyFairBool;
    private float LastFairBoolEcology;
    private float FairBoolCD = 3;
    public void SunRay_Ecology_Manager()
    {
        isAttacking = true;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        if(PlayerManager.BackFlip || PlayerManager.isDashing || PlayerManager.isSliding)
        {
            EcologyFairBool = true;
        }

        if(!PlayerManager.isDashing && !PlayerManager.BackFlip && !PlayerManager.isSliding && !EcologyFairBool)
        {
            StreakManager.streak = 0;
            PlayerManager.HeCanMove = false;
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;
            PlayerHealth.MegaCharge -= 4;


            AudioP.PlayOneShot(StickSound);
            StickAnim.SetTrigger("Stun");
        }
        else
        {
            EcologyFairBool = true;
        }
        
    }

    public void AnimationKey_JingleSFX()
    { //Only USe This For Boss
        TimeManager.SloMO();
        AudioP.PlayOneShot(Jingle);
        BattleTheme.gameObject.SetActive(false);
        if (isEcology)
        {
            PlayerManager.HasKilledEcology = true;
            PlayerManager.SaveProgress();
        }
    }
    public void Physics_Stun()
    {
        LastStunATK = Time.time;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        if (PlayerManager.Grounded)
        {
            StreakManager.streak = 0;
            PlayerManager.HeCanMove = false;
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - StunDMG;
            AudioP.PlayOneShot(StickSound);
            StickAnim.SetTrigger("Stun");
        }
        else
        {
            if (PlayerManager.Grounded == false)
            {

                PlayerManager.EarthQ_Dodge = true;
                StreakManager.streak++;
                AudioP.PlayOneShot(DodgeSFX);
                StreakManager.StreakThing();

            }
        }
    }
    public void PhysicsUI_Instantiate()
    {
        StartCoroutine(PhyiscsUI());

    }
    public IEnumerator PhyiscsUI()
    {
        PhysicsUI_HUD.gameObject.SetActive(true);

        // Physics UI Thing
        //F = MA
        bool Grounded;
        if (PlayerManager.Grounded)
        {
            Grounded = true;
        }
        
        PhysicsText.text = ("F = MA");
        yield return new WaitForSecondsRealtime(0.25f);
        PhysicsText.text = ("F = MA");
        yield return new WaitForSecondsRealtime(0.2f);
        PhysicsText.text = ("F = (102 LBS) * A"); 
        yield return new WaitForSecondsRealtime(0.1f);
        if (PlayerManager.Grounded)
        {
            PhysicsText.text = ("F = (102 LBS) * 0");

        }
        else
        {
           PhysicsText.text = ("F = (102 LBS) * 1"); 



        }
        yield return new WaitForSecondsRealtime(0.1f);
        if(PlayerManager.Grounded)
        {
            PhysicsText.text = ("= 0");
            Vector2 Accelerat = new Vector2(0, 0);
            PlayerManager.rb.AddForce(Accelerat * PlayerManager.rb.mass);
            StickAnim.SetTrigger("Stun");

        }
        else
        {
            int RoundStickSpeed = (int)PlayerManager.MaxSpeed;
            PhysicsText.text = ("=" + RoundStickSpeed);


        }
        yield return new WaitForSecondsRealtime(1);
        PhysicsUI_HUD.gameObject.SetActive(false);



    }

    public void Archaeology_Stun_Manager()
    {
        StreakManager.streak = 0;
        PlayerManager.HeCanMove = false;
        LastStunATK = Time.time;
        AudioP.PlayOneShot(StickSound);
        StickAnim.SetTrigger("Stun");
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

    }


    public void EcologyStun()
    {
        isAttacking = true;

        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        SunManager.RotateTowardStick();
    }
    public void AnimationKey_Archae_Dig_SFX()
    {
        AudioP.PlayOneShot(DigSFX);

    }

    public void AnimationKey_Archae_Throw_SFX()
    {
        AudioP.PlayOneShot(Throw);
    }
    public void Animation_Key_DodgeFall()
    {
        isDodging = false;
    }

    public void AnimationKey_PlaySolarBlastSFX()
    {
        SolarAudioP.PlayOneShot(SolarSFX);
    }
    public void DodgeSFXPlay()
    {
        if(globaldistance <= 70)
        {
            AudioP.PlayOneShot(DodgeBookSfx);

        }
    }
    public void MeleeAttack()
    {
        // This is for the Tutorial in Chapter 0
        if (isCombat && !PlayerManager.BackFlip)
        {
            TextMeshProUGUI txugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
            txugi.text = TutTXT;
            TutKeyInput.InputS = KeyCode.Q;
            TutText.gameObject.SetActive(true);
            TutKeyInput.FreezeTime = true;
            TutTXT = "Backflip[Q] To Dodge Attacks";


        }
        else if (isCombat)
        {
            TextMeshProUGUI txugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
            txugi.text = TutTXT;
            TutKeyInput.InputS = KeyCode.Q;
            TutText.gameObject.SetActive(true);
            TutKeyInput.FreezeTime = false;
            TutTXT = "Backflip[Q] To Dodge Attacks";

        }
        

        // if (!AlrAttacked && isCombat)
        // {
        //     StartCoroutine(TutorialManager());
        //     AlrAttacked = true;
        // }
        if (!isBoss)
        {
            StartCoroutine(Initiate_ATK_After_Melee_If_Needed());
            
        }
        if(isEmbryology && PhaseNum == 3)
        {
            NoThrow = false;
            CurrentThrowsLastPhase = 0;
        }

        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        if (!PlayerManager.BackFlip  && !PlayerManager.isDashing && globaldistance <= MeleeDistanceMin)
        {
            if(PlayerManager.isSliding && PlayerManager.SlideCancelSpeed < 100)
            {
                return;
            }
            else
            {
                EnemyAnim.SetBool("Run", false);
                if (isBoss && !PlayerManager.Dead)
                {
                    PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.5f));
                    PlayerManager.StickAnim.CrossFade("Stick_Mega_Hurt", 0, 0);
                }
                if (HasHeavyAttack)
                {
                    PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(65.5f, 0.3f));
                    PlayerManager.StickAnim.CrossFade("Stick_Mega_Hurt", 0, 0);


                }
                else
                {
                    PlayerManager.CameraScript.StartCoroutine(PlayerManager.CameraScript.ShakeCamGroundFeeling(45.5f, 0.5f));

                }
                //DMG_Melee = UnityEngine.Random.Range(13, 20);
                MainCamAnim.ResetTrigger("ShakeMelee");
                MainCamAnim.SetTrigger("ShakeMelee");
                PlayerHealth.PlayerHP -= DMG_Melee;
                if (!isBoss && !HasHeavyAttack)
                {
                    StickAnim.SetTrigger("Hurt");

                }

                Debug.Log("Player hit for " + DMG_Melee);
                if (globaldistance <= MeleeDistanceMin && !isBoss)
                {
                    EnemyAnim.SetBool("WantATK", true);
                    EnemyAnim.SetTrigger("FollowUp");
                }
                else if(!isBoss)
                {
                    EnemyAnim.SetBool("WantATK", false);
                }
            }

           
        }
        isAttacking = false;

    }


    public IEnumerator Initiate_ATK_After_Melee_If_Needed()
    {
        float threshold = 0.6f;
        float timer = 0;

        while(timer < threshold)
        {
            if (PlayerManager.BackFlip)
            {
                Debug.Log("Yeah we got under control Boi");
                EnemyAnim.SetTrigger("Ranged");
                yield return null;
            }
            else if (PlayerManager.isSliding)
            {
                EnemyAnim.SetTrigger("Stun");
                yield return null;

            }
            else
            {
                yield return null;

            }
            yield return null;
            timer += 0.1f;
        }
        
    }

    public void PhysicsThrowStickman()
    {
        if (PlayerManager.Grounded )
        {
            if (PlayerHealth.PlayerHP > 0)
            {
                PlayerHealth.PlayerHP -= DMG_Melee;
                AudioP.PlayOneShot(Throw);
                Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

                if (targetPosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (targetPosition.x > transform.position.x)
                {


                    KnockPlayer(StickmanPOS);
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
        float upforce = 1f;
        float horizKnockbackForce = 7f;

        if (PlayerManager != null)
        {
            StartCoroutine(StickFall());
            StartCoroutine(PlayerManager.KnockBackAction(
                new Vector2(hitDirection.x * horizKnockbackForce, upforce),
                Vector2.zero,
                0f
            ));
        }

    }
    public IEnumerator StickFall()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerManager.StickAnim.SetBool("Fall", true);
        PlayerManager.Grounded = false;
    }
    public void AnimationKey_PlayPunchSFX()
    {
        if (!PlayerManager.BackFlip && !PlayerManager.isSliding && !PlayerManager.isDashing && globaldistance <= MeleeDistanceMin)
        {
            if (!TuffDodge)
            {
                AudioP.PlayOneShot(PunchSFX);

            }

        }
        else
        {
            if (!TuffDodge)
            {
                DodgeSFXPlay();


            }

        }
    }
    public void AnimationKey_isAttackingFalse()
    {
        isAttacking = false;
    }
    public IEnumerator Dodge()
    {
        if (PlayerHealth.PlayerHP > 0)
        {
            MainCamAnim.ResetTrigger("ShakeMelee");
            MainCamAnim.SetTrigger("ShakeMelee");
            CamScript.StartCoroutine(CamScript.ShakeCam(3.5f, 0.1f));
            float horizontalMomentum = 20;
            float flipTime = 0.1f;
            float elapsedTime = 0f;


            while (flipTime > elapsedTime)
            {


                if (transform.position.x >= 0)
                {



                    rb.rotation += 1;

                    rb.velocity = new Vector2(-Mathf.Abs(-horizontalMomentum), rb.velocity.y);
                }
                else
                {
                    rb.rotation -= 1;
                    rb.velocity = new Vector2(Mathf.Abs(horizontalMomentum), rb.velocity.y);
                }



                elapsedTime += Time.deltaTime;

                yield return null;

            }
            rb.velocity = new Vector2(transform.localScale.x * (-1 * (horizontalMomentum)), DodgeForce);

            LastDodgeCDTime = Time.time;
            Debug.Log("Dodge");
            EnemyAnim.SetTrigger("Dodge");
            rb.rotation = 0f;
        }
        else
        {
            yield return null;
        }


    }

    public void Counter()
    {
       
        if (Time.time >= LastCounterCDTime + CounterCD)
        {
            LastCounterCDTime = Time.time;
            Debug.Log("Counter");
            EnemyAnim.SetTrigger("Counter");
        }
        else
        {
            return;
        }

    }


    public bool RepeatedTip;
    private bool AlrSlidThroughEnemy_TUT = false;
    public void ThrowATK()
    {


        // THIS IS THE COUNTER ATTACK IM ALSO TRASH AT NAMING STUFF
        if (PlayerHealth.PlayerHP > 0)
        {
            if (isTutorial && !AlrTaught_Tutorial_Bool && !TutText.gameObject.activeSelf && !RepeatedTip)
            {

                AlrTaught_Tutorial_Bool = true;
                TutTXT = "Counter[R]";
                TutText.gameObject.SetActive(true);
                TutKeyInput.SloMo = true;
                TutKeyInput.InputS = KeyCode.R;
                TextMeshProUGUI ugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
                ugi.text = TutTXT;
            }
            else if (RepeatedTip && isTutorial && !TutText.gameObject.activeSelf)
            {
                TutTXT = "Counter[R]";
                TutText.gameObject.SetActive(true);
                TutKeyInput.SloMo = true;
                TutKeyInput.InputS = KeyCode.R;
                TextMeshProUGUI ugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
                ugi.text = TutTXT;
            }
            Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
            // look at him (stick)
            
            AudioP.pitch = UnityEngine.Random.Range(1, 1.3f);
            AudioP.PlayOneShot(Throw);

            Vector2 direction = (StickmanPOS.position - AimPointCounter.position).normalized;
            GameObject projectile = ObjectPooler.SpawnObj(CounterBullet, AimPointCounter.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
            BeakerProjectile projectileScript = projectile.GetComponent<BeakerProjectile>();
            projectileScript.WantCollisionCD = true;
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(AimPointCounter.position, StickmanPOS.position);
            float baseThrowSpeed = CounterBulletSpeed;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = UnityEngine.Random.Range(0.4f, 0.9f);
            float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            ///rb.AddTorque(570f);
        }
    }
    //

    private float AstroMultipleBlackHolesGlitchFix = 0.5f;
    private float LastGlitchAstro;
    public void Astro_RangedATK()
    {
        if(Time.time >=  AstroMultipleBlackHolesGlitchFix + LastGlitchAstro)
        {
            LastGlitchAstro = Time.time;
            isAttacking = true;
            Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);


            Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;

            GameObject blackhole = ObjectPooler.SpawnObj(RangedProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);

            Blackhole blackholeScript = blackhole.GetComponent<Blackhole>();
            if (blackholeScript != null)
            {
                float directionValue = transform.localScale.x > 0 ? -1 : 1;
                blackholeScript.SettingDirection(directionValue);
            }
        }
        

    }

    public void PhysicsGruntBeforeDeath()
    {
        AudioP.PlayOneShot(DeathPhy);
    }

    public void Neuro_RangedATK()
    {
        LastRangedATK = Time.time;
        isAttacking = true;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

       
        Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;

        GameObject Neuro = ObjectPooler.SpawnObj(RangedProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);

        Blackhole blackholeScript = Neuro.GetComponent<Blackhole>();
        if (blackholeScript != null)
        {
            float directionValue = transform.localScale.x > 0 ? -1 : 1;
            blackholeScript.SettingDirection(directionValue);
        }

    }
    public void Physics_RangedATK()
    {
        isAttacking = true;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
        
        Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;
        GameObject Apple = ObjectPooler.SpawnObj(RangedProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        float Direc = transform.localScale.x > 0 ? -1 : 1;
        Blackhole AppleScript = Apple.GetComponent<Blackhole>();
        AppleScript.isApple = true;
        AppleScript.AppleAcceleration.x = AppleAcceleration * Direc;

    }

    public void Animation_Key_Want_A_Extra_Gap_For_Ranged_CD()
    {
        LastRangedATK = Time.time;

    }


    public void Geo_RangedATK()
    {
        isAttacking = true;
        Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);

       
        Vector2 direction = (StickmanPOS.position - AimPoint.position).normalized;

        GameObject blackhole = ObjectPooler.SpawnObj(RangedProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);

        Blackhole blackholeScript = blackhole.GetComponent<Blackhole>();
        if (blackholeScript != null)
        {
            if (transform.localScale.x > 0)
            {
                blackholeScript.SettingDirection(-1);
            }
            else
            {
                blackholeScript.SettingDirection(1);
            }
        }

    }

    
    public IEnumerator TutorialManager()
    {
        if (isCombat)
        {

            TextMeshProUGUI txugi = TutText.gameObject.GetComponent<TextMeshProUGUI>();
            txugi.text = TutTXT;
            TutKeyInput.InputS = KeyCode.Q;
            TutText.gameObject.SetActive(true);
            yield return null;




        }
    }

}
