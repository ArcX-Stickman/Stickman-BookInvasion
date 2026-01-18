using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
[System.Serializable]


public enum EnemyState
{
    GoingToPlayer, Attacking, Idle
}
public enum EnemyBehavior
{
    Neutral, OverConfident, Desperate, Surrender
}


public enum StickmanBehavior
{
    Acrobatic, Evasive, onAdrenaline, Neutral, NoSkill_Spammer
}
public enum StickmanState
{
    Neutral, InAir, Pushing
}
public class BookAI : MonoBehaviour
{
    [Header("Basic Stuff")]
    public Transform StickPOS;
    public Movement StickManager;
    public Health StickHP;

    [Header("Enemy References")]
    public Vision EnemVision;

    [Header("Enemy State Manager")]
    private float HeyImJustHereForFormattingIMeanNothing;

    public EnemyState EnemyState;

    public EnemyBehavior EnemyBehavior;

    public StickmanBehavior StickmanBehavior;

    public StickmanState StickmanState;



    [Header("Enemy Booleans")]
    //public bool HighEnergyHappened;
    private bool isAttackingHighEnergy;


    [Header("Enemy Basic Data")]
    public float Health;

    private float StartHP;
    public HitMarker HitMarker;
    public Animator EnemyAnim;


    private float TimeSincePlayerSpotted;


    [Header("Enemy Distance Data")]

    public float DistanceFromPlayer;
    public float MinMeleeDistance;
    public float MinDIS_Movement; // This is the min dis for book appraching the player



    [Header("Enemy Movement ")]
    public float Speed;
    public float MaxSpeed;
    [SerializeField] private float OG_SPEED;


    [Header("Enemy Cooldowns")]
    public float MeleeCD;
    public float HighEnergyAttackCD;
    public float ChargedMeleeCD;
    public float RangedCD;
    public float MiniRangedAttackCD;
    public float CounterCD;

    public float LastDodge;
    public float DodgeCD;

    [Header("Enemy Attack Times")]

    public float LastMelee;
    public float LastHighEnergyAttack;
    public float LastChargedMelee;
    public float LastRanged;
    public float LastMiniRangedAttack;
    public float LastCounter;

    [Header("Enemy FX")]
    public GameObject MeleeAttackFX;
    public GameObject MeleeAttackImpactFX;
    public float RangedAttackFX;
    public GameObject HighEnergyAttackFX;


    [Header("Enemy Knowledge On The Player")]
    public float PlayerEvasiveActionCount;
    public float PlayerPunchCount;
    public float PlayerAcrobaticActionCount;
    public float TimesDamagedPlayer;
    public float TimesPlayerDamagedEnemy;
    public PlayerDebuff PlayerDebuffManager;

    [Header("Enemy Colliders")]
    public BoxCollider2D MeleeCollider;
    public BoxCollider2D SelfCollider;
    public BoxCollider2D HighEnergyAttackCollider_IfNeeded;

    public AttackSight MeleeRange;


    [Header("Attack Data")]
    public float KnockbackOnMeleeHorizontalVelocity;
    public float KnockbackOnMeleeVerticalVelocity;
    public float DurationOfKnockbackMelee;
    public int TimesHighEnergyAttack;
    public bool inAir;
    public float JumpForce;
    public bool AlrInMobilityAttack;
    public string LastStickAttack;
    public float HighEnergyAttackDMG;
    public bool isAttacking;


    [Header("High Energy Attack Data")]

    public GameObject HighEnergyFX;
    public AudioClip HighEnergyAttackSFX;
    public bool HighEnergyStaggers;
    public float HighEnergyStaggerTime;
    public float HighEnergyFreezeTime;
    public float KnockbackOnMeleeHorizontalVelocityHighEnergy;
    public float KnockbackOnMeleeVerticalVelocityHighEnergy;

    [Header("Projectiles")]

    public GameObject NormProjectile;
    public GameObject MiniProjectile;
    public GameObject MegaRangedAttack;
    public Transform AimPoint;



    [Header("Audio Stuff")]
    public AudioSource AudioP;
    public AudioSource AudioP_PitchShifter;
    public AudioClip MeleeImpact;
    public AudioClip CounterMeleeSFX;
    public AudioClip KnockbackSFX;
    public AudioClip GroundImpact;
    public AudioClip EnemyDodge;
    public AudioClip PlayerStagger;
    public AudioClip PunchWhoosh;
    public AudioClip HighEnergyAttackImpact;


    [Header("Camera Angle")]
    public bool WantZoomOnCounter;
    public float ZoomInValue_On_Counter;
    public float DurationZoomInValue_On_Counter;


    [Header("Impact Frame")]
    public GameObject ImpactFrameBG;
    public string StickAnim_Impact;
    public GameObject Graphix;
    public SpriteRenderer SelfRender;
    public GameObject ImpactCam;
    public GameObject MainCam;
    public GameObject ImpactFrameHighContrast;
    public GameObject HighContrastWhiteRadialLines;
    public GameObject ImpactFrameEffect; // This are like the comic lines

    public Material UnlitRender;

    public Material LitRender;


    [Header("Audio")]

    public AudioClip ThrowSFX;
    public AudioClip PunchImpactFromPlayerFinisher;

    [Header("ExtraAnimattions")]
    public AnimationClip IdleNormal;
    public AnimationClip IdleTired;
    private AnimatorOverrideController overrideController;

    [Header("Death Animation Options ")]
    public float KnockBackValueDeathX;
    public float KnockBackValueDeathY;
    public float DurationOfKnockbackDeath;
    public GameObject Corpse;
    public LayerMask WhatsGround;
    public GameObject EnemySpriteObject;


    [Header("Specific Attack References")]

    [Header("Geology")]
    public GameObject Geology_Hyper_Crack;
    public GameObject Geology_Hyper_RockAvalauche;
    public AudioSource AttackAudioCue;
    public GameObject MegaAttackEffect;
    public GameObject Geology_GroundCrack_Decal;
    public GameObject MegaQuakeAfterMath;
    [Header("NeuroScience")]
    public GameObject NeuroElectricEffect;
    [Header("Astronomy")]
    public GameObject WarpSpace;
    public AudioClip Astro_WARP_Spawn_OBJ;
    public GameObject[] WarpSpaceObjectPool;


    [Header("Enemy Behavior Options")]
    public bool DefensiveDodger;


    [Header("Defensive Dodger Behavior ")]
    public bool Defensive_Dodger_Opening;
    public bool ShieldDefense;

    // Start is called before the first frame update
    void Start()
    {
        StartUPAI();
    }


    private void StartUPAI()
    {
        StartHP = Health;
        if (EnemyAnim == null)
        {
            EnemyAnim = GetComponent<Animator>();
        }
        overrideController = new AnimatorOverrideController(EnemyAnim.runtimeAnimatorController);
        EnemyAnim.runtimeAnimatorController = overrideController;


        // Speed Set up
        OG_SPEED = Speed;

    }

