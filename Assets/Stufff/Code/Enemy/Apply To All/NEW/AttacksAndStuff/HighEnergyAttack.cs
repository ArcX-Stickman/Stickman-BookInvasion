using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HighEnergyAttack : MonoBehaviour
{

    // this is gonna mostly help with the beam type attacks
    public BookAI EnemyAI;
    private bool AlrHit;
    public Movement StickManager;
    public PlayerDebuff PlayerDebuff;

    /// Attack Stuff
    public float DMG;
    public bool Paralysis;

    //Misc Stuff

    public float ShakeAmount;
    public float ShakeDuratrion;
    public float KnockbackValueX;
    public float KnockbackValueY;
    public float KnockbackDuration;
    public float FreezeFrameTime;

    public bool WantSpriteBasedLight;
    private Light2D Light;
    public SpriteRenderer sR;
    public bool WantSpriteBlackBasedOnEnemyWhiteflash;
    private Color OG_SpriteColor;
    public Material OG_MAT;
    public Material BlackIFrameMat;

    private void Update()
    {
        if(WantSpriteBlackBasedOnEnemyWhiteflash && EnemyAI.inPlainWhiteImpact)
        {
            if(sR == null)
            {
                sR = GetComponent<SpriteRenderer>();
            }
            else
            {
 
                sR.color = Color.black;
            }
        }
        else if(WantSpriteBlackBasedOnEnemyWhiteflash && !EnemyAI.inPlainWhiteImpact)
        {
            if (sR == null)
            {
                sR = GetComponent<SpriteRenderer>();
            }
            sR.color = Color.white;

        }
        else if (WantSpriteBlackBasedOnEnemyWhiteflash && !EnemyAI.inPlainBlackImpact)
        {
            if (sR == null)
            {
                sR = GetComponent<SpriteRenderer>();
            }
            sR.material = OG_MAT;
        }
        else if (WantSpriteBlackBasedOnEnemyWhiteflash && EnemyAI.inPlainBlackImpact)
        {
            if (sR == null)
            {
                sR = GetComponent<SpriteRenderer>();
            }
            sR.material = BlackIFrameMat;
        }
    }



    private void OnEnable()
    {
        
        AlrHit = false;
    }

    public void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !AlrHit && PlayerHittable()) 
        {
            StartCoroutine(HandleTypeOfAttackEffect());
        }
    }

    private bool PlayerHittable()
    {
        bool returnval;
        if (!StickManager.BackFlip && !StickManager.isDashing)
        {
            returnval = true;
        }
        else
        {
            returnval = false;
        }
        return returnval;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    public IEnumerator HandleTypeOfAttackEffect()
    {

        StickManager.StickAnim.SetTrigger("Hurt");
        if(KnockbackValueX > 0)
        {
            StartCoroutine(StickManager.PlayerKnockBack(KnockbackValueX, KnockbackValueY, KnockbackDuration));
    
        }
        StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(ShakeAmount, ShakeDuratrion));
        StickManager.TimeManager.FreezeTime(FreezeFrameTime);

        StickManager.StickHP.PlayerHP -= DMG;
        AlrHit = true;
        if (Paralysis) 
        {
            Debug.Log("Tried to paralyze");
            PlayerDebuff.StartCoroutine(PlayerDebuff.ParalyzedManager());
        }
        yield break;
    }


    


    
    
}
