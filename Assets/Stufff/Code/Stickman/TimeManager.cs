using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slomoRate;
    public float slomoDelay;
    public AudioSource AudioP;
    public AudioSource SlowChase;
    public float FlashslomoRate;
    public SpriteRenderer Stickman;
    public bool IsFlash;
    public GameObject SloMoFX;
    public PauseManager PauseChecker;
    public bool In_SloMo;
    public bool WantAudioSame;
    public Movement StickManager;
    public bool ForceMusicSame;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        SloMoFX.gameObject.SetActive(false);
    }
    private void Update()
    {
        
        

        if (Time.timeScale == 1)
        {
            SloMoFX.gameObject.SetActive(false);
            In_SloMo = false;
            WantAudioSame = false;
        }

        if (!IsFlash && !StickManager.isInPerfectDodge)
        {
            Time.timeScale += (1f / slomoDelay) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.1f, 1f); 
            AudioP.pitch += (1f / slomoDelay) * Time.unscaledDeltaTime;
            AudioP.pitch = Mathf.Clamp(AudioP.pitch, 0.5f, 1f);
            if(SlowChase != null && !ForceMusicSame)
            {
                SlowChase.pitch += (1f / slomoDelay) * Time.unscaledDeltaTime;
                SlowChase.pitch = Mathf.Clamp(SlowChase.pitch, 0.5f, 1f);
            }
           
        }
    }




    public void SloMO() 
    {
        Debug.Log("Slo Mo Babbyyy");
        In_SloMo = true;


        if (Time.timeScale == 0)
        {
            Debug.Log("Fixing time scale before slow-mo");
            Time.timeScale = 1f;
        }

        Time.timeScale = slomoRate;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        if (!WantAudioSame && !ForceMusicSame)
        {
            AudioP.pitch = 0.5f;
            SlowChase.pitch = 0.5f;


        }
        SloMoFX.gameObject.SetActive(true);
    }

    public void RevertSpeed()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        AudioP.pitch = 1;

    }

    private bool Waiting;
    public void FreezeTime(float duration)
    {
        if (Time.timeScale < 1) return;
        if (Waiting) return;
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        AudioP.pitch = 1;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        Waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        AudioP.pitch = 1;
        Waiting = false;

    }


}
