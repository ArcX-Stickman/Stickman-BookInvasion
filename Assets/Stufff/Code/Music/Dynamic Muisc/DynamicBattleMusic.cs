using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateOfFight
{
    EnemyDominance, Instability, EnemyDominace2, PlayerRising, PlayerTurningPoint, Concluding, Finale
}

public class DynamicBattleMusic : MonoBehaviour
{

    public BookAI[] Enemies;
    public Movement Stickman;

    public float AverageDis;

    public StateOfFight currentState;

    public AudioSource BattleAudioP;

    public AudioClip Riser;
    public AudioClip BattleStart;

    public AudioClip BattleIntensifyingStart;

    public AudioClip StringsEssemble1;

    public AudioClip AfterStringEssemblePlayerNotInstenseAttacking;

    public AudioClip CallForHeroismRedemptionComeback;

    public AudioClip PlayerComback;

    public AudioClip AlmostConcluding;

    public AudioClip OddsAreInPlayer;
    public AudioClip StringsEssemble2;




    public float DurationOfFight;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerSpotted())
        {
            DurationOfFight += Time.deltaTime;
        }
    }

    public void ManageMusic()
    {
        if (!PlayerSpotted())
        {
            BattleAudioP.clip = Riser;
        }
        
    }


    public bool PlayerSpotted()
    {
        foreach(BookAI  bookAIai in Enemies)
        {
            if (bookAIai.EnemVision.PlayerSpotted)
            {
                return true;
                
            }
        }
        return false;

    }

    public void AverageDIS_Calc()
    {
        float DisCalcTemp = 0;
        for(int i = 0; i < Enemies.Length; i++)
        {
            DisCalcTemp += Enemies[i].DistanceFromPlayer;
        }

        AverageDis = DisCalcTemp / Enemies.Length;


    }




    public void SwitchState()
    {
        
    }


}
