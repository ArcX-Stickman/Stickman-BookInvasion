using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Movement : MonoBehaviour
{
    public float Speed;
    public float JumpForce;
    public Rigidbody2D rb;
    public Animator StickAnim;

    public bool Grounded;
    public bool CallJumpDEBUG;

    public GameObject MuzzleFire;
    public Sprite[] MuzzleSprites;
    public AudioClip Gun;
    public AudioSource AudioPlayer;
    public GameObject GunBullet;
    public GameObject MegaBullet;
    public GameObject GunBulletArc;
    public Transform AimPoint;
    public Transform ArcAimPoint;
    public Transform StickPOSGlitch;
    public Volume PostProces;
    public MotionBlur MB;
    public Health StickHP;
    public float MaxSpeed;
    public bool JumpDodge;
    public AudioClip BassDrop;
    public GameObject PitchBlack;
    public GameObject Graphix;
    public bool HeCanMove;
    public bool GraphixOn;
    public bool AbleToJump;
    public Transform StickPOS;
    public bool EarthQ;
    public bool EarthQ_Dodge;
    public bool Dead;
    public AudioClip Fail;
    public AudioSource GunAudio;
    private float resetYPosition = -2.338124f;
    public bool IsShooting;
    public bool IsFalling;
    public MotionBlur motionBlurHandler;
    public MeleeComboSpeed MeleeCollision;
    public BoxCollider2D StickCollider;
    public bool SlideJump;
    public bool IsStunned;
    //Movement
    [Header("Movement ")]

    public float Acceleration;
    public float Deacceleration;
    public bool isJumping;
    public Biology_AttackZone Streak_Bio;
    public float shootCooldown = 0.5f;
    private float nextShootTime = 0f;
    public bool isShooting;
    [Header("Sliding ")]

    //Slide Cancel Code
    public float SlideCancelTime;
    public float SlideCancelSpeed;
    public float cancelCD;
    public float SlideGravity;
    public bool canSlide;
    public bool canCancel;
    private float slideTime = 0;
    private float cancelTime = 0;
    public bool isSliding;
    public float MaxSlideSpeed;
    public bool SlideVolume;
    public float SlideMomentum;
    public bool SlideVolumeEnd;
    public bool BuildUp;
    public bool SlopeSlide;
    public AudioClip SlideSFX;
    //Animation States
    public bool InSlideAnimation;
    //Slope Check and Collider Code
    private BoxCollider2D BoxC;
    private Vector2 ColliderSize;
    private float SlopeCheckDistance;
    public LayerMask WhatsGround;
    //Dash Code
    private bool canDash = true;
    public bool isDashing;
    public float dashTime;
    public float dashPower;
    public float dashCD;
    public AudioClip DashSFX;
    //KnockBack Code
    public float knockbackTime = 0.2f;
    public float hitdirectionforce = 10f;
    public float constantForce = 5f;
    public float inputForce;
    private Coroutine Knockbackcoroutine;
    public bool StickmanGetsKnocked { get; private set; }
    //Knockback Values
    //Knockback time: 0.62
    //Hit Direction force 13.07
    // constant force 8.6
    //Bools From books
    public bool BiologyThrow;

    //Hiding Mechanics
    public bool Hiding;
    public bool CanHide;
    public bool TouchHiding;
    public bool TouchedCollider;
    // Misc
    public bool FirstClear;
    [Header("BackFlip WHoooo")]
    // BackFlip WHoooo
    public bool BackFlip;
    public float BackFlipRotation;
    public float BackFlipForce;
    private float LastBackFlip;
    public float BackFlipCD;
    public AudioClip BackFlipSFX;
    public AudioSource BackFlipAS;
    public float LastBackflipTime;
    public bool AirBackFlip;
    public bool TriggerBackflipMovement;
    [Header("Melee Stuff ")]
    // Melee I guess
    public AudioSource PunchAudioP;
    public AudioClip[] PunchSFX;
    public BoxCollider2D MeleeCollider;
    public string punchAnimationName = "Punch";
    public float PunchspeedIncrease = 0.2f;
    public float PunchdecayRate = 0.5f;
    private float defaultSpeed = 1f;
    private float currentSpeed;
    private float lastPunch;
    public float resetPunchSpeedTime = 2f;
    public float MaxPunchSpeed;
    public bool PunchBool;
    public float PunchAnimationSpeed;
    public bool CanCombo;
    public float MeleeCD;
    private float lastPunchTime;
    public float ComboCount;
    public int CurrentPunches;
    public int MaxPunches;
    public float Maxdelay4Punch;
    private float OG_MeleeCD;
    public bool isUppercut;
    public GameObject RaycastableObj;
    public float RayDistance;
    public LayerMask WhatsTheBookLayer;
    public bool isInPunchAttach;
    private float LastAttachFreeFlow;
    public float FreeFlowAttachCD;
    public float AttachSpeed;
    public AudioClip MegaPunch;
    public AudioClip RopeShift;

    [Header("Progression Values ")]
    // Progression Values
    public string SceneName;
    public bool HasGun;
    public bool BreakIn;
    public bool AlreadyPickedGun;
    public bool CanDoublePunch;
    public int Checkpoint;
    public bool HasKilledEmbry;
    public bool HasKilledEcology;
    public bool HasAlreadySeenEmbryTips;
    public bool HasAlreadySeenEmbryTipsGlitchPreventer;
    public bool FinshedEncounterAfterLab;
    public bool HasBrokeOutOfRope;
    public LoadingSystem LoadingSystem;
    
    [Header("Progression For Abilites ")]
    public bool UnlockedBackflip;
    public bool UnlockedDash;
    public bool UnlockedSlide;


    [Header("PerkTree Values ")]
    public int XP_Orbs;


    // Cutscene Progression
    public bool Cutscene_IntroFight;
    public bool NeedsToViewDirecFirstIntro;
    public bool HasFullyWatchedFirst;
    public bool SeenGunDirections;
    [Header("PaperCollectables")]
    // Values For PaperCollectables
    public bool BackflipCollected;
    public bool InfoCollected;
    public bool NoMoreCollected;

    [Header(" Lab Stuff ")]
    public int ExamHeliID_Checkpoint;
    public GameObject LabLevel;
    public GameObject LabBG_SPRITE;
    public GameObject Cart;
    public Vector2 AccelerationCart;
    public Transform Point4Cart;
    public float MinDIS4Cart;
    public float DistanceFromCartPoint;
    [Header(" HeliChase Stuff ")]
    public Transform HeliLadder;
    public Transform UpLadder;
    public Transform DownLadder;
    public Transform HeliCinematicClimbDestination;
    public Transform InHeliPoS;
    public float MinDisHeliCineClimbDes;
    public bool HasFullyClimbed;
    public bool InHeliCineBeatdown;
    [Header(" Forest Heli Chase Stuff ")]
    public GameObject Engine;

    // Footsteps
    public bool WalkOnGrass;
    public AudioSource FootP;
    public AudioSource FootPMetal;
    public AudioClip GrassFootSFX;
    public bool WalkOnWood;
    public AudioSource WoodP;
    [Header("Countering IG ")]
    // Countering IG
    public bool IsInThrowAnim;
    public bool HasCaught;
    public GameObject EmbryProjectile;
    public Transform EmbryPOS;
    public bool CanCatch;
    public bool StickPlayerWannaCatch;
    public Transform CounterAimPoint;
    public AudioClip ThrowSFX;
    public GameObject ExamBullet;
    public Transform Helicopter;
    public float catchWindow;

    //Encounters
    public bool InEmbryBoss;
    public bool InBIO_Boss;
    public bool InScienceBoss;
    public bool InExamBoss;
    [Header("Stealth Kill")]
    //Stealth Kill
    public bool InStealth;
    public bool InStealthKill;
    public Animator MainCamAnim;
    public AudioClip FallingSFX;
    public AudioClip PunchSFXStealth;
    [Header("Gust Of Bullets (Ult)")]
    public bool GustOfBullets;
    public AudioSource Hypothesis;
    public AudioSource BattleTheme;
    public AudioSource UltTheme;
    public AudioSource LabTheme;
    public float UltCD;
    public float LastULT;
    public PlayableDirector UltCam;
    // jump stuff coyote time
    private float coyoteTime = 0.07f;
    private float coyoteTimerCounter;

    private bool AlrPlayedDeath;
    public AudioClip DeathSFX;
    public AudioClip PerfectDodge;

    [Header("Perfect Dodge")]
    public AudioSource PerfectDodgeSloMO;
    public bool isInPerfectDodge;
    public int PerfectDodgeCount;
    public TimeManager TimeManager;
    public Slider LockedInMeterSlider;
    public int MaxDodge;
    public float SloMORate;
    public bool isActive;
    public Image LockedInThunder;
    public bool LockedIN;
    public AudioClip WeAreLockedIn_SFX_ACTIVATE;
    public CameraScript CameraScript;
    public AudioClip GaugeFull;
    public GameObject Particle4LockingIn;
    public Animator LockedThunderAnim;
    public bool MegaHeal;
    public AudioClip OrbSpawn;
    public bool AuraSigmaRizzyDodge;

    [Header("DebuggingValues")]
    public float DistanceToBook = 0;
    public bool isVoid;
    public float JumpCD;
    public float LastJump;
    public float timeBecameAirborne;
    public float jumpAnimationHelper = 0.15f;
    public bool HeliFinale;
    public float DashDisDebug;
    public GameObject HeliForestCheck;
    [Header("Cinematic Values")]
    public float CineMaticSpeed;
    public float MaxCineMaticSpeed;
    public bool InCinematic;
    public float CinematicSlideSpeed;
    public float MaxCinematicSlideSpeed;

    public bool inQTE;
    
    [Header(" FX and Graphical Enhancements ")]
    public ParticleSystem JumpSmoke;
    public ParticleSystem SlideSmoke;
    public ParticleSystem GrassBlades;
    public bool PlayerReadyForImpactFrame;
    public GameObject GroundImpactFX;
    public GameObject GroundImpactFXSmoke;
    public ParticleSystem DashSmoke;

    [Header(" Gravity Stuff With RigidBody and stuff ")]

    public float FallingGravityMAX;

    public float FallGravity;
    private float OG_FallGravity;
    private float OG_NormGravity;

    [Header(" Misc ")]
    //Performance level destoryer
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level1Grass;
    public GameObject BloodFX;
    public bool Anim_Locked;
    public PhysicsMaterial2D BouncyMat;
    public PhysicsMaterial2D StickmanMat;
    public GameObject TutTXT;
    public BoxCollider2D AlternateColliderDuringKnockback;
    
    public Vector3 SafePosition;


    [Header("Misc SFX")]
    public AudioClip GroundImpact;
    public AudioClip LandSFX;
    public AudioClip FootstepsStart;


    void Start()
    {

        OG_NormGravity = rb.gravityScale;

        AlternateColliderDuringKnockback.enabled = false;

        Grounded = true;
        Jump();
        if (Level1 != null && Level2 != null)
        {
            Level1.gameObject.SetActive(true);

            Level2.gameObject.SetActive(true);
        }

        StickCollider = GetComponent<BoxCollider2D>();

        LastULT = -UltCD;


        OG_MeleeCD = MeleeCD;

        StickHP.MegaCharge = 30;

        if (AlreadyPickedGun)
        {
            HasGun = true;
        }
        else
        {
            HasGun = false;

        }


        BoxC = GetComponent<BoxCollider2D>();

        ColliderSize = BoxC.size;

        Time.timeScale = 1;


        HeCanMove = true;
        Dead = false;
        IsStunned = false;
        AbleToJump = true;

        PitchBlack.gameObject.SetActive(false);

        //Speed = MaxSpeed;

        Vector3 currentPosition = transform.position;

        // Set the Y axis to 0, keeping X and Z unchanged
        currentPosition.y = 0;


        rb = GetComponent<Rigidbody2D>();
        StickAnim = GetComponent<Animator>();
    }

    private void OnValidate()
    {

        Acceleration = (50 * Acceleration) / MaxSpeed;
        Deacceleration = (50 * Deacceleration) / MaxSpeed;

        #region Variable Ranges
        Acceleration = Mathf.Clamp(Acceleration, 0.01f, MaxSpeed);
        Deacceleration = Mathf.Clamp(Deacceleration, 0.01f, MaxSpeed);
        #endregion

    }

    public void AURA_CLIPFARM_RIZZY_DodgeBoolAura()  // bro how did i come up with this name 
    {
        AudioPlayer.PlayOneShot(BackFlipSFX);
        if (!AuraSigmaRizzyDodge)
        {
            AuraSigmaRizzyDodge = true;

        }
        else
        {
            AuraSigmaRizzyDodge = false;

        }
    }




    private bool AlrDid4DeathCor;

    public IEnumerator KnockBackAction(Vector2 hitdirection, Vector2 constantForceDirection, float inputDirection)
    {
        StickmanGetsKnocked = true;
        hitdirection = new Vector2(hitdirection.x, -Mathf.Abs(hitdirection.y));
        constantForceDirection = new Vector2(constantForceDirection.x, -Mathf.Abs(constantForceDirection.y));
        Vector2 _hitForce = hitdirection * hitdirectionforce;
        Vector2 _constantForce = constantForceDirection * constantForce;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            elapsedTime += Time.fixedDeltaTime;


            Vector2 _knockbackForce = _hitForce + _constantForce;
            Vector2 _combinedForce = new Vector2(_knockbackForce.x, Mathf.Min(_knockbackForce.y, 0));

            if (inputDirection != 0)
            {
                _combinedForce += new Vector2(inputDirection * inputForce, 0f);
            }

            rb.velocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        StickAnim.SetBool("Fall", false);


        rb.gravityScale = originalGravity;
        StickmanGetsKnocked = false;
    }
    private bool AlrAttaching;
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    void FixedUpdate()
    {

        // Manage The Jump Gravity
        ManageJumpGravity();
        PhysicsManagerManual();

        if (Input.GetKey(KeyCode.F4) && Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Debugging Save");
            SaveProgress();
        }


        if (Grounded && StickAnim.GetBool("Jump"))
        {
            if (timeBecameAirborne != 0f && Time.time >= timeBecameAirborne + jumpAnimationHelper)
            {
                StickAnim.SetBool("Jump", false);
            }
            else if (timeBecameAirborne == 0f)
            {
                StickAnim.SetBool("Jump", false);
                StickAnim.SetBool("Airborne", false);

            }
        }
        else if (!Grounded && !BackFlip)
        {
            if (rb.velocity.y > 0.1f)
            {
                StickAnim.SetBool("Jump",true);
                StickAnim.SetBool("Airborne", false);

            }
            else if (rb.velocity.y < -0.1f && !BackFlip && !isDashing && !Shooting && !PunchBool && !Grounded)
            {
                StickAnim.SetBool("Airborne", true);
                StickAnim.SetBool("Jump", false);

            }
            else
            {
                StickAnim.SetBool("Airborne", false);

            }
        }

        SlopeCheck();
        if (Cart.gameObject.activeSelf)
        {
            SupaFast_Cart_BreakIn();
        }
        //if(rb.constraints == RigidbodyConstraints2D.None)
        //{
        //    if (Grounded)
        //        rb.rotation = Mathf.Lerp(rb.rotation, 0f, Time.fixedDeltaTime * 70f);

        //}

        if (HeliForestCheck != null || InCinematic)
        {
            if (MaxSpeed <= CineMaticSpeed)
            {
                Speed = CineMaticSpeed;
                MaxSpeed = CineMaticSpeed;
            }
            if (MaxSpeed >= MaxCineMaticSpeed)
            {
                MaxSpeed = MaxCineMaticSpeed;
            }
            if (MaxSlideSpeed <= CinematicSlideSpeed)
            {
                MaxSlideSpeed = CinematicSlideSpeed;

            }
            if (SlideCancelSpeed <= MaxCinematicSlideSpeed)
            {
                SlideCancelSpeed = MaxCinematicSlideSpeed;
            }
            // rb.gravityScale = 15;
        }
        if (OnLadder && !HasFullyClimbed && !NeverDoCineBeatDownAgain)
        {
            StartCoroutine(ClimbHeliCinematic());
        }

        if (InHeliCineBeatdown && !NeverDoCineBeatDownAgain && HasFullyClimbed)
        {
            HeliCineBeatDownManager();
        }

        if (!Falling_KnockBack && rb.sharedMaterial == BouncyMat)
        {
            rb.sharedMaterial = StickmanMat;
        }


        DistanceToGroundRefer = GetDistanceToGround();

    }
    private bool AlrDid4Reload;
    public bool FacingRight;
    private bool PlayedLockedIn_CompletedGauge;


    private void Update()
    {
        AbilityInputHandler();
        if (hingeJoint2D == null)
        {
            hingeJoint2D = GetComponent<HingeJoint2D>();
        }
        if (OnLadder && !isFallingHeli && HeliFinale)
        {
            StickAnim.SetBool("Heli", true);
            StickAnim.SetBool("Dash", false);
            HeliManager();
        }


        if (CallJumpDEBUG)
        {
            Jump();
            CallJumpDEBUG = false;
        }
        if (!Grounded && timeBecameAirborne == 0f)
        {
            timeBecameAirborne = Time.time;
        }
        else if (Grounded)
        {
            timeBecameAirborne = 0f;
        }
        // Dodge Meter
        if (LockedInMeterSlider != null)
        {
            if (PerfectDodgeCount >= MaxDodge)
            {
                PerfectDodgeCount = MaxDodge;
            }
            LockedInMeterSlider.maxValue = MaxDodge;
            LockedInMeterSlider.value = PerfectDodgeCount;
            if (!LockedIN)
            {
                LockedInThunder.color = Color.gray;
            }
            else
            {
                LockedInThunder.color = Color.white;
            }
            // Enable this and bellow in later builds
            //if (PerfectDodgeCount == MaxDodge && !PlayedLockedIn_CompletedGauge && Particle4LockingIn != null)
            //{
            //    Particle4LockingIn.gameObject.SetActive(true);
            //    AudioPlayer.PlayOneShot(GaugeFull);
            //    CameraScript.StartCoroutine(CameraScript.ShakeCam(3.5f, 0.15f));
            //    PlayedLockedIn_CompletedGauge = true;
            //}
            /* TODO: Re-enable this later in the build */
            //if (Input.GetKeyDown(KeyCode.LeftControl) && !LockedIN && PerfectDodgeCount == MaxDodge)
            //{
            //    Particle4LockingIn.gameObject.SetActive(true);
            //    LockedThunderAnim.SetBool("1", true);
            //    LockedIN = true;
            //    CameraScript.StartCoroutine(CameraScript.ShakeCam(5.5f, 0.15f));
            //    AudioPlayer.PlayOneShot(WeAreLockedIn_SFX_ACTIVATE);
            //}
            if (Input.GetKeyDown(KeyCode.E) && isInPerfectDodge && !PowerfulPunch && !Dead)
            {
                ResetAllAnimationStates();
                StickAnim.CrossFade("I Understand it Now Stickman Mega Heal", 0, 0);
                MegaHeal = true;
            }
        }



        // force no gun for the Hideout section
        if (Checkpoint == 551)
        {
            HasGun = false;
        }

        if (HeCanMove == false && !IsStunned)
        {
            HeCanMove = true;
        }

        if (BackFlip)
        {
            //StickAnim.ResetTrigger("Punch");
            //BoxCollider2D punchCollision = MeleeCollider.gameObject.GetComponent<BoxCollider2D>();
            //punchCollision.enabled = false;
        }

        // performance
        if (Checkpoint == ExamHeliID_Checkpoint && LabLevel != null && LabBG_SPRITE != null)
        {
            Move();
            HasGun = true;
            PauseManager pm = FindAnyObjectByType<PauseManager>();
            pm.Hypothesis.gameObject.SetActive(false);
            if (!pm.BreakIN.gameObject.activeSelf)
            {
                BreakIn = true;
                LabBG_SPRITE.gameObject.SetActive(true);
                pm.Hypothesis.gameObject.SetActive(false);
                pm.BGLOW.gameObject.SetActive(false);
                pm.BreakIN.gameObject.SetActive(true);
            }
            if (LabLevel != null)
            {
                LabLevel.gameObject.SetActive(true);

            }
        }
        if (Level1 != null && Level2 != null)
        {
            if (Checkpoint >= 3 && Checkpoint != 726438 && Checkpoint != ExamHeliID_Checkpoint)
            {
                Level1.gameObject.SetActive(false);

                Level2.gameObject.SetActive(true);
            }
            if (Checkpoint < 3 || Checkpoint == 726438)
            {
                Level2.gameObject.SetActive(false);

                Level1.gameObject.SetActive(true);
            }
        }


        if (InEmbryBoss && MaxSpeed >= 50)
        {
            MaxSpeed = 50;
        }

        if (InEmbryBoss && JumpForce >= 50)
        {
            JumpForce = 50;

        }

        if (MaxSpeed < 60 && HeCanMove)
        {
            MaxSpeed = 60;
        }

        if (!Grounded)
        {
            StopFootsteps();
        }
        if (isDashing)
        {
            StopFootsteps();
        }

        if (BackFlip)
        {
            StopFootsteps();
        }
        if (isSliding)
        {
            StopFootsteps();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Footsteps();
        }
        else if (!Input.GetKey(KeyCode.A))
        {
            StopFootsteps();
        }


        // TODO: Disable Late in the game Especially in chapter 4 this can be
        // after an emotional set piece
        if (Input.GetKey(KeyCode.G))
        {
            if (Time.time >= LastULT + UltCD && StickHP.Testing)
            {
                LastULT = Time.time;
                StickAnim.SetTrigger("Hyper");
            }

        }

        if (Input.GetKey(KeyCode.A))
        {
            Footsteps();
        }
        else if (!Input.GetKey(KeyCode.D))
        {
            StopFootsteps();
        }

        Grounded = CheckIfGrounded();




        if (lastPunchTime + MeleeCD >= Maxdelay4Punch)
        {
            MeleeCD = OG_MeleeCD;
            ComboCount = 0;
        }


        if (Input.GetButton("Fire2"))
        {

            if (Time.time >= lastPunchTime + MeleeCD && !BackFlip || isInPerfectDodge)
            {
                if (BackFlip && isInPerfectDodge && !MegaHeal)
                {
                    PowerfulPunch = true;
                    MeleeCollider.enabled = true;

                }

                else if (!BackFlip && !isInPerfectDodge)
                {
                    if (InHeliCineBeatdown)
                    {
                        Exam examBook = GameObject.FindAnyObjectByType<Exam>();
                        if (!examBook.isPunch)
                        {
                            StickAnim.SetTrigger("Punch");

                        }
                    }
                    else
                    {
                        StickAnim.SetTrigger("Punch");

                    }

                }
                if (Time.time >= lastPunchTime + MeleeCD + 0.4f)
                {
                    StartCoroutine(PunchFreeFlow());

                }

                MeleeCD = Mathf.Max(OG_MeleeCD - ComboCount, 0.1f);
                MeleeCD = MeleeCD - ComboCount;
                lastPunchTime = Time.time;




            }
        }



        if (JumpForce >= 25)
        {
            JumpForce = 25;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            FacingRight = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            FacingRight = false;
        }


        if (isDashing)
        {
            motionBlurHandler.HandleMotionBlur();
        }

        //Kill himself
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StickHP.PlayerHP = 0;
        }


        //Slidecancel and Move speed min max






        // Handle Max Speed
        if (SlideCancelSpeed < 100)
        {
            if (!inRush && HeliForestCheck == null)
            {
                SlideCancelSpeed = 100;

            }
        }

        if (SlideCancelSpeed > 220)
        {
            SlideCancelSpeed = 220;
        }

        if (BreakIn == true && MaxSpeed > 80 && Checkpoint != ExamHeliID_Checkpoint)
        {
            MaxSpeed = 80;
        }

        if (Checkpoint == ExamHeliID_Checkpoint && HeliForestCheck == null)
        {
            if (MaxSpeed >= 100 || MaxSpeed <= 100)
            {
                MaxSpeed = 100;
            }

            if (rb.gravityScale < 15 && !isDashing && HeliForestCheck == null)
            {
                rb.gravityScale = 15;
            }
            else if (HeliForestCheck == null)
            {
                rb.gravityScale = 20;

            }


        }

        if (HeliForestCheck != null)
        {

            if (MaxSpeed < 100)
            {
                MaxSpeed = 100;
            }

        }







        //Countering KeyBoard

        if (Input.GetKeyDown(KeyCode.R))
        {
            StickAnim.SetTrigger("Catch");
            catchWindow = 0.25f;
            StickPlayerWannaCatch = true;
            CanCatch = true;
        }
        if(catchWindow > 0) 
        {
            catchWindow -= Time.deltaTime;
        }
        else
        {
            StickPlayerWannaCatch = false;
        }

        if (HasCaught)
        {
            Helicopter exam = FindFirstObjectByType<Helicopter>();
            if (exam != null)
            {
                if (exam.ExamHealth <= 1 && !PlayedCutHeli)
                {
                    PlayedCutHeli = true;
                    StartCoroutine(EndDemoVersion1());
                }
            }
            HasCaught = false;
            StickAnim.SetTrigger("Caught");
        }




        
       


        //hide


        if (Hiding)
        {
            StickAnim.SetBool("Hide", true);
        }
        if (!Hiding)
        {
            StickAnim.SetBool("Hide", false);
        }

        //if (Input.GetKeyDown(KeyCode.Q) && CanHide)
        //{
        //    if (!Hiding && TouchedCollider)
        //    {
        //        // Start running toward the hiding position
        //        StartCoroutine(RunToHidingPosition());
        //    }
        //    else
        //    {
        //        // Stop hiding and allow movement
        //        Hiding = false;
        //        HeCanMove = true;
        //        StickAnim.SetBool("Hide", false);
        //        StickAnim.SetBool("Walking", false);
        //    }
        //}



        if ((Input.GetKeyDown(KeyCode.E)) && Hiding)
        {
            Hiding = false;
            //transform.position = HidePosition.position;
            //transform.position = Vector3.MoveTowards(HidePosition.position, transform.position, MaxSpeed / 5 * Time.deltaTime);
            HeCanMove = true;
            StickAnim.SetBool("Hide", false);
        }


        //Time.timeScale = 1;

        if (HidePosition != null)
        {
            TouchedCollider = true;

        }
        else
        {
            TouchedCollider = false;
        }

        if (isDashing)
        {
            canSlide = false;
            return;
        }

        if (!isSliding )
        {
            if(SlideSmoke != null)
            {
                SlideSmoke.gameObject.SetActive(false);

            }
            StickAnim.SetBool("Slide", false);
        }




        if (EarthQ_Dodge == true)
        {
            //StickAnim.SetBool("Jump", false);
            //StickAnim.SetBool("DodgeLow", true);
        }


        if (EarthQ_Dodge == false)
        {

            StickAnim.SetBool("DodgeLow", false);
        }






        // dead
        if (StickHP.PlayerHP <= 0)
        {
            rb.velocity = new Vector2(0,rb.velocity.y);
            if (TutTXT != null)
            {
                TutTXT.gameObject.SetActive(false);

            }
            //if (!AlrDid4DeathCor)
            //{
            //    AlrDid4DeathCor = true;
            //    StartCoroutine(WaitForBit());

            //}
            StickAnim.CrossFade("Dead", 0, 0);
            StickAnim.SetTrigger("Dead");
            Speed = 0;
            Dead = true;
            if (!AlrPlayedDeath)
            {
                AlrPlayedDeath = true;
                AudioPlayer.PlayOneShot(DeathSFX);
            }

        }


        if (Dead == true)
        {
            StickHP.PlayerHP = 0;
            TimeManager.RevertSpeed();
        }




        // call move void
        if (Cart != null)
        {
            if (StickmanGetsKnocked == false && !SlopeSlide && !Cart.gameObject.activeSelf && HeCanMove && !Dead && !PunchBool)
            {

                Move();
            }
        }
        else
        {
            if (StickmanGetsKnocked == false && !SlopeSlide && HeCanMove && !Dead && !PunchBool)
            {

                Move();
            }
        }

        if (PunchBool && !isInPunchAttach && !isInPerfectDodge)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            StopFootsteps();
        }



        if (StickPOS.position.y < resetYPosition)
        {
            FallStick();
        }
        if (Input.GetKey(KeyCode.Space) && StickPOS.position.y < resetYPosition)
        {
            FallStick();
        }
        if (Input.anyKey && StickPOS.position.y < resetYPosition)
        {
            FallStick();
        }






        if (Grounded == true)
        {
            StickAnim.SetBool("Airborne", false);

            ForcedJumpMegaHurtBool = false;
            AirBackFlip = false;
            isJumping = false;
            StickAnim.SetBool("OnGround", true);

            AbleToJump = true;
            SlideJump = false;

            canSlide = true;

            StickAnim.SetBool("DodgeLow", false);



            StickAnim.SetBool("Jump", false);
            //StickAnim.ResetTrigger("JumpShoot");


            JumpDodge = false;

            StickAnim.SetBool("DodgeHigh", false);

            StickAnim.SetBool("Fall", false);
        }

        if (!Grounded)
        {
            if (!Grounded && BackFlip)
            {
                StickAnim.SetBool("Backflip", true);

            }
            StickAnim.SetBool("OnGround", false);
            AbleToJump = false;
            canSlide = false;
            //StickAnim.SetBool("Slide", false);
            if (HeliForestCheck == null) // Helps With Slope Levels
            {
                isSliding = false;

            }

        }


        if (Input.GetKey(KeyCode.Space) && !isSliding && !SlideJump && !InSlideAnimation && !Dead && Time.time >= LastJump + JumpCD && Grounded)
        {
            LastJump = Time.time;
//            Debug.Log("Jump Has Been Requested");
            if (StickmanGetsKnocked == false && !InSlideAnimation && !inRush)
            {
                //Debug.Log("Jump Has Went Through");
                coyoteTimerCounter = 0;
                Jump();

                if (!SlideJump)
                {
                    rb.gravityScale = 10.55f;
                }



            }

        }

        if (Input.GetKey(KeyCode.P))
        {
            Graphix.gameObject.SetActive(true);
        }


        if (Input.GetKey(KeyCode.L))
        {
            Graphix.gameObject.SetActive(false);
        }



        if (Input.GetButton("Fire1"))
        {
            if (HasGun && StickHP.MegaCharge > 0.5f && Time.deltaTime > 0 && StickHP.MegaCharge >= 5 && !isInPerfectDodge)
            {
                if (StickHP.PlayerHP > 0)
                {
                    if (!isDashing)
                    {
                        if (Hiding)
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                        Shoot();

                    }

                }
            }


            // inpuit uffer thing
            


        }
        InputDelay_ion_rlly_know_what_toCall_THIS();

        //if (Input.GetButton("Fire2"))
        //{
        //    if (!isDashing)
        //    {
        //        StickAnim.SetTrigger("ShootVariPistol");
        //    }

        //}

        if (Input.GetKeyDown(KeyCode.S))
        {
            StickAnim.SetTrigger("Crouch");
        }

      

        if (StickmanGetsKnocked == true)
        {
            Speed = 0;
            Grounded = false;

        }

        if (StickmanGetsKnocked == false)
        {
            RevertSpeed();

        }

        AssignPossibleNulls();
        ShootNPunch();



        if (!Grounded &&!PlayingTheLandSFX_AFTER_BEING_IN_AIR)
        {
            PlayingTheLandSFX_AFTER_BEING_IN_AIR = true;
            StartCoroutine(GetReadyForLandSFX());
        }
    }

    private bool PlayingTheLandSFX_AFTER_BEING_IN_AIR;
    public IEnumerator GetReadyForLandSFX()
    {
        float timer = 0;
        if (isBouncyKnockbackActive || Falling_KnockBack)
        {
            PlayingTheLandSFX_AFTER_BEING_IN_AIR = false;
            yield break;
        }
        while (!Grounded)
        {
            if(isBouncyKnockbackActive || Falling_KnockBack)
            {
                PlayingTheLandSFX_AFTER_BEING_IN_AIR = false;
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if(timer >= 2f && timer < 2.01f)
        {
            BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
            BackFlipAS.PlayOneShot(GroundImpact);
            if (JumpSmoke != null)
            {
                ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

            }
            CameraScript.StartCoroutine(CameraScript.ShakeCamGroundFeeling(17, 0.2f));

        }
        else if(timer > 10)
        {
            StickHP.PlayerHP = 0;
            BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
            BackFlipAS.PlayOneShot(GroundImpact);
            if (JumpSmoke != null)
            {
                ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

            }
            CameraScript.StartCoroutine(CameraScript.ShakeCamGroundFeeling(100, 0.4f));
        }
        else if(timer > 0.2f)
        {
            BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
            BackFlipAS.PlayOneShot(LandSFX);
            if (JumpSmoke != null)
            {
                ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

            }
            CameraScript.StartCoroutine(CameraScript.ShakeCamGroundFeeling(10, 0.2f));
        }   
        PlayingTheLandSFX_AFTER_BEING_IN_AIR = false;
        yield break;
    }
    
    //private bool FoundLedge;
    //private Vector2 LedgePosition;
    // there was gonna be a section where you climb a Hill but ngl it made me mad and now no more climb
    // HE HE HE HAA!!!!
    // >:D
    //public void CheckForThemLedges()
    //{
    //    Vector2 rayDirection = new Vector2(0f, LedgeCheckDistance);
    //    RaycastHit2D raycast = Physics2D.Raycast(LedgeChecker.position, rayDirection, LedgeCheckDistance, WhatsALedge);
    //    Debug.DrawRay(LedgeChecker.position, rayDirection * RayDistance, Color.red);

    //    if (raycast.collider != null)
    //    {

    //    }


    //}
    public bool inSlipperyState;
    public bool PowerfulPunch;

    //public void CalculateStuffForLedge()
    //{
    //    // nah
    //}
    // Things for balancing  energy draining when shooting;
    private float DelayCD = 0.2f;
    private float LastDelay;

    public void TURN_OFFF_RBBBBBBBB()
    {
        rb.simulated = false;
    }

    public void TURN_ONNNNN_RBBBBBB()
    {
        rb.simulated = true;
    }
    private bool CheckIfGrounded()
    {


        float extraHeight = 1.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }

    private bool CheckIfGroundedStrict(float extraHeight)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }

    public void ReadyForImpactFrame()
    {
        PlayerReadyForImpactFrame = true;
    }

    public void NotReadyImpactFrame()
    {
        PlayerReadyForImpactFrame = false;

    }

    public void AnimationKey_StunTrue()
    {
        IsStunned = true;
    }
    public void StartCounterCouroutine()
    {
        AudioPlayer.PlayOneShot(ThrowSFX);
        StartCoroutine(CounterAttack());
    }

    // freeze fram
    public void FreeezeAnimation_KEY()
    {
        if (Checkpoint != ExamHeliID_Checkpoint)
        {
            TimeManager.FreezeTime(0.2f);

        }
    }
    public void ThrowSFX_ANIMTION_KEY()
    {
        AudioPlayer.PlayOneShot(ThrowSFX);

    }

    public IEnumerator WaitForBit()
    {
        yield return new WaitForSeconds(3f);
        LoadingSystem.LoadGameScene();

    }


    public void HURT_COMBO_MESS_UP_ANIMATION_KEY()
    {
        ///TutTXT.gameObject.SetActive(false);
        CurrentPunches -= 1;
    }

    public bool HasCaughtSniper;
    public IEnumerator CounterAttack()
    {
        StartCoroutine(CameraScript.ShakeCamGroundFeeling(15.5f, 0.2f));
        IsInThrowAnim = false;
        if (InExamBoss)
        {
            //yield return new WaitForSecondsRealtime(0.1f);
            //Vector2 direction = (Helicopter.position - CounterAimPoint.position).normalized;
            //GameObject projectile = Instantiate(ExamBullet, CounterAimPoint.position, Quaternion.identity);
            //Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            //float enemySpeed = rb.velocity.x;
            //float distance = Vector2.Distance(CounterAimPoint.position, Helicopter.position);
            //float ThrowSpeed = 100;
            //float velocityX = direction.x * ThrowSpeed + enemySpeed;
            //float arcHeight = 0.5f;
            //float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            //rb.velocity = new Vector2(velocityX, velocityY);
            yield return new WaitForSecondsRealtime(0.1f);
            GameObject projectile = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);
            projectile.tag = "CounterBullet";
            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            projectile.GetComponent<Bullet>().SetDirection(shootDirection);
            projectile.GetComponent<Bullet>().isEnemyCounter = true;
        }
        else
        {
            if (!InEmbryBoss)
            {
                yield return new WaitForSecondsRealtime(0.1f);
                GameObject projectile = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);
                projectile.tag = "CounterBullet";
                Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
                projectile.GetComponent<Bullet>().SetDirection(shootDirection);
                projectile.GetComponent<Bullet>().isEnemyCounter = true;
            }

        }


        if (InEmbryBoss)
        {

            yield return new WaitForSecondsRealtime(0.1f);
            Vector2 direction = (EmbryPOS.position - CounterAimPoint.position).normalized;
            GameObject projectile = Instantiate(EmbryProjectile, CounterAimPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(CounterAimPoint.position, EmbryPOS.position);
            float baseThrowSpeed = 100;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = 0.5f;
            float velocityY = Mathf.Sqrt(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);
            //rb.AddTorque(570f);  // fix this later tooo many visual bugs that make it not gucci :/
        }

        yield break;

    }


    public void InCounterAnimBoolTrue()
    {
        IsInThrowAnim = true;
    }


    public void InCounterAnimBoolFalse()
    {
        IsInThrowAnim = false;
    }

    public void WeDontgotitAnymore()
    {
        HasCaught = false;
    }

    public void CanCatchBool()
    {
        CanCatch = true;
    }

    public void CanCatchBoolIsFalse()
    {
        CanCatch = false;
    }


    private float slideMomentum = 0f;

    public void AnimationKey_IsDashingBool()
    {
        isDashing = true;
    }

    public void AnimationKey_IsSlidingBool()
    {
        isSliding = true;
    }

    public void AnimationKey_DashFalse()
    {
        isDashing = false;
    }
    public void Jump()
    {

        
        // Gravity Manager
        //StartCoroutine(Jump_Gravity_Manager());


        // Shaking stuff
        if (shake_AnimationKey == null)
        {
            shake_AnimationKey = GetComponent<Shake_AnimationKey>();

        }
        if (shake_AnimationKey != null)
        {
            if(MaxSpeed >= 100)
            {
                shake_AnimationKey.ShakeWithSpecificGivenAmount(15.5f, 0.3f);

            }
            else
            {
                shake_AnimationKey.ShakeWithSpecificGivenAmount(10.5f, 0.34f);

            }
        }

        if (JumpSmoke != null)
        {
            ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);

        }
        Grounded = false;
        coyoteTimerCounter = 0;
        BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
        BackFlipAS.PlayOneShot(BackFlipSFX);
        rb.gravityScale = 20;
        isJumping = true;
        JumpDodge = true;

        if (rb.velocity.magnitude > 5.1f && MaxSpeed >= 40 && StickHP.MegaCharge > 1)
        {
            motionBlurHandler.HandleMotionBlur();
        }
        if (!InSlideAnimation && !SlideJump)
        {
            if (StickHP.MegaCharge > 1)
            {
                if (Checkpoint != ExamHeliID_Checkpoint || HeliForestCheck != null)
                {
                    JumpForce = Mathf.Max(25, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));

                }
                else
                {
                    JumpForce = 90;

                }
                MaxSpeed += 0.4f;
                StickHP.MegaCharge -= 2.5f;
                //StickHP.StartCoroutine(StickHP.DelayinMegaCharge());
            }
            else
            {
                float TempJumpForce = Mathf.Max(25, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));
                float MinimizedForce = TempJumpForce / 2;

                JumpForce = MinimizedForce;
                MaxSpeed -= 0.7f;


            }
           // Debug.Log("Jump Void()");

            if (AbleToJump && !isSliding)
            {
//                Debug.Log("Jump Void Is Active");
                if (!isDashing && !isSliding && !IsShooting && !Dead && !inSlipperyState && !Anim_Locked && !BackFlip && !ForcedJumpMegaHurtBool && !Falling_KnockBack)
                {
                    StickAnim.CrossFade("Jump", 0, 0);

                }
                else if (!BackFlip)
                {

                    StickAnim.SetBool("Jump", true);

                }

                if (JumpForce >= 50)
                {
                    JumpForce = 50; // this is how we limit the jump to not make it crazy
                }
                float horizontalMomentum = rb.velocity.x;
                rb.velocity = new Vector2(horizontalMomentum, JumpForce);

                Grounded = false;
            }
        }
    }

    private void ManageJumpGravity()
    {
        if(isDashing) return;
        if (rb.velocity.y < 0 && !Grounded && !isDashing)
        {
            rb.gravityScale = FallGravity;
            StartCoroutine(ManageFallGravity());
        }
        else
        {
            rb.gravityScale = OG_NormGravity;
            FallGravity = OG_FallGravity;
        }
    }

    private bool isRunningMangeFallGravityInstance;

    private IEnumerator ManageFallGravity()
    {

        if(FallGravity >= FallingGravityMAX || isRunningMangeFallGravityInstance) yield break;
        FallGravity = 0;
        while(!Grounded && !isDashing && FallGravity < FallingGravityMAX)
        {
            isRunningMangeFallGravityInstance = true;
            FallGravity += 1.2f * Time.deltaTime;
            if(isDashing)
            {
                rb.gravityScale = 0;
                yield break;
            }
        }
        isRunningMangeFallGravityInstance = false;
        yield break;
    }



    // private IEnumerator Jump_Gravity_Manager_UnUSed()
    // {
    //     float timer = 0;
    //     float GravityScaler = 10000; // this thing will be hard coded cuz ion feel like making it public
    //     //  so if ts crap ya gotta change it here
    //     float Timeout = 10; // this gonna be the max time and after that gravity will be locked 
    //     float OriginalGravityOfRB = rb.gravityScale;

    //     Debug.Log("Entered Jump Gravity Manager");


    //     while(rb.velocity.y > 0)
    //     {
    //         yield return null; //Player is still in the jump impulse thing like he still gaining height
    //     }
    //     Debug.Log("Jump Velocity is 0");
    //     while(!Grounded && !isDashing && timer < 10)
    //     {
    //         // no player is losing gravity and too lose the floaty feeling we just reduce gravity
    //         timer += Time.deltaTime;
    //         float NewVelocity = rb.velocity.y + (timer * GravityScaler);
    //         rb.velocity = new Vector2(rb.velocity.x, NewVelocity);  

    //         yield return null;
    //     }
    //     while(!Grounded)
    //     {
    //         yield return null; // This runs only if the timeout has happened so we just play the waiting game

    //     }
    //     //rb.gravityScale = OriginalGravityOfRB;
    //     yield break;
    // }

    public void AnimationKey_UppercutBool()
    {
        isUppercut = true;

    }

    public void ShootNPunch()
    {
        if (CurrentPunches < 0)
        {
            CurrentPunches = 0;
        }
        if (CurrentPunches >= MaxPunches && HasGun)
        {
            CurrentPunches = 0;
            rb.rotation = 0;

            StickAnim.SetTrigger("ComboSnP");
            inCombo = true;

        }
    }

    public void ShootNPunch_Combo_Move()
    {
        float TimeMax = 1;
        float horizontalMomentum = 25;
        float TimeDuration = 0;
        while (TimeMax > TimeDuration)
        {
            TimeDuration += Time.deltaTime;
            rb.velocity = new Vector2(transform.localScale.x * (-1 * (horizontalMomentum)), 0);

        }
        // Vector3 MoveDirec = new Vector3(horizontalMomentum, 0f, 0f) * Time.deltaTime;
        //rb.velocity = MoveDirec;
        //Vector3 MovementThing = new Vector3(x, y, 0f) * Time.deltaTime;
        //motionBlurHandler.HandleMotionBlur();
        //transform.position -= MoveDirec;

    }

    //public IEnumerator ShootNPunchCourtine()
    //{

    //}
    public void AnimationKey_UppercutFalse()
    {
        isUppercut = false;
    }
    private IEnumerator RunToHidingPosition()
    {

        StickAnim.SetBool("RunHide", true);


        HeCanMove = false;

        while (Vector3.Distance(transform.position, HidePosition.position) > 0.1f)
        {

            transform.position = Vector3.MoveTowards(transform.position, HidePosition.position, (MaxSpeed * 2.4f) * Time.deltaTime);


            Vector3 direction = HidePosition.position - transform.position;
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);

            yield return null;
        }


        StickAnim.SetBool("RunHide", false);


        Hiding = true;



        transform.localScale = new Vector3(-1, 1, 1);



        StickAnim.SetBool("Hide", true);
    }




    public void SHOW_BLOOD_FX()
    {
        BloodFX.gameObject.SetActive(true);
        Animator anim = BloodFX.GetComponent<Animator>();
        anim.Play("BloodFX");
    }

    // Final Bullet For EndingDemo V1 
    private bool PlayedCutHeli = false;
    private float TimeDivided;
    private bool HadDividedDeathCam;
    public IEnumerator EndDemoVersion1()
    {
        if (!HadDividedDeathCam)
        {
            TimeDivided = CameraScript.durationofDeath / 6;
            CameraScript.durationofDeath = TimeDivided;
            HadDividedDeathCam = true;
        }
        CameraScript.StartCoroutine(CameraScript.DeathCam());

        while (CameraScript.CamSize > 21)
        {
            yield return null;
        }
        SwitchSceneToCutscene();


    }

    public void SwitchSceneToCutscene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("HeliCut");
        loadOperation.allowSceneActivation = true;


    }

    private IEnumerator FallStick()
    {
        // Reset the Y position of Stickman
        Vector3 newPOS = StickPOS.position;
        newPOS.y = resetYPosition;
        StickPOS.position = newPOS;

        yield return new WaitForSeconds(0.000000000000001f);
    }


    public IEnumerator AccelerateSpeed(float MSpeed, float time)
    {
        if (Speed == MSpeed) yield break;
        while (Speed != MSpeed && rb.velocity.x > 0)
        {
            Speed = Mathf.Lerp(MaxSpeed, MSpeed, time);
            yield return null;

        }

        yield break;
    }


    public void JumpBoolean()
    {
        JumpDodge = true;
        //StopCoroutine(PlayerKnockbackBouncy(0,0,0,0,0));
        //rb.sharedMaterial = StickmanMat;
        //Falling_KnockBack = false;
        //StickAnim.SetBool("KnockedBack", false);
        //isBouncyKnockbackActive = false;
        //if(rb.velocity.y > JumpForce)
        //{
        //    rb.velocity = new Vector2 (rb.velocity.x,0);
        //}

    }

    public void JumpBooleanFalse()
    {
        JumpDodge = false;
    }
    private bool AlrAttached;

    private bool StartedDecay;
    // Perfect Dodge Mechanic
    public IEnumerator PerfectDodge_SloMO()
    {
        LockedThunderAnim.SetBool("1", false);
        LockedIN = false;
        PerfectDodgeSloMO.gameObject.SetActive(true);
        PerfectDodgeSloMO.Play();
        ResetAllAnimationStates();
        PerfectDodgeCount = 0;
        //StickAnim.enabled = false;
        float OGsloRate = TimeManager.slomoRate;
        TimeManager.slomoRate = SloMORate;
        TimeManager.SloMO();
        if (!BackFlip)
        {
            StartCoroutine(BackFlipMove());
        }
        while (PerfectDodgeSloMO.isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isInPerfectDodge = false;
                MegaHeal = false;
                TimeManager.RevertSpeed();
                StickAnim.speed = 1;
                StickAnim.enabled = true;
                PerfectDodgeSloMO.gameObject.SetActive(false);
                TimeManager.slomoRate = OGsloRate;
                PerfectDodgeCount = 0;
                PowerfulPunch = false;
                PlayedLockedIn_CompletedGauge = false;
                AlrAttached = false;
                yield break;
            }
            if (PowerfulPunch && !MegaHeal)
            {
                if (!AlrAttached && !Dead)
                {
                    AlrAttached = true;
                    StickAnim.CrossFade("StickAttach", 0, 0);

                }
                ResetAllAnimationStates();
                isInPerfectDodge = true;
                PerfectDodgeCount = 0;
                StickAnim.enabled = true;
                StickAnim.speed = 10;
                StartCoroutine(PunchFreeFlow());
                Grounded = true;
            }
            isInPerfectDodge = true;
            yield return null;
            if (MegaHeal == true)
            {
                rb.rotation = 0;
                yield return new WaitUntil(() => !MegaHeal);
                isInPerfectDodge = false;
                PerfectDodgeSloMO.gameObject.SetActive(false);
                MegaHeal = false;
                TimeManager.RevertSpeed();
                StickAnim.speed = 1;
                StickAnim.enabled = true;
                TimeManager.slomoRate = OGsloRate;
                PerfectDodgeCount = 0;
                PowerfulPunch = false;
                PlayedLockedIn_CompletedGauge = false;
                yield break;

            }
        }
        isInPerfectDodge = false;
        MegaHeal = false;
        TimeManager.RevertSpeed();
        StickAnim.speed = 1;
        StickAnim.enabled = true;
        TimeManager.slomoRate = OGsloRate;
        PerfectDodgeCount = 0;
        PowerfulPunch = false;
        PlayedLockedIn_CompletedGauge = false;
        AlrAttached = false;
        yield break;

    }
    public bool inRush = false;

    public IEnumerator Stickman_AdrenalineRush()
    {
        PerfectDodgeSloMO.gameObject.SetActive(true);
        PerfectDodgeSloMO.Play();
        PauseManager pauseManager = GameObject.FindObjectOfType<PauseManager>();
        pauseManager.BreakIN.Pause();
        ResetAllAnimationStates();
        inRush = true;
        PerfectDodgeCount = 0;
        float OGsloRate = TimeManager.slomoRate;
        TimeManager.slomoRate = SloMORate / 1.5f;
        TimeManager.SloMO();
        ResetAllAnimationStates();
        StickAnim.SetBool("Slide", true);
        StickAnim.ResetTrigger("Shoot");
        StickAnim.ResetTrigger("SlideShoot");
        StartCoroutine(Slide());
        while (PerfectDodgeSloMO.isPlaying)
        {
            SlideCancelSpeed = 220;
            StickHP.MegaCharge = 30;
            yield return null;

        }
        isInPerfectDodge = false;
        MegaHeal = false;
        TimeManager.RevertSpeed();
        StickAnim.speed = 1;
        pauseManager.BreakIN.Play();
        StickAnim.enabled = true;
        TimeManager.slomoRate = OGsloRate;
        PerfectDodgeCount = 0;
        PowerfulPunch = false;
        PlayedLockedIn_CompletedGauge = false;
        AlrAttached = false;
        inRush = false;
        yield break;

    }
    public void AssignPossibleNulls()
    {
        if (TimeManager == null)
        {
            TimeManager = FindAnyObjectByType<TimeManager>();
        }
        if (CameraScript == null)
        {
            CameraScript = FindAnyObjectByType<CameraScript>();
        }
    }

    public void ResetAllAnimationStates()
    {
        StickAnim.ResetTrigger("Punch");
        StickAnim.ResetTrigger("Jump");
        StickAnim.ResetTrigger("Dash");
        StickAnim.ResetTrigger("Caught");
        StickAnim.ResetTrigger("Catch");
        StickAnim.ResetTrigger("Shoot");
        StickAnim.ResetTrigger("JumpShoot");
        StickAnim.ResetTrigger("ComboSnP");
        StickAnim.ResetTrigger("PistolCombo");
        StickAnim.ResetTrigger("StealthKill");
        StickAnim.ResetTrigger("VariFixer");
        StickAnim.ResetTrigger("Idle");
        StickAnim.ResetTrigger("Heal");

        StickAnim.SetBool("OnGround", false);
        StickAnim.SetBool("DodgeLow", false);
        StickAnim.SetBool("DodgeHigh", false);
        StickAnim.SetBool("Jump", false);
        StickAnim.SetBool("Slide", false);
        StickAnim.SetBool("Walking", false);
        StickAnim.SetBool("Hide", false);
        StickAnim.SetBool("Attach", false);
        StickAnim.SetBool("Dash", false);
        StickAnim.SetBool("RunHide", false);
        StickAnim.SetBool("Stun", false);
    }

    public void AddSlideMomentum_WhileShooting_MOMENTUM_ANIMTION_KEY()
    {
        SlideCancelSpeed = 250;
        Slide();
    }
    public void ShootingBool()
    {

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }


    public void ShootingFalse()
    {

        rb.constraints = RigidbodyConstraints2D.None;
        MaxSpeed *= 0.9f;


    }











    //private Vector2 moveinput;


    // these are just for the footstep build up sfx nothin else
    bool wasMoving;
    bool isMoving;

    public void Move()
    {
        if (HeCanMove == true && StickmanGetsKnocked == false)
        {
            isMoving = Mathf.Abs(rb.velocity.x) > 0.1f && CheckIfGrounded();

            if (!wasMoving && isMoving)
            {
                BackFlipAS.pitch = UnityEngine.Random.Range(0.8f, 1.7f);
                BackFlipAS.PlayOneShot(FootstepsStart);
            }

            wasMoving = isMoving;



            //BuildUp = true;
            float horizontalInput = Input.GetAxis("Horizontal");

            //if (Checkpoint != ExamHeliID_Checkpoint)
            //{
            //    horizontalInput = Input.GetAxis("Horizontal");
            //}
            //else
            //{
            //    horizontalInput = 1;
            //}
            rb.velocity = new Vector2(horizontalInput * Speed, rb.velocity.y);
            FacingRight = transform.localScale.x >= 1 ? true : false;
            float targetspeed = horizontalInput * Speed;

            if(Grounded && FootP.gameObject.activeSelf && rb.velocity.x > 0 && FootP.clip == GrassFootSFX||Grounded && FootP.gameObject.activeSelf && rb.velocity.x < 0 && FootP.clip == GrassFootSFX )
            {
                GrassBlades.gameObject.SetActive(true);
                GrassBlades.gameObject.transform.localScale = new Vector3 (GrassBlades.gameObject.transform.localScale.x * -transform.localScale.x,GrassBlades.transform.localScale.y,GrassBlades.transform.localScale.z);
            }
            else if(!isSliding && GrassBlades != null)
            {
                GrassBlades.gameObject.SetActive(false);
            }
            if (rb.velocity.magnitude > 0.1f )
            {
                if(BackFlip || isDashing || isSliding || PunchBool || isShooting)
                {
                    motionBlurHandler.HandleMotionBlur();

                }
            }
        //    if (isDashing | isSliding | isJumping | BackFlip && StickHP.MegaCharge >= 1)
        //         {
        //             motionBlurHandler.HandleMotionBlur();

        //         }
            if (StickmanGetsKnocked == false && !isInPunchAttach && !BackFlip)
            {
                if (horizontalInput > 0.01)
                {
                    //FacingRight = true;
                    StickDirection = 1;
                    transform.localScale = Vector3.one;

                }
                else if (horizontalInput < -0.01)
                {
                    //FacingRight = false;
                    StickDirection = -1;
                    transform.localScale = new Vector3(-1, 1, 1);

                }

            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (StickHP.PlayerHP > 0 && canSlide == true && !isInPerfectDodge && UnlockedSlide)
                {
                    BuildUp = true;
                    StartCoroutine(Slide());

                }
            }

            if (Grounded)
            {
                StickAnim.SetBool("Walking", horizontalInput != 0);

            }
        }
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0.01 && !BackFlip)
            {
                //FacingRight = true;
                StickDirection = 1;
                transform.localScale = Vector3.one;
            }
            else if (horizontalInput < -0.01 && !BackFlip)
            {
                // FacingRight = false;
                StickDirection = -1;
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }









    }
    public void CallKnockBack(Vector2 hitdirection, Vector2 constantForceDirection, float inputDirection)
    {
        Debug.Log("KnockBack Called");
        Knockbackcoroutine = StartCoroutine(KnockBackAction(hitdirection, constantForceDirection, inputDirection));
    }



    private void SlopeCheck()
    {
        Vector2 CheckPOS = transform.position - new Vector3(0.0f, ColliderSize.y / 2);

        SlopeCheckVerticle(CheckPOS);
    }



    private void SlopeCheckVerticle(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, SlopeCheckDistance, WhatsGround);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal);
        }

    }

    public void AnimationKey_GainA_Little_Energy_While_Stunned()
    {
        rb.velocity = Vector2.zero;
        StickHP.MegaCharge += 1.2f;
    }

    public void AnimationKey_UltisFalse()
    {
        BattleTheme.gameObject.SetActive(false);

        GustOfBullets = false;

    }

    public void AnimationKey_DisableAndEnableUltTheme()
    {
        UltCam.enabled = true;
        MainCamAnim.speed = 3;
        GustOfBullets = true;
        BattleTheme.gameObject.SetActive(false);
        UltTheme.gameObject.SetActive(true);
        Hypothesis.gameObject.SetActive(false);
        LabTheme.gameObject.SetActive(false);
    }


    public IEnumerator ClimbHeliCinematic()
    {
        float Distance = Mathf.Abs(transform.position.y - HeliCinematicClimbDestination.position.y);

        while (Distance >= MinDisHeliCineClimbDes)
        {
            if (NeverDoCineBeatDownAgain)
            {
                yield break;
            }
            Distance = Mathf.Abs(transform.position.y - HeliCinematicClimbDestination.position.y);
            HeCanMove = false;
            transform.position = Vector2.MoveTowards(transform.position, HeliCinematicClimbDestination.position, 15 * Time.deltaTime);
            Debug.Log("Distance HeliCineMaticLadder: " + Distance);
            yield return null;
        }
        InHeliCineBeatdown = true;
        StickAnim.SetTrigger("HeliCinePunch");
        StickAnim.SetBool("Heli", false);
        OnLadder = false;
        HasFullyClimbed = true;
        yield break;

    }

    public bool NeverDoCineBeatDownAgain;
    public void HeliCineBeatDownManager()
    {
        if (!InHeliCineBeatdown) return;
        rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = InHeliPoS.position;
        if (InHeliCineBeatdown)
        {
            canDash = false;

        }
        else
        {
            canDash = true;

        }
        Grounded = false;
        StickAnim.SetBool("Heli", false);
        StickAnim.SetBool("Jump", false);
        StickAnim.SetBool("Cinematic", true);
        HeCanMove = false;
    }


    public void AnimationKey_DisableUltThem()
    {
        MainCamAnim.speed = 1;

        GustOfBullets = false;

        if (!BreakIn)
        {
            Hypothesis.gameObject.SetActive(true);

        }
        else
        {
            LabTheme.gameObject.SetActive(false);

        }
        UltTheme.gameObject.SetActive(false);
    }


    public void CantMove()
    {
        MaxSpeed = 0;
        HeCanMove = false;

        //if (PressedDash == false)
        //{
        //    HeCanMove = false;
        //}
        //else
        //{
        //    HeCanMove = true;
        //}
    }

    public void CanMove()
    {
        MaxSpeed = 60;
        isPunching = false;
        HeCanMove = true;
    }


    private float CD_FallOFFHELI = 0.6f;
    private float LastFELLOFFHELI = 0;
    public bool isFallingHeli;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && !BackFlip)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                bool animJump = StickAnim.GetBool("Jump");

                // Check if surface is fairly flat and below player
                bool isGroundSurface = Vector2.Angle(contact.normal, Vector2.up) < 30f;
                bool contactBelow = contact.point.y < transform.position.y - 0.1f;

                if (isGroundSurface && contactBelow && !animJump)
                {
                    Grounded = true;

                    // Save only if player is nearly still vertically
                    if (Mathf.Abs(rb.velocity.y) < 0.05f)
                    {
                        // Slightly offset upward so the player doesn't spawn "inside" the collider
                        SafePosition = new Vector3(transform.position.x, transform.position.y + 0.1f, 0);
                    }
                    return;
                }
            }
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Ground") && !IsFalling)
        //{
        //    if(Grounded && !isDashing && !isSliding && rb.velocity.y == 0 && !JumpDodge )
        //    {
        //        SafePosition = new Vector3(transform.position.x, transform.position.y, 0);

        //    }
        //    Grounded = true;
        //}

        if (collision.gameObject.CompareTag("Tree") && HeliFinale)
        {
            if (!isDashing)
            {
                OnLadder = false;
                StickAnim.SetBool("Heli", false);
                StartCoroutine(FallOfLadder());
            }


        }


        string otherObjectName = collision.gameObject.name;
        if (StickHP.Testing)
        {
            Debug.Log("Collided with: " + otherObjectName);

        }



        if (collision.gameObject.CompareTag("Slope"))
        {
            Debug.Log("SlideVolume Worked");
            SlideVolume = true;


        }


        if (collision.gameObject.CompareTag("SlideEnd"))
        {
            SlideVolume = false;


        }


        if (collision.gameObject.CompareTag("Book"))
        {
            BoxCollider2D MyCollider = GetComponent<BoxCollider2D>();
            BoxCollider2D bxcd = collision.gameObject.gameObject.GetComponent<BoxCollider2D>();
            if (isDashing)
            {
                Physics2D.IgnoreCollision(bxcd, MyCollider, true);

            }
            else
            {
                Physics2D.IgnoreCollision(bxcd, MyCollider, false);

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }




    //private void OnCollisionExit2D(Collision2D collision)
    //{
    // if (!collision.gameObject.CompareTag("SldieVolume"))
    // {
    //SlideVolume = false;


    //}
    //  }


    private bool OneTimeHealth_AlarmDoor = false;
    public bool TouchingATKSight;
    public bool OnLadder;

    private HingeJoint2D hingeJoint2D;

    public bool UpLadderWant;
    public void HeliManager()
    {
        if (!OnLadder || isFallingHeli || InHeliCineBeatdown) Debug.Log("Heli Failed Stick Falling nOw");
        if (!OnLadder || isFallingHeli || InHeliCineBeatdown || !HeliFinale) return;
        StickAnim.SetBool("Walking", false);
        Debug.Log("HeliManager On");
        canDash = true;
        if (!InHeliCineBeatdown)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

        }
        if (OnLadder && !isFallingHeli)
        {
            transform.position = HeliLadder.position;
            rb.gravityScale = 0;
            transform.rotation = HeliLadder.rotation;
        }
        else
        {
            return;
        }

        isFallingHeli = false;
        if (isDashing)
        {
            isDashing = false;
            StickAnim.CrossFade("Helicopter", 0, 0);
        }


        HeCanMove = false;
        StickAnim.SetBool("Heli", true);


        if (Input.GetKeyDown(KeyCode.S))
        {
            UpLadderWant = false;
            SwitchHeliPOS();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpLadderWant = true;
            SwitchHeliPOS();

        }

    }

    public IEnumerator FallOfLadder()
    {
        //if(isDashing) yield break;
        isDashing = false;
        rb.rotation = 0;
        OnLadder = false;
        isFallingHeli = true;
        Debug.Log("Heli Fell OFF");
        StartCoroutine(BackFlipMove());
        isFallingHeli = false;
        yield break;


    }
    public void SwitchHeliPOS()
    {
        // Switches Stickman's POS in HeliFight
        AudioPlayer.PlayOneShot(RopeShift);
        if (HasFullyClimbed)
        {
            if (UpLadderWant == false)
            {
                UpLadderWant = false;
                HeliLadder = DownLadder;
            }
            else
            {
                UpLadderWant = true;
                HeliLadder = UpLadder;
            }
        }

    }

    public Transform HidePosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hiding"))
        {
            CanHide = true;
        }
        if (collision.CompareTag("Ladder") && !isFallingHeli && !InHeliCineBeatdown)
        {
            StickAnim.SetBool("Dash", false);
            OnLadder = true;
            HeliManager();

        }
        else if (InHeliCineBeatdown)
        {
            StickAnim.SetBool("Heli", false);

        }

        if (collision.CompareTag("HidePOS"))
        {
            TouchHiding = true;
            HidePosition = collision.transform;

        }
        if (collision.CompareTag("AttackSight"))
        {
            TouchingATKSight = true;
        }
        if (MeleeCollider.CompareTag("Book"))
        {
            if (Time.time >= lastPunchTime + MeleeCD)
            {
                ComboCount += 1;

            }
        }

        if (collision.CompareTag("DoublePunchTime"))
        {
            CanDoublePunch = true;

        }

        if (collision.CompareTag("Safe"))
        {
            if (OneTimeHealth_AlarmDoor == false)
            {
                OneTimeHealth_AlarmDoor = true;
                StickHP.PlayerHP += 15;
                StickHP.MegaCharge += 6;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hiding"))
        {
            CanHide = false;
        }
        if (collision.CompareTag("AttackSight"))
        {
            TouchingATKSight = false;
        }
        if (!collision.CompareTag("HidePOS"))
        {
            TouchHiding = false;
        }
    }

    public void NotSlopSlide()
    {
        SlopeSlide = false;
    }

    public void YAxisFixer()
    {
        Vector3 currentPosition = transform.position;


        currentPosition.y = 0;
    }

    private bool Shooting;


    public void ShootingFalseStatement()
    {
        Shooting = false;
    }


    public void Shoot()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootCooldown;




            if (Hiding)
            {
                StartCoroutine(HideShoot());
            }
            else if (!Grounded && !BackFlip || isDashing)
            {
                StickAnim.SetTrigger("JumpShoot");

            }
            else if (BackFlip)
            {
                StickAnim.CrossFade("Backflip_Shooting",0,0);
                BackFlip = false;
                //StickAnim.SetTrigger("FlipShoot");   <--- Need TO Make that ANimation

            }
            else if (isSliding)
            {
                StickAnim.SetTrigger("SlideShoot");

            }
            else if (PunchBool && PunchAnimationSpeed >= 2.5)
            {
                SigmaCombo();
            }
            else
            {
                Speed = 0;
                StickAnim.SetTrigger("Shoot");

            }
        }
    }


    public void CanComboSet()
    {
        CanCombo = true;
    }


    public void CanNotCombo()
    {
        CanCombo = false;
    }

    public void SigmaCombo()
    {
        StickAnim.SetTrigger("PistolCombo");





    }

    public void AnimationKey_JumpBool()
    {
        isJumping = true;
    }
    public void SigmaComboAnimFunc()
    {
        float horizontalMomentum = 90;


        rb.velocity = new Vector2(transform.localScale.x * (-1 * (horizontalMomentum)), 0);

        rb.rotation = 0;
    }


    public IEnumerator HideShoot()
    {

        transform.localScale = new Vector3(1, 1, 1);


        Speed = 0;
        StickAnim.SetTrigger("Shoot");


        yield return new WaitForSeconds(StickAnim.GetCurrentAnimatorStateInfo(0).length);


        transform.localScale = new Vector3(-1, 1, 1);



    }






    public void NoSlide()
    {
        canSlide = false;
    }

    public void YeaSlide()
    {
        canSlide = true;
    }

    public bool inCombo;
    public void ShootABullet()
    {
        Debug.Log("ShootABullet called");
        if (Time.time >= DelayCD + LastDelay)
        {
            if (!inCombo)
            {
                StickHP.MegaCharge -= 5.2f;

            }
            else
            {
                StickHP.MegaCharge += 0.2f;

            }
            LastDelay = Time.time;
            StartCoroutine(StickHP.DelayinMegaCharge());
            StartCoroutine(MuzzleFireAnim());
            GameObject bullet = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);
            //GameObject bullet = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);

            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);

        }


        //Legacy Code Used Too Shoot a massive bullet one shottting enemies didnt feel good and would mess up the Challange and ngl not rlly
        //fit it


        //if (!StickHP.CanSuper)
        //{

        //}
        //else // Super bullet
        //{
        //    GameObject bullet = Instantiate(MegaBullet, AimPoint.position, Quaternion.identity);


        //    Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        //    bullet.GetComponent<Bullet>().SetDirection(shootDirection);

        //    StickHP.CanSuper = false;
        //    StickHP.MegaCharge = 0;
        //}
    }



    // Make Stickman GO Supa Fast brbrrr Gives PLayer Adreline Feel type thing in the lab break in
    public bool HasKilledYappers_Sciests_WithACart;
    public void SupaFast_Cart_BreakIn()
    {
        if (!inSlipperyState && Cart.activeSelf)
        {
            StickHP.MegaCharge = 0;
            Grounded = false;
            float MaxPushSpeed = 0f;
            if (rb.velocity.x < 120)
            {
                rb.velocity = new Vector2(120, 0);
            }
            if (HasKilledYappers_Sciests_WithACart)
            {
                MaxPushSpeed = 135;

            }
            else
            {
                MaxPushSpeed = 120;

            }
            transform.localScale = new Vector3(1, 1, 1);
            if (rb.velocity.magnitude > MaxPushSpeed)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxPushSpeed);
            }
            rb.AddForce(AccelerationCart * rb.mass);
        }
        else
        {
            return;
        }

    }

    public void ArcBullet()
    {
        GameObject bullet = Instantiate(GunBulletArc, ArcAimPoint.position, Quaternion.identity);


        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;


        bullet.GetComponent<Bullet>().SetDirection(shootDirection);
    }


    public void EarthQ_Dodge_False()
    {
        EarthQ_Dodge = false;
    }


    public void IdleFix()
    {
        StickAnim.SetTrigger("VariFixer");
    }


    public void ReloadLevel()
    {
        PitchBlack.gameObject.SetActive(true);
        if (!AlrDid4DeathCor)
        {
            AlrDid4DeathCor = true;
            StartCoroutine(WaitForBit());

        }
    }






    public void GunAnim()
    {
        //Speed = 0;
        GunAudio.pitch = Random.Range(0.9f, 1.2f);
        GunAudio.PlayOneShot(Gun);
    }



    public void StopSpeed()
    {
        Speed = 0;
    }

    public void RevertSpeed()
    {
        Speed = MaxSpeed;
    }

    public void CantJump()
    {
        AbleToJump = false;
        IsStunned = true;
    }



    public void CanJump()
    {
        IsStunned = false;
        AbleToJump = true;
        RevertSpeed();


    }


    public void JumpFalserAnim()
    {



        //StickAnim.SetBool("Jump", false);
    }



    // Earthquake from geology code
    public void IsEQ()
    {
        EarthQ = true;
    }


    public void SlideCancel()
    {
        isSliding = false;
    }

    public void Charger()
    {
        if (BreakIn)
        {
            StickHP.PlayerHP = StickHP.PlayerHP + 0.1f;
        }
        else
        {
            StickHP.PlayerHP = StickHP.PlayerHP + 0.03f;
        }

    }


    // Punching Code

    public void NotPunchAnimatorBool()
    {
        PunchBool = false;
        StickAnim.speed = 1.0f;
    }


    public void PunchReduce()
    {
        PunchAnimationSpeed -= 0.5f;
    }

    private Coroutine punchDecayCoroutine;
    private bool isPunching;
    public void Punch()
    {
        AuraSigmaRizzyDodge = false;
        AudioClip play = null;
        isPunching = true;
        if (StickHP.Testing)
        {
            CurrentPunches += 1;

        }
        PunchBool = true;
        if (CanDoublePunch)
        {
            PunchAudioP.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        }
        PunchAudioP.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        if (!PowerfulPunch)
        {
            play = PunchSFX[UnityEngine.Random.Range(0, PunchSFX.Length)];
        }
        else
        {
            play = MegaPunch;
        }


        //PunchAudioP.PlayOneShot(play);
        lastPunch = Time.time;

    }

    public void PlayerPunchSFX()
    {
        AudioClip play = null;

        if (CanDoublePunch)
        {
            PunchAudioP.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        }
        PunchAudioP.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        if (!PowerfulPunch)
        {
            play = PunchSFX[UnityEngine.Random.Range(0, PunchSFX.Length)];
        }
        else
        {
            play = MegaPunch;
        }


        PunchAudioP.PlayOneShot(play);
    }

    public LayerMask WutsEnemy;
    public BoxCollider2D VolumeFreeFlowCombat;

    public bool EnemyRecivedHit;
    private float SFXCDFREEFLOWCD = 1.2f;
    private float LastCalledSFXCDFREEFLOW;
    public float StickDirection;
    public IEnumerator PunchFreeFlow()
    {

        // Add things for collision
        //tempListEnems.Append() / <--- NOO dont do this crap Actually <--- Actually YESSSS DO SUM LIKE THISSS!!!!
        // Create a fake origin point from where the player jumps
        //Then find that angle Math.Atan2(origin y - Book y / origin x - Book x) to get the angle
        // find trig ratios of enemy and player
        // X = Math.Sin(angle) * AttachSpeed;
        // Y = Math.Cos(angle) * AttachSpeed;
        // Tell Player to Then Travel to Enemy, Player.transform += new Vector thing(x,y,0) and i think time.deltatime???
        rb.rotation = 0;
        // Make dat Rayyyy 
        float tempRayDistance = 0;
        if (isInPerfectDodge)
        {
            tempRayDistance = 999;
        }
        else
        {
            tempRayDistance = RayDistance;
        }
        if (RaycastableObj != null)
        {
            Vector2 rayDirection = new Vector2(StickDirection, 0f);
            RaycastHit2D raycast = Physics2D.Raycast(RaycastableObj.transform.position, rayDirection, tempRayDistance, WhatsTheBookLayer);
            Debug.DrawRay(RaycastableObj.transform.position, rayDirection * RayDistance, Color.red);
            BoxCollider2D punchCollision = MeleeCollider.gameObject.GetComponent<BoxCollider2D>();
            if (!BackFlip && !isInPerfectDodge)
            {
                punchCollision.enabled = true;

            }
            else if (BackFlip && isInPerfectDodge)
            {
                punchCollision.enabled = true;

            }


            if (raycast.collider != null)
            {
                RaycastHit2D[] raycasts = Physics2D.RaycastAll(RaycastableObj.transform.position, rayDirection, RayDistance, WhatsTheBookLayer);
                List<Transform> list = new List<Transform>();
                List<float> listDis = new List<float>();
                float MaxDistance = float.MinValue;
                int IndexForGreatest = 0;
                GameObject EmptyBook = raycast.collider.gameObject;
                foreach (RaycastHit2D hit in raycasts)
                {
                    list.Add(raycast.collider.gameObject.transform);
                    Debug.Log("Yo we got" + list.Count + "Raycast Hits Whoo");

                }

                if (list.Count > 1)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        float TempDis = Vector2.Distance(transform.position, list[i].position);
                        Debug.Log("Distance To Enemy From Launching FreeFlow" + TempDis);
                        listDis.Add(TempDis);
                    }
                    for (int i = 0; i < listDis.Count; i++)
                    {
                        if (listDis[i] > MaxDistance)
                        {
                            MaxDistance = listDis[i];
                            IndexForGreatest = i;
                        }
                    }

                    EmptyBook = list[IndexForGreatest].gameObject;
                    list.Clear();
                    listDis.Clear();
                    IndexForGreatest = 0;

                }
                else
                {
                    EmptyBook = raycast.collider.gameObject;
                    list.Clear();
                    listDis.Clear();
                    IndexForGreatest = 0;
                }
                Debug.Log("Colliison:" + raycast.collider.gameObject);
                GameObject book = EmptyBook;
                float distance = Vector2.Distance(transform.position, book.transform.position);
                Debug.Log("Distance To Enemy While FreeFlow" + distance);
                BoxCollider2D BookCollider = book.GetComponent<BoxCollider2D>();
                // Check if he can dash only if facing him with out this comabat jus spam and spam >:( // i mean it kinda still is 
                float varfordirec = (book.transform.position.x - transform.position.x);
                while (distance >= 11 && StickHP.PlayerHP > 0)
                {
                    Grounded = false;

                    BoxCollider2D bxcd = GetComponent<BoxCollider2D>();
                    BoxCollider2D BookBxcd = book.GetComponent<BoxCollider2D>();

                    if (BookBxcd != null && bxcd.IsTouching(BookBxcd))
                    {
                        StickAnim.SetBool("Attach", false);
                        StickAnim.CrossFade("Punch", 0, 0);
                        HeCanMove = true;
                        AlrAttaching = false;
                        LastAttachFreeFlow = Time.time;
                        break;
                    }
                    DistanceToBook = distance;
                    if (distance >= 11)
                    {
                        rb.rotation = 0;
                        StickAnim.SetBool("Attach", true);

                        if (Time.time >= SFXCDFREEFLOWCD + LastCalledSFXCDFREEFLOW)
                        {
                            LastCalledSFXCDFREEFLOW = Time.time;
                            AudioPlayer.PlayOneShot(DashSFX);

                        }
                        HeCanMove = false;
                        isInPunchAttach = true;
                        distance = Vector2.Distance(transform.position, book.transform.position);
                        Vector3 PlayerPos = transform.position;
                        Vector3 BookPos = book.transform.position;
                        Debug.Log("YOOO BOOK Got detected by ray");
                        float angle = Mathf.Atan2(PlayerPos.y - BookPos.y, PlayerPos.x - BookPos.x);

                        float x = Mathf.Cos(angle) * 350;
                        float y = Mathf.Sin(angle) * 350;
                        GameObject DashSmokeClone = ObjectPooler.SpawnObj(DashSmoke.gameObject,DashSmoke.transform.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);
                        Vector3 MovementThing = new Vector3(x, y, 0f) * Time.deltaTime;
                        motionBlurHandler.HandleMotionBlur();
                        transform.position -= MovementThing;
                        if (StickCollider.IsTouching(BookCollider))
                        {
                            StickAnim.SetBool("Attach", false);
                            StickAnim.CrossFade("Punch", 0, 0);
                            HeCanMove = true;
                            TimeManager.FreezeTime(0.1f);

                            break;
                        }
                        if (BookPos.x < transform.position.x)
                        {
                            FacingRight = false;
                            transform.localScale = new Vector3(-1, 1, 1);

                        }
                        else if (BookPos.x > transform.position.x)
                        {
                            FacingRight = true;
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                        distance = Vector2.Distance(transform.position, BookPos);
                        if (distance < 12)
                        {
                            StickAnim.SetBool("Attach", false);
                            StickAnim.CrossFade("Punch", 0, 0);
                            HeCanMove = true;
                            AlrAttaching = false;
                            LastAttachFreeFlow = Time.time;
                            TimeManager.FreezeTime(0.1f);

                            break;
                        }

                        yield return null;
                    }
                    else if (distance <= 11)
                    {
                        StickAnim.SetBool("Attach", false);
                        StickAnim.CrossFade("Punch", 0, 0);
                        HeCanMove = true;
                        AlrAttaching = false;
                        LastAttachFreeFlow = Time.time;
                        TimeManager.FreezeTime(0.1f);

                        break;
                    }
                    else
                    {
                        LastAttachFreeFlow = Time.time;
                        StickAnim.SetBool("Attach", false);
                        StickAnim.CrossFade("Punch", 0, 0);
                        TimeManager.FreezeTime(0.1f);

                        AlrAttaching = false;
                        break;
                    }
                    StickAnim.SetBool("Attach", false);
                    yield return null;

                }
                list.Clear();
                listDis.Clear();
                IndexForGreatest = 0;
            }




        }
        else
        {
            LastAttachFreeFlow = Time.time;
            StickAnim.SetBool("Attach", false);
            StickAnim.SetTrigger("Punch");
            HeCanMove = true;
            yield break;

        }
        HeCanMove = true;
        EnemyRecivedHit = false;
        StickAnim.SetBool("Attach", false);



    }
    public IEnumerator PunchDecay()
    {
        Debug.Log("DecayPunch");
        yield return new WaitForSeconds(1.3f);

        StickAnim.speed = 1;
        PunchBool = false;
        PunchAnimationSpeed = 0;

    }

    public void Animation_KeysIsinPunchAtttachFalse()
    {
        isInPunchAttach = false;

    }

    public void IsEQEND()
    {
        EarthQ = false;
    }

    public void DashSFXPLAY()
    {
        AudioPlayer.PlayOneShot(DashSFX);
    }

    private bool PressedDash;
    public LayerMask StickLayer;
    public IEnumerator Dash()
    {
        AirBackFlip = false;
        StickCollider.enabled = true;
        IsStunned = false;
        float NecessaryAmount;
        if (BreakIn)
        {
            NecessaryAmount = 2.1f;
        }
        else
        {
            NecessaryAmount = 8.5f;
        }

        if (StickHP.MegaCharge > NecessaryAmount)
        {
            GameObject DashSmokeClone = ObjectPooler.SpawnObj(DashSmoke.gameObject,DashSmoke.transform.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);
            StartCoroutine(StickHP.DelayinMegaCharge());

            if (HeliFinale && HeliLadder != null)
            {
                StickAnim.SetBool("Dash", true);
                canDash = false;
                isDashing = true;
                //dashPower = dashPower * (MaxSpeed / dashPower + 0.5f);
                // Physics2D.IgnoreLayerCollision(StickLayer,WhatsTheBookLayer,true);
                float originalGravity = rb.gravityScale;
                rb.gravityScale = 0f;
                float DisToLadder = Vector2.Distance(transform.position, HeliLadder.position);
                DashDisDebug = DisToLadder;
                Debug.Log("DashDis" + DisToLadder);
                while (DisToLadder > 10)
                {
                    DisToLadder = Vector2.Distance(transform.position, HeliLadder.position);
                    DashDisDebug = DisToLadder;
                    Debug.Log("DashDis" + DisToLadder);

                    transform.position = Vector2.MoveTowards(transform.position, HeliLadder.position, 260 * Time.deltaTime);
                    yield return null;
                }
                StickAnim.SetBool("Dash", false);
                OnLadder = true;
                isFallingHeli = false;
                canDash = true;
                yield break;

            }
            else if (HeCanMove == true)
            {
                MaxSpeed += 1.3f;
                StickHP.MegaCharge = StickHP.MegaCharge - NecessaryAmount;
                StickAnim.SetBool("Dash", true);
                canDash = false;
                isDashing = true;
                //dashPower = dashPower * (MaxSpeed / dashPower + 0.5f);
                // Physics2D.IgnoreLayerCollision(StickLayer,WhatsTheBookLayer,true);
                float originalGravity = rb.gravityScale;
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);
                yield return new WaitForSeconds(dashTime);
                rb.gravityScale = originalGravity;
                isDashing = false;
                yield return new WaitForSeconds(dashCD);
                canDash = true;
                StickAnim.SetBool("Dash", false);
                Debug.Log("Dash" + dashPower);
                // Physics2D.IgnoreLayerCollision(StickLayer, WhatsTheBookLayer, false);

            }
            else
            {
                PressedDash = true;
                StickHP.MegaCharge = StickHP.MegaCharge - StickHP.MegaCharge;
                HeCanMove = true;
                // Physics2D.IgnoreLayerCollision(StickLayer, WhatsTheBookLayer, true);
                StickAnim.SetBool("Dash", true);
                canDash = false;
                isDashing = true;
                float originalGravity = rb.gravityScale;
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
                yield return new WaitForSeconds(dashTime);
                rb.gravityScale = originalGravity;
                isDashing = false;
                yield return new WaitForSeconds(dashCD);
                canDash = true;
                StickAnim.SetBool("Dash", false);
                PressedDash = false;
                // Physics2D.IgnoreLayerCollision(StickLayer, WhatsTheBookLayer, false);

            }




        }
        else
        {
            AudioPlayer.PlayOneShot(Fail);
        }






    }




    private bool AlrMuzzle;
    private Shake_AnimationKey shake_AnimationKey;
    public IEnumerator MuzzleFireAnim()
    {
        if(shake_AnimationKey == null)
        {
            shake_AnimationKey = GetComponent<Shake_AnimationKey>();

        }
        if(shake_AnimationKey != null)
        {
            shake_AnimationKey.ShakeWithSpecificGivenAmount(15.5f,0.3f);
        }
        GameObject FX = Instantiate(MuzzleFire, transform.position, Quaternion.identity);
        FX.transform.localScale = new Vector3(FacingRight ? 1 : -1, 1, 1);
        FX.transform.position = transform.position;
        //rb.gravityScale = slideMomentum >= MaxSlideSpeed ? 4 : 10;
        //MuzzleFire.transform.localScale = new Vector3(FacingRight ? 1 : -1, 1, 1);


        //if (FacingRight)
        //{
        //    MuzzleFire.transform.localScale = new Vector3(1, 1, 1);
        //}
        //else
        //{
        //    MuzzleFire.transform.localScale = new Vector3(-1, 1, 1);

        //}

        //MuzzleFire.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.4f);
        //MuzzleFire.gameObject.SetActive(false);
        inCombo = false;


    }


    public void InSlideAnimationTrue()
    {
        InSlideAnimation = true;
    }


    public void Footsteps()
    {
        if (WalkOnGrass && Grounded && !isSliding && !isDashing && !BackFlip && !Dead)
        {
            if (BreakIn)
            {
                FootPMetal.pitch = Random.Range(1, 1.2f);
                FootPMetal.gameObject.SetActive(true);
            }
            else
            {
                FootP.pitch = Random.Range(1, 1.2f);
                FootP.gameObject.SetActive(true);
                WoodP.gameObject.SetActive(false);

            }


        }
        if (WalkOnWood && Grounded && !isSliding && !isDashing && !BackFlip && !Dead)
        {
            WoodP.pitch = Random.Range(1, 1.2f);
            WoodP.gameObject.SetActive(true);
            FootP.gameObject.SetActive(false);

        }




    }


    public void StopFootsteps()
    {
        if (BreakIn)
        {
            FootPMetal.pitch = Random.Range(1, 1.2f);
            FootPMetal.gameObject.SetActive(false);
        }
        FootP.gameObject.SetActive(false);
        WoodP.gameObject.SetActive(false);
    }

    public void StopPunchInMiddle()
    {
        if (CanDoublePunch == false)
        {

            StickAnim.SetTrigger("Idle");
        }
        else
        {
            Debug.Log("uhhh NO");
        }
    }

    public void HidingTrue()
    {

        Hiding = true;
    }



    //Slide Speed Voids



    public void SlideSpeed40()
    {
        SlideCancelSpeed = 40;
    }

    public void SlideSpeed80()
    {
        SlideCancelSpeed = 80;
    }


    public void Gravity0()
    {
        rb.gravityScale = 0;
        rb.mass = 0.01f;
    }

    public void GravityNormal()
    {
        if (!SlideJump && !isJumping && !JumpDodge)
        {
            rb.gravityScale = 10.55f;
            rb.mass = 76.55f;
        }


    }

    public void LowGravity()
    {
        rb.gravityScale = 10.55f;
        rb.mass = 16.55f;
    }

    public void HeavyGravity()
    {
        rb.gravityScale = 100;
    }









    //Movement Overhaul


    public IEnumerator BackFlipMove()
    {
        //Debug.Log("We Want A Flip");
        float horizontalMomentum = 0;


        if (StickHP.MegaCharge >= 1.3f && !isSliding)
        {


            // CamShake stuff
            if (shake_AnimationKey == null)
            {
                shake_AnimationKey = GetComponent<Shake_AnimationKey>();

            }
            if (shake_AnimationKey != null)
            {
                if (!Grounded)
                {
                    shake_AnimationKey.ShakeWithSpecificGivenAmount(13.5f, 0.2f);

                }
                else if (MaxSpeed >= 100)
                {
                    shake_AnimationKey.ShakeWithSpecificGivenAmount(25, 0.2f);

                }
                
                else
                {
                    shake_AnimationKey.ShakeWithSpecificGivenAmount(18.5f, 0.2f);

                }
            }

            if (JumpSmoke != null)
        {
            ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);
           // StartCoroutine(CameraScript.ShakeCamGroundFeeling(14,0.02f));

        }
            StickAnim.SetBool("Backflip", true);
            if(catchWindow == 0)
            {
                StickAnim.CrossFade("Backflip", 0, 0);

            }
            if (StickHP.MegaCharge <= 5)
            {
                horizontalMomentum = 10;
            }
            else
            {
                horizontalMomentum = 50;
            }
            StartCoroutine(StickHP.DelayinMegaCharge());

            Grounded = false;
            StickHP.MegaCharge -= 1.2f;
            BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
            BackFlipAS.PlayOneShot(BackFlipSFX);
            LastBackflipTime = Time.time;
            BackFlip = true;
            if (!FacingRight)
            {
                horizontalMomentum = horizontalMomentum * -1;
            }
            rb.velocity = new Vector2(transform.localScale.x * ((horizontalMomentum)), BackFlipForce);
            float flipTime = 0.7f;
            float elapsedTime = 0f;
            while (flipTime > elapsedTime && !InHeliCineBeatdown)
            {
                //Debug.Log("We Want A Flip Flippppp");

                motionBlurHandler.HandleMotionBlur();
                HeCanMove = false;
                if (StickDirection >= 0)
                {
                   //rb.rotation += 10;
                    rb.velocity = new Vector2(-Mathf.Abs(-horizontalMomentum), rb.velocity.y);
                }
                else
                {
                    //rb.rotation -= 10;
                    rb.velocity = new Vector2(Mathf.Abs(horizontalMomentum), rb.velocity.y);
                }
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            BackFlip = false;
            HeCanMove = true;
            rb.rotation = 0;
            StickAnim.SetBool("Backflip", false);


        }






    }


    //Mostly happens When Hurt
    public IEnumerator FreezeFX()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSeconds(0.07f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

    }
    public void RunSpeedUp()
    {
        if (!isShooting)
        {
            MaxSpeed += 1.5f;
        }

    }

    public void SlowerHim()
    {
        if (MaxSpeed > 40)
        {
            MaxSpeed -= 0.1f;
            SlideCancelSpeed -= 3.5f;
        }
    }

    public void isShootingBool()
    {
        if (!isShooting)
        {
            isShooting = true;
        }


    }





    public void SlowStickABitWhileShoot()
    {
        MaxSpeed -= 2.5f;
    }

    public void FastenStickABitWhileShoot()
    {
        MaxSpeed += 0.5f;
    }


    public void SlideLossMomentum()
    {
        if (SlideCancelSpeed > 100)
        {
            if (!Grounded)
            {
                SlideCancelSpeed -= 5;
            }
            else
            {
                SlideCancelSpeed -= 60;
            }

        }

    }


    public void SlideBuildUP()
    {

        //SlideCancelSpeed = SlideCancelSpeed + (MaxSpeed / SlideCancelSpeed);
        if (Speed > 45)
        {
            JumpForce += 0.1f;
            StickHP.MegaCharge = StickHP.MegaCharge + 0.000001f;
            float time2 = 0;
            time2 = time2 + 3;
            SlideCancelSpeed = SlideCancelSpeed + 1.8f;
        }


    }


    public void FallingBool()
    {
        IsFalling = true;
    }


    public void NotFallingBool()
    {
        IsFalling = false;
    }


    public void isShootingBoolFalse()
    {
        isShooting = false;
        MaxSpeed = MaxSpeed + 20;

    }



    private bool ForcedJumpMegaHurtBool;
    public void ForcedJumpMegaHurt()
    {
        StickAnim.CrossFade("Stick_Mega_Hurt", 0, 0);
        if (JumpSmoke != null)
        {
            ObjectPooler.SpawnObj(JumpSmoke.gameObject, JumpSmoke.gameObject.transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

        }
        Grounded = false;
        coyoteTimerCounter = 0;
        BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
        BackFlipAS.PlayOneShot(BackFlipSFX);
        rb.gravityScale = 20;
        isJumping = true;

        if (rb.velocity.magnitude > 5.1f && MaxSpeed >= 40 && StickHP.MegaCharge > 1)
        {
            motionBlurHandler.HandleMotionBlur();
        }
        if (!InSlideAnimation && !SlideJump)
        {
            if (StickHP.MegaCharge > 1)
            {
                if (Checkpoint != ExamHeliID_Checkpoint || HeliForestCheck != null)
                {
                    JumpForce = Mathf.Max(25, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));

                }
                else
                {
                    JumpForce = 90;

                }
                MaxSpeed += 0.4f;
                StickHP.MegaCharge -= 2.5f;
                //StickHP.StartCoroutine(StickHP.DelayinMegaCharge());
            }
            else
            {
                float TempJumpForce = Mathf.Max(25, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));
                float MinimizedForce = TempJumpForce / 2;
                JumpForce = MinimizedForce;
                MaxSpeed -= 0.7f;


            }
            Debug.Log("Jump Void()");

            if (AbleToJump && !isSliding)
            {
                Debug.Log("Jump Void Is Active");
                if (!isDashing && !isSliding && !IsShooting && !Dead && !inSlipperyState && !Anim_Locked && !BackFlip)
                {
                    StickAnim.CrossFade("Jump", 0, 0);

                }
                else if (!BackFlip)
                {

                    StickAnim.SetBool("Jump", true);

                }

                if (JumpForce >= 50)
                {
                    JumpForce = 50; // this is how we limit the jump to not make it crazy
                }
                float horizontalMomentum = rb.velocity.x;
                rb.velocity = new Vector2(horizontalMomentum, JumpForce);

                Grounded = false;
            }
        }
    }

    public void InSlideAnimationFalse()
    {
        InSlideAnimation = false;
    }


    private float SlideTime;
    private float SlideDuration;
    public IEnumerator Slide()
    {



        if (StickHP.PlayerHP > 0)
        {
            float NecessaryAmount;
            if (BreakIn)
            {
                NecessaryAmount = 2.1f;
            }
            else
            {
                NecessaryAmount = 3.5f;
            }


            if (HeCanMove)
            {
                AudioPlayer.PlayOneShot(SlideSFX);
                StartCoroutine(StickHP.DelayinMegaCharge());

                if (rb.velocity.magnitude > 0.1f)
                {
                    motionBlurHandler.HandleMotionBlur();
                }

                StickAnim.SetBool("Slide", true);
                canSlide = false;
                isSliding = true;
                StickHP.MegaCharge -= NecessaryAmount;
                float slideMomentum = SlideCancelSpeed;
                float originalGravity = rb.gravityScale;
                rb.gravityScale = SlideGravity;

                rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);

                while (isSliding)
                {
                    // Shake the cam
                    if (shake_AnimationKey == null)
                    {
                        shake_AnimationKey = GetComponent<Shake_AnimationKey>();

                    }
                    if (shake_AnimationKey != null)
                    {
                        if (!Grounded)
                        {
                            shake_AnimationKey.ShakeWithSpecificGivenAmount(13.5f, 0.12f);

                        }
                        else if (SlideCancelSpeed >= 100)
                        {
                            shake_AnimationKey.ShakeWithSpecificGivenAmount(15, 0.1f);

                        }

                        else
                        {
                            shake_AnimationKey.ShakeWithSpecificGivenAmount(8.5f, 0.1f);

                        }
                    }


                    SlideSmoke.gameObject.SetActive(true);
                    if(FootP.clip == GrassFootSFX && BreakIn) // TODO: Replace the break in var with a bool called on grass on wood on metal etc to differentiate surfaces
                    {
                        GrassBlades.gameObject.SetActive(true);
                        GrassBlades.gameObject.transform.localScale = new Vector3 (GrassBlades.gameObject.transform.localScale.x * transform.localScale.x,GrassBlades.transform.localScale.y,GrassBlades.transform.localScale.z);
                    }
                   
                    SlideMomentum = slideMomentum;

                    SlopeSlide = false;

                    if (!inRush && slideMomentum > 0)
                    {
                        slideMomentum -= 90 * Time.deltaTime;
                    }

                    rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);

                    if (Input.GetKeyDown(KeyCode.Space) && AbleToJump && !isInPerfectDodge)
                    {
                        if (isSliding && Grounded)
                        {
                            StickAnim.SetBool("Slide", false); // Reset slide animation
                            StickAnim.SetBool("Jump", true);  // Trigger jump animation

                            rb.gravityScale = slideMomentum >= MaxSlideSpeed ? 4 : 10;

                            float jumpVelocityX = slideMomentum * Mathf.Sign(transform.localScale.x);

                            isSliding = false;
                            SlideJump = true;

                            yield return new WaitForSeconds(0.2f); // Prevent immediate reset
                            SlideJump = false;
                            break;
                        }
                    }
                    if (slideMomentum <= 0.1f)
                    {
                        isSliding = false;
                        StickAnim.SetBool("Slide", false);
                        rb.gravityScale = originalGravity;
                        GrassBlades.gameObject.SetActive(false);

                        break;
                    }



                    yield return null;
                }


            }
            else if (HeCanMove == false)
            {
                AudioPlayer.PlayOneShot(SlideSFX);

                // if (rb.velocity.magnitude > 0.1f)
                // {
                //     motionBlurHandler.HandleMotionBlur();
                // }

                StickAnim.SetBool("Slide", true);
                canSlide = false;
                isSliding = true;
                float slideMomentum = SlideCancelSpeed;
                float originalGravity = rb.gravityScale;
                rb.gravityScale = SlideGravity;

                rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);

                while (isSliding)
                {

                    if (!SlideVolume)
                    {

                        SlopeSlide = false;


                        if (!inRush)
                        {
                            slideMomentum -= SlideCancelSpeed * (MaxSpeed / SlideCancelSpeed) * Time.deltaTime;

                        }
                        else
                        {
                            slideMomentum += SlideCancelSpeed * (MaxSpeed / SlideCancelSpeed) * Time.deltaTime;

                        }
                        rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);

                        if (Input.GetKeyDown(KeyCode.Space) && AbleToJump)
                        {
                            if (isSliding && Grounded)
                            {
                                StickAnim.SetBool("Slide", false); // Reset slide animation
                                StickAnim.SetBool("Jump", true);  // Trigger jump animation

                                rb.gravityScale = slideMomentum >= MaxSlideSpeed ? 4 : 10;

                                float jumpVelocityX = slideMomentum * Mathf.Sign(transform.localScale.x);
                                rb.velocity = new Vector2(jumpVelocityX, JumpForce);

                                isSliding = false;
                                SlideJump = true;

                                yield return new WaitForSeconds(0.2f); // Prevent immediate reset
                                SlideJump = false;
                                break;
                            }
                        }

                    }
                    else
                    {

                        SlopeSlide = true;

                        float MaxSlideMomentum = 70;


                        slideMomentum += SlideCancelSpeed * Time.deltaTime;
                        rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);
                        slideMomentum = Mathf.Min(slideMomentum, MaxSlideMomentum);

                        if (Input.GetKeyDown(KeyCode.Space) && AbleToJump)
                        {
                            if (isSliding && Grounded)
                            {
                                if (slideMomentum >= MaxSlideMomentum)
                                {
                                    rb.gravityScale = 4;

                                }
                                float targetspeed = slideMomentum * Speed;

                                float speeddif = targetspeed - rb.velocity.x;

                                //float accelrate = (Mathf.Abs(targetspeed) > 0.01) ? Acceleration : Deacceleration;

                                float slideVelocityX = rb.velocity.x;


                                rb.velocity = new Vector2(slideVelocityX, JumpForce);

                                StickAnim.SetBool("Jump", true);

                                Grounded = false;
                                SlideJump = true;
                                yield return new WaitForSeconds(0.2f);

                                SlideJump = false;
                                isSliding = false;
                                break;
                            }
                        }

                        //if (slideMomentum > 200)
                        //{
                        //    slideMomentum = 200;
                        //}
                    }





                    if (!SlideVolume)
                    {
                        slideMomentum = Mathf.Max(0, slideMomentum - SlideCancelSpeed * Time.deltaTime);
                        rb.velocity = new Vector2(slideMomentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);
                    }

                    yield return null;
                }
            }

        }
        else
        {
            AudioPlayer.PlayOneShot(Fail);
        }
    }




    // Saving

    public IEnumerator ReloadGameScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public IEnumerator ImmediateReloadGameScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void PlayPunchSFX4Stealth()
    {
        AudioPlayer.PlayOneShot(PunchSFXStealth);
    }


    public void MegaHeal_OrbSpawnSFX()
    {
        AudioPlayer.PlayOneShot(OrbSpawn);
    }
    public void MegaHealSFX()
    {
        AudioPlayer.PlayOneShot(StickHP.HealSFX);
        StickHP.PlayerHP = 100;
    }

    public void MegaHealStoppper()
    {
        MegaHeal = false;

    }

    private bool PlayedFallSFX;
    public IEnumerator StealthKill(Transform EnemyPos)
    {
        MainCamAnim.SetBool("Zoom", true);
        AudioPlayer.PlayOneShot(FallingSFX);
        StickAnim.SetTrigger("StealthKill");
        Debug.Log("Called Stealth Kill");
        float distanceToBook = Vector2.Distance(transform.position, EnemyPos.position);
        InStealth = false;
        InStealthKill = true;

        //Vector2 direction = (EnemyPos.position - transform.position).normalized;
        //rb.velocity = (direction * Speed);

        while (distanceToBook > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, EnemyPos.position, Speed * Time.deltaTime);
            yield return null;




        }


        PlayedFallSFX = false;
        InStealthKill = false;






    }


    public void SaveProgress()
    {
        SaveSystem.SaveProgess(this);
    }
    public void TellThat1stDone()
    {
        HasFullyWatchedFirst = true;
        SaveProgress();
    }

    public void LoadProgress()
    {
        PlayerData data = SaveSystem.LoadSave();
        HasGun = data.HasGun;
        SceneName = data.SceneName;
        Checkpoint = data.Checkpoint;
        Cutscene_IntroFight = data.Cutscene_IntroFight;
        //Positions
        Vector3 position;
        position.x = data.PlayerPOS[0];
        position.y = data.PlayerPOS[1];
        position.z = data.PlayerPOS[2];
        FinshedEncounterAfterLab = data.HasFinishedEncounterAfterLab;
        transform.position = position;
        HasBrokeOutOfRope = data.HasBrokeOutOfRope;


        // Collectibles and Paper
        BackflipCollected = data.Backflip;
        InfoCollected = data.InfoCollect;
        NoMoreCollected = data.NoMoreCollect;

        //Cutscenes
        NeedsToViewDirecFirstIntro = data.NeedsToViewDirecFirstIntro;
        HasFullyWatchedFirst = data.HasFullyWatchedFirst;
        SeenGunDirections = data.SeenGunDirections;

        //Bosses
        HasKilledEmbry = data.HasKilledEmbry;
        HasKilledEcology = data.HasKilledEcology;

        //Abilities
        UnlockedBackflip = data.UnlockedBackflip;
        UnlockedDash = data.UnlockedDash;
        UnlockedSlide = data.UnlockedSlide;

    }

    public void OverrideSave_Position(float x, float y, float z)
    {
        // Overide Position
        transform.position = new Vector3(x, y, z);
        SaveProgress();
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }


    public void AbilityInputHandler()
    {
          if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !Dead && !BackFlip && !isInPerfectDodge && !OnLadder && UnlockedDash|| Input.GetKeyDown(KeyCode.LeftShift) && canDash && HeliFinale && !OnLadder)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.C) && UnlockedSlide)
        {
            if (StickHP.PlayerHP > 0 && canSlide == true)
            {
                StartCoroutine(Slide());

            }
        }

         if (Input.GetKeyDown(KeyCode.Q) && UnlockedBackflip || Input.GetKeyDown(KeyCode.Q) && inQTE)
        {
            if (InHeliCineBeatdown)
            {
                StickAnim.SetTrigger("Dodge");

                StickAnim.CrossFade("CrazySigmaDodgeYOOOOOO", 0, 0);

            }
            else
            {
                StickAnim.SetTrigger("Dodge");

                if (PerfectDodgeCount == MaxDodge && LockedIN && !inRush)
                {
                    StartedDecay = false;
                    StartCoroutine(PerfectDodge_SloMO());
                }
                else if (StickHP.PlayerHP > 0 && Time.time >= LastBackFlip + BackFlipCD && !inSlipperyState && !inCombo && !isJumping && Grounded && !inRush)
                {
                    LastBackFlip = Time.time;
                    StartCoroutine(BackFlipMove());

                }
                else if (StickHP.PlayerHP > 0 && Time.time >= LastBackFlip + BackFlipCD && !inSlipperyState && !inCombo && !AirBackFlip && !inRush)
                {
                    AirBackFlip = true;
                    LastBackFlip = Time.time;
                    StartCoroutine(BackFlipMove());
                }
                //else if (StickHP.PlayerHP > 0 && Time.time >= LastBackFlip + BackFlipCD && isJumping && !inSlipperyState)
                //{
                //    isJumping = false;
                //    LastBackFlip = Time.time;
                //    StartCoroutine(BackFlipMove());
                //}

            }




        }


    }

    public bool Falling_KnockBack;

    public IEnumerator PlayerKnockBack(float horizontalForce, float verticalForce, float duration)
    {
        Debug.Log("PlayerKnockBack Was Called!!");
        float timer = 0f;

        float dir = transform.localScale.x > 0 ? 1f : -1f;

        float timeout = 2;
        while (timer < duration && timer <timeout)
        {
            Debug.Log("Finishing up the timer part in Knockback ");

            Falling_KnockBack = true;
            StickAnim.CrossFade("StickKnockBack", 0, 0);
            rb.velocity = new Vector2(horizontalForce * -dir, verticalForce);
            timer += Time.deltaTime;
            yield return null;
        }
        if(timer >= timeout)
        {
            Debug.Log("unsucessfully Knockbacked Player Had to timeout ");

        }
        else
        {
            Debug.Log("Sucessfully Knockbacked Player ");

        }

        Falling_KnockBack = false;
        StickAnim.SetBool("KnockedBack", false);
        //rb.velocity = Vector2.zero;
    }

    public bool inPlayerKnockbackAlt;


    private float LastForceJumpKnockBack = 0;
    private float ForceJumpKnockBackCD = 0.2f;
    public bool isinFriction;
    public bool isPlayerKnockbackFinished { get; private set; }

    public IEnumerator PlayerKnockBackAltered(float horizontalForce, float verticalForce, float duration)
    {
        isPlayerKnockbackFinished = false;

        isinFriction = false;
        inPlayerKnockbackAlt = true;
        if (StickHP.Testing)
        {
            Debug.Log("PlayerKnockBack Was Called!!");

        }
        float timer = 0f;

        float dir = transform.localScale.x > 0 ? 1f : -1f;


        while (timer < duration)
        {
            StickCollider.enabled = false;
            AlternateColliderDuringKnockback.enabled = true;
            if (StickHP.PlayerHP <= 0)
            {
                yield break;
            }
            //if (!InKnockbackTimeoutCor)
            //{
            //   break;
            //}
            Falling_KnockBack = true;
            StickAnim.CrossFade("StickKnockBack", 0, 0);
            rb.velocity = new Vector2(horizontalForce * -dir, rb.velocity.y);
            timer += Time.deltaTime;
            if (GetDistanceToGround() <= 1.5f && Time.time >= ForceJumpKnockBackCD + LastForceJumpKnockBack)
            {
                LastForceJumpKnockBack = Time.time;
                //rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                float TempJumpForce = Mathf.Max(25, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));
                float MinimizedForce = TempJumpForce / 2;
                shake_AnimationKey.ShakeWithSpecificGivenAmount(90.5f, 0.1f);
                JumpForce = MinimizedForce;
                float horizontalMomentum = rb.velocity.x;
                rb.velocity = new Vector2(horizontalMomentum, JumpForce);
                BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
                ObjectPooler.SpawnObj(GroundImpactFX.gameObject, LastGroundPos,Quaternion.identity,ObjectPooler.PoolType.Partcles);
                BackFlipAS.PlayOneShot(GroundImpact);
            }
            yield return null;
            inPlayerKnockbackAlt = false;
            isPlayerKnockbackFinished = true;  

        }
        AlternateColliderDuringKnockback.enabled = false;
        StickCollider.enabled = true;
        isinFriction = true;

        yield return new WaitForSecondsRealtime(0.2f);
        float Momentum = 160;

        ObjectPooler.SpawnObj(GroundImpactFX.gameObject, LastGroundPos, Quaternion.identity, ObjectPooler.PoolType.Partcles);
        BackFlipAS.PlayOneShot(GroundImpact);
        float LastFX = 0;
        float FXCD = 0.1f;
        float reducer = 70;
        AudioPlayer.PlayOneShot(SlideSFX);
        while (Momentum > 0)
        {
            StickAnim.SetBool("Friction", true);
            reducer += 5 * Time.deltaTime;
            Momentum -= reducer * Time.deltaTime;
            rb.velocity = new Vector2(Momentum * Mathf.Sign(-transform.localScale.x), rb.velocity.y);
            if(!BackFlip && !JumpDodge && StickHP.PlayerHP > 0)
            {
                StickAnim.CrossFade("StickFriction", 0, 0);
                if (Time.time >= LastFX + FXCD)
                {
                    LastFX = Time.time;
                    shake_AnimationKey.ShakeWithSpecificGivenAmount(20.5f, 0.1f);
                    ObjectPooler.SpawnObj(GroundImpactFXSmoke.gameObject, LastGroundPos, Quaternion.identity, ObjectPooler.PoolType.Partcles);


                }
            }
           




            //float tempVx = rb.velocity.x;
            //float velX = tempVx -= 5 * Time.deltaTime;
            //rb.velocity = new Vector2(rb.velocity.x - 5f * Time.deltaTime, rb.velocity.y);
            yield return null;
        }
        isinFriction = false;
        StickAnim.SetBool("Friction", false);
        if (StickHP.Testing)
        {
            Debug.Log("PlayerKnockBackAltered ");

        }
        inPlayerKnockbackAlt = false;
        yield break;
        //rb.velocity = Vector2.zero;
    }

    private bool isBouncyKnockbackActive = false;

    public bool InKnockbackTimeoutCor;



    //public IEnumerator PlayerKnockbackBouncy2(float horizontalForce, float verticalForce, float durationOfEachKnockback, float duration, float ForceWhenLandedForY)
    //{
    //    if (isBouncyKnockbackActive)
    //        yield break;

    //    isBouncyKnockbackActive = true;
    //    Falling_KnockBack = true;
    //    if (!InKnockbackTimeoutCor)
    //    {
    //        StartCoroutine(TimerKnockbackBouncy(duration));

    //    }
    //    while (InKnockbackTimeoutCor)
    //    {

    //        rb.sharedMaterial = BouncyMat;
    //        StickAnim.CrossFade("StickKnockBack", 0, 0);

    //        if (!inPlayerKnockbackAlt)
    //        {
    //            StartCoroutine(PlayerKnockBackAltered(horizontalForce, verticalForce, durationOfEachKnockback));

    //        }
    //        Debug.Log("if u see this while Knockback ain it");

    //        while (inPlayerKnockbackAlt)
    //        {
    //            Debug.Log("if u see this we yielding it");

    //            yield return null;
    //        }
    //        Debug.Log("if u see this while loop ain it");

    //        float timeoutTimer = 0f;
    //        float groundedWait = 0f;
    //        while (!Grounded && groundedWait < 1.5f)
    //        {
    //            groundedWait += Time.deltaTime;
    //            yield return null;
    //        }
    //        Debug.Log("if u see this 2ND while loop ain it");

    //        Jump();
    //        StickAnim.CrossFade("StickKnockBack", 0, 0);

    //    }

    //    Debug.Log("PlayerKnockbackBouncy finished all iterations");

    //    rb.sharedMaterial = StickmanMat;
    //    Falling_KnockBack = false;

    //    float finalTimeout = 0f;
    //    while (!Grounded)
    //    {
    //        finalTimeout += Time.deltaTime;
    //        if (finalTimeout > 3f)
    //        {
    //            Debug.LogWarning("Final grounded wait timed out");
    //            isBouncyKnockbackActive = false;
    //            InKnockbackTimeoutCor = false;

    //            break;
    //        }
    //        yield return null;
    //    }

    //    StickAnim.SetBool("KnockedBack", false);
    //    isBouncyKnockbackActive = false;

    //    Debug.Log("PlayerKnockbackBouncy finished all iterations");
    //    yield break;
    //}


    public IEnumerator PlayerKnockbackBouncy(float horizontalForce, float verticalForce, float durationOfEachKnockback, float duration, float ForceWhenLandedForY)
    {

        StartCoroutine(PlayerKnockBackAltered(horizontalForce, verticalForce, durationOfEachKnockback));

       // float timer = 0;
       // //rb.sharedMaterial = BouncyMat;
      

       // while (timer < duration)
       // {
            
       //     StickAnim.CrossFade("StickKnockBack", 0, 0);

       //     timer += 0.02f;


       //     //Debug.Log("We  finished yielding");

       //     if(GetDistanceToGround() <= 10)
       //     {
       //         //rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

       //         Jump();
       //     }
            
       //     yield return null;
       // }
       //// rb.sharedMaterial = StickmanMat;

       // Debug.Log("PlayerKnockbackBouncy finished all iterations");



        yield break;
    }

    public IEnumerator TimerKnockbackBouncy(float Giventimer)
    {
        float timer = 0;

        while(timer < Giventimer)
        {
            timer += Time.deltaTime;
            InKnockbackTimeoutCor = true;
            yield return null;

        }
        InKnockbackTimeoutCor = false;
        Debug.Log("Timer FInished");
        yield break;

    }


    public float DistanceToGroundRefer;


    private Vector3 LastGroundPos;
    public float GetDistanceToGround()
    {
        float output = 0;
        if (StickCollider.enabled)
        {
            Vector2 origin = (Vector2)transform.position + Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y);
            float maxDistance = 100f;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxDistance, WhatsGround);

            Debug.DrawRay(origin, Vector2.down * maxDistance, Color.red);

            if (hit.collider != null)
            {
                LastGroundPos = new Vector3(hit.point.x, hit.point.y, 0);
                output = hit.distance;
            }
            else
            {
                output = maxDistance;
            }
        }
        else if(AlternateColliderDuringKnockback.enabled)
        {
            Vector2 origin = (Vector2)transform.position + Vector2.down * (AlternateColliderDuringKnockback.bounds.extents.y);
            float maxDistance = 100f;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxDistance, WhatsGround);

            Debug.DrawRay(origin, Vector2.down * maxDistance, Color.red);

            if (hit.collider != null)
            {
                LastGroundPos = new Vector3(hit.point.x, hit.point.y, 0);
                output = hit.distance;
            }
            else
            {
                output = maxDistance;
            }
        }
        return output;
      
    }



    // Manage Physics Glitches 
    // i hate this

    public void PhysicsManagerManual()
    {
        // ts manages phsyics because unity kinda doo doo with its physics calcs in 2D
        if (Grounded)
        {
            ManageVelocityOnGround();
        }
        
        
    }


    private bool JumpBoolAnim()
    {
        bool jump = StickAnim.GetBool("Jump");
        return jump;
    }

    private bool AirAnim()
    {
       
        bool air = StickAnim.GetBool("Airborne");
        return air;

    }

    private bool BackflipAnimBool()
    {
        bool Bf = StickAnim.GetBool("Backflip");
        return Bf;
    }

    public float LastJumpInput;
    private float LastBackflipInput;

    public float JumpInCD = 0.4f;
    public float BackFlipIn = 0.35f;
    private void InputDelay_ion_rlly_know_what_toCall_THIS()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LastJumpInput = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LastBackflipInput = Time.time;

        }
    }

    private bool AirborneInputDelay()
    {
        bool ReturnVal = false;
        if (Time.time >= LastJumpInput + JumpInCD &&
    Time.time >= LastBackflipInput + BackFlipIn)

        {
            ReturnVal = true;
        }
        else
        {
            ReturnVal = false;
        }
        return ReturnVal;
        
    }

    public void ManageVelocityOnGround()
    {

      
        if (CheckIfGrounded() && !JumpBoolAnim() && !AirAnim()  && !BackflipAnimBool() && AirborneInputDelay() && !isDashing && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y, 0));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, WhatsGround);
            if (hit.collider != null)
            {
                float snapY = hit.point.y + 1.4f;
                if (transform.position.y > snapY)
                    transform.position = new Vector2(transform.position.x, snapY);
            }
        }



        // Debug statement
        if (StickHP.Testing)
        {
            if (AirAnim())
            {
                Debug.Log("AirAnim = true");

            }
            else
            {
                Debug.Log("AirAnim = false");
            }

            if (JumpBoolAnim())
            {
                Debug.Log("JumpBoolAnim = true");

            }
            else
            {
                Debug.Log("JumpBoolAnim = false");
            }
        }
        
        return;

    }



}




