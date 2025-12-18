using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
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
    Neutral, InAir
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
   

    [Header("Enemy Basic Data")]
    public float DistanceFromPlayer;
    public float MinMeleeDistance;
    public float MinDIS_Movement; // This is the min dis for book appraching the player

    

    [Header("Enemy Movement ")]
    public float Speed;
    public float MaxSpeed;
    
    
    [Header("Enemy Cooldowns")]
    public float MeleeCD;
    public float RangedCD;
    public float CounterCD;


    [Header("Enemy FX")]
    public float MeleeAttackFX;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackManager();
        MoveTowardsPlayer();
        DistanceCalc();
    }




    public void AttackManager()
    {
        // This is solely animaiton based attacks all the attacks are done by using animation key frames
        // This function/void is pretty much just to tell the enemy to do which attack based on what behavior or state the enemy brain is thinking
    }

    public void MoveTowardsPlayer()
    {
        if (EnemVision.PlayerSpotted)
        {

            if(EnemyBehavior == EnemyBehavior.Neutral)
            {
                // This is normal walk up to player and meant for direct Melee combat
                if(DistanceFromPlayer >= MinDIS_Movement)
                {
                    transform.position = Vector2.MoveTowards(transform.position, StickPOS.position, Speed * Time.deltaTime);

                }
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
    }

    public void AirPunch()
    {
        // This Occurs is Player Loves Being in the Sky
    }

    // --------------------------RANGED-----------------------------------------------------------

    public void NormalRangedAttack()
    {
        // Predicable basic ranged attack
    }

    public void MiniRangedAttack()
        // A Fast Projectile thats small and follows the same  path
    {
    }

    public void HighEnergyRangedAttack()
    {
       // this unfortanelty even though is the hardest to prgram has to be a rare occurance and if so one of the enemies in a wave should eb doing this
    }

    public void DesperateRangedAttack()
    {
        // This is going to be a pain in the butt to code but these attacks shouldnt "actually hit" the player mostly meant to miss on purpose
    }

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


    // --------------------- MISC. ----------------------------------------------------------------

    public void DistanceCalc()
    {
        DistanceFromPlayer =  Vector2.Distance(transform.position, StickPOS.position);
    }

 

}
