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

    void OnEnable()
    {

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


        yield return new WaitForSecondsRealtime(DelayCourintine);
        if(WantDirfferentFace)
        {
            transform.localScale = new Vector3(transform.localScale.x * -StartDirec, transform.localScale.y,transform.localScale.z);

        }

        yield return new WaitForSecondsRealtime(DelayCourintine);
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



        EnemyAI.RangeManager.enabled = true;
        yield return new WaitForSecondsRealtime(1);
        while(!StickScirpt.CanCatch)
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


        this.enabled = false;
        yield return null;
    }
}
