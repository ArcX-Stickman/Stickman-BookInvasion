using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Embry_IntroCinematic : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator Anim; 
    public int StartDirec;

    public bool WantDirfferentFace;

    public EnemyAI EnemyAI;

    public float DelayCourintine;

    public Movement StickScirpt;
    public StickMoveGlitch stickMoveGlitch;

    public Health StickHP;

    public AudioSource BattleTheme;

    public AudioSource BG_Theme;

    public AudioSource BG_ThemeLow;

    public BattleManager battleManager;
    public AudioClip Wind;
    public GameObject bossTxt;
    public AudioSource AudioP;
    public AudioClip CinematicBassDrop;
    public DeactivateCinematicEnemyTrigger WindSFX;
    public AudioSource HeartBeart;
    public bool Turned;
    void OnEnable()
    {
        StickScirpt.TimeManager.WantAudioSame = true;

        bossTxt.gameObject.SetActive(false);
        StickScirpt.GrassBlades.gameObject.SetActive(false);
        StickScirpt.FootP.mute = true;
        battleManager.enabled = false;
        BG_Theme.enabled = false;   
        BG_ThemeLow.enabled = false;
        StickScirpt.ResetAllAnimationStates();
        StickScirpt.rb.velocity = Vector3.zero;
        StickScirpt.enabled = false;

        stickMoveGlitch.enabled = false;
        EnemyAI.enabled = false;
        Anim.enabled = false;
        if(WantDirfferentFace)
        {
            Vector3 originalScale = transform.localScale;
            if (WantDirfferentFace)
            {
                transform.localScale = new Vector3(originalScale.x * StartDirec, originalScale.y, originalScale.z);
            }


        }
        StartCoroutine(Thing());

    }

    public IEnumerator Thing()
    {
        StickScirpt.HeCanMove = false;
        StickScirpt.enabled = false;
        stickMoveGlitch.enabled = false;

        yield return new WaitForSecondsRealtime(2);
        if(WantDirfferentFace)
        {
            transform.localScale = new Vector3(transform.localScale.x * StartDirec, transform.localScale.y,transform.localScale.z);
            Turned = true;
        }
        HeartBeart.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        // Attack Anim
        Anim.enabled = true;
     
        Anim.SetTrigger("Ranged");
        StickScirpt.enabled = true;
        stickMoveGlitch.enabled = true;


        //This is only for Embryology or any counter based enemies
        EnemyAI.RangeManager.enabled = false;
        float HealthTemp = EnemyAI.Health;
        float PlayerTempHP = StickHP.PlayerHP;
        // while (EnemyAI.Health == HealthTemp && PlayerTempHP == StickHP.PlayerHP && timer < 10f)
        // {
        //     StickScirpt.HeCanMove = false;
        //     timer += Time.deltaTime;
        //     yield return null;
        // }
        EnemyAI.enabled = true;
        EnemyAI.HPSlide.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.00001f);
        Anim.SetBool("Run", false);
        EnemyAI.enabled = false;



        EnemyAI.RangeManager.enabled = true;
        yield return new WaitForSecondsRealtime(1);
        if (CinematicBassDrop != null && AudioP != null)
        {
            AudioP.PlayOneShot(CinematicBassDrop);
        }
        while (!StickScirpt.CanCatch)
        {

            EnemyAI.enabled = false;

            StickScirpt.TimeManager.SloMO();
            yield return null;
        }
        battleManager.enabled = true;
        BG_Theme.enabled = true;   
        BG_ThemeLow.enabled = true;
        EnemyAI.enabled = true;
        EnemyAI.HPSlide.gameObject.SetActive(true);
        StickScirpt.FootP.mute = false;
        bossTxt.gameObject.SetActive(false);
        StickScirpt.TimeManager.WantAudioSame = false;
        bossTxt.gameObject.SetActive(true) ;
        WindSFX.DestroyThisThing();
        Destroy(HeartBeart);
        this.enabled = false;
        yield return null;
    }
}