    // Update is called once per frame
    void Update()
    {
        if (Health > 0)
        {
            AttackManager();
            if (Speed > 0 && !isAttacking)
            {
                MoveTowardsPlayer();
            }
            DistanceCalc();
            PlayerStateManager();
            PlayerObserver();
            EnemyBehaviorDecider();
            LookAtPlayer();
            /// Time and Stuff
            CalcTimeAlive();
        }
        //Proper Evasion
        if (Time.time >= ResetCD + LastReset)
        {
            EvasionManager();

        }
        else if (HasParameter(EnemyAnim, "Dodge1", AnimatorControllerParameterType.Bool) && HasParameter(EnemyAnim, "Dodge2", AnimatorControllerParameterType.Bool))
        {
            EnemyAnim.ResetTrigger("Dodge1");
            EnemyAnim.ResetTrigger("Dodge2");

        }
        // Death
        if (Health <= 0)
        {
            DeathManager();
        }
        if (Corpse != null)
        {
            CorpseMatch();
        }
        if (StickHP.PlayerHP <= 0)
        {
            EnemyAnim.SetBool("Moving", false);
            this.enabled = false;

        }
        /// misc
        MakeSurePlayerEnablesOrDisables();
        if (IdleNormal != null && IdleTired != null)
        {
            IdleAnimationManager();
        }
    }

    public void CorpseMatch()
    {
        Corpse.transform.position = transform.position;
    }

    private bool DeathAnimPlayed;
    public void DeathManager()
    {
        if (!DeathAnimPlayed)
        {
            DeathAnimPlayed = true;
            if (LastStickAttack == "Melee")
            {
                EnemyAnim.CrossFade("MeleeDeath", 0, 0);
                StickAnim_Impact = "StickMeleeImpact";
                ImpactFrameBG = ImpactFrameHighContrast;
            }
        }
        else
        {
            return;
        }
        
    }

    public void DeathDestory()
    {

        //Destroy(gameObject);
    }


    public void AttackManager()
    {
        if (!EnemVision.PlayerSpotted) return;
        // This is solely animaiton based attacks all the attacks are done by using animation key frames
        // This function/void is pretty much just to tell the enemy to do which attack based on what behavior
        //  or state the enemy brain is thinking

        if (EnemyState == EnemyState.GoingToPlayer)
        {
            HandleGoingToPlayer();
            return;
        }

        if (EnemyState == EnemyState.Idle)
        {
            HandleIdle();
            return;
        }
    }


    public void MoveTowardsPlayer()
    {
        if (EnemVision.PlayerSpotted && EnemyState != EnemyState.Attacking)
        {
            EnemyState = EnemyState.GoingToPlayer;

            if (EnemyBehavior == EnemyBehavior.Neutral)
            {
                Vector2 FakeStickMan = new Vector2(StickPOS.position.x, transform.position.y);
                // This is normal walk up to player and meant for direct Melee combat
                if (DistanceFromPlayer >= MinDIS_Movement)
                {
                    transform.position = Vector2.MoveTowards(transform.position, FakeStickMan, Speed * Time.deltaTime);
                    EnemyAnim.SetBool("Moving", true);

                }
            }
            if (EnemyBehavior == EnemyBehavior.OverConfident)
            {
                Vector2 FakeStickMan = new Vector2(StickPOS.position.x, transform.position.y);
                // This is normal walk up to player and meant for direct Melee combat
                if (DistanceFromPlayer >= MinDIS_Movement)
                {
                    transform.position = Vector2.MoveTowards(transform.position, FakeStickMan, MaxSpeed * Time.deltaTime);
                    EnemyAnim.SetBool("Moving", true);

                }
            }


            if (EnemyBehavior == EnemyBehavior.Surrender)
            {
                // he gonna dip or he gonna like beg for mercy
            }


            // move yea
        }
        else
        {
            EnemyAnim.SetBool("Moving", false);

        }
    }

    public void PlayerStateManager()
    {
        // TODO: Make sure this can only run when player is in field of range of enemy this makes the enemies reaction very realistc
        // as he enemy is reacting to what he is seeing


    }

    public void EnemyStateManager()
    {
        // TODO: Yo dont make the enemy static make it sure it reflect the Enemy personalty/ behvior in the situation
        // Neutral - Enemy just chrages up to player like a normal enemy and does basic attacks from his moveset
        // OverConfident -  Enemy gets confortable in a spot and does high animie packed action based ranged attacks
        // Desperste - Similar to Overconfident but wehen the enemy is low health the enemy starts spamming attacks in a very predicatle pattern
    }


    // ---------------------------MELEE----------------------------------------------------------

    public bool FollowUpPunchFinish;
    public void NormalMeleeAttack()
    {
        // litteraly a punch what did ya think
        LastMelee = Time.time;
        isAttackingHighEnergy = false;
        //FollowUpPunchFinish = false;

        // Follow Up
        if (FollowUpPunchFinish && HasParameter(EnemyAnim, "NormMelee", AnimatorControllerParameterType.Trigger) || !HasParameter(EnemyAnim, "MeleeFollow", AnimatorControllerParameterType.Trigger))
        {
            EnemyAnim.SetTrigger("NormMelee");
            FollowUpPunchFinish = false;


        }
        else if (HasParameter(EnemyAnim, "MeleeFollow", AnimatorControllerParameterType.Trigger))
        {
            EnemyAnim.SetTrigger("MeleeFollow");

        }


    }

    public void ChargedMelee()
    {
        isAttackingHighEnergy = false;
        LastChargedMelee = Time.time;
        EnemyAnim.SetTrigger("CharMelee");

    }

    public void AirPunch()
    {
        // This Occurs is Player Loves Being in the Sky
        EnemyAnim.SetTrigger("AirMelee");

    }

    public void StaggerAttack(float time)
    {
        if (PlayerIsVulnerable())
        {
            StartCoroutine(StaggerPlayer(time));
        }
    }


    // --------------------------- Collisions --------------------------------------

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        HandleThemExitCollisions(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        HandleThemExitCollisions(collision.collider);

    }

    // Extra collision bools i guess

    private bool MeleeContact;


    private float CollisionTick = 0.1f;
    private float LastCollisionTick = 0;
    public void HandleCollision(Collider2D collion)
    {
        if (!isDodging)
        {

            if (collion.CompareTag("StickMelee") && Time.time >= LastCollisionTick + CollisionTick)
            {
                StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(UnityEngine.Random.Range(65.6f, 70.0f), UnityEngine.Random.Range(0.2f, 0.5f)));
                StickManager.PlayerPunchSFX();
                HitMarker.HitMarkerSFX();
                LastCollisionTick = Time.time;
                Health -= 0.7f;
                TimesPlayerDamagedEnemy += 1;
                MeleeContact = true;
                LastStickAttack = "Melee";
            }
            if (collion.CompareTag("Bullet") && Time.time >= LastCollisionTick + CollisionTick)
            {
                HitMarker.HitMarkerSFX();
                LastCollisionTick = Time.time;
                Health -= 0.9f;
                TimesPlayerDamagedEnemy += 1;
                LastStickAttack = "Bullet";
                Destroy(collion.gameObject);
            }
        }

