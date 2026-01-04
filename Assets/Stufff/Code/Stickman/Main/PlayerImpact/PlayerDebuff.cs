using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Paralyzed,Poisoned,Burned,Neutral
}

public class PlayerDebuff : MonoBehaviour
{
    [Header("Main Stuff")]
    public PlayerState State;
    public Movement Stickman;
    [Header("Debuff Info")]
    public float PoisonCD;
    public float PoisonDamage;
    private float LastPoisoned;
    public float BurnCD;
    public float BurnDamage;
    private float LastBurned;
    public float ParalyzeCD;
    public float ParalyzeFreezeTime;
    private float LastParalyzed;
    public float DurationPara;
    public float BurnDuration;
    public float PoisonDuration;
    public float ParalyzeDuration;


    public float CurrentTimer;

    [Header("Debuff FX")]
    public GameObject ParalyzeFX;
    [SerializeField] private AnimationClip StickParalizeClip;


    void Update()
    {
        StateManager();
        if(ParalyzeFreezeTime <= 0)
        {
            ParalyzeFreezeTime = StickParalizeClip.length;
        }

        if (ParalyzeFX.gameObject.activeSelf && !RunningParalyzedManager)
        {
            ParalyzeFX.gameObject.SetActive(false);

        }


    }

    public void StateManager()
    {
        switch (State)
        {
            case PlayerState.Poisoned:
                Poison();
                break;
            case PlayerState.Burned:
                Burned();
                break ;
            case PlayerState.Paralyzed:
                Paralyzed();
                break ;
            default:
                break;
        }
    }


    public void Poison()
    {
        if (!RunningPoisonManager)
        {
            RunningPoisonManager = true;
            StartCoroutine(PoisonManager());
        }
    }

    private bool RunningPoisonManager;
    public IEnumerator PoisonManager()
    {
        float timer = 0;
        while(timer < PoisonDuration)
        {
            timer += Time.deltaTime;
            if(Time.time >= LastPoisoned + PoisonCD)
            {
                Stickman.StickHP.PlayerHP -= PoisonDamage;
                LastPoisoned = Time.time;   
            }
        }
        State = PlayerState.Neutral;
        RunningPoisonManager = false;
        yield break;
    }


    public void Burned()
    {

    }
    private bool RunningBurnManager;



    public void Paralyzed()
    {

    }
    private bool RunningParalyzedManager;
    
    public IEnumerator ParalyzedManager()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Debug.Log("Entered Paralysis State  ");
        State = PlayerState.Paralyzed;
        float timer = 0;
        RunningParalyzedManager = true;
        while (timer < ParalyzeDuration)
        {
            CurrentTimer = timer;
            Debug.Log("Nomarl Paralysis Loop");

            timer += Time.deltaTime;
            if(Time.time >= LastParalyzed + ParalyzeCD)
            {
                Stickman.rb.velocity = new Vector2 (0,Stickman.rb.velocity.y);
                LastParalyzed = Time.time;  
                Stickman.enabled = false;
                Stickman.StickAnim.CrossFade("Stick_Paralysis", 0, 0);
                float secondTimer = 0;
                while(secondTimer < ParalyzeFreezeTime)
                {
                    Debug.Log("Second timer Paralysis");
                    Stickman.rb.velocity = new Vector2(0, Stickman.rb.velocity.y);
                    Stickman.ResetAllAnimationStates();
                    CurrentTimer = timer;

                    ParalyzeFX.gameObject.SetActive(true);
                    secondTimer += Time.deltaTime;
                    Stickman.enabled = false;
                    timer += Time.deltaTime;
                    yield return null;
                }
                

                yield return null;

            }
            Stickman.enabled = true;
            ParalyzeFX.gameObject.SetActive(false);
        }
        Debug.Log("Finish Paralysis");
        ParalyzeFX.gameObject.SetActive(false);
        Stickman.enabled = true ;
        yield break;
    }


}
