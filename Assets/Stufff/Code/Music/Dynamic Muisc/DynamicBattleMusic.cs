using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public enum StateOfFight
{
    EnemyDominance, Instability, EnemyDominace2, PlayerTurningPoint, Finale
}
// this is technically a battle manager not just for music
public class DynamicBattleMusic : MonoBehaviour
{
    public List<BookAI> Enemies;
    public Movement Stickman;

    public float AverageDis;

    public StateOfFight currentState;

    public AudioSource BattleAudioP;
    public AudioSource AudioP;
    public AudioClip Riser;
    public AudioClip BattleStart;

    public AudioClip BattleIntensifyingStart;
    public AudioClip StringsEssemble1;
    public AudioClip EnemyDominance;
    public AudioClip CallForHeroismRedemptionComeback;
    public AudioClip PlayerComback;
    public AudioClip AlmostConcluding;
    public AudioClip OddsAreInPlayer;
    public AudioClip StringsEssemble2;

    public float DurationOfFight;
    public AudioClip Jingle;
    private float StartAveragesOfEnemiesHealth;
    private StateOfFight lastState;

    public AudioSource DefaultBG_Music;

    public int HighEnergyAttackTimesForTrueHype;


    public float DistanceFromPlayerToBattleManager;
    public Volume BattleBefore;


    public string CampName;

    void Start()
    {
        float healthAccum = 0;
        foreach (BookAI bookAIai in Enemies)
        {
            healthAccum += bookAIai.Health;
        }
        StartAveragesOfEnemiesHealth = healthAccum / Enemies.Count;


        
    }

    private bool battleStarted;

    private bool ChangingGraphixOFBattlePrep;
    private IEnumerator BattlePrepGraphix_TurnUpDown(bool turnOn)
    {
        if (ChangingGraphixOFBattlePrep) yield break;
        ChangingGraphixOFBattlePrep = true;

        float target = turnOn ? 1f : 0f;

        while (!Mathf.Approximately(BattleBefore.weight, target))
        {
            BattleBefore.weight = Mathf.MoveTowards(
                BattleBefore.weight,
                target,
                Time.deltaTime * 1.5f 
            );

            yield return null;
        }

        BattleBefore.weight = target;
        ChangingGraphixOFBattlePrep = false;
    }


    void Update()
    {
        PlayerSaveDataManager(); //checks if player cleared the batle area

        CheckForImpactFrame();
        bool spotted = PlayerSpotted();

        //if(Enemies.Count <= 0)
        //{
        //    this.enabled = false;
        //}

        if (!spotted && Enemies.Count > 0 && !battleStarted)
        {
            DistanceFromPlayerToBattleManager = Vector2.Distance(transform.position,Stickman.transform.position);
            if(DistanceFromPlayerToBattleManager <= 100)
            {
                if (BattleBefore.weight == 0 && !ChangingGraphixOFBattlePrep)
                {
                    StartCoroutine(BattlePrepGraphix_TurnUpDown(true));

                }

                DefaultBG_Music.gameObject.SetActive(false);
                BattleAudioP.clip = Riser;
                BattleAudioP.gameObject.SetActive(true);
                if (!BattleAudioP.isPlaying)
                {
                    BattleAudioP.time = 0f;
                    BattleAudioP.Play();

                }
            }
            else if(DistanceFromPlayerToBattleManager > 100)
            {
                if (BattleBefore.weight == 1 && !ChangingGraphixOFBattlePrep)
                {
                    StartCoroutine(BattlePrepGraphix_TurnUpDown(false));

                }

                DefaultBG_Music.gameObject.SetActive(true);
                BattleAudioP.gameObject.SetActive(false);

                BattleAudioP.Pause();
            }
            battleStarted = false;
            return;
        }

        if (!battleStarted)
        {

            battleStarted = true;
            currentState = StateOfFight.EnemyDominance;
            lastState = currentState;

            RunningSwitchMusicInstance = false;
            StopAllCoroutines();
            StartCoroutine(BattlePrepGraphix_TurnUpDown(false));

            Stickman.CameraScript.ZoomTo(Stickman.CameraScript.BattleCamSize, 1);


            BattleAudioP.clip = BattleStart;
            BattleAudioP.time = 0f;
            BattleAudioP.Play();
            return;
        }

        DurationOfFight += Time.deltaTime;

        SwitchState();

        if (currentState != lastState && !RunningSwitchMusicInstance)
        {
            HandleStateMusic(currentState);
            lastState = currentState;
        }

        RemoveDeadEnemies();

        if(!AlrJingle && Enemies.Count <= 0)
        {
            AlrJingle = true;
            BattleAudioP.enabled = false;
            StartCoroutine(WaitForAudioPStopPlay());
            Stickman.CameraScript.ZoomTo(Stickman.CameraScript.OG_CAM_SIZE, 1);

        }









        if (TutorialChecker())
        {
            TutorialManager();
        }



    }

