using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum StateOfFight
{
    EnemyDominance, Instability, EnemyDominace2, PlayerTurningPoint, Finale
}

public class DynamicBattleMusic : MonoBehaviour
{
    public List<BookAI> Enemies;
    public Movement Stickman;

    public StateOfFight currentState;
    private StateOfFight lastState;

    public AudioSource BattleAudioP;
    public AudioSource AudioP;
    public AudioSource DefaultBG_Music;

    public AudioClip Riser;
    public AudioClip BattleStart;
    public AudioClip StringsEssemble1;
    public AudioClip EnemyDominance;
    public AudioClip CallForHeroismRedemptionComeback;
    public AudioClip PlayerComback;
    public AudioClip AlmostConcluding;
    public AudioClip OddsAreInPlayer;
    public AudioClip Jingle;

    public float DurationOfFight;
    private float StartAveragesOfEnemiesHealth;

    public Volume BattleBefore;

    public string CampName;

    private bool battleStarted;
    private bool isInBattle;
    private bool AlrJingle;

    private bool ChangingGraphixOFBattlePrep;
    public float DistanceFromPlayerToBattleManager;

    void Start()
    {
        CalculateStartHealth();
    }

    void Update()
    {
        PlayerSaveDataManager();
        CheckForImpactFrame();
        if (CampAlreadyCleared)
        {
            AlrJingle = true;
            this.enabled = false;
        }

        float dist = DistanceFromPlayer();

        if (!isInBattle && !battleStarted)
        {
            if (dist <= 100)
            {
                HandleBattlePrep(true);
            }
            else
            {
                HandleBattlePrep(false);
                return;
            }
        }

        EnforceAudioState();

        if (Enemies.Count == 0)
        {
            HandleBattleEnd();
            return;
        }

        if (!PlayerSpotted() && !isInBattle)
            return;

        if (!battleStarted && PlayerSpotted())
        {
            StartBattle();
            return;
        }

        if (!isInBattle)
            return;

        DurationOfFight += Time.deltaTime;

        SwitchState();

        if (currentState != lastState)
        {
            HandleStateMusic(currentState);
            lastState = currentState;
        }

        RemoveDeadEnemies();
    }


