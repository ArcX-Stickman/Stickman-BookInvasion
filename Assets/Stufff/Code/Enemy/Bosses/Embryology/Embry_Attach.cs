using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    // Update is called once per frame
    void Update()
    {
        if (EnemyAI == null || movement == null)
        {
            return;
        }

        RecoverEnemyAIIfNeeded();

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

        if (SpecialEventOver_1st && Time.time >= LastTelePort + TeleCD && Time.time - LastTelePort > 6
            && !IsBossBusy() && LastTen + TenCD > Time.time && HasDone1stLandKick && teleportRoutine == null)
        {
            LastTelePort = Time.time;
            teleportRoutine = StartCoroutine(TelePort());


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
        return EnemyAI.isAttacking || IsScriptedSequenceRunning() || AnimatorShowsAttack();
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
        SetEnemyAIEnabled(false);

        float StickScale = movement.StickDirection;
        Vector2 StickPOS = movement.transform.position;
        float DashSpeed = 670;
        float behindOffset = 30f;

        while (!movement.Grounded)
        {
            yield return null; // wait for stick boi to get on the ground
        }
        EnemyAnim.CrossFade("Embry_Telewhoosh", 0, 0);
        EnemyAnim.SetBool("Dash",true);
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

        EnemyAI.EnemyAnim.CrossFade(rnd.name,0,0);


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
        EnemyAnim.SetBool("LOOP_SJK",false);
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
}
