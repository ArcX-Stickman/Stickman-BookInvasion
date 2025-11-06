using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biology_AttackZone : MonoBehaviour
{

    public bool TouchingPlayer;
    public Health PlayerManager;

    public Movement Stick;


    public AudioClip LeafMissed;
    public AudioClip LeafHit;
    public AudioSource AudioS;


    public bool StickDodged;

    public Biology BioManager;

    public float streak;



    //Damage Values
    public float LeafDMG = 10.5f;


    // Start is called before the first frame update
    void Start()
    {
        streak = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TouchingPlayer = true;
            BioManager.AttackStickman();

        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if(collision.CompareTag("Bullet") && collision.CompareTag("Player"))
            {
                TouchingPlayer = true;
                BioManager.AttackStickman();
            }
            else
            {
                TouchingPlayer = false;
            }
            

        }
    }



    public void DamageStickmanLEAF()
    {
        if(PlayerManager.PlayerHP > 0)
        {
            if (TouchingPlayer == true)
            {
                BioManager.lastAttackTimeBAC = BioManager.lastAttackTimeBAC + 1.5f;


                if (Stick.JumpDodge == false)
                {
                    BioManager.LeafAttackCount = 0;
                    streak = 0;
                    BioManager.Hurt.gameObject.SetActive(true);
                    Stick.StickAnim.SetTrigger("Hurt");
                    PlayerManager.PlayerHP = PlayerManager.PlayerHP - LeafDMG;
                    AudioS.PlayOneShot(LeafHit);
                    BioManager.ThrowStickman();
                }
                if (Stick.JumpDodge == true)
                {
                    BioManager.LeafAttackCount = BioManager.LeafAttackCount + 1;
                    StickDodged = true;

                    streak = streak + 1;
                    PlayerManager.MegaCharge = PlayerManager.MegaCharge + (4 + (streak * PlayerManager.changerate));
                    PlayerManager.PlayerHP = PlayerManager.PlayerHP + 8.9f;


                    AudioS.PlayOneShot(LeafMissed);
                    BioManager.PhysicalKB = true;
                    BioManager.ThrowStickman();
                }
            }

        



        }
        if (TouchingPlayer == false)
        {
            StickDodged = true;
            BioManager.LeafAttackCount = BioManager.LeafAttackCount + 1;

            streak = streak + 1;
            PlayerManager.MegaCharge = PlayerManager.MegaCharge + (2 + (streak * PlayerManager.changerate));
            PlayerManager.PlayerHP = PlayerManager.PlayerHP + 3;


            AudioS.PlayOneShot(LeafMissed);
            BioManager.PhysicalKB = true;
            BioManager.ThrowStickman();
        }


            
        



      
        
    }



}
