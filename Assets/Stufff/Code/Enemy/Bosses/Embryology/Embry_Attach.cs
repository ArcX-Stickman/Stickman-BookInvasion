using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Embry_Attach : MonoBehaviour
{
    public bool SpecialEventOver_1st;
    public BookAI EnemyAI;
    public float moveSpeed;
    public AudioClip MegaPunchStick;
    public Animator Embry;
    public GameObject BossTheme;
    public GameObject CineCam1;

    public AudioClip QTEButtonPOP;
    public AudioClip ButtonClick;
    public AudioClip Fail;
    public AudioClip AlertLaunchKick;
    public AudioClip JetWhoosh;
    public AudioClip Impact;
    public GameObject FX_Dirt;
    public bool isAttachedVer_Attack;
    public bool istele;

    public AudioSource AudioP;


    public Animator Embry_QTE_1_AVOID_ZY;
    public Animation Embry_QTE_1_AVOID_ZY_STARTUP;
    public Animator EnemyAnim;


    public GameObject[] QTEs;



    public Slider slider;

    public Movement movement;




    public float LastLaunchKick;
    public float LauchCD;
    public float Y_Axis_Launch;



    public GameObject Tendril;
    public AudioClip SpawnTen;
    public AudioClip TenRip;
    public AudioClip ThrowSFX;
    public AudioClip DodgeSFX;

    public float LastTen;
    public float TenCD;
    public Transform AimPoint;
    private bool HasDone1stLandKick;

    // Teleport Type thing

    public float LastTelePort;
    public float LastMelee;
    public float TeleCD;
    public AudioClip Dash;
    public float TeleportPlayerCollisionIgnoreTime = 0.2f;

    private Rigidbody2D rb;
    private Collider2D selfCollider;
    private Coroutine launchKickRoutine;
    private Coroutine teleportRoutine;
    private Coroutine tendrilRoutine;
    private bool physicsControlLocked;
    private bool ignoringPlayerCollision;



    // Ground Attack
    public GameObject Ten_Ground_ATK;
    public float Ground_Ten_CD;
    public float Last_Ground_TenATK;


    // Defensive stuff
    public bool Vunerable;



    // Stick- Stick Gatling Scripted Event
    [SerializeField] private int TimesDodged_PunchDefPhase;
    public int MaxDodgesForStickGatle;
    //public StickStickGatling StickGat; //this is kinda a OP referenmce you see i said OP cuz ion wanna get copyrighted :0

    // Start is called before the first frame update
    void Start()
    {
        if (BossTheme != null)
        {
            BossTheme.gameObject.SetActive(false);
        }

        EnemyAI = GetComponent<BookAI>();
        rb = GetComponent<Rigidbody2D>();
        selfCollider = GetComponent<Collider2D>();
    }

    private void OnDisable()
    {
        physicsControlLocked = false;
        launchKickRoutine = null;
        teleportRoutine = null;
        tendrilRoutine = null;

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        SetIgnorePlayerCollision(false);
    }

    private bool OP34243242467676;


    [SerializeField] private int TimesStick_Stick = 0;
    [SerializeField] private int StickStick_TimesNeeded = 25;
    private int queuedStickStickPunches;
    // Update is called once per frame
    void Update()
    {

        if (ExtremlyBusy_StopUpdate) return;

        if (Input.GetButtonDown("Fire2"))
        {
            SSG_LastClickLeftClick = Time.time;

            if (PlayerReadyToGattle)
            {
                queuedStickStickPunches++;
            }
        }

        if (TimesDodged_PunchDefPhase >= MaxDodgesForStickGatle && QTE_AV_ZY)
        {
            Stick_Stick();
        }


        if (!StickStickOccuring)
        {
            if (physicsControlLocked)
            {
                return;
            }

            LastMelee = EnemyAI.LastMelee;



            if (movement.StickHP.Testing && !OP34243242467676)
            {
                OP34243242467676 = true;
                LauchCD /= 2;
            }


            if (!SpecialEventOver_1st)
            {
                EnemyAI.enabled = false;
            }
            else
            {
                movement.MaxSpeed = 100;
                movement.Speed = 100;
            }


            if (!IsBossBusy() && (movement.isInPunchAttach || movement.PunchBool && EnemyAI.DistanceFromPlayer <= EnemyAI.MinMeleeDistance + 5))
            {
                if (LastLaunchKick <= 0 && SpecialEventOver_1st && launchKickRoutine == null)
                {
                    LastLaunchKick = Time.time;
                    launchKickRoutine = StartCoroutine(LaunchKick());
                }
            }
            else if (SpecialEventOver_1st && LastLaunchKick > 0 && Time.time >= LastLaunchKick + LauchCD && launchKickRoutine == null)
            {
                LastLaunchKick = Time.time;
                isAttachedVer_Attack = true;

                launchKickRoutine = StartCoroutine(LaunchKick());
            }

            if (Time.time >= LastTen + TenCD && SpecialEventOver_1st && HasDone1stLandKick && !IsBossBusy() && tendrilRoutine == null)
            {
                LastTen = Time.time;
                tendrilRoutine = StartCoroutine(TendrilAttackRoutine());
            }

            bool CanTele = SpecialEventOver_1st && Time.time >= LastTelePort + TeleCD && Time.time - LastTelePort > 6
                && !IsBossBusy() && LastTen + TenCD > Time.time && HasDone1stLandKick && teleportRoutine == null || SpecialEventOver_1st && Time.time >= LastTelePort + TeleCD && Time.time - LastTelePort > 6
                && !IsBossBusy() && LastTen + TenCD - 3 > Time.time && HasDone1stLandKick && teleportRoutine == null && movement.PunchBool;


            if (CanTele)
            {
                LastTelePort = Time.time;
                teleportRoutine = StartCoroutine(TelePort());


            }



            ExtraVoids();
        }
            if (EnemyAI == null || movement == null)
        {
            return;
        }


       

    }

    public void ExtraVoids()
    {
        RecoverEnemyAIIfNeeded();
        HealthBased_Actions();

    }

    public float LastKick;
    public float KickCD;

    public void HealthBased_Actions()
    {
        if (EnemyAI.Health <= 96) // its kinda hard to hit him
        {
            DodgePhase_Started = true;
            DodgePhase();
        }

     

    }


    private bool DodgePhase_Started;

    public float DodgeCD_Move;
    public float LastDodgePhase_Move;
    private float TeleportCD_DefPhase = 7;

    public void DodgePhase()
    {
        if (StickStickOccuring) return;
        TeleCD = TeleportCD_DefPhase;

       // EnemyAnim.SetBool("Def", true);


        //EnemyAI.Move_Towards_Player_OP_Breaker = true;

        bool punchStarted = movement.PunchBool && !lastPunchState;

        if (punchStarted && EnemyAI.DistanceFromPlayer <= EnemyAI.MinMeleeDistance + 5 || movement.StickAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch")
            && EnemyAI.DistanceFromPlayer <= EnemyAI.MinMeleeDistance + 5
            || movement.StickAnim.GetCurrentAnimatorStateInfo(0).IsName("StickKick") && EnemyAI.DistanceFromPlayer <= EnemyAI.MinMeleeDistance + 5)
        {
            if (Time.time >= LastDodgePhase_Move + DodgeCD_Move && !isDodging && !IsBossBusy())
            {
                IgnoreStickMelee(true);

                LastDodgePhase_Move = Time.time;
                StartCoroutine(DodgeMove());
            }
        }

        lastPunchState = movement.PunchBool;


        //bool CanTele = SpecialEventOver_1st && Time.time >= LastTelePort + TeleCD
        //        && !IsBossBusy();


        //if (CanTele)
        //{
        //    LastTelePort = Time.time;
        //    teleportRoutine = StartCoroutine(TelePort());


        //}


        //if (Time.time >= LastTen + TenCD && SpecialEventOver_1st && HasDone1stLandKick && !IsBossBusy() && tendrilRoutine == null)
        //{
        //    LastTen = Time.time;
        //    tendrilRoutine = StartCoroutine(TendrilAttackRoutine());
        //}

        //if (!Operation_StopThatMovement_DEFENSIVE_EDITION_67)
        //{
        //    // walking / movement
        //    if (!isDodging && EnemyAI.DistanceFromPlayer <= 60 && !istele)
        //    {
        //        EnemyAnim.SetBool("Moving", true);
        //        if (EnemyAnim.GetCurrentAnimatorStateInfo(0).IsName("EmbryDefenWalk"))  
        //        {
        //            // there was a dumb glitch where the movement playeed every second screwing up the teleportaion
        //            float direction = (transform.position.x < movement.transform.position.x) ? -1 : 1;
        //            Vector2 startPos = transform.position;
        //            Vector2 targtPos = new Vector2(startPos.x + (direction), startPos.y);

        //            transform.position = Vector2.MoveTowards(transform.position, targtPos, EnemyAI.Speed / 1.2f * Time.deltaTime);
        //        }


        //    }
        //    else
        //    {
        //        EnemyAnim.SetBool("Moving", false);

        //    }
        //}



    }

    private bool Operation_StopThatMovement_DEFENSIVE_EDITION_67;
    private bool isDodging;
    private bool lastPunchState;

    public IEnumerator DodgeMove()
    {
        if (isDodging) yield break;
        TimesDodged_PunchDefPhase += 1;


        isDodging = true;

        float direction = (transform.position.x < movement.transform.position.x) ? -1 : 1;

        float dodgeDistance = 10f;
        float duration = 0.25f;

        Vector2 startPos = transform.position;
        Vector2 targtPos = new Vector2(startPos.x + (direction * dodgeDistance), startPos.y);

        EnemyAnim.CrossFade("EmbryDodgeMoving", 0, 0);
        SetEnemyAIEnabled(false);
        SetScriptedPhysicsLock(true);
        AudioP.PlayOneShot(DodgeSFX);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float ease = 1f - Mathf.Pow(1f - t, 2f); // ts makes it feel kool and smoother

            Vector2 newPos = Vector2.Lerp(startPos, targtPos, ease);
            rb.MovePosition(newPos);

            elapsed += Time.unscaledDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targtPos);

        yield return new WaitForSecondsRealtime(0.05f);

        SetScriptedPhysicsLock(false);
        SetEnemyAIEnabled(true);
        IgnoreStickMelee(false);
        rb.velocity = Vector3.zero;
        isDodging = false;
    }


    public BoxCollider2D PunchCollider;
    public void IgnoreStickMelee(bool t)
    {
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        HitMarker h = GetComponent<HitMarker>();

        if (t)
        {
            Physics2D.IgnoreCollision(box, PunchCollider, true);

            h.enabled = false;
        }
        else
        {
            Physics2D.IgnoreCollision(box, PunchCollider, false);
            h.enabled = true;

        }
    }





    private void SetEnemyAIEnabled(bool enabled)
    {
        if (EnemyAI == null)
        {
            return;
        }

        if (!SpecialEventOver_1st && enabled)
        {
            return;
        }

        EnemyAI.enabled = enabled;

        if (enabled && EnemyAnim != null)
        {
            EnemyAnim.SetBool("Moving", false);
        }
    }

    private bool IsScriptedSequenceRunning()
    {
        return physicsControlLocked || launchKickRoutine != null || teleportRoutine != null || tendrilRoutine != null || InLaunchKick || isAttachedVer_Attack;
    }

    private bool AnimatorShowsAttack()
    {
        if (EnemyAnim == null)
        {
            return false;
        }

        AnimatorStateInfo stateInfo = EnemyAnim.GetCurrentAnimatorStateInfo(0);
        string stateName = stateInfo.IsName("EmbryTendril_Ranged") ? "EmbryTendril_Ranged" : string.Empty;

        if (string.IsNullOrEmpty(stateName))
        {
            foreach (AnimatorClipInfo clipInfo in EnemyAnim.GetCurrentAnimatorClipInfo(0))
            {
                if (clipInfo.clip == null)
                {
                    continue;
                }

                string clipName = clipInfo.clip.name;
                if (clipName.Contains("Tendril") || clipName.Contains("Ranged") || clipName.Contains("Tele") ||
                    clipName.Contains("Dash") || clipName.Contains("JK") || clipName.Contains("SJK") ||
                    clipName.Contains("Melee") || clipName.Contains("Attack"))
                {
                    return true;
                }
            }

            return false;
        }

        return true;
    }

    private bool IsBossBusy()
    {
        return EnemyAI.isAttacking || IsScriptedSequenceRunning() || AnimatorShowsAttack() || isDodging;
    }

    private void RecoverEnemyAIIfNeeded()
    {
        if (!SpecialEventOver_1st || EnemyAI == null)
        {
            return;
        }

        if (!IsScriptedSequenceRunning() && !EnemyAI.enabled)
        {
            SetEnemyAIEnabled(true);
        }
    }

    private void SetScriptedPhysicsLock(bool locked)
    {
        physicsControlLocked = locked;

        if (rb == null)
        {
            return;
        }

        if (locked)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
            rb.simulated = true;
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition, float speed, float stopDistance = 0.1f, bool useUnscaledTime = false)
    // unscale is just for when i need to do sum if slomo or pause time
    {
        while (Vector2.Distance(rb.position, targetPosition) > stopDistance)
        {
            float delta = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * delta);
            rb.MovePosition(nextPosition);
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(targetPosition);
    }

    private void SetIgnorePlayerCollision(bool ignore)
    {
        if (ignoringPlayerCollision == ignore)
        {
            return;
        }

        if (selfCollider == null || movement == null || movement.StickCollider == null)
        {
            return;
        }

        Physics2D.IgnoreCollision(selfCollider, movement.StickCollider, ignore);
        ignoringPlayerCollision = ignore;
    }

    private IEnumerator MoveVerticallyTo(float targetY, float speed)
    {
        while (Mathf.Abs(rb.position.y - targetY) > 0.1f)
        {
            Vector2 targetPosition = new Vector2(rb.position.x, targetY);
            Vector2 nextPosition = Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(nextPosition);
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator TelePort()
    {
        Operation_StopThatMovement_DEFENSIVE_EDITION_67 = true;
        SetEnemyAIEnabled(false);
        istele = true;
        float StickScale = movement.StickDirection;
        Vector2 StickPOS = movement.transform.position;
        float DashSpeed = 700;
        float behindOffset = 30f;


        while (!movement.Grounded)
        {
            yield return null; // wait for stick boi to get on the ground
        }
        EnemyAnim.CrossFade("Embry_Telewhoosh", 0, 0);
        EnemyAnim.SetBool("Dash", true);
        SetScriptedPhysicsLock(true);
        SetIgnorePlayerCollision(true);
        AudioP.PlayOneShot(Dash);

        Vector2 stickBehind = new Vector2(StickPOS.x - (Mathf.Sign(StickScale) * behindOffset), transform.position.y);

        if (StickScale > 0)
        {
            // that means he is facing right
            yield return MoveToPosition(stickBehind, DashSpeed, 0.01f);

        }
        else
        {

            // nah he facing left fr gang
            yield return MoveToPosition(stickBehind, DashSpeed, 0.01f);


        }
        SetEnemyAIEnabled(true);
        EnemyAnim.SetBool("Dash", false);

        // give a break for all attacks
        EnemyAI.LastMelee = Time.time - 0.7f;
        EnemyAI.LastMiniRangedAttack = Time.time;
        LastTen = Time.time;


        yield return new WaitForSeconds(TeleportPlayerCollisionIgnoreTime);
        SetIgnorePlayerCollision(false);
        SetScriptedPhysicsLock(false);

        teleportRoutine = null;

        EnemyAI.LookAtPlayer();



        AnimationClip rnd = BackStab_Anim[UnityEngine.Random.Range(0, BackStab_Anim.Length)];

        EnemyAI.EnemyAnim.CrossFade(rnd.name, 0, 0);
        istele = false;

        yield return new WaitForSecondsRealtime(0.3f);
        Operation_StopThatMovement_DEFENSIVE_EDITION_67 = false;
        yield break;
    }

    public AnimationClip[] BackStab_Anim;


    private IEnumerator TendrilAttackRoutine()
    {
        isAttachedVer_Attack = true;
        SetEnemyAIEnabled(false);
        AudioP.PlayOneShot(SpawnTen);
        EnemyAnim.CrossFade("EmbryTendril_Ranged", 0.3f, 0);

        float timeout = 2.5f;
        float timer = 0f;
        bool stateStarted = false;

        while (timer < timeout)
        {
            timer += Time.deltaTime;
            AnimatorStateInfo stateInfo = EnemyAnim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("EmbryTendril_Ranged"))
            {
                stateStarted = true;
            }

            if (stateStarted && stateInfo.IsName("EmbryTendril_Ranged") && stateInfo.normalizedTime >= 0.98f)
            {
                break;
            }

            yield return null;
        }

        isAttachedVer_Attack = false;
        SetEnemyAIEnabled(true);
        tendrilRoutine = null;
    }



    private Projectiles lastthrownProjectile;
    public void MiniRangedProjectile()
    {
        AudioP.PlayOneShot(ThrowSFX);
        GameObject Projectile = Instantiate(Tendril, AimPoint.position, Quaternion.identity);
        lastthrownProjectile = Projectile.GetComponent<Projectiles>();

        Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        Projectile.GetComponent<Projectiles>().SetDirection(shootDirection);
        Projectile.GetComponent<Projectiles>().Accelerating = true;
        Projectile.GetComponent<Projectiles>().isOG = false;

        Projectile.transform.localScale = new Vector2(Projectile.transform.localScale.x * shootDirection.x, Projectile.transform.localScale.y);


    }
    //





    private bool Start1stEvent;


    public bool AnimatiorBoolLaunchKickReady;
    public bool AnimatiorBoolJumpReady;

    private bool InLaunchKick;
    public IEnumerator LaunchKick()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        SetScriptedPhysicsLock(true);
        isAttachedVer_Attack = true;

        yield return new WaitForSecondsRealtime(0.2f);
        EnemyAnim.SetTrigger("JK");
        Vector2 FakeStickPos = new Vector2(rb.position.x, movement.transform.position.y);

        SetEnemyAIEnabled(false);
        InLaunchKick = true;
        // turn off any crap that messes the launch up

        Vector2 FakePos = new Vector2(FakeStickPos.x, Y_Axis_Launch);

        //float FakeDIS = Mathf.Abs(transform.position.y - FakePos.y);

        while (!AnimatiorBoolJumpReady)
        {
            yield return null;
        }
        float speed = 150;


        yield return MoveVerticallyTo(FakePos.y, speed);

        while (!AnimatiorBoolLaunchKickReady)
        {
            yield return null;
        }

        AudioP.PlayOneShot(AlertLaunchKick);
        yield return new WaitForSecondsRealtime(0.1f); // bufffer time for the player to gettt outta there


        float fallSpeed = 500;
        float gravity = 200f;
        AudioP.PlayOneShot(JetWhoosh);
        float maxtimer = 1.3f;
        float timerSlamdown = 0;
        EnemyAnim.SetBool("LOOP_SJK", true);

        while (Vector2.Distance(rb.position, FakeStickPos) > 0.5f)
        {
            timerSlamdown += Time.deltaTime;

            if (rb.position.y <= FakeStickPos.y)
            {
                break;
            }
            fallSpeed += gravity * Time.deltaTime;
            Vector2 nextPosition = rb.position + Vector2.down * fallSpeed * Time.deltaTime;
            rb.MovePosition(nextPosition);


            if (timerSlamdown >= maxtimer)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        AudioP.PlayOneShot(Impact);



        SetEnemyAIEnabled(true);
        EnemyAnim.SetBool("LOOP_SJK", false);
        SetScriptedPhysicsLock(false);



        //prevent ground clipping





        if (TouchingPlayer)
        {
            StartCoroutine(movement.PlayerKnockBack(5, 0, 0.5f));
            EnemyAI.DamagePlayer(25);

        }
        movement.CameraScript.StartCoroutine(movement.CameraScript.ShakeCamGroundFeeling(80, 0.4f));
        while (!EnemyAI.CheckIfGrounded())
        {
            yield return null;
        }
        // EnemyAI.SpawnGroundDecal(new Vector2(FakeStickPos.x,transform.position.y - 5));  -- add ts back when you make it work
        Instantiate(FX_Dirt.gameObject, new Vector2(FakeStickPos.x, rb.position.y - 5), Quaternion.identity);

        //while (checker < maxcheck)
        //{
        //    if (transform.position.y < movement.transform.position.y - 8 && movement.Grounded || transform.position.y > movement.transform.position.y + 2 && movement.Grounded)
        //    {
        //        // this is the worst case scenario if this ahappens a lot make it better
        //        transform.position = new Vector2(transform.position.x, movement.transform.position.y);
        //    }
        //    checker += Time.deltaTime;
        //    yield return null;
        //}

        isAttachedVer_Attack = false;
        if (!HasDone1stLandKick)
        {
            HasDone1stLandKick = true;

        }


        yield return new WaitForSecondsRealtime(3);
        InLaunchKick = false;
        launchKickRoutine = null;

    }



    private bool TouchingPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee"))
        {

            if (!isDodging)
            {
                if (!SpecialEventOver_1st && !Start1stEvent)
                {
                    Start1stEvent = true;
                    StartCoroutine(Special1stEvent());
                    BossTheme.gameObject.SetActive(true);
                }

                if (collision.CompareTag("StickMelee") && Time.time >= EnemyAI.LastCollisionTick + EnemyAI.CollisionTick && EnemyAI.enabled == false)
                {
                    if (EnemyAI.isDodging) return;
                    StartCoroutine(EnemyAI.StickManager.CameraScript.ShakeCamGroundFeeling(UnityEngine.Random.Range(65.6f, 70.0f), UnityEngine.Random.Range(0.2f, 0.5f)));
                    EnemyAI.StickManager.PlayerPunchSFX();
                    EnemyAI.HitMarker.HitMarkerSFX();
                    EnemyAI.LastCollisionTick = Time.time;
                    EnemyAI.Health -= 0.7f;
                    EnemyAI.TimesPlayerDamagedEnemy += 1;
                    EnemyAI.MeleeContact = true;
                    EnemyAI.LastStickAttack = "Melee";
                    EnemyAI.HurtAnim();
                }
            }

            Embry_RecivedPunch = true;



        }

        if (collision.CompareTag("Player"))
        {
            if (!movement.BackFlip)
            {
                TouchingPlayer = true;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TouchingPlayer = false;
        }
        if (collision.CompareTag("StickMelee"))
        {
            Embry_RecivedPunch = false;

        }
    }

    public IEnumerator Special1stEvent()
    {

        float ShakeCam = 60;
        Embry.CrossFade("Sliding", 0, 0);
        SetScriptedPhysicsLock(true);
        float t = 0;
        float maxtime = 0;
        if (movement.StickHP.Testing)
        {
            maxtime = 2;
        }
        else
        {
            maxtime = 3.5f;
        }
        while (t < maxtime)
        {
            ShakeCam -= 2 * Time.unscaledDeltaTime;
            movement.CameraScript.StartCoroutine(movement.CameraScript.ShakeCamGroundFeeling(ShakeCam, 0.1f));
            Vector2 nextPosition = rb.position + Vector2.right * moveSpeed * Time.unscaledDeltaTime;
            rb.MovePosition(nextPosition);
            t += Time.unscaledDeltaTime;

            if (t >= maxtime - 1.4f)
            {
                SpecialEventOver_1st = true;
                SetEnemyAIEnabled(true);

                Embry.SetTrigger("SlideEnd");
            }
            yield return null;
        }

        /// make the player QTE doging all the zygotes



        SetEnemyAIEnabled(true);
        float MaxSpeed_Enem = EnemyAI.MaxSpeed;
        while (!QTE_AV_ZY)
        {
            EnemyAI.MaxSpeed = 0;
            yield return null;
        }
        EnemyAI.MaxSpeed = MaxSpeed_Enem;
        SetScriptedPhysicsLock(false);

        yield break;
    }


    public void SwitchCam_1()
    {
        CineCam1.gameObject.SetActive(true);
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 0f;
        TimeManager tm = FindAnyObjectByType<TimeManager>();
        tm.enabled = false;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        StartCoroutine(QTE_1_AVOID_ZY());



    }

    public void SwitchOG_Cam()
    {
        movement.enabled = true;
        SetEnemyAIEnabled(true);

        if (CineCam1 != null)
        {
            CineCam1.gameObject.SetActive(false);

        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        TimeManager tm = FindAnyObjectByType<TimeManager>();
        if (tm != null)
        {
            tm.enabled = true;
        }

    }

    private bool QTE_AV_ZY = false;
    public IEnumerator QTE_1_AVOID_ZY()
    {
        slider.gameObject.SetActive(true);
        slider.maxValue = 100;

        slider.value = slider.maxValue;

        KeyCode[] qteSequence = { KeyCode.Q, KeyCode.Q, KeyCode.D, KeyCode.A };
        List<KeyCode> playerInput = new List<KeyCode>();

        for (int i = 0; i < QTEs.Length && i < qteSequence.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            AudioP.PlayOneShot(QTEButtonPOP);
            QTEs[i].gameObject.SetActive(true);
            Animator anim = QTEs[i].GetComponent<Animator>();

            anim.CrossFade("Keynorm", 0, 0);

            TextMeshProUGUI text = QTEs[i].GetComponentInChildren<TextMeshProUGUI>();
            text.gameObject.SetActive(true);
            text.text = qteSequence[i].ToString();
        }

        QTE_Attributes QA = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();

        int index = 0;
        while (!QA.Ready)
        {




            if (index < qteSequence.Length)
            {
                if (Input.GetKeyDown(qteSequence[index]))
                {
                    playerInput.Add(qteSequence[index]);
                    Animator anim = QTEs[index].GetComponent<Animator>();
                    TextMeshProUGUI text = QTEs[index].GetComponentInChildren<TextMeshProUGUI>();
                    text.gameObject.SetActive(false);
                    anim.CrossFade("KeyCrackFX", 0, 0);


                    AudioP.PlayOneShot(ButtonClick);

                    index++;
                }
                //  wrong input = fail immediately
                else if (Input.anyKeyDown)
                {
                    KeyCode pressed =
                        Input.GetKeyDown(KeyCode.Q) ? KeyCode.Q :
                        Input.GetKeyDown(KeyCode.A) ? KeyCode.A :
                        Input.GetKeyDown(KeyCode.D) ? KeyCode.D : KeyCode.None;

                    if (pressed != KeyCode.None && pressed != qteSequence[index])
                    {
                        AudioP.PlayOneShot(Fail);
                        // instant fail
                        break;
                    }
                }
            }
            slider.value -= (1 * Time.unscaledDeltaTime) * 100;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.2f); // buffer
        // finished QTE
        slider.gameObject.SetActive(false);
        bool success = index == qteSequence.Length;

        if (success)
        {
            Embry_QTE_1_AVOID_ZY.CrossFade("EMBRY_PASS_CH0_P1_DOGING", 0.2f, 0);
        }
        else
        {
            Embry_QTE_1_AVOID_ZY.CrossFade("EMBRY_FAIL_CH0_P1_DOGING", 0.4f, 0);

        }

        foreach (GameObject G in QTEs)
        {
            G.SetActive(false);
        }


        // misc
        for (int i = 0; i < playerInput.Count; i++)
        {
            Debug.Log("Player Input List: " + playerInput[i]);
        }

        QTE_Attributes QTE_A = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();
        while (!QTE_A.HasFinished)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.6f);
        SwitchOG_Cam();
        QTE_AV_ZY = true;
        yield break;
    }
    private bool QTE_Right(KeyCode[] OG, List<KeyCode> Player)
    {
        if (Player.Count < OG.Length)
            return false;

        for (int c = 0; c < OG.Length; c++)
        {
            if (Player[c] != OG[c])
                return false;
        }

        return true;
    }

    // Stick-Stick Gatling

    private bool SSG_Start;
    private float SSG_LastClickLeftClick;
    public bool StickStickOccuring;

    public void Stick_Stick()
    {
        SetEnemyAIEnabled(false);
        if (!SSG_Start )
        {
            SSG_Start = true;
            TimesStick_Stick = 0;
            queuedStickStickPunches = 0;
            AlreadyStarted_EmbryKnockFast_StickStick = false;
            Embry_RecivedPunch = false;
            PlayerReadyToGattle = false;

            StartCoroutine(Stick_Stick_ScriptedEvent());

        }


      

    }

    private bool Embry_RecivedPunch;
    private bool HasGattledYet;
    private bool PlayerReadyToGattle;
    public float StickStickPunchGap = 12f;
    public float StickStickAlignSpeed = 180f;
    public float EmbryKnockbackSpeed_StickStick = 90f;
    public float EmbryKnockbackDuration_StickStick = 10f;

    private IEnumerator AlignStickStickPositions()
    {
        float timeout = 0.35f;
        float timer = 0f;

        while (timer < timeout)
        {
            float directionToPlayer = movement.transform.position.x >= transform.position.x ? 1f : -1f;
            Vector2 midpoint = new Vector2((movement.transform.position.x + transform.position.x) * 0.5f, movement.transform.position.y);
            Vector2 enemyTarget = new Vector2(midpoint.x - (directionToPlayer * StickStickPunchGap * 0.5f), transform.position.y);
            Vector2 playerTarget = new Vector2(midpoint.x + (directionToPlayer * StickStickPunchGap * 0.5f), movement.transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, enemyTarget, StickStickAlignSpeed * Time.deltaTime);
            movement.transform.position = Vector2.MoveTowards(movement.transform.position, playerTarget, StickStickAlignSpeed * Time.deltaTime);

            bool enemyAligned = Vector2.Distance(transform.position, enemyTarget) <= 0.05f;
            bool playerAligned = Vector2.Distance(movement.transform.position, playerTarget) <= 0.05f;
            if (enemyAligned && playerAligned)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        float finalDirectionToPlayer = movement.transform.position.x >= transform.position.x ? 1f : -1f;
        Vector2 finalMidpoint = new Vector2((movement.transform.position.x + transform.position.x) * 0.5f, movement.transform.position.y);
        transform.position = new Vector2(finalMidpoint.x - (finalDirectionToPlayer * StickStickPunchGap * 0.5f), transform.position.y);
        movement.transform.position = new Vector2(finalMidpoint.x + (finalDirectionToPlayer * StickStickPunchGap * 0.5f), movement.transform.position.y);
    }


    public GameObject EmbryCamera;
    private bool AlreadyStarted_EmbryKnockFast_StickStick;
    private float originalStickStickAnimSpeed = 1f;


    private float LastPunch_SSG_SuperVersion;
    public IEnumerator Stick_Stick_ScriptedEvent()
    {
        StickStickOccuring = true;
        originalStickStickAnimSpeed = movement.StickAnim.speed;

        while (!Embry_RecivedPunch)
        {
            yield return null;
        }

        while (!movement.StickAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            movement.StickAnim.CrossFade("Idle", 0.01f, 0);
            yield return new WaitForSecondsRealtime(0.01f);
            yield return null;
        }

        yield return AlignStickStickPositions();
        movement.ResetAllAnimationStates();
        EnemyAnim.CrossFade("Embry_Heavy_Blam", 0, 0);
        movement.StickAnim.CrossFade("Punch (Hold)", 0f, 0);
        AudioP.PlayOneShot(EnemyAI.PlayerStagger);
        movement.StickAnim.enabled = false;
        movement.enabled = false;
        yield return new WaitForSecondsRealtime(0.3f);
        movement.StickAnim.enabled = true;

        PlayerReadyToGattle = true;
        EnemyAnim.SetBool("Hold", true);

        while (PlayerReadyToGattle)
        {
            float directionToPlayer = movement.transform.position.x >= transform.position.x ? 1f : -1f;
            Vector2 midpoint = new Vector2((movement.transform.position.x + transform.position.x) * 0.5f, movement.transform.position.y);
            transform.position = new Vector2(midpoint.x - (directionToPlayer * StickStickPunchGap * 0.5f), transform.position.y);
            movement.transform.position = new Vector2(midpoint.x + (directionToPlayer * StickStickPunchGap * 0.5f), movement.transform.position.y);

            if (queuedStickStickPunches > 0)
            {
                movement.StickAnim.SetBool("Stick-Stick-G", true);
                EnemyAnim.SetBool("Hold", true);
                movement.StickAnim.speed += 0.2f;
                LastPunch_SSG_SuperVersion = Time.time;
                queuedStickStickPunches--;
                TimesStick_Stick += 1;
            }

            if (Time.time >= 0.2f + LastPunch_SSG_SuperVersion)
            {
                movement.StickAnim.SetBool("Stick-Stick-G", false);
            }

            if (TimesStick_Stick >= StickStick_TimesNeeded && !AlreadyStarted_EmbryKnockFast_StickStick)
            {
                Debug.Log("Finished Stick Stick Gatling");
                AlreadyStarted_EmbryKnockFast_StickStick = true;
                PlayerReadyToGattle = false;
                movement.StickAnim.speed = originalStickStickAnimSpeed;
                movement.StickAnim.SetBool("Stick-Stick-G", false);
                yield return StartCoroutine(EmbryKnockFast_StickStick());
                yield break;
            }

            yield return null;
        }
    }

    private bool ExtremlyBusy_StopUpdate;
    public ChangeCamSize CCS;
    private void ResetStickStickState()
    {
        queuedStickStickPunches = 0;
        TimesStick_Stick = 0;
        PlayerReadyToGattle = false;
        StickStickOccuring = false;
        SSG_Start = false;
        AlreadyStarted_EmbryKnockFast_StickStick = false;
        Embry_RecivedPunch = false;
        ExtremlyBusy_StopUpdate = false;
       // movement.StickAnim.enabled = true;
        movement.StickAnim.speed = originalStickStickAnimSpeed;
        EnemyAnim.SetBool("Hold", false);
        EmbryCamera.gameObject.SetActive(false);
        SetEnemyAIEnabled(true);
    }

    public IEnumerator EmbryKnockFast_StickStick()
    {
        Debug.Log("REached EmbryKnockFast");
        float timer = 0;
        float OG_CAMCCG_ZOOM_TIME = CCS.CamSizeZoomDuration;
        float OG_CAMCCG_ZOOMED_IN = CCS.CamSizeZoomedin;
        movement.StickAnim.CrossFade("StickDragBackAfterGattle", 0, 0);
        Debug.Log("REached StickDragBackAfterGattle");

        AudioP.PlayOneShot(EnemyAI.PlayerStagger);
        EnemyAnim.SetBool("Hold", false);
        Embry_RecivedPunch = false;
        movement.StickAnim.CrossFade("Stick_Punch(Slowed)", 0.1f, 0);
        movement.StickAnim.SetBool("Stick-Stick-G", false);

        Debug.Log("REached Slowed");


        while (!Embry_RecivedPunch)
        {
            yield return null;
        }
        CCS.CamSizeZoomDuration = 0.1f;
        CCS.CamSizeZoomedin = 7.83f;

        yield return new WaitForSecondsRealtime(0.6f);
        EnemyAnim.SetTrigger("Block_Con");
        //yield return new WaitForSecondsRealtime(0.6f);


        // embry crloss fade into shocked animtion or recovery

        // while loop here waiting for the punch event (animation keys)

        // EmbryCrossFade knockback animation ( do a bool type insiead of crossfade)


        BoxCollider2D Bxcd = EnemyAI.gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D Sbxcd = movement.gameObject.GetComponent<BoxCollider2D>();
        ExtremlyBusy_StopUpdate = true;
        CCS.CamSizeZoomedin = 25;
        SetEnemyAIEnabled(false);
        SetScriptedPhysicsLock(true);
        EmbryCamera.gameObject.SetActive(true);
        Physics2D.IgnoreCollision(Bxcd, Sbxcd, true);
        float knockDirection = transform.position.x >= movement.transform.position.x ? 1f : -1f;
        float z = 0;
        while (timer < EmbryKnockbackDuration_StickStick)
        {
           // z += 20 * knockDirection;
            //transform.rotation = Quaternion.Euler(0,0,z);
            Vector2 nextPosition = rb.position + Vector2.right * knockDirection * EmbryKnockbackSpeed_StickStick * Time.deltaTime;
            rb.MovePosition(nextPosition);
            timer += Time.deltaTime;
            yield return null ;

        }


        CCS.CamSizeZoomDuration = OG_CAMCCG_ZOOM_TIME;
        CCS.CamSizeZoomedin = OG_CAMCCG_ZOOMED_IN;

        Physics2D.IgnoreCollision(Bxcd, Sbxcd, false);
        SetScriptedPhysicsLock(false);
        ResetStickStickState();
    }


    public void AnimationKeys_Enable_EnemyAI()
    {
        SetEnemyAIEnabled(true);
    }


    public void AnimationKeys_Disable_EnemyAI()
    {
        SetEnemyAIEnabled(false);
    }


    public void PlayRip()
    {
        AudioP.PlayOneShot(TenRip);
    }

    public void GroundAttack_Enabled()
    {
        Ten_Ground_ATK.gameObject.SetActive(true);
    }



}