    private bool AlrJingle;



    public bool TutorialChecker()
    {
        foreach(BookAI ai in Enemies)
        {
            if (ai.isTutorial)
            {
                return true;
            }
        }
        return false; // if nothing aint  a tut
    }

    public IEnumerator WaitForAudioPStopPlay()
    {
        while (Stickman.PlayerReadyForImpactFrame)
        {
            yield return null;
        }
        AudioP.PlayOneShot(Jingle);
        yield return new WaitForSecondsRealtime(3f);
        DefaultBG_Music.gameObject.SetActive(true);

        yield break;
    }
    public void RemoveDeadEnemies()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] != null && Enemies[i].Health <= 0)
            {
                Enemies.RemoveAt(i);
            }
        }
    }

    void HandleStateMusic(StateOfFight state)
    {
        if (RunningSwitchMusicInstance) return;

        switch (state)
        {
            case StateOfFight.EnemyDominance:
                PlayIfNot(BattleStart);
                break;

            case StateOfFight.Instability:
                StartCoroutine(SwitchMusic(
                    BattleIntensifyingStart,
                    true,
                    StringsEssemble1
                ));
                break;

            case StateOfFight.EnemyDominace2:
                PlayIfNot(EnemyDominance);
                break;

            case StateOfFight.PlayerTurningPoint:
                StartCoroutine(SwitchMusic(
                    CallForHeroismRedemptionComeback,
                    true,
                    PlayerComback
                ));
                break;

            case StateOfFight.Finale:
                StartCoroutine(SwitchMusic(
                    AlmostConcluding,
                    true,
                    OddsAreInPlayer
                ));
                break;
        }
    }

    void PlayIfNot(AudioClip clip)
    {
        if (BattleAudioP.clip == clip) return;

        BattleAudioP.clip = clip;
        BattleAudioP.Play();
    }

    private bool RunningSwitchMusicInstance;

    public IEnumerator SwitchMusic(AudioClip Music, bool TransitionAudioNeeded, AudioClip TransitionMusic)
    {
        Debug.Log("Entered Switch Music");
        RunningSwitchMusicInstance = true;
        AudioClip currentClip = BattleAudioP.clip;
        float currentClipDuration = currentClip.length - BattleAudioP.time;

        float timer = 0;
        while (timer < currentClipDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        BattleAudioP.clip = Music;
        Debug.Log(" Switched Music!!");

        if (!BattleAudioP.isPlaying)
        {
            BattleAudioP.Play();
        }

        RunningSwitchMusicInstance = false;

        if (TransitionAudioNeeded)
        {
            StartCoroutine(SwitchMusic(TransitionMusic, false, null));
        }

        yield break;
    }

    public bool PlayerSpotted()
    {
        foreach (BookAI bookAIai in Enemies)
        {
            if (bookAIai.EnemVision.PlayerSpotted)
            {
                foreach (BookAI enemy in Enemies)
                {
                    enemy.EnemVision.PlayerSpotted = true;
                }

                    if (DefaultBG_Music.gameObject.activeSelf)
                {
                    DefaultBG_Music.gameObject.SetActive(false);
                    BattleAudioP.gameObject.SetActive(true);

                }
                return true;
            }
        }
        return false;
    }

    public float AveragesOfEnemyHealth()
    {
        float healthAccum = 0;
        foreach (BookAI bookAIai in Enemies)
        {
            healthAccum += bookAIai.Health;
        }
        return healthAccum / Enemies.Count;
    }

    public void AverageDIS_Calc()
    {
        float DisCalcTemp = 0;
        for (int i = 0; i < Enemies.Count; i++)
        {
            DisCalcTemp += Enemies[i].DistanceFromPlayer;
        }
        AverageDis = DisCalcTemp / Enemies.Count;
    }

    public int TimesEnemiesReleasedHighEnergyAttacks()
    {
        int times = 0;
        foreach (BookAI bookAI in Enemies)
        {
            times += bookAI.TimesHighEnergyAttack;
        }
        return times;
    }

    public void PlayClipSection(AudioClip clip, float startTime = 0f)
    {
        BattleAudioP.clip = clip;
        BattleAudioP.time = startTime;
        BattleAudioP.Play();
    }

    public float AveragePlayerHits()
    {
        float times = 0;
        foreach (BookAI ai in Enemies)
        {
            times += ai.TimesDamagedPlayer;
        }
        return times;
    }

    public float AverageEnemyHits()
    {
        float times = 0;
        foreach (BookAI ai in Enemies)
        {
            times += ai.TimesPlayerDamagedEnemy;
        }
        return times;
    }

    public void SwitchState()
    {
        if (Enemies.Count <= 0) return;

        if (AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.90f && currentState == StateOfFight.EnemyDominance)
        {
            currentState = StateOfFight.Instability;
        }

        if (
            currentState == StateOfFight.Instability &&
            AverageEnemyHits() + 5 < AveragePlayerHits()
        )
        {
            currentState = StateOfFight.PlayerTurningPoint;
        }
        else if (
            currentState == StateOfFight.Instability &&
            AveragePlayerHits() + 5 < AverageEnemyHits()
        )
        {
            currentState = StateOfFight.EnemyDominace2;
        }

        if (
            currentState == StateOfFight.EnemyDominace2 &&
            AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.30f &&
            (BattleAudioP.clip == PlayerComback || BattleAudioP.clip == EnemyDominance)
        )
        {
            currentState = StateOfFight.Finale;
        }

        if (
            currentState == StateOfFight.PlayerTurningPoint &&
            AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.30f &&
            (BattleAudioP.clip == PlayerComback || BattleAudioP.clip == EnemyDominance)
        )
        {
            currentState = StateOfFight.Finale;
        }

        if (
            Stickman.StickHP.PlayerHP <= 50 &&
            AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.30f ||

            AveragePlayerHits() > 30 &&
            currentState == StateOfFight.EnemyDominace2 ||

            AveragePlayerHits() > 30 &&
            currentState == StateOfFight.PlayerTurningPoint ||

            Enemies[UnityEngine.Random.Range(0, Enemies.Count)].StickmanState == StickmanState.Pushing &&
            AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.30f ||

            Enemies[UnityEngine.Random.Range(0, Enemies.Count)].StickmanBehavior == StickmanBehavior.onAdrenaline &&
            AveragesOfEnemyHealth() / StartAveragesOfEnemiesHealth < 0.30f ||

            TimesEnemiesReleasedHighEnergyAttacks() >= HighEnergyAttackTimesForTrueHype
        )
        {
            currentState = StateOfFight.Finale;
        }
    }



    public void CheckForImpactFrame() 
    {
        foreach (BookAI ai in Enemies)
        {
            if (ai.isInIFrame) 
            {
                StartCoroutine(SomeoneIsInIF(ai));
            }
        }
    }
    private bool isRunningIF_Manager;
    public IEnumerator SomeoneIsInIF(BookAI AI)
    {
        if (isRunningIF_Manager) yield break;
        while(AI.isInIFrame)
        {
            foreach (BookAI aiC in Enemies)
            {
                if (!aiC.isInIFrame)
                {
                   aiC.gameObject.SetActive(false);
                }
            }
            yield return null;
        }
        foreach (BookAI aiC in Enemies)
        {
            if (!aiC.gameObject.activeSelf)
            {
                aiC.gameObject.SetActive(true);
            }
        }

        yield break;


    }


    public void PlayerSaveDataManager()
    {
        /// yo this code is extremly strict so careful with naming yo strings
        if (CampName == "CH0_Camp1")
        {
            if (Enemies.Count <= 0)
            {
                Stickman.Camp1ClearedCH0 = true;
            }

            if (Stickman.Camp1ClearedCH0 && Enemies.Count > 0)
            {
                Debug.Log("Camp Has Been Cleared");
                DefaultBG_Music.gameObject.SetActive(true);
                BattleAudioP.enabled = false;

                for (int i = Enemies.Count - 1; i >= 0; i--)
                {
                    if (Enemies[i] != null)
                    {
                        Destroy(Enemies[i].gameObject);
                        Enemies.RemoveAt(i);
                    }
                }

                Enemies.Clear();
                this.enabled = false;
            }
        }
    }


    public void TutorialManager()
    {
        //foreach (BookAI aiC in Enemies)
        //{
        //    if(aiC.GN_F_T != null)
        //    {
        //        if(aiC.TimesHitPlayer > 3)
        //        {
        //            foreach(BookAI ai in Enemies)
        //            {
        //                ai.TimesHitPlayer = 0;
        //            }
        //        }
        //    }
        //}
        //int sumOfAllAttack = 0;

        ////foreach (BookAI aiC in Enemies)
        ////{
        ////    sumOfAllAttack += aiC.TimesHitPlayer;
        ////}

        //for(int i = 0; i < Enemies.Count; i++)
        //{
        //    sumOfAllAttack += Enemies[i].TimesHitPlayer;
        //}
        

        //if(sumOfAllAttack > 2)
        //{
        //    // check the enemies for a tutorial script

        //    foreach(BookAI ai in Enemies)
        //    {
        //        if(ai.GN_F_T != null)
        //        {
        //            ai.GN_F_T.Failed = true;
        //        }
        //    }
        //}
    }


}