    public float DistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, Stickman.transform.position);
    }

 
    void HandleBattlePrep(bool turnOn)
    {
        DistanceFromPlayerToBattleManager = DistanceFromPlayer();

        if (DistanceFromPlayerToBattleManager > 100)
        {
            DefaultBG_Music.gameObject.SetActive(true);
            BattleAudioP.gameObject.SetActive(false);
            return;
        }

        if (BattleBefore != null && !ChangingGraphixOFBattlePrep)
        {
            StartCoroutine(BattlePrepGraphix_TurnUpDown(turnOn));
        }

        DefaultBG_Music.gameObject.SetActive(!turnOn);
        BattleAudioP.gameObject.SetActive(turnOn);

        if (turnOn)
        {
            BattleAudioP.clip = Riser;

            if (!BattleAudioP.isPlaying)
            {
                BattleAudioP.time = 0f;
                BattleAudioP.Play();
            }
        }
        else
        {
            BattleAudioP.Pause();
        }
    }

    private IEnumerator BattlePrepGraphix_TurnUpDown(bool turnOn)
    {
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


    void StartBattle()
    {
        battleStarted = true;
        isInBattle = true;

        currentState = StateOfFight.EnemyDominance;
        lastState = currentState;

        StopAllCoroutines();

        if (BattleBefore != null)
            StartCoroutine(BattlePrepGraphix_TurnUpDown(false));

        DefaultBG_Music.Stop();
        BattleAudioP.gameObject.SetActive(true);

        PlayMusic(BattleStart);
    }

    void EnforceAudioState()
    {
        if (isInBattle)
        {
            if (DefaultBG_Music.isPlaying)
                DefaultBG_Music.Stop();
        }
        else
        {
            if (!DefaultBG_Music.isPlaying)
                DefaultBG_Music.Play();
        }
    }


    void HandleStateMusic(StateOfFight state)
    {
        if (!isInBattle) return;

        switch (state)
        {
            case StateOfFight.EnemyDominance:
                PlayMusic(BattleStart);
                break;

            case StateOfFight.Instability:
                PlayMusic(StringsEssemble1);
                break;

            case StateOfFight.EnemyDominace2:
                PlayMusic(EnemyDominance);
                break;

            case StateOfFight.PlayerTurningPoint:
                PlayMusic(PlayerComback);
                break;

            case StateOfFight.Finale:
                PlayMusic(OddsAreInPlayer);
                break;
        }
    }


    public void PlayMusic(AudioClip clip, float fadeTime = 0.1f)
    {
        if (!isInBattle) return;
        if (clip == null) return;
        if (BattleAudioP.clip == clip) return;

        StartCoroutine(FadeToNewClip(clip, fadeTime));
    }

    IEnumerator FadeToNewClip(AudioClip newClip, float fadeTime)
    {
        float startVolume = BattleAudioP.volume;

        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            BattleAudioP.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }

        BattleAudioP.clip = newClip;
        BattleAudioP.Play();

        t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            BattleAudioP.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
            yield return null;
        }

        BattleAudioP.volume = startVolume;
    }

    void SwitchState()
    {
        float ratio = GetEnemyHealthRatio();

        if (ratio < 0.9f && currentState == StateOfFight.EnemyDominance)
            currentState = StateOfFight.Instability;

        if (currentState == StateOfFight.Instability)
        {
            if (AverageEnemyHits() + 5 < AveragePlayerHits())
                currentState = StateOfFight.PlayerTurningPoint;
            else if (AveragePlayerHits() + 5 < AverageEnemyHits())
                currentState = StateOfFight.EnemyDominace2;
        }

        if (ratio < 0.3f)
            currentState = StateOfFight.Finale;
    }


    void HandleBattleEnd()
    {
        if (AlrJingle || CampAlreadyCleared) return;

        AlrJingle = true;
        isInBattle = false;
        battleStarted = false;

        BattleAudioP.Stop();

        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        AudioP.PlayOneShot(Jingle);

        yield return new WaitForSecondsRealtime(2.5f);

        DefaultBG_Music.gameObject.SetActive(true);
        DefaultBG_Music.Play();
    }


    void RemoveDeadEnemies()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            if (Enemies[i] == null || Enemies[i].Health <= 0)
                Enemies.RemoveAt(i);
        }
    }

    bool PlayerSpotted()
    {
        foreach (var e in Enemies)
            if (e.EnemVision.PlayerSpotted)
                return true;

        return false;
    }


    void CalculateStartHealth()
    {
        if (Enemies.Count == 0) return;

        float total = 0;
        foreach (var e in Enemies)
            total += e.Health;

        StartAveragesOfEnemiesHealth = total / Enemies.Count;
    }

    float GetEnemyHealthRatio()
    {
        if (Enemies.Count == 0) return 0;

        float total = 0;
        foreach (var e in Enemies)
            total += e.Health;

        return (total / Enemies.Count) / StartAveragesOfEnemiesHealth;
    }

    float AveragePlayerHits()
    {
        float total = 0;
        foreach (var e in Enemies)
            total += e.TimesDamagedPlayer;

        return Enemies.Count == 0 ? 0 : total / Enemies.Count;
    }

    float AverageEnemyHits()
    {
        float total = 0;
        foreach (var e in Enemies)
            total += e.TimesPlayerDamagedEnemy;

        return Enemies.Count == 0 ? 0 : total / Enemies.Count;
    }


    public void PlayerSaveDataManager()
    {
        if (CampName == "CH0_Camp1")
        {
            if (Enemies.Count <= 0)
            {
                Stickman.Camp1ClearedCH0 = true;
            }

            if (Stickman.Camp1ClearedCH0 && Enemies.Count > 0)
            {
                DefaultBG_Music.gameObject.SetActive(true);
                BattleAudioP.enabled = false;

                for (int i = Enemies.Count - 1; i >= 0; i--)
                {
                    if (Enemies[i] != null)
                        Destroy(Enemies[i].gameObject);
                }

                Enemies.Clear();
                CampAlreadyCleared = true;
                this.enabled = false;
            }
        }
        if (CampName == "CH0_Creek")
        {
            if (Enemies.Count <= 0)
            {
                Stickman.Camp1ClearedCH0 = true;
            }

            if (Stickman.Camp1ClearedCH0 && Enemies.Count > 0)
            {
                DefaultBG_Music.gameObject.SetActive(true);
                BattleAudioP.enabled = false;

                for (int i = Enemies.Count - 1; i >= 0; i--)
                {
                    if (Enemies[i] != null)
                        Destroy(Enemies[i].gameObject);
                }

                Enemies.Clear();
                CampAlreadyCleared = true;
                this.enabled = false;
            }
        }
    }

    private bool CampAlreadyCleared;
  
    public void CheckForImpactFrame()
    {
        foreach (BookAI ai in Enemies)
        {
            if (ai != null && ai.isInIFrame)
            {
                StartCoroutine(SomeoneIsInIF(ai));
            }
        }
    }

    private bool isRunningIF_Manager;

    public IEnumerator SomeoneIsInIF(BookAI AI)
    {
        if (isRunningIF_Manager) yield break;
        isRunningIF_Manager = true;

        while (AI != null && AI.isInIFrame)
        {
            foreach (BookAI e in Enemies)
            {
                if (e != null && !e.isInIFrame)
                    e.gameObject.SetActive(false);
            }
            yield return null;
        }

        foreach (BookAI e in Enemies)
        {
            if (e != null)
                e.gameObject.SetActive(true);
        }

        isRunningIF_Manager = false;
    }
}