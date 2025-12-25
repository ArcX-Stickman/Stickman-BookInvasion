using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StateOfFight
{
    EnemyDominance, Instability, EnemyDominace2, PlayerTurningPoint, Finale
}

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

    void Update()
    {
        bool spotted = PlayerSpotted();

        if (!spotted)
        {
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
        }
    }

    private bool AlrJingle;

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
}
