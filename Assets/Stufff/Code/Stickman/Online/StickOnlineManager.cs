
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.Netcode;
using Cinemachine;
using TMPro;

public class StickOnlineManager : NetworkBehaviour
{
    public float Speed;
    public float JumpForce;
    public Rigidbody2D rb;
    public Animator StickAnim;
    public bool Grounded;
    public AudioClip Gun;
    public AudioSource AudioPlayer;
    public GameObject GunBullet;
    public GameObject MegaBullet;
    public GameObject GunBulletArc;
    public Transform AimPoint;
    public Transform ArcAimPoint;
    public Transform StickPOSGlitch;
    public Volume PostProces;
    public StickOnlineHealth StickHP;
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
    public MeleeComboSpeed MeleeCollision;



    public bool SlideJump;



    




    public bool IsStunned;

    //Movement
    public float Acceleration;
    public float Deacceleration;
    public bool isJumping;


    public Biology_AttackZone Streak_Bio;



    public float shootCooldown = 0.5f; // Cooldown time between shots
    private float nextShootTime = 0f;



    public bool isShooting;

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

    [Header("Melee Stuff ")]

    // Melee I guess
    public AudioSource PunchAudioP;
    public AudioClip PunchSFX;
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
    public float Maxdelay4Punch;
    private float OG_MeleeCD;


    /// Progression Values


    [Header("PaperCollectables")]
    // Values For PaperCollectables
    public bool BackflipCollected;
    public bool InfoCollected;
    public bool NoMoreCollected;


    [Header(" ")]

    //Performance level destoryer
    public GameObject Level1;
    public GameObject Level2;

    // Footsteps
    public bool WalkOnGrass;
    public AudioSource FootP;
    public AudioSource FootPMetal;
    public AudioClip GrassFootSFX;

    [Header("Countering IG ")]

    // Countering IG
    public bool IsInThrowAnim;
    public bool HasCaught;
    public GameObject EmbryProjectile;
    public Transform EmbryPOS;
    public bool CanCatch;
    public Transform CounterAimPoint;
    public AudioClip ThrowSFX;
    public GameObject ExamBullet;
    public Transform Helicopter;

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
    public float StartPointX;
    public float StartPointY;
    public NetworkVariable<int> StickID = new NetworkVariable<int>();
    
    
    [Header("OnlineStuff")]
    public Ownership ownership1;
    public Camera PlayerCam;
    private bool GavePlayerID;
    public GameObject HitFX;
    public AudioClip HitSFX;
    public GameObject SpectateUI;
    public NetworkVariable<int> Score = new NetworkVariable<int>();
    public TextMeshProUGUI ScoreDisplay;



    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private AudioListener Listen;


   
    void Start()
    {

        StartPointX = transform.position.x;
        StartPointY = transform.position.y;


        Debug.Log($"playerID: {StickID.Value}");
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        PlayerCam = cam.GetComponent<Camera>();
        

        LastULT = -UltCD;

        

        OG_MeleeCD = MeleeCD;



    

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









    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    void FixedUpdate()
    {
        SlopeCheck();

        //Grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private bool AlrDid4Reload;
    public bool FacingRight;

    public override void OnNetworkSpawn()
    {


        

        if (IsOwner)
        {
            StickID.Value = (int)OwnerClientId;
            Listen.enabled = true;
            cam.Priority = 29;
        }
        else
        {
            Listen.enabled = false;
            cam.Priority = 0;
        }
    }

    private void Update()
    {







        if (!GavePlayerID)
        {
            StickID.Value = (int)OwnerClientId;
            GavePlayerID = true;
        }

        if(!IsLocalPlayer)
        {
            gameObject.layer = LayerMask.NameToLayer("Remote");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Local");
        }

        if (!IsOwner) return;

        

      

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
        if (Input.GetKey(KeyCode.A))
        {
            Footsteps();
        }




        transform.localScale = new Vector3(FacingRight ? 0.75f : -0.75f, 0.75f, 0.75f);

        if (Input.GetKey(KeyCode.G))
        {
            if (Time.time >= LastULT + UltCD)
            {
                LastULT = Time.time;
                StickAnim.SetTrigger("Hyper");
            }

        }

        else if (Input.GetKey(KeyCode.A) && Grounded)
        {
            if (IsOwner)
            {
                Footsteps();

            }
        }
        else 
        {
            if (Input.GetKey(KeyCode.D) && Grounded)
            {
                if (IsOwner)
                {
                    Footsteps();
                }
            }
            else
            {
                if (IsOwner)
                {
                    StopFootsteps();
                }

            }
        }

        Grounded = CheckIfGrounded();




        if (lastPunchTime + MeleeCD >= Maxdelay4Punch)
        {
            MeleeCD = OG_MeleeCD;
            ComboCount = 0;
        }


        if (Input.GetButton("Fire2"))
        {
            if (Time.time >= lastPunchTime + MeleeCD)
            {

                MeleeCD = Mathf.Max(OG_MeleeCD - ComboCount, 0.1f);
                MeleeCD = MeleeCD - ComboCount;
                lastPunchTime = Time.time;
                StickAnim.SetTrigger("Punch");


            }
        }



        if (JumpForce >= 55)
        {
            JumpForce = 55;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            FacingRight = true;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            FacingRight = false;
        }


       

  


        //Slidecancel and Move speed min max


        if (JumpForce > 50)
        {
            JumpForce = 50;
        }


        if (dashPower >= 70)
        {
            dashPower = 70;
        }

        if (SlideCancelSpeed < 100)
        {
            SlideCancelSpeed = 100;
        }

        if (SlideCancelSpeed > 220)
        {
            SlideCancelSpeed = 220;
        }

       

        if (MaxSpeed > 100)
        {

            MaxSpeed = 100;

        }





        //Countering KeyBoard

        if (Input.GetKeyDown(KeyCode.R))
        {
            StickAnim.SetTrigger("Catch");
        }

        if (HasCaught)
        {
            HasCaught = false;
            StickAnim.SetTrigger("Caught");
        }




        if (Input.GetKeyDown(KeyCode.C))
        {
            if (StickHP.PlayerHP.Value > 0 && canSlide == true)
            {
                StartCoroutine(Slide());

            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {


            if (StickHP.PlayerHP.Value > 0 && Time.time >= LastBackFlip + BackFlipCD && Grounded)
            {
                isJumping = false;
                LastBackFlip = Time.time;
                StartCoroutine(BackFlipMove());

            }
            else if (StickHP.PlayerHP.Value > 0 && Time.time >= LastBackFlip + BackFlipCD && isJumping)
            {
                isJumping = false;
                LastBackFlip = Time.time;
                StartCoroutine(BackFlipMove());
            }




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

        if (!isSliding)
        {
            StickAnim.SetBool("Slide", false);
        }




        if (EarthQ_Dodge == true)
        {
            StickAnim.SetBool("Jump", false);
            StickAnim.SetBool("DodgeLow", true);
        }


        if (EarthQ_Dodge == false)
        {

            StickAnim.SetBool("DodgeLow", false);
        }






        // dead stuff
        // kill him >:)
        if (IsOwner && StickHP.PlayerHP.Value <= 0)
        {
            if (!AlrDid4DeathCor)
            {
                Speed = 0;
                Instantiate(HitFX, transform.position, Quaternion.identity);
                StartCoroutine(Respawn());
                AlrDid4DeathCor = true;
            }
            StickAnim.SetTrigger("Dead");
        }









        if (StickmanGetsKnocked == false && !SlopeSlide)
        {
            Move();
        }


        if (StickHP.InCombat == false)
        {
            IsStunned = false;
            EarthQ = false;
            StickAnim.SetBool("Stun", false);
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

            isJumping = false;
            StickAnim.SetBool("OnGround", true);

            AbleToJump = true;
            SlideJump = false;

            canSlide = true;

            StickAnim.SetBool("DodgeLow", false);

            StickAnim.SetBool("Jump", false);
            StickAnim.ResetTrigger("JumpShoot");


            JumpDodge = false;

            StickAnim.SetBool("DodgeHigh", false);

            StickAnim.SetBool("Fall", false);
        }

        if (!Grounded)
        {

            StickAnim.SetBool("OnGround", false);
            AbleToJump = false;
            canSlide = false;
            StickAnim.SetBool("Slide", false);
            isSliding = false;

        }

        if (Input.GetKey(KeyCode.Space) && Grounded == true && !isSliding && !SlideJump && !InSlideAnimation)
        {
            if (StickmanGetsKnocked == false && !InSlideAnimation)
            {

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
            Shoot();





        }


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

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
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


    }



    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        if (!IsOwner) yield break;

        StickAnim.ResetTrigger("Dead");
        StickAnim.SetTrigger("Revive");

        StickHP.PlayerHP.Value = 100;
        AlrDid4DeathCor = false;

        Vector3 newPos = transform.position;
        newPos.x = StartPointX;
        newPos.y = StartPointY;
        transform.position = newPos;
    }


    private bool CheckIfGrounded()
    {
        float extraHeight = 0.4f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }


    public void StartCounterCouroutine()
    {
        if (StickHP.MegaCharge > 0)
        {
            StickHP.MegaCharge -= 4;
        }
        if (StickHP.MegaCharge < 0)
        {
            StickHP.MegaCharge = 0;
        }
        AudioPlayer.PlayOneShot(ThrowSFX);
        StartCoroutine(CounterAttack());
    }

    
    public IEnumerator CounterAttack()
    {

        if (InExamBoss)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Vector2 direction = (Helicopter.position - CounterAimPoint.position).normalized;
            GameObject projectile = Instantiate(ExamBullet, CounterAimPoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float enemySpeed = rb.velocity.x;
            float distance = Vector2.Distance(CounterAimPoint.position, Helicopter.position);
            float baseThrowSpeed = 100;
            float velocityX = direction.x * baseThrowSpeed + enemySpeed;
            float arcHeight = 0.5f;
            float velocityY = Mathf.Atan(2 * arcHeight * Mathf.Abs(Physics2D.gravity.y));
            rb.velocity = new Vector2(velocityX, velocityY);

        }

        if (!InExamBoss && !InEmbryBoss)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            GameObject projectile = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);
            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            projectile.GetComponent<Bullet>().SetDirection(shootDirection);



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
            rb.AddTorque(570f);
        }

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


    public void Jump()
    {
        BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
        BackFlipAS.PlayOneShot(BackFlipSFX);

        isJumping = true;

       






        if (!InSlideAnimation && !SlideJump)
        {

            JumpForce = Mathf.Max(30, JumpForce * ((MaxSpeed / JumpForce) / 1.5f));




            MaxSpeed += 0.4f;

            Debug.Log("Jump Void()");

            if (AbleToJump)
            {
                Debug.Log("Jump Void Is Active");

                StickAnim.SetBool("Jump", true);


                float horizontalMomentum = rb.velocity.x;
                rb.velocity = new Vector2(horizontalMomentum, JumpForce);

                Grounded = false;
            }
        }
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








    private IEnumerator FallStick()
    {
        // Reset the Y position of the Stickman
        Vector3 newPOS = StickPOS.position;
        newPOS.y = resetYPosition;
        StickPOS.position = newPOS;

        yield return new WaitForSeconds(0.000000000000001f);
    }

    public void JumpBoolean()
    {
        JumpDodge = true;
    }

    public void JumpBooleanFalse()
    {
        JumpDodge = false;
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

    public void Move()
    {

        if (HeCanMove == true && StickmanGetsKnocked == false)
        {

            //BuildUp = true;
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * Speed, rb.velocity.y);

            float targetspeed = horizontalInput * Speed;

            float speeddif = targetspeed - rb.velocity.x;

            float accelrate = (Mathf.Abs(targetspeed) > 0.01) ? Acceleration : Deacceleration;

         

            if (StickmanGetsKnocked == false)
            {
                if (horizontalInput > 0.01)
                {
                    FacingRight = true;
                    transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                }
                else if (horizontalInput < -0.01)
                {
                    FacingRight = false;
                    transform.localScale = new Vector3(-0.75f, 0.75f, 0.75f);
                }







            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                if (StickHP.PlayerHP.Value > 0 && canSlide == true)
                {
                    BuildUp = true;
                    StartCoroutine(Slide());

                }
            }


            StickAnim.SetBool("Walking", horizontalInput != 0);
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
        StickHP.MegaCharge += 0.3f;
    }

    public void AnimationKey_UltisFalse()
    {
        BattleTheme.gameObject.SetActive(false);

        GustOfBullets = false;

    }

    public void AnimationKey_DisableAndEnableUltTheme()
    {
        MainCamAnim.speed = 3;
        GustOfBullets = true;
        BattleTheme.gameObject.SetActive(false);
        UltTheme.gameObject.SetActive(true);
        Hypothesis.gameObject.SetActive(false);
        LabTheme.gameObject.SetActive(false);
    }

    public void AnimationKey_DisableUltThem()
    {
        MainCamAnim.speed = 1;

        GustOfBullets = false;

       
        UltTheme.gameObject.SetActive(false);
    }


    public void CantMove()
    {
        MaxSpeed = 0;

        if (PressedDash == false)
        {
            HeCanMove = false;
        }
        else
        {
            HeCanMove = true;
        }


    }

    public void CanMove()
    {
        MaxSpeed = 60;
        HeCanMove = true;
    }




    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !IsFalling)
        {
            Grounded = true;
        }


        string otherObjectName = collision.gameObject.name;
        Debug.Log("Collided with: " + otherObjectName);
        if (collision.gameObject.CompareTag("TopBook"))
        {
            Biology bio = GetComponent<Biology>();


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



    public Transform HidePosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hiding"))
        {
            CanHide = true;
        }

        if (collision.CompareTag("StickMelee"))
        {
            StickOnlineManager stick = collision.GetComponentInParent<StickOnlineManager>();
            if(stick.StickID != StickID)
            {
                GotPunch_OhNoo_GetFX();
            }
        }

        if (collision.CompareTag("HidePOS"))
        {
            TouchHiding = true;
            HidePosition = collision.transform;

        }

        if (MeleeCollider.CompareTag("Book"))
        {
            if (Time.time >= lastPunchTime + MeleeCD)
            {
                ComboCount += 1;

            }
        }

        

        if (collision.CompareTag("Safe"))
        {
            if (OneTimeHealth_AlarmDoor == false)
            {
                OneTimeHealth_AlarmDoor = true;
                StickHP.PlayerHP.Value += 15;
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
            else if (!Grounded)
            {
                StickAnim.SetTrigger("JumpShoot");

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

    public void GotShot_OhNoo_GetFX()
    {
        GameObject FX = Instantiate(HitFX, transform.position, Quaternion.identity);
        StickHP.PlayerHP.Value -= 9;
        AudioPlayer.PlayOneShot(HitSFX);
    }


    public void GotPunch_OhNoo_GetFX()
    {
        GameObject FX = Instantiate(HitFX, transform.position, Quaternion.identity);
        StickHP.PlayerHP.Value -= 2;
        AudioPlayer.PlayOneShot(HitSFX);
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

    public void ShootABullet()
    {
        Debug.Log("ShootABullet called");

        if (!StickHP.CanSuper)
        {
            GameObject bullet = Instantiate(GunBullet, AimPoint.position, Quaternion.identity);



            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            bullet.GetComponent<BulletOnline>().SetDirection(shootDirection);
            bullet.GetComponent<BulletOnline>().playerID.Value = StickID.Value;


        }
        else // Super bullet
        {
            GameObject bullet = Instantiate(MegaBullet, AimPoint.position, Quaternion.identity);


            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);

            StickHP.CanSuper = false;
            StickHP.MegaCharge = 0;
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

    }


    public void JumpFalserAnim()
    {



        StickAnim.SetBool("Jump", false);
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
        
        
            StickHP.PlayerHP.Value = StickHP.PlayerHP.Value + 0.03f;
        

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

    public void Punch()
    {
        //if (MeleeCollision != null && MeleeCollision.Book) // Null check for safety
        //{
        //    if (PunchAnimationSpeed < 3)
        //    {
        //        PunchAnimationSpeed = Mathf.Min(PunchAnimationSpeed + 2f, 3f); // Ensure it doesn't exceed 3
        //    }
        //}
        //else
        //{
        //    if (punchDecayCoroutine != null)
        //    {
        //        StopCoroutine(punchDecayCoroutine);
        //    }
        //    punchDecayCoroutine = StartCoroutine(PunchDecay());
        //}

        PunchBool = true;
        
        PunchAudioP.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        PunchAudioP.PlayOneShot(PunchSFX);
        lastPunch = Time.time;
        //StickAnim.speed = 1 + PunchAnimationSpeed;


        //if (punchDecayCoroutine != null)
        //{
        //    StopCoroutine(punchDecayCoroutine);
        //}
        //punchDecayCoroutine = StartCoroutine(PunchDecay());
    }


    public IEnumerator PunchDecay()
    {
        Debug.Log("DecayPunch");
        yield return new WaitForSeconds(1.3f);

        StickAnim.speed = 1;
        PunchBool = false;
        PunchAnimationSpeed = 0;

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
    private IEnumerator Dash()
    {
        if (StickHP.MegaCharge >= 4)
        {

            if (HeCanMove == true)
            {
                MaxSpeed += 1.3f;
                StickHP.MegaCharge = StickHP.MegaCharge - 4;
                StickAnim.SetBool("Dash", true);
                canDash = false;
                isDashing = true;
                dashPower = dashPower * (MaxSpeed / dashPower + 0.5f);

                float originalGravity = rb.gravityScale;
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(transform.localScale.x * dashPower, 3.5f);
                yield return new WaitForSeconds(dashTime);
                rb.gravityScale = originalGravity;
                isDashing = false;
                yield return new WaitForSeconds(dashCD);
                canDash = true;
                StickAnim.SetBool("Dash", false);
                Debug.Log("Dash" + dashPower);
            }
            else
            {
                PressedDash = true;
                StickHP.MegaCharge = StickHP.MegaCharge - StickHP.MegaCharge;
                HeCanMove = true;
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
            }




        }
        else
        {
            AudioPlayer.PlayOneShot(Fail);
        }






    }








    //private IEnumerator Slide()
    //{
    //    if (StickHP.PlayerHP > 0)
    //    {

    //        StickAnim.SetBool("Slide", true);
    //        canSlide = false;
    //        isSliding = true;
    //        //float originalGravity = rb.gravityScale;
    //        //rb.gravityScale = 0f;
    //        rb.velocity = new Vector2(transform.localScale.x * SlideCancelSpeed, 0f);
    //        yield return new WaitForSeconds(dashTime);
    //        float momentum = 0f;
    //        rb.velocity = new Vector2(SlideCancelSpeed + momentum * Mathf.Sign(transform.localScale.x), rb.velocity.y);
    //        isDashing = false;
    //        yield return new WaitForSeconds(dashCD);
    //        canDash = true;
    //        StickAnim.SetBool("Dash", false);


    //    }
    //    else
    //    {
    //        AudioPlayer.PlayOneShot(Fail);
    //    }
    //}

    public void InSlideAnimationTrue()
    {
        InSlideAnimation = true;
    }

    public void Footsteps()
    {
        FootP.pitch = Random.Range(1, 1.2f);
        FootP.gameObject.SetActive(true);



    }


    public void StopFootsteps()
    {
       
        FootP.gameObject.SetActive(false);
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
        if (!SlideJump)
        {
            rb.gravityScale = 10.55f;
            rb.mass = 76.55f;
        }

    }


    public void HeavyGravity()
    {
        rb.gravityScale = 100;
    }









    //Movement Overhaul



    public IEnumerator BackFlipMove()
    {

        Grounded = false;

        BackFlipAS.pitch = Random.Range(0.7f, 1.5f);
        BackFlipAS.PlayOneShot(BackFlipSFX);



        //constantForceDirection = new Vector2(constantForceDirection.x, -Mathf.Abs(constantForceDirection.y));


        BackFlip = true;

        float horizontalMomentum = 50;



        rb.velocity = new Vector2(transform.localScale.x * (-1 * (horizontalMomentum)), BackFlipForce/6);


        float flipTime = 0.5f;
        float elapsedTime = 0f;


        while (flipTime > elapsedTime)
        {
            HeCanMove = false;

            if (FacingRight)
            {




                rb.velocity = new Vector2(-Mathf.Abs(-horizontalMomentum), 20);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Abs(horizontalMomentum), 20);
            }



            elapsedTime += Time.deltaTime;

            yield return null;

        }
        BackFlip = false;
        HeCanMove = true;
        rb.rotation = 0;


        // yield return new WaitForSeconds(0.1f);
    }


    //Mostly happens When Hurt
    public IEnumerator FreezeFX()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSeconds(0.0007f);
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






    public void InSlideAnimationFalse()
    {
        InSlideAnimation = false;
    }


    private float SlideTime;
    private float SlideDuration;

    private IEnumerator Slide()
    {



        if (StickHP.PlayerHP.Value > 0)
        {
            AudioPlayer.PlayOneShot(SlideSFX);

          

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


                    slideMomentum -= SlideCancelSpeed * (MaxSpeed / SlideCancelSpeed) * Time.deltaTime;
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

    public void PlayPunchSFX4Stealth()
    {
        AudioPlayer.PlayOneShot(PunchSFXStealth);
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











}