        if (collion.CompareTag("Player") && StickManager.isSliding || collion.CompareTag("Player") && StickManager.isDashing)
        {

        }

    }

    // private IEnumerator EnemyFlash()
    // {

    // }

    public void HandleThemExitCollisions(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee"))
        {
            MeleeContact = false;
        }
    }




    // --------------------------RANGED-----------------------------------------------------------




    public void NormalRangedAttack()
    {
        // Predicable basic ranged attack
        LastRanged = Time.time;
        EnemyAnim.SetTrigger("NormRanged");
    }

    public void MiniRangedAttack()
    // A Fast Projectile thats small and follows the same  path
    {

        LastMiniRangedAttack = Time.time;
        EnemyAnim.SetTrigger("MiniRanged");


    }


    public void AnimationKeyRangedProjectile()
    {
        AnimationKeyMiniRangedProjectile(); // idk why ts fucntion exists
    }

    private Projectiles lastthrownProjectile;
    public void AnimationKeyMiniRangedProjectile()
    {
        AudioP.PlayOneShot(ThrowSFX);
        GameObject Projectile = ObjectPooler.SpawnObj(MiniProjectile, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        Projectile.GetComponent<Projectiles>().SetDirection(shootDirection);
        Projectile.GetComponent<Projectiles>().Accelerating = true;
        lastthrownProjectile = Projectile.GetComponent<Projectiles>();
        Projectile.GetComponent<Projectiles>().isOG = false;

        Projectile.transform.localScale = new Vector2(Projectile.transform.localScale.x * shootDirection.x, Projectile.transform.localScale.y);

    }

    // these two functions are meant to be useed as animation keys
    public void CustomizeSpeedThroughAnimation_Projectile(float speed)
    {
        lastthrownProjectile.Speed = speed;
    }

    public void ChangeAccelerationValue_Projectile(int Use1ForTrue2ForFalse)
    {
        if (Use1ForTrue2ForFalse == 1)
        {
            lastthrownProjectile.Accelerating = true;

        }
        else
        {
            lastthrownProjectile.Accelerating = false;

        }
    }


    private float Tick = 0.5f;
    private float lastTick;
    public void HighEnergyRangedAttack()
    {

        if (!HasParameter(EnemyAnim, "HighEnergy", AnimatorControllerParameterType.Trigger)) return;

        // Specific Enemy Settings

        if (Geology_Hyper_Crack != null)
        {

            // this tells us that this boi is geology
            EnemyAnim.SetBool("GeoHyper", true);

        }




        if (!HighEnergyisMeleeType || HighEnergyisMeleeType && MeleeRange.StickmanIsHERE)
        {
            if (StickHP.Testing)
            {
                Debug.Log("High Energy Ranged Called");

            }
            // this unfortanelty even though is the hardest to prgram has to be a rare occurance and if so one of the enemies in a wave should eb doing this
            LastHighEnergyAttack = Time.time;
            isAttackingHighEnergy = true;
            TimesHighEnergyAttack += 1;
            if (HighEnergyAttackCollider_IfNeeded != null && Time.time >= Tick + lastTick)
            {
                if (StickHP.Testing)
                {
                    Debug.Log("HighEnergyCollider Is Not Null Yessirrrr");

                }
                lastTick = Time.time;
                if (HighEnergyAttackCollider_IfNeeded.IsTouching(StickManager.gameObject.GetComponent<BoxCollider2D>()) && !StickManager.BackFlip)
                {
                    HighEnergyAttackCD -= 6.7f; // HAHAHAHAHAH 67!!!!!!!!! HAHAHAHAHAHAHA


                }
                else if (HighEnergyAttackCollider_IfNeeded.IsTouching(StickManager.gameObject.GetComponent<BoxCollider2D>()) && StickManager.BackFlip)
                {
                    HighEnergyAttackCD += 5.5f;

                }
            }

            EnemyAnim.SetTrigger("HighEnergy");
        }


    }

    public bool HighEnergyisMeleeType;
    public void HighEnergy_Damage()
    {

        if (HighEnergyisMeleeType && PlayerIsVulnerable())
        {
            HighEnergy_DamageToPLayer();
            if (HighEnergyFX != null)
            {
                GameObject FX = ObjectPooler.SpawnObj(HighEnergyFX, StickPOS.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
            }
            if (HighEnergyStaggers)
            {
                StartCoroutine(StaggerPlayer(HighEnergyStaggerTime));
            }
            StickManager.TimeManager.FreezeTime(HighEnergyFreezeTime);
        }
    }

    private void HighEnergy_DamageToPLayer()
    {
        StickHP.PlayerHP -= HighEnergyAttackDMG;

    }

    public void DesperateRangedAttack()
    {
        // This is going to be a pain in the butt to code but these attacks shouldnt "actually hit" the player mostly meant to miss on purpose
        LastRanged = Time.time;

    }

    // --------------------------RANGED---------------------------------------------




    // --------------------------COUNTERING--------------------------------------------------------


    public void CounterPlayerBullet()
    {

    }

    public void CounterPlayerMelee()
    {
        // Use this thing on The Animation Key Not as calling\
        StartCoroutine(StickManager.CameraScript.ZoomCam(ZoomInValue_On_Counter, DurationZoomInValue_On_Counter));


    }



    public void ZoomOut_Camera()
    {
        // Use this thing on The Animation Key Not as calling\

        StartCoroutine(StickManager.CameraScript.ZoomCam(StickManager.CameraScript.OG_CAM_SIZE, DurationZoomInValue_On_Counter));


    }



    public void ParryMeleeAttack()
    {
        //This is a follow up to the dodge player melee attack and occurs if the player is cocky and keep punching even after enemy dodging everything
        // and then the enemy make the player not be able to move and knocks him back (if possible and player far enough the high energy ranged or mini ranged can occur)
    }



    // Crazy Attacks

    //public GameObject TempMegaRange;
    public IEnumerator MegaRangedAttackFunction(string EnemyType)
    {
        // these attacks are forced attacks only should be called from another script no cooldowns or anything
        // meant to be after the high energy animie type of thing

        if (isAttackingHighEnergy)
        {
            yield return null;
        }
        EnemyAnim.SetTrigger("MegaRanged");
        Debug.Log("We in Megaranged ATK");

        if (EnemyType == "Physics")
        {
            Debug.Log("We in Physics Physics PhysicsMegaranged ATK");

            // THe wind thing
            GameObject WindSlash = ObjectPooler.SpawnObj(MegaRangedAttack, AimPoint.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
            Projectiles projectiles = WindSlash.GetComponent<Projectiles>();
            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            projectiles.SetDirection(shootDirection);
            //projectiles.Accelerating = true;
            while (!projectiles.Hit || projectiles != null)
            {
                yield return null;
            }
            Debug.Log("Projectile Sucesfully Hit");
            // Do the Charge Dash attack right here

        }




        // these are like sickles in ther air like the tengen vs gyaoy fight
    }


    public IEnumerator WaitForPlayerToGetUp(string EnemyType)
    {
        while (!StickManager.inPlayerKnockbackAlt)
            yield return null;

        while (!StickManager.isPlayerKnockbackFinished)
            yield return null;

        Debug.Log("Player fully recovered � time to attack!");
        StartCoroutine(MegaRangedAttackFunction(EnemyType));
    }



    //public IEnumerator Leap()
    //{
    //    Debug.Log("We in da leap funti0on");
    //    AlrInMobilityAttack = true;
    //    inAir = true;
    //    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //    float horizontalMomentum = rb.velocity.x;

    //    rb.velocity = new Vector2(horizontalMomentum, JumpForce);

    //   // yield return new WaitForSecondsRealtime(0.1f); // lil delay
    //    Vector2 PlayerFakePos = new Vector2(StickPOS.position.x, transform.position.y);
    //    float minDis = 40;
    //    Debug.Log("We Jumped");
    //    float DistanceFromFakePlayer = Vector2.Distance(transform.position, PlayerFakePos);
    //    while(DistanceFromFakePlayer > minDis)
    //    {
    //        Debug.Log("We Checking Dis in air");
    //        PlayerFakePos = new Vector2(StickPOS.position.x, transform.position.y);

    //        DistanceFromFakePlayer = Vector2.Distance(transform.position, PlayerFakePos);
    //        rb.gravityScale = 0;
    //        transform.position = Vector2.MoveTowards(transform.position, PlayerFakePos, 600 * Time.deltaTime);

    //    }

    //    //while (DistanceFromFakePlayer > minDis)
    //    //{
    //    //    Debug.Log("We in da leap Distance check fort when in air");

    //    //    DistanceFromFakePlayer = Vector2.Distance(transform.position, PlayerFakePos);
    //    //    PlayerFakePos = new Vector2(StickPOS.position.x, transform.position.y);
    //    //    transform.position = Vector2.MoveTowards(transform.position, PlayerFakePos, 90 * Time.deltaTime);
    //    //    yield return null;
    //    //}
    //    float playerangle = Mathf.Atan2(StickPOS.position.y, transform.position.x);
    //    float playerX = Mathf.Cos(playerangle) * 100;
    //    float playerY = Mathf.Sin(playerangle) * 100;
    //    Vector3 MovementThing = new Vector3(playerX, playerY, 0f);
    //    Debug.Log("Trig Ratios Calulated For Leap Ready to go down");
    //    while(DistanceFromPlayer > 20)
    //    {
    //        Debug.Log("We in da leap Distance check fort when in slamming");
    //        rb.gravityScale = 1;
    //        transform.position = Vector2.MoveTowards(transform.position,StickPOS.position, 90 * Time.deltaTime);
    //        yield return null;

    //    }
    //    inAir = false;
    //    AlrInMobilityAttack = false;
    //    Debug.Log("We finsih leaping");

    //    yield break;
    //}




    // --------------------------Evading-----------------------------------------------------------


    private float LastReset;
    private float ResetCD = 4;
    private float DodgeCount;
    private bool RunningEvasionManagerTimer;
    public void EvasionManager()
    {
        if (Time.time >= LastReset + ResetCD && MeleeContact)
        {
            DodgeMelee();
        }
        if (!RunningEvasionManagerTimer)
        {
            RunningEvasionManagerTimer = true;
            StartCoroutine(RunEvasionTimer());

        }
    }

    public IEnumerator RunEvasionTimer()
    {
        float Timer = 0;
        while (Timer < 10)
        {
            //Debug.Log("RunEvasionTimer: " + Timer);
            Timer += Time.deltaTime;
            yield return null;
        }
        LastReset = Time.time;
        DodgeCount = 0;

    }

    private bool isDodging;
    public void DodgeMelee()
    {
        // mostly to punish spam
        int randint = UnityEngine.Random.Range(1, 3);

        if (DodgeCount <= 2 && !isAttackingHighEnergy && !isDodging && Time.time >= LastDodge + DodgeCD)
        {
            isDodging = true;
            LastDodge = Time.time;
            DodgeCount += 1;
            if (randint == 1)
            {
                EnemyAnim.SetTrigger("Dodge1");

            }
            else
            {
                EnemyAnim.SetTrigger("Dodge2");

            }
        }
        else if (!isAttackingHighEnergy && !DefensiveDodger)
        {
            EnemyAnim.SetTrigger("Dodge3");
            DodgeCount = 0;
            LastReset = Time.time;
            LastDodge = Time.time;

        }
        else if (DefensiveDodger && DodgeCount > 2)
        {
            DodgeCount = 0;
            EnemyAnim.SetTrigger("Defense");
            if (!Running_DeFensiveEvasion_IEnumTHing)
            {
                StartCoroutine(DefensiveEvasion());

            }

        }



    }

    private bool Running_DeFensiveEvasion_IEnumTHing;
    private bool PlayerSpam;
    public IEnumerator DefensiveEvasion()
    {

        Running_DeFensiveEvasion_IEnumTHing = true;
        if (ShieldDefense)
        {
            yield return new WaitForSecondsRealtime(0.2f); // ts is just for buffereing this so its not unfair and the player can parry the attaack
            while (!Defensive_Dodger_Opening)
            {
                if (MeleeContact)
                {
                    PlayerSpam = true;
                }
                yield return null;
            }
            while (Defensive_Dodger_Opening)
            {

                if (MeleeContact && !PlayerSpam)
                {
                    EnemyAnim.SetBool("Protected", false);
                    Debug.Log("Enemy Got Staggred");
                    yield break;
                }
                if (PlayerSpam)
                {
                    break;
                }
                yield return null;
            }
            Debug.Log("Enemy is Protected");
            EnemyAnim.SetBool("Protected", true);
            float timer = 0;
            while (EnemyAnim.GetBool("Protected") && timer < 0.9f)
            {
                timer += Time.deltaTime;
                if (MeleeContact)
                {

                    StartCoroutine(StaggerPlayer(0.4f));
                    // He cooked fr player gets staggerd and gets hit with a melee attack
                    NormalMeleeAttack();
                    Debug.Log("Player Got Staggred");
                    Running_DeFensiveEvasion_IEnumTHing = false;
                    isDodging = false;

                    yield break;
                }
                yield return null;
            }
            EnemyAnim.SetBool("Protected", false);
            isDodging = false;
            Running_DeFensiveEvasion_IEnumTHing = false;
            yield break;


        }
    }


    public IEnumerator StaggerPlayer(float time)
    {
        StickManager.StickAnim.CrossFade("StickStaggered", 0, 0);
        StickManager.StickAnim.SetBool("Stagger", true);
        StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(40, 0.35f));
        StickManager.enabled = false;
        yield return new WaitForSecondsRealtime(0.10f); // ts is the time where the sprite of stickman switches to the knock back pose if ya change animation change this or it sint gonna look good
        TimeManager TimeFreezeThing = FindAnyObjectByType<TimeManager>();
        TimeFreezeThing.FreezeTime(0.3f);
        float timer = 0;
        AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
        AudioP_PitchShifter.PlayOneShot(PlayerStagger);
        yield return new WaitForSecondsRealtime(time);
        StickManager.enabled = true;
        StickManager.StickAnim.SetBool("Stagger", false);

        yield break;
    }


    public void DodgeFalse()
    {
        isDodging = false;
    }

    public void DodgeBullet()
    {
        // Rare Occurance
    }



    /// ---------------------- AI Attack Decisions------------------------------------

    void HandleGoingToPlayer()
    {
        // This is when The Enemy is Going Towards the player while on the move and has a distance
        // from the player

        if (DistanceFromPlayer > MinDIS_Movement)
        {
            HandleFarFromPlayer();
        }
        else
        {
            HandleCloseToPlayer();
        }
    }


    void HandleFarFromPlayer()
    {
        if (EnemyBehavior == EnemyBehavior.Neutral)
        {
            HandleNeutralFar();
        }

        if (EnemyBehavior == EnemyBehavior.Desperate)
        {
            HandleDesperateFar();
        }

        if (EnemyBehavior == EnemyBehavior.OverConfident)
        {
            HandleOverConfidentFar();
        }
    }

    void HandleNeutralFar()
    {
        if (StickmanState == StickmanState.Pushing)
        {
            // Do a charge up melee attack or a high power ranged
        }
    }

    void HandleDesperateFar()
    {
        if (StickmanState == StickmanState.Pushing)
        {
            // Start Spaming random stuff 
            // Or Litteraly just dip and run
            if (Time.time >= LastHighEnergyAttack + HighEnergyAttackCD - 5 && HasParameter(EnemyAnim, "HighEnergy", AnimatorControllerParameterType.Trigger)
)
            {
                HighEnergyRangedAttack();
            }
            if (Time.time >= LastMiniRangedAttack + MiniRangedAttackCD && Time.time < LastHighEnergyAttack + HighEnergyAttackCD - 3)
            {
                MiniRangedAttack();
            }
            EnemyAnim.SetBool("Moving", false);
        }
    }

    void HandleOverConfidentFar()
    {
        if (StickHP.Testing)
        {
            Debug.Log("Handling Over Confident Enemy Far");

        }


        //if (StickmanState == StickmanState.Pushing && DistanceFromPlayer < 20 && 
        //StickmanBehavior == StickmanBehavior.NoSkill_Spammer)
        //{
        //    // Charge Up Knockback Melee Attack
        //}
        else if (StickmanState == StickmanState.Pushing && EnemyBehavior == EnemyBehavior.OverConfident
        && Time.time >= LastHighEnergyAttack + HighEnergyAttackCD || EnemyBehavior == EnemyBehavior.OverConfident && Time.time >= LastHighEnergyAttack + HighEnergyAttackCD && HasParameter(EnemyAnim, "HighEnergy", AnimatorControllerParameterType.Trigger))// like when the player is tryna aggresive head towards the enemy
        {
            HighEnergyRangedAttack();
        }

        if (Time.time >= LastMiniRangedAttack + MiniRangedAttackCD && Time.time < LastHighEnergyAttack + HighEnergyAttackCD)
        {
            MiniRangedAttack();
        }
    }

    void HandleOverConfidentClose()
    {
        if (StickHP.Testing)
        {
            Debug.Log("Handling Over Confident Enemy Close");

        }


        if (Time.time >= MeleeCD + LastMelee || Time.time >= MeleeCD + LastMelee && StickManager.isInPunchAttach)
        {
            NormalMeleeAttack();
        }
        else if (StickmanState == StickmanState.Pushing && EnemyBehavior == EnemyBehavior.OverConfident
        && Time.time >= LastHighEnergyAttack + HighEnergyAttackCD || EnemyBehavior == EnemyBehavior.OverConfident && Time.time >= LastHighEnergyAttack + HighEnergyAttackCD && HasParameter(EnemyAnim, "HighEnergy", AnimatorControllerParameterType.Trigger))// like when the player is tryna aggresive head towards the enemy
        {
            HighEnergyRangedAttack();
        }

        if (Time.time >= LastMiniRangedAttack + MiniRangedAttackCD && Time.time < LastHighEnergyAttack + HighEnergyAttackCD)
        {
            MiniRangedAttack();
        }


    }


    void HandleCloseToPlayer()
    {
        // This is when enemy is close enough for player to do melee attacks

        if (EnemyBehavior == EnemyBehavior.Neutral)
        {
            HandleNeutralClose();
        }
        if (EnemyBehavior == EnemyBehavior.OverConfident)
        {
            HandleOverConfidentClose();
        }
        if (EnemyBehavior == EnemyBehavior.Desperate)
        {
            HandleDesperateFar(); // doesnt matter if far or nah its the same thing
        }
    }

    void HandleNeutralClose()
    {
        if (StickHP.Testing)
        {
            Debug.Log("Hangling Enemy's Neutral Close");

        }
        if (StickmanState == StickmanState.InAir && StickmanBehavior == StickmanBehavior.Acrobatic)
        {
            // Do an Air block attack that knockbacks player
        }

        if (StickManager.isSliding && StickmanBehavior == StickmanBehavior.NoSkill_Spammer
        && StickmanBehavior == StickmanBehavior.Evasive)
        {
            // Punish for player for being too evasive and only relying on evasion
        }
        else if (StickmanState == StickmanState.Neutral)
        {
            // either do first melee attack or second one
        }
        else if (StickmanState == StickmanState.Pushing && EnemyBehavior != EnemyBehavior.OverConfident || Time.time >= MeleeCD + LastMelee && StickManager.isInPunchAttach)// like when the player is tryna aggresive head towards the enemy
        {
            NormalMeleeAttack();
        }

    }

    void HandleIdle()
    {

    }






    //-------------------------ANIMATIONS------------------------------------------------------


    public void IdleAnimationManager()
    {
        if (Health / StartHP <= 0.50)
        {
            overrideController["Idle"] = IdleTired;
        }
        else
        {
            overrideController["Idle"] = IdleNormal;

        }
    }


    // -------------------------- IMPORTANT STUFF-------------------------------------


    public void EnemyBehaviorDecider()
    {
        // OverConfidence can happen only once with the attack but if the player gets hit by the high energy attack overconfidence may come back




        if (TimeSincePlayerSpotted >= 6 && Health >= StartHP - 4 && LastHighEnergyAttack == 0)
        {
            EnemyBehavior = EnemyBehavior.OverConfident;
        }
        else if (TimeSincePlayerSpotted >= 14 && Health / StartHP <= 0.2f)
        {
            int randomDecision = UnityEngine.Random.Range(0, 10);
            if (randomDecision == 2)
            {
                EnemyBehavior = EnemyBehavior.Surrender;

            }
            else
            {
                EnemyBehavior = EnemyBehavior.Desperate;

            }
        }
    }

    public void DamagePlayer(float DMG)
    {
        if (PlayerIsVulnerable())
        {
            EnemyState = EnemyState.Idle;
            StickManager.StickAnim.SetTrigger("Hurt");
            StickHP.PlayerHP -= DMG;
            AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.9f, 1.6f);
            AudioP_PitchShifter.PlayOneShot(MeleeImpact); // TODO: Change this so you can use a audioclip in the input
            if (!isAttackingHighEnergy)
            {
                StickManager.StartCoroutine(StickManager.PlayerKnockBack(KnockbackOnMeleeHorizontalVelocity, KnockbackOnMeleeVerticalVelocity, DurationOfKnockbackMelee));

            }
            else
            {
                // this means that the high ebergy attack was just a normal melee attack but boosted up
                StickManager.StartCoroutine(StickManager.PlayerKnockBack(KnockbackOnMeleeHorizontalVelocityHighEnergy, KnockbackOnMeleeVerticalVelocityHighEnergy, DurationOfKnockbackMelee));


            }
            TimesDamagedPlayer += 1;

            if (isAttackingHighEnergy)
            {
                isAttackingHighEnergy = false;
            }
        }

    }


    public void DamagePlayerVariation_Staggered(float DMG)
    {
        if (PlayerIsVulnerable())
        {

            EnemyState = EnemyState.Idle;
            StickHP.PlayerHP -= DMG;

            StartCoroutine(StaggerPlayer(HighEnergyStaggerTime));
            StickManager.TimeManager.FreezeTime(HighEnergyFreezeTime);
            TimesDamagedPlayer += 1;
            if (isAttackingHighEnergy)
            {
                isAttackingHighEnergy = false;
            }




            /// Extra things needed for specific enemies

            //NeuroScience
            if (NeuroElectricEffect != null)
            {
                NeuroElectricEffect.transform.position = StickPOS.position;
            }
        }

    }

    public void AnimationKey_FollowUp_Finished()
    {
        FollowUpPunchFinish = true;

    }

    public bool PlayerIsVulnerable()
    {
        // Only use for melee based attack conditions this wont work on ranged stuff
        if (!StickManager.BackFlip && !StickManager.isSliding && !StickManager.isDashing && MeleeRange.StickmanIsHERE && DistanceFromPlayer <= MinMeleeDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // --------------------- SFX. ----------------------------------------------------------------

    public void PlayCounterMeleeSFX()
    {
        AudioP.PlayOneShot(CounterMeleeSFX);
    }

    public void PlayDodgeSFX()
    {
        AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.8f, 1.4f);
        AudioP_PitchShifter.PlayOneShot(EnemyDodge);
    }
    public void PlayPlayerImpactFromPunchImpactFrameFinisher()
    {
        AudioP.PlayOneShot(PunchImpactFromPlayerFinisher);

    }

    public void PlayAirVelocityOfPunchSFX()
    {
        AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.8f, 1.7f);
        AudioP_PitchShifter.PlayOneShot(PunchWhoosh);
    }

    public void PlayKnockbackSFX()
    {
        if (KnockbackSFX != null)
        {
            AudioP.PlayOneShot(KnockbackSFX);
        }
    }

    public void PlayHighEnergyImpact()
    {
        if (PlayerIsVulnerable())
        {
            AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.8f, 1.4f);
            AudioP_PitchShifter.PlayOneShot(HighEnergyAttackImpact);
        }
    }

    public void HighEnergySFX_NOT_BASED_ON_IMPACT()
    {
        AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.8f, 1.4f);
        AudioP_PitchShifter.PlayOneShot(HighEnergyAttackImpact);
    }


    // --------------------- MISC. ----------------------------------------------------------------

    public void DistanceCalc()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position, StickPOS.position);
    }





    private float ObserverCD = 0.2f;
    private float LastObserve = 0;
    public void PlayerObserver()
    {
        // This is what the enemy sees the player doing
        if (Time.time >= ObserverCD + LastObserve)
        {
            // Stickman Action Observer
            if (MeleeContact)
            {
                PlayerPunchCount += 1;
                LastObserve = Time.time;
            }
            if (StickManager.isSliding || StickManager.isDashing)
            {
                PlayerEvasiveActionCount += 1;
            }
            if (StickManager.BackFlip || StickManager.isDashing)
            {
                PlayerAcrobaticActionCount += 1;
            }


        }
        RegisterPlayerActionsInBrain();



    }

    public void RegisterPlayerActionsInBrain()
    {
        if (PlayerPunchCount >= 5)
        {
            StickmanBehavior = StickmanBehavior.NoSkill_Spammer;
        }
        if (PlayerEvasiveActionCount >= 4)
        {
            StickmanBehavior = StickmanBehavior.Evasive;
        }
        if (PlayerAcrobaticActionCount >= 8)
        {
            StickmanBehavior = StickmanBehavior.Acrobatic;
        }
        if (StickmanCharging())
        {
            StickmanBehavior = StickmanBehavior.onAdrenaline;
            StickmanState = StickmanState.Pushing;
        }

    }

    public bool StickmanCharging()
    {
        bool StickRunAnim = StickManager.StickAnim.GetBool("Walking");
        if (Math.Abs(StickManager.rb.velocity.x) == StickManager.MaxSpeed && StickAnimsChargeChecker() && StickManager.rb.velocity.y <= 0)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    private bool StickAnimsChargeChecker()
    {
        bool StickRun = StickManager.StickAnim.GetBool("Walking");
        bool JumpStickAnim = StickManager.StickAnim.GetBool("Jump");
        bool BackflipStick = StickManager.StickAnim.GetBool("Backflip");
        bool Airbone = StickManager.StickAnim.GetBool("Airbone");
        if (StickRun || JumpStickAnim || BackflipStick || Airbone)
        {
            return true;
        }
        else
        {
            return false;
        }



    }
    public void LookAtPlayer()
    {
        Vector2 targetPosition = new Vector2(StickPOS.position.x, transform.position.y);
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }


    }
    public void CalcTimeAlive()
    {
        if (!EnemVision.PlayerSpotted) return;
        TimeSincePlayerSpotted += Time.deltaTime;
    }


    public IEnumerator ImpactFrame(float DelayIfWanted)
    {



        if (!PlayerIsVulnerable() && Health > 0) yield break;
        float EnemAnimOGSpeed = EnemyAnim.speed;
        if (PlayerIsVulnerable() || Health <= 0)
        {
            CounterPlayerMelee();
            Animator StickAnim = StickManager.GetComponent<Animator>();
            StickManager.enabled = false;
            StickAnim.CrossFade(StickAnim_Impact, 0, 0);

            float timeoutTimer = 0;

            while (!StickManager.PlayerReadyForImpactFrame)
            {
                timeoutTimer += Time.deltaTime;
                if (timeoutTimer > 1.3f)
                {
                    EnemyAnim.speed = EnemAnimOGSpeed;
                    StartCoroutine(StickManager.CameraScript.ZoomOutCam(43.9f, 1f));

                    yield break;

                }
                EnemyAnim.speed = 0;
                yield return null;
            }
            EnemyAnim.speed = EnemAnimOGSpeed;


            SpriteRenderer StickSprite = StickManager.GetComponent<SpriteRenderer>();
            SelfRender.material = UnlitRender;
            StickSprite.material = UnlitRender;

            //CameraScript ImpactCamSctiptCam = ImpactCam.GetComponent<CameraScript>();

            //StartCoroutine(StickManager.CameraScript.ShakeCam(4, 0.3f));
            float timer = 0;
            ImpactFrameBG.gameObject.SetActive(true);
            ImpactFrameEffect.gameObject.SetActive(true);
            if (Health <= 0)
            {
                StickManager.TimeManager.FreezeTime(0.01f);

            }
            else
            {
                StickManager.TimeManager.FreezeTime(0.1f);

            }

            MainCam.gameObject.SetActive(false);
            ImpactCam.gameObject.SetActive(true);




            Graphix.gameObject.SetActive(false);




            yield return new WaitForSecondsRealtime(DelayIfWanted);
            float timer2 = 0;
            while (StickManager.PlayerReadyForImpactFrame)
            {
                timer2 += Time.deltaTime;
                yield return null;
                // ts helps with timeouts like if player somehow miracoulddsy ( <--- how the hell u spell this) switched animation it wouldnt be nice >:(
                if (timer2 > 1.3f)
                {

                    MainCam.gameObject.SetActive(true);
                    ImpactCam.gameObject.SetActive(false);
                    ImpactFrameBG.gameObject.SetActive(false);
                    ImpactFrameEffect.gameObject.SetActive(false);
                    StartCoroutine(StickManager.CameraScript.ZoomOutCam(43.9f, 1f));
                    Graphix.gameObject.SetActive(true);
                    SelfRender.material = LitRender;
                    StickSprite.material = LitRender;
                    StickManager.enabled = true;
                    yield break;

                }


            }
            MainCam.gameObject.SetActive(true);
            ImpactCam.gameObject.SetActive(false);
            ImpactFrameBG.gameObject.SetActive(false);
            ImpactFrameEffect.gameObject.SetActive(false);
            StartCoroutine(StickManager.CameraScript.ZoomOutCam(43.9f, 1f));
            Graphix.gameObject.SetActive(true);
            SelfRender.material = LitRender;
            StickSprite.material = LitRender;
            StickManager.enabled = true;
            yield return null;
        }

    }


    //public void FlasherImpactFrame()
    //{
    //    SpriteRenderer Iframe = ImpactFrameBG.GetComponent<SpriteRenderer>();

    //    SpriteRenderer RadialWhite = HighContrastWhiteRadialLines.GetComponent<SpriteRenderer>();
    //    SpriteRenderer RadialBlack = ImpactFrameEffect.GetComponent<SpriteRenderer>();


    //    if(Iframe.color == Color.white)
    //    {
    //        RadialWhite.enabled = true;
    //        RadialBlack.enabled = false ;
    //        Iframe.color = Color.black;
    //    }
    //    else if(Iframe.color == Color.black)
    //    {
    //        RadialWhite.enabled = false;
    //        RadialBlack.enabled =  true;
    //        Iframe.color = Color.white;
    //    }
    //    else
    //    {
    //        RadialWhite.enabled = true;
    //        RadialBlack.enabled = false;
    //        Iframe.color = Color.black;

    //    }
    //}


    public void FlashWhiteImpactFrame()
    {
        SpriteRenderer Iframe = ImpactFrameBG.GetComponent<SpriteRenderer>();

        SpriteRenderer RadialWhite = HighContrastWhiteRadialLines.GetComponent<SpriteRenderer>();
        SpriteRenderer RadialBlack = ImpactFrameEffect.GetComponent<SpriteRenderer>();
        RadialWhite.enabled = false;
        RadialBlack.enabled = true;
        Iframe.color = Color.white;
    }

    public void FlashBlackImpactFrame()
    {
        SpriteRenderer Iframe = ImpactFrameBG.GetComponent<SpriteRenderer>();

        SpriteRenderer RadialWhite = HighContrastWhiteRadialLines.GetComponent<SpriteRenderer>();
        SpriteRenderer RadialBlack = ImpactFrameEffect.GetComponent<SpriteRenderer>();
        RadialWhite.enabled = true;
        RadialBlack.enabled = false;
        Iframe.color = Color.black;

    }

    public bool inPlainWhiteImpact;
    public IEnumerator PlainWhiteImpactFrame(float time)
    {
        // this is mostly for explosions and stuff
        while (inPlainBlackImpact)
        {
            yield return null;
        }


        SpriteRenderer StickSR = StickManager.GetComponent<SpriteRenderer>();
        SpriteRenderer ThisBook = EnemySpriteObject.GetComponent<SpriteRenderer>();
        Color col = StickSR.color;
        StickSR.color = Color.black;
        ThisBook.color = Color.black;
        inPlainWhiteImpact = true;


        MainCam.gameObject.SetActive(false);
        ImpactCam.gameObject.SetActive(true);
        SpriteRenderer Iframe = ImpactFrameBG.GetComponent<SpriteRenderer>();
        Iframe.color = Color.white;
        Iframe.gameObject.SetActive(true);
        float currenttime = 0;
        StickManager.TimeManager.FreezeTime(0.1f); // hardcoded cuz ion rrly want anything higher than that num
        while(currenttime < time)
        {
            currenttime += Time.deltaTime;
            yield return null;
        }
        Iframe.gameObject.SetActive(false);
        MainCam.gameObject.SetActive(true);
        ImpactCam.gameObject.SetActive(false);

        if(col == Color.black)
        {
            col = Color.white;

        }
        StickSR.color = col;
        ThisBook.color = Color.white;
        inPlainWhiteImpact = false;
        yield break;
    }


    public Material StickMaterial;
    public Material BookMaterial;
    public bool inPlainBlackImpact;

    public Material BlackIF_Material;

    public IEnumerator PlainBlackImpactFrame(float time)
    {
        while (inPlainWhiteImpact)
        {
            yield return null;
        }
        HighContrastWhiteRadialLines.gameObject.SetActive(false);
        ImpactFrameEffect.gameObject.SetActive(false);
        // this is mostly for explosions and stuff
        SpriteRenderer StickSR = StickManager.GetComponent<SpriteRenderer>();
        SpriteRenderer ThisBook = EnemySpriteObject.GetComponent<SpriteRenderer>();
        StickSR.material = BlackIF_Material;
        ThisBook.material = BlackIF_Material;
        inPlainBlackImpact = true;


        MainCam.gameObject.SetActive(false);
        ImpactCam.gameObject.SetActive(true);
        SpriteRenderer Iframe = ImpactFrameBG.GetComponent<SpriteRenderer>();
        Iframe.color = Color.black;
        Iframe.gameObject.SetActive(true);
        float currenttime = 0;
        StickManager.TimeManager.FreezeTime(0.1f); // hardcoded cuz ion rrly want anything higher than that num
        while (currenttime < time)
        {
            currenttime += Time.deltaTime;
            yield return null;
        }
        Iframe.gameObject.SetActive(false);
        MainCam.gameObject.SetActive(true);
        ImpactCam.gameObject.SetActive(false);

        StickSR.material = StickMaterial;
        ThisBook.material = BookMaterial;
        inPlainBlackImpact = false;
        yield break;
    }

    private bool CheckIfGrounded()
    {
        float extraHeight = 1.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }

    public void FreezeTime(float Time)
    {
        if (isDodging && DistanceFromPlayer > MinMeleeDistance + 2f)
        {
            return; // cuz itll look weird if the enemy dodges and makes the screen freeze when the player alr long gone by then
        }
        StickManager.TimeManager.FreezeTime(Time);
    }

    public IEnumerator KnockBack()
    {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        Debug.Log("PlayerKnockBack Was Called!!");
        float timer = 0f;

        float dir = transform.localScale.x > 0 ? 1f : -1f;

        float timeout = 2;
        while (timer < DurationOfKnockbackDeath && timer < timeout)
        {

            Debug.Log("Finishing up the timer part in Knockback ");

            rb.velocity = new Vector2(KnockBackValueDeathX * dir, KnockBackValueDeathY);
            timer += Time.deltaTime;
            yield return null;
        }
        if (timer >= timeout)
        {
            Debug.Log("unsucessfully Knockbacked Book Had to timeout ");

        }
        else
        {
            Debug.Log("Sucessfully Knockbacked Book ");

        }
        while (!CheckIfGrounded())
        {
            yield return null;
        }
        if (CheckIfGrounded())
        {
            if (Corpse != null)
            {
                BoxCollider2D box = GetComponent<BoxCollider2D>();
                rb.simulated = false;
                AudioP.PlayOneShot(GroundImpact);
                box.enabled = false;
                Debug.Log("Touch ground and turn on body");
                Corpse.gameObject.SetActive(true);
                EnemySpriteObject.SetActive(false);
                this.enabled = false;

            }
        }


        //rb.velocity = Vector2.zero;
    }

    public IEnumerator NormalDeathWithCorpse()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        while (!CheckIfGrounded())
        {
            yield return null;
        }
        if (CheckIfGrounded())
        {
            if (Corpse != null)
            {
                BoxCollider2D box = GetComponent<BoxCollider2D>();
                rb.simulated = false;
                AudioP.PlayOneShot(GroundImpact);
                box.enabled = false;
                Debug.Log("Touch ground and turn on body");
                Corpse.gameObject.SetActive(true);
                EnemySpriteObject.SetActive(false);
                this.enabled = false;

            }
        }
    }


    // Attacks that need Specific Coding



    // Melee

    // Ranged

    // High Energy


    public IEnumerator MegaQuake_Geology()
    {
        AudioP.PlayOneShot(GroundImpact);
        float CarryOldSpeed = Speed;
        Speed = 0;
        Geology_Hyper_Crack.transform.position = new Vector2(transform.position.x, Geology_Hyper_Crack.transform.position.y);
        Geology_Hyper_Crack.gameObject.SetActive(true);
        Geology_Hyper_Crack.transform.position = new Vector2(transform.position.x, Geology_Hyper_Crack.transform.position.y);
        AttackAudioCue.gameObject.SetActive(true);
        float SpeedOfCrack = 80;
        Vector2 PlayerPos = new Vector2(StickPOS.transform.position.x, Geology_Hyper_Crack.transform.position.y);
        float Dis = Vector2.Distance(Geology_Hyper_Crack.transform.position, PlayerPos);
        float mindis = 2; // if needed change this cuz ts is hard coded
        float timeoutTracking = 0;
        while (Dis > mindis)
        {
            if (timeoutTracking > 1.6f)
            {
                break;
            }
            PlayerPos = new Vector2(StickPOS.transform.position.x, Geology_Hyper_Crack.transform.position.y);
            StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(20, 0.2f));
            timeoutTracking += Time.deltaTime;

            Dis = Vector2.Distance(Geology_Hyper_Crack.transform.position, PlayerPos);
            Geology_Hyper_Crack.transform.position = Vector2.MoveTowards(Geology_Hyper_Crack.transform.position, PlayerPos, SpeedOfCrack * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(20, 0.9f));

        yield return new WaitForSecondsRealtime(1);
        Geology_Hyper_Crack.gameObject.SetActive(false);
        EnemyAnim.SetBool("GeoHyper", false);
        Debug.Log("Rock Launch now!!!");
        StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(190, 0.4f));
        if (Geology_GroundCrack_Decal != null)
        {
            GameObject Decal = Instantiate(Geology_GroundCrack_Decal, Geology_Hyper_Crack.transform.position, Quaternion.identity);
        }
        Vector2 FXPos = new Vector2(Geology_Hyper_Crack.transform.position.x, Geology_Hyper_Crack.transform.position.y + 29);
        Vector2 PositionWhereRocks = new Vector2(FXPos.x, Geology_Hyper_Crack.transform.position.y - 30);
        GameObject Boulders = ObjectPooler.SpawnObj(Geology_Hyper_RockAvalauche, PositionWhereRocks, Quaternion.identity);
        GameObject FX = Instantiate(MegaAttackEffect.gameObject, FXPos, Quaternion.identity);
        Rigidbody2D rb = Boulders.GetComponent<Rigidbody2D>();
        float timer = 0;
        float IntensityCamShake = 120;
        AttackAudioCue.gameObject.SetActive(false);
        PlayASoundAnimation PASA = GetComponent<PlayASoundAnimation>();
        if (PASA != null)
        {
            PASA.SFX();
        }
        while (timer < 1.7f)
        {

            float horizontalMomentum = rb.velocity.x;
            rb.velocity = new Vector2(horizontalMomentum, 130);
            StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(IntensityCamShake, 0.2f));
            IntensityCamShake -= 10 * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2);
        GameObject AfterMath = Instantiate(MegaQuakeAfterMath, Boulders.transform.position, Quaternion.identity);
        ObjectPooler.ReturnObjectToPool(Boulders);
        Speed = CarryOldSpeed;

        yield break;

    }


    // Astrology


    public IEnumerator A_WarpedSpace()
    {
        Vector3 PositionOfWarp;
        if (StickManager.StickDirection >= 0)
        {
            PositionOfWarp = new Vector3(StickPOS.position.x, transform.position.y + 30f, 0);

        }
        else
        {
            PositionOfWarp = new Vector3(StickPOS.position.x, transform.position.y + 30f, 0);

        }
        GameObject WarpSpaceObj = ObjectPooler.SpawnObj(WarpSpace, PositionOfWarp, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        Light2D Light = WarpSpaceObj.GetComponent<Light2D>();
        while (Light.enabled == false)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.6f);
        StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(25, 0.3f));
        AudioP_PitchShifter.pitch = UnityEngine.Random.Range(0.9f, 1.5f);
        AudioP_PitchShifter.PlayOneShot(Astro_WARP_Spawn_OBJ);
        Debug.Log("Spawning Object from objectpool Warp Space Astrology");
        // pick a random enviomrent thing to kill stickboi
        GameObject RandomizedObj = WarpSpaceObjectPool[UnityEngine.Random.Range(0, WarpSpaceObjectPool.Length)];
        Transform SpawnPOS = WarpSpaceObj.GetComponentInChildren<Transform>();
        GameObject EnviormentObj = ObjectPooler.SpawnObj(RandomizedObj, SpawnPOS.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
        float timer4timeOut = 0;
        while (EnviormentObj != null && timer4timeOut < 3.9f)
        {
            timer4timeOut += Time.deltaTime;
            yield return null;
        }
        Animator WarpAnim = WarpSpaceObj.GetComponent<Animator>();
        yield return new WaitForSecondsRealtime(0.5f);
        WarpAnim.CrossFade("ASTRO_Warp_In_Space_Portal_Leave", 0, 0); // if ya change the name in files ya change it here too
        yield break;

    }





    public void ApplyParalysisToPlayerOnAttack()
    {
        if (PlayerIsVulnerable())
        {
            PlayerDebuffManager.StartCoroutine(PlayerDebuffManager.ParalyzedManager());

        }
    }


    // Glitches or Graphical Stuff 


    private StickMoveGlitch StickMoveEnable;
    public void MakeSurePlayerEnablesOrDisables()
    {
        if (StickMoveEnable == null)
        {
            StickMoveEnable = FindAnyObjectByType<StickMoveGlitch>();
        }
        // StickMoveEnable.enabled = StickManager.enabled;
    }


    public void GraphicEffectsOnImpact()
    {
        if (PlayerIsVulnerable())
        {
            if (NeuroElectricEffect != null)
            {
                NeuroElectricEffect.gameObject.SetActive(true);

            }
            else
            {
                HighEnergyAttackFX.gameObject.SetActive(true);

            }
        }
    }

    public void TurnOffGraphicEffect()
    {

        if (NeuroElectricEffect != null)
        {
            NeuroElectricEffect.gameObject.SetActive(false);

        }
        else
        {
            HighEnergyAttackFX.gameObject.SetActive(true);

        }
    }

    bool HasParameter(Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        foreach (var param in animator.parameters)
        {
            if (param.name == paramName && param.type == type)
                return true;
        }
        return false;
    }

    public void AttackTrue()
    {
        isAttacking  = true;
    }
    public void AttackFalse()
    {
        isAttacking = false;
    }

}
