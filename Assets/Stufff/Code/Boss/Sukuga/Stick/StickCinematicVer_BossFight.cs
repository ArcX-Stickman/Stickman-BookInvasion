using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class StickCinematicVer_BossFight : MonoBehaviour
{

    [Header("Type")]

    public bool Platformer;


    [Header("Platformer")]
    public float coyoteTime;
    public float coyoteTimeCounter;

    [Header("vars")]

    public Rigidbody2D rb;
    public float Speed;
    public float JumpForce;
    public bool Grounded;
    public LayerMask WhatsGround;
    public Animator StickAnim;
    private float horizontalInput;
    public GameObject Enemy;
    public float MinDisEnemy;
    public float DisEnemy;

    public float MeleeCD;
    public float LastMelee;
    public AudioClip[] PunchWhooshes;
    public AudioClip[] PunchMiss;
    public AudioClip[] PunchHit;
    public AudioClip StaminaOut;
    private bool inPunchVoidAnim;
    public LayerMask WhatsTheBookLayer;
    public float RayDistance;
    public AudioSource PunchAudioP;
    public bool Attack;

    public bool RequireFirstPunchDone;
    private bool FirstPunDone;
    public bool InPunch;

    public bool PunchA;
    public bool KickA;

    public float Stamina;
    [SerializeField] private float MaxStamina;
    [SerializeField] private float StaminaIncreaseRate;

    public float Health;


    public Slider HealthSlider;
    public Slider StaminaSlider;
    private bool StaminaPaused;

    public float MinStaminaForMelee;
    public float StaminaRecovery;

    public AudioClip BackFlipSFX;
    public float LastBackflipTime;
    public float BackflipCD;
    public float BackFlipForce;
    public float BackFlipHorizontalForce;
    public AudioClip JumpSfx;
    public AudioSource Footsteps;

    public bool CanJump;

    void Start()
    {

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        // Stats Set-up
        CanJump = true;
        // Stamina
        MaxStamina = Stamina;
        StaminaSlider.maxValue = MaxStamina;

        // Health
        HealthSlider.maxValue = Health;



        StickAnim.CrossFade("StickFarPunchFirstHeavy", 0, 0);

    }

    private void FixedUpdate()
    {
        StaminaUpdate();
    }
    void Update()
    {
        HealthUpdate();
        RunImportantCrap();
        InputManager();
        AnimForJump();




        if (Platformer)
        {
            coyoteTimePLAT();

        }
    }

    public void coyoteTimePLAT()
    {
        if (Grounded || CheckIfGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    public void AnimForJump()
    {
        StickAnim.SetBool("Jump", !Grounded && !CheckIfGrounded());
    }

    public void StaminaUpdate()
    {
        if (RunningPausedStam || StaminaPaused) return;  // woah that stamina is paused, YOOOO THATS BADDDD AAAAAAAAAAAAAAAAAAAAHHHH!!!!!!!

        float stamIn = StaminaIncreaseRate;

        if (Stamina < MaxStamina / 3)
        {
            stamIn -= 2.5f; // prevent da spammingggg
        }
        else if (Stamina < MaxStamina / 2)
        {
            stamIn -= 0.5f;
        }
        else
        {
            stamIn = StaminaIncreaseRate;
        }


        if (Stamina < MaxStamina)
        {
            Stamina += stamIn * Time.deltaTime;
        }
        StaminaSlider.value = Stamina;
    }

    public void HealthUpdate()
    {
        HealthSlider.value = Health;

    }

    private bool RunningPausedStam;
    public IEnumerator PauseStamina(float t)
    {
        if (RunningPausedStam) yield break;
        RunningPausedStam = true;
        StaminaPaused = true;

        if (Stamina > 10)
        {
            t += 1;
        }
        else if (Stamina < MaxStamina / 3)
        {
            t += 0.5f;
        }


        yield return new WaitForSecondsRealtime(t);
        RunningPausedStam = false;

        StaminaPaused = false;

        yield break;
    }

    public void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0 && CanJump || (Input.GetKeyDown(KeyCode.Space) && !Platformer))
        {
            Jump();
        }


        if (Input.GetButtonDown("Fire2") && Time.time >= MeleeCD + LastMelee)
        {

            LastMelee = Time.time;
            if (!StickAnim.GetBool("Spam"))
            {
                Punch();

            }
            else
            {
                Spammer();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && Grounded && !InBackflip && Stamina >= 8 || Input.GetKeyDown(KeyCode.Q) && !Grounded && !InBackflip && Stamina >= 8)
        {
            if (BackflipCD + LastBackflipTime <= Time.time)
            {
                LastBackflipTime = Time.time;
                InBackflip = true;
                StartCoroutine(Backflip());
            }

        }
        else if (Stamina <= 6 && Input.GetKeyDown(KeyCode.Q))
        {
            PunchAudioP.PlayOneShot(StaminaOut);
        }
    }

    private void Spammer()
    {

        if (StaminaSlider.gameObject.activeSelf)
        {
            StaminaSlider.gameObject.SetActive(false);
            HealthSlider.gameObject.SetActive(false);
        }

        MeleeCD = 0.2f;
        Stamina = 30;
        // skibi stuff go here
        bool DontMakeMoreIndexxGrr = false;

        inPunchVoidAnim = true;

        if (EnemyClose())
        {
            PunchWhoosh_NonMiss_SFX();
        }
        else
        {
            PunchMiss_SFX();
        }


        switch (punchIndex)
        {



            case 0:
                StickAnim.CrossFade("PUNCH1HOLD", 1, 0);
                PunchA = true;
                break;

            case 1:
                StickAnim.CrossFade("PUNCH2HOLD", 0, 0);
                PunchA = true;
                break;

            default:
                StickAnim.CrossFade("Punch", 0, 0);
                PunchA = true;
                break;
        }


        if (!DontMakeMoreIndexxGrr)
        {
            DontMakeMoreIndexxGrr = true;

            punchIndex += 1;

        }

        if (punchIndex > 2)
            punchIndex = 0;
        inPunchVoidAnim = false;
        return;
    }

    public void InPunchAnimKey()
    {
        InPunch = true;
    }

    public void InPunchAnimKeyFalse()
    {
        InPunch = false;
    }

    private bool CheckIfGrounded()
    {

        float extraHeight = 1.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }

    public void RunImportantCrap()
    {
        Move();
        CheckIfGrounded();
        DirectionCrap();
    }

    [SerializeField] private bool NoNeedDirectionCrap;
    private void DirectionCrap()
    {
        if (!NoNeedDirectionCrap)
        {
            if (Enemy.transform.localScale.x > 0)
            {
                transform.localScale = Vector3.one;

            }
            else if (Enemy.transform.localScale.x <= 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }


    }

    public int punchIndex;
    public void Punch()
    {
        // put stuff for prediticing enemy

        if (Grounded)
        {
            PunchAnim();
        }


    }

    public void Move()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInputT = Input.GetAxis("Horizontal");
        StickAnim.SetBool("Move", horizontalInput != 0);
        if (rb.velocity != (Vector2.zero) && CheckIfGrounded() || rb.velocity != Vector2.zero && Grounded)
        {
            Footsteps.enabled = true;
        }
        else
        {
            Footsteps.enabled = false;
        }

        rb.velocity = new Vector2(horizontalInput * Speed, rb.velocity.y);

        if (horizontalInputT > 0.01)
        {

            transform.localScale = Vector3.one;

        }
        else if (horizontalInputT < -0.01)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }


    }


    public AudioClip GroundImpact;
    public void GroundedSFX()
    {
        if (Grounded || CheckIfGrounded())
        {
            PunchAudioP.PlayOneShot(GroundImpact);

        }
    }

    public void Jump()
    {
        PunchAudioP.pitch = Random.Range(0.7f, 1.5f);
        Stamina -= 3;
        PunchAudioP.PlayOneShot(JumpSfx);
        float horizontalMomentum = rb.velocity.x;
        rb.velocity = new Vector2(horizontalMomentum, JumpForce);


    }

    public void AnimFirstPunchDone()
    {
        FirstPunDone = true;
    }

    public bool ForceSoundPunch_WHOOSH;
    public void PunchAnim()
    {
        if (RequireFirstPunchDone && !FirstPunDone) return;
        if (InPunch) return;
        if (inPunchVoidAnim) return;




        bool KickAnim = (StickAnim.GetCurrentAnimatorStateInfo(0).IsName("Kick"));
        bool DontMakeMoreIndexxGrr = false;

        inPunchVoidAnim = true;

        if (EnemyClose())
        {
            PunchWhoosh_NonMiss_SFX();
        }
        else
        {
            PunchMiss_SFX();
        }


        if (Grounded && !KickAnim && Stamina > MinStaminaForMelee + 0.2f) // 0.2f isjust a lil balance thing
        {

            switch (punchIndex)
            {



                case 0:
                    StickAnim.CrossFade("Punch", 1, 0);
                    PunchA = true;
                    break;

                case 1:
                    StickAnim.CrossFade("Punch2", 0, 0);
                    PunchA = true;
                    break;

                case 2:
                    StickAnim.CrossFade("Kick", 0, 0);
                    KickA = true;
                    break;
                default:
                    StickAnim.CrossFade("Punch", 0, 0);
                    PunchA = true;
                    break;
            }
        }
        else if (Stamina < MinStaminaForMelee)
        {
            PunchAudioP.pitch = 1;
            PunchAudioP.PlayOneShot(StaminaOut);
        }


        if (!DontMakeMoreIndexxGrr)
        {
            DontMakeMoreIndexxGrr = true;

            punchIndex += 1;

        }

        if (punchIndex > 2)
            punchIndex = 0;
        inPunchVoidAnim = false;
        return;
    }

    public void DecreaseStamina()
    {
        Stamina -= MinStaminaForMelee;
    }


    private void PunchMiss_SFX()
    {
        AudioClip rnd = PunchWhooshes[UnityEngine.Random.Range(0, PunchMiss.Length)];
        float pitch = UnityEngine.Random.Range(1, 1.3f);
        PunchAudioP.pitch = pitch;
        StartCoroutine(PunchAnimWhoosh(rnd));


    }

    private void PunchWhoosh_NonMiss_SFX()
    {
        AudioClip rnd = PunchWhooshes[UnityEngine.Random.Range(0, PunchWhooshes.Length)];
        float pitch = UnityEngine.Random.Range(1, 1.3f);
        PunchAudioP.pitch = pitch;
        StartCoroutine(PunchAnimWhoosh(rnd));
        if (ForceSoundPunch_WHOOSH)
        {

            PunchAudioP.PlayOneShot(rnd);
            PunchAnimKeyFrame = false;
        }

    }

    public IEnumerator PunchAnimWhoosh(AudioClip rnd)
    {
        while (!PunchAnimKeyFrame)
        {
            yield return null;
        }

        PunchAudioP.PlayOneShot(rnd);
        PunchAnimKeyFrame = false;
        yield break;

    }
    private bool PunchAnimKeyFrame;

    public void Animationkey_PunchSound()
    {
        PunchAnimKeyFrame = true;
    }


    private bool EnemyClose()  // since im kinda buns at this coding thing --- NOTE TO FUTURE SELF:  When adding min dis in Unity editor make sure it matches the enmey min dis for Punches or anything
    {
        DisEnemy = Vector2.Distance(transform.position, Enemy.gameObject.transform.position);

        if (DisEnemy <= MinDisEnemy)
        {
            return true;
        }

        return false;
    }

    public void AttackBool_ANIMKey(int T)
    {
        bool temp = false;
        if (T > 0)
        {
            temp = true;
        }
        Attack = temp;
        if (!temp)
        {
            PunchA = false;
            KickA = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            StickAnim.SetBool("Ground", true);
            Grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            StickAnim.SetBool("Ground", false);

            Grounded = false;
        }
    }

    public bool InBackflip;
    public IEnumerator Backflip()
    {

        Stamina -= 5;


        float horizontalMomentum = BackFlipHorizontalForce;
        StickAnim.SetBool("Backflip", true);



        PunchAudioP.pitch = Random.Range(0.7f, 1.5f);
        PunchAudioP.PlayOneShot(BackFlipSFX);
        LastBackflipTime = Time.time;
        InBackflip = true;
        if (transform.localScale.x > 0)
        {
            horizontalMomentum = horizontalMomentum * -1;
        }
        rb.velocity = new Vector2(transform.localScale.x * ((horizontalMomentum)), BackFlipForce);
        float flipTime = 0.5f;
        float elapsedTime = 0f;
        while (flipTime > elapsedTime)
        {

            if (transform.localScale.x > 0)
            {
                rb.velocity = new Vector2(-Mathf.Abs(-horizontalMomentum), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Abs(horizontalMomentum), rb.velocity.y);
            }
            elapsedTime += Time.deltaTime;
            yield return null;

        }


        InBackflip = false;
        rb.rotation = 0;
        StickAnim.SetBool("Backflip", false);

        while (!Grounded)
        {
            yield return null;
        }

        GroundedSFX();

        yield break;

    }


    public IEnumerator BackflipMofified(float Force, float y)
    {



        float horizontalMomentum = Force;

        PunchAudioP.pitch = Random.Range(0.7f, 1.5f);
        PunchAudioP.PlayOneShot(BackFlipSFX);
        LastBackflipTime = Time.time;
        InBackflip = true;
        if (transform.localScale.x > 0)
        {
            horizontalMomentum = horizontalMomentum * -1;
        }
        rb.velocity = new Vector2(transform.localScale.x * ((horizontalMomentum)), y);
        float flipTime = 0.5f;
        float elapsedTime = 0f;
        while (flipTime > elapsedTime)
        {

            if (transform.localScale.x > 0)
            {
                rb.velocity = new Vector2(-Mathf.Abs(-horizontalMomentum), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Abs(horizontalMomentum), rb.velocity.y);
            }
            elapsedTime += Time.deltaTime;
            yield return null;

        }


        InBackflip = false;
        rb.rotation = 0;

        while (!Grounded)
        {
            yield return null;
        }

        GroundedSFX();

        yield break;

    }



}
