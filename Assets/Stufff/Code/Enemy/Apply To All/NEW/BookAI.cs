using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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
    public Animator BookAnim;

    [Header("Enemy State Manager")]
    private float HeyImJustHereForFormattingIMeanNothing;
    
    public EnemyState EnemyState;

    public EnemyBehavior EnemyBehavior;
    
    public StickmanBehavior StickmanBehavior;
    
    public StickmanState StickmanState;



    [Header("Enemy Booleans")]
    public bool HighEnergyHappened;
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
    
    
    [Header("Enemy Cooldowns")]
    public float MeleeCD;
    public float HighEnergyAttackCD;
    public float ChargedMeleeCD;
    public float RangedCD;
    public float MiniRangedAttackCD;
    public float CounterCD;

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


    [Header("Enemy Knowledge On The Player")]
    public float PlayerEvasiveActionCount;
    public float PlayerPunchCount;
    public float PlayerAcrobaticActionCount;


    [Header("Enemy Colliders")]
    public BoxCollider2D MeleeCollider;
    public BoxCollider2D SelfCollider;

    public AttackSight MeleeRange;

    [Header("Audio Stuff")]
    public AudioSource AudioP;
    public AudioClip MeleeImpact;


    // Start is called before the first frame update
    void Start()
    {
        StartHP = Health;
    }

    // Update is called once per frame
    void Update()
    {
        if(StickHP.PlayerHP <= 0) return;
        AttackManager();
        MoveTowardsPlayer();
        DistanceCalc();
        PlayerStateManager(); 
        PlayerObserver();
        EnemyBehaviorDecider();

        /// Time and Stuff
        CalcTimeAlive();
        
    }




    public void AttackManager()
    {
        if(!EnemVision.PlayerSpotted) return;
        // This is solely animaiton based attacks all the attacks are done by using animation key frames
        // This function/void is pretty much just to tell the enemy to do which attack based on what behavior
        //  or state the enemy brain is thinking

        if(EnemyState == EnemyState.GoingToPlayer)
        {
            HandleGoingToPlayer();
            return;
        }

        if(EnemyState == EnemyState.Idle)
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

            if(EnemyBehavior == EnemyBehavior.Neutral)
            {
                Vector2 FakeStickMan = new Vector2(StickPOS.position.x,transform.position.y);
                // This is normal walk up to player and meant for direct Melee combat
                if(DistanceFromPlayer >= MinDIS_Movement)
                {
                    transform.position = Vector2.MoveTowards(transform.position, FakeStickMan, Speed * Time.deltaTime);

                }
            }
            if(EnemyBehavior == EnemyBehavior.OverConfident && HighEnergyHappened)
            {
                Vector2 FakeStickMan = new Vector2(StickPOS.position.x,transform.position.y);
                // This is normal walk up to player and meant for direct Melee combat
                if(DistanceFromPlayer >= MinDIS_Movement)
                {
                    transform.position = Vector2.MoveTowards(transform.position, FakeStickMan, MaxSpeed * Time.deltaTime);

                }
            }
        
        
            if(EnemyBehavior == EnemyBehavior.Surrender)
            {
                // he gonna dip or he gonna like beg for mercy
            }


            // move yea
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

    public void NormalMeleeAttack()
    {
        // litteraly a punch what did ya think
        LastMelee = Time.time;
        isAttackingHighEnergy = false;
        EnemyAnim.SetTrigger("NormMelee");


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
    }

    public void HighEnergyRangedAttack()
    {
        if(HighEnergyHappened) return;
       // this unfortanelty even though is the hardest to prgram has to be a rare occurance and if so one of the enemies in a wave should eb doing this
        LastHighEnergyAttack = Time.time;
        isAttackingHighEnergy = true;
        HighEnergyHappened = true;
        EnemyAnim.SetTrigger("HighEnergy");

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

    }

    public void ParryMeleeAttack()
    {
        //This is a follow up to the dodge player melee attack and occurs if the player is cocky and keep punching even after enemy dodging everything
        // and then the enemy make the player not be able to move and knocks him back (if possible and player far enough the high energy ranged or mini ranged can occur)
    }

    // --------------------------Evading-----------------------------------------------------------

    public void DodgeMelee()
    {
        // mostly to punish spam
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

        if(DistanceFromPlayer > MinDIS_Movement)
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
        if(EnemyBehavior == EnemyBehavior.Neutral)
        {
            HandleNeutralFar();
        }

        if(EnemyBehavior == EnemyBehavior.Desperate)
        {
            HandleDesperateFar();
        }

        if(EnemyBehavior == EnemyBehavior.OverConfident)
        {
            HandleOverConfidentFar();
        }
    }

    void HandleNeutralFar()
    {
        if(StickmanState == StickmanState.Pushing)
        {
            // Do a charge up melee attack or a high power ranged
        }
    }

    void HandleDesperateFar()
    {
        if(StickmanState == StickmanState.Pushing)
        {   
            // Start Spaming random stuff 
            // Or Litteraly just dip and run
        }
    }

    void HandleOverConfidentFar()
    {
        Debug.Log("Handling Over Confident Enemy Far");


        if(StickmanState == StickmanState.Pushing && DistanceFromPlayer < 20 && 
        StickmanBehavior == StickmanBehavior.NoSkill_Spammer)
        {
            // Charge Up Knockback Melee Attack
        }
        else if(StickmanState == StickmanState.Pushing && EnemyBehavior == EnemyBehavior.OverConfident 
        && Time.time >= LastHighEnergyAttack + HighEnergyAttackCD )// like when the player is tryna aggresive head towards the enemy
        {      
            HighEnergyRangedAttack();
        }
    }

    void HandleOverConfidentClose()
    {
        Debug.Log("Handling Over Confident Enemy Close");


        if(Time.time >= 0.6f + LastMelee)
        {
            NormalMeleeAttack();
        }
     
       
    }


    void HandleCloseToPlayer()
    {
        // This is when enemy is close enough for player to do melee attacks

        if(EnemyBehavior == EnemyBehavior.Neutral)
        {
            HandleNeutralClose();
        }
        if(EnemyBehavior == EnemyBehavior.OverConfident)
        {
            HandleOverConfidentClose();
        }
    }

    void HandleNeutralClose()
    {
        Debug.Log("Hangling Enemy's Neutral Close");
        if(StickmanState == StickmanState.InAir && StickmanBehavior == StickmanBehavior.Acrobatic)
        {
            // Do an Air block attack that knockbacks player
        }

        if(StickManager.isSliding && StickmanBehavior == StickmanBehavior.NoSkill_Spammer 
        && StickmanBehavior == StickmanBehavior.Evasive)
        {
            // Punish for player for being too evasive and only relying on evasion
        }
        else if(StickmanState == StickmanState.Neutral)
        {
            // either do first melee attack or second one
        }
        else if(StickmanState == StickmanState.Pushing && EnemyBehavior != EnemyBehavior.OverConfident)// like when the player is tryna aggresive head towards the enemy
        {      
            NormalMeleeAttack();
        }
        
    }

    void HandleIdle()
    {
    
    }






    //-------------------------ANIMATIONS------------------------------------------------------


    // -------------------------- IMPORTANT STUFF-------------------------------------


    public void EnemyBehaviorDecider()
    {
        // OverConfidence can happen only once with the attack but if the player gets hit by the high energy attack overconfidence may come back




        if(TimeSincePlayerSpotted >= 6 && Health >= StartHP - 4 && LastHighEnergyAttack == 0)
        {
            EnemyBehavior = EnemyBehavior.OverConfident;
        }
        else if(TimeSincePlayerSpotted >= 14 && Health <= StartHP - 6)
        {
            int randomDecision = UnityEngine.Random.Range(0, 10);
            if(randomDecision == 2)
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
        if(PlayerIsVulnerable())
        {
            EnemyState = EnemyState.Idle;
            StickManager.StickAnim.SetTrigger("Hurt");
            StickHP.PlayerHP -= DMG;
            AudioP.PlayOneShot(MeleeImpact); // TODO: Change this so you can use a audioclip in the input
            if(isAttackingHighEnergy)
            {
                isAttackingHighEnergy = false;
                HighEnergyHappened = false;
            }
        }

    }

    public bool PlayerIsVulnerable()
    {
        if(!StickManager.BackFlip && !StickManager.isSliding && !StickManager.isDashing && MeleeRange.StickmanIsHERE)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // --------------------- MISC. ----------------------------------------------------------------

    public void DistanceCalc()
    {
        DistanceFromPlayer =  Vector2.Distance(transform.position, StickPOS.position);
    }





    private float ObserverCD = 0.2f;
    private float LastObserve = 0;
    public void PlayerObserver()
    {
        // This is what the enemy sees the player doing
        if(Time.time >= ObserverCD + LastObserve)
        {
            // Stickman Action Observer
            if(StickManager.PunchBool)
            {
                PlayerPunchCount +=1;
                LastObserve = Time.time;
            }
            if(StickManager.isSliding || StickManager.isDashing)
            {
                PlayerEvasiveActionCount += 1;
            }
            if(StickManager.BackFlip || StickManager.isDashing)
            {
                PlayerAcrobaticActionCount += 1;
            }
            

        }
        RegisterPlayerActionsInBrain();

        

    }

    public void RegisterPlayerActionsInBrain()
    {
        if(PlayerPunchCount >= 5)
        {
            StickmanBehavior = StickmanBehavior.NoSkill_Spammer;
        }
        if(PlayerEvasiveActionCount >= 4)
        {
            StickmanBehavior = StickmanBehavior.Evasive;
        }
        if(PlayerAcrobaticActionCount >= 8)
        {
            StickmanBehavior = StickmanBehavior.Acrobatic;
        }
        if(StickmanCharging())
        {
            StickmanBehavior = StickmanBehavior.onAdrenaline;
            StickmanState = StickmanState.Pushing;
        }

    }

    public bool StickmanCharging()
    {
        bool StickRunAnim = StickManager.StickAnim.GetBool("Walking");
        if(Math.Abs(StickManager.rb.velocity.x) == StickManager.MaxSpeed && StickAnimsChargeChecker() && StickManager.rb.velocity.y <= 0)
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
        if(StickRun || JumpStickAnim || BackflipStick || Airbone)
        {
            return true;
        }
        else
        {
            return false;
        }


        
    }

    public void CalcTimeAlive()
    {
        if(!EnemVision.PlayerSpotted) return;
        TimeSincePlayerSpotted += Time.deltaTime;
    }


 

}
