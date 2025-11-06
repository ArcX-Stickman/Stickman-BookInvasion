using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public EnemyAI[] enemyAIs;
    private bool alertTriggered = false;
    private bool AlrJingle;
    public AudioSource AudioP;
    public AudioClip Jingle;
    public AudioSource BG;
    public AudioSource Battle;
    private bool EnemBattle;
    public Movement Stick;
    public AudioClip BassDrop;
    public AudioSource BassDropAudioP;
    public bool NoWantSloMo;

    public bool BossFight;

    void Update()
    {
        if(Stick == null)
        {
            Stick = FindAnyObjectByType<Movement>();
        }
        if (!alertTriggered)
        {
            foreach (EnemyAI enemy in enemyAIs)
            {
                if (enemy == null) continue;

                if (enemy.RangeManager.PlayerSpotted)
                {

                    EnemBattle = true;
                    AlertAllEnemies();
                    alertTriggered = true;
                    break;
                }
            }
        }

        if (!AlrJingle && AllEnemiesDefeated())
        {
            AlrJingle = true;
            EnemBattle = false; 
            AudioP.PlayOneShot(Jingle);
            Battle.gameObject.SetActive(false);
            StartCoroutine(WaitForJingleEnd());
        }

        else if (EnemBattle)
        {
            BG.gameObject.SetActive(false);
            Battle.gameObject.SetActive(true);
        }
    }
    private bool NotYetSloMo;
    public bool AllEnemiesDefeated()
    {
        for (int i = 0; i < enemyAIs.Length; i++)
        {
            EnemyAI enemy = enemyAIs[i];
            if (enemy == null) continue;
            if (enemy.Health <= 0)
            {
                enemyAIs[i] = null;
                continue;
            }
            return false;
        }
        if (!NotYetSloMo)
        {
            if(BassDropAudioP != null)
            {
                BassDropAudioP.PlayOneShot(BassDrop);

            }
            NotYetSloMo = true;
            if (!NoWantSloMo)
            {
                Stick.TimeManager.SloMO();

            }

        }
        Debug.Log("Yo you cooked them!");
        return true;
    }


    IEnumerator WaitForJingleEnd()
    {
        Stick.CameraScript.notlocked = true;
        float ogCamSize = Stick.CameraScript.CamSize;
        while(Stick.CameraScript.CamSize > 35)
        {

            Stick.CameraScript.CamSize -= 67.9f * Time.deltaTime;
            yield return null;
        }
        float gaptime = 1.5f;
        float timer = 0;
        while (timer < gaptime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        while(Stick.CameraScript.CamSize < ogCamSize)
        {
            Stick.CameraScript.CamSize += 97.9f * Time.deltaTime;
            yield return null;

        }
        BG.gameObject.SetActive(true);
       // Stick.CameraScript.ZoomOut(2);
        Stick.CameraScript.notlocked = false;


    }

    public void AlertAllEnemies()
    {
        foreach (EnemyAI enemy in enemyAIs)
        {
            if (enemy == null) continue;
            enemy.EnemyAnim.SetBool("PlayerSpotted", true);
            enemy.RangeManager.PlayerSpotted = true;
            enemy.PlayerSpotted();
        }

        Debug.Log("Yo You cooked fr now");
    }
}
