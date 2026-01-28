using System.Collections;
using UnityEngine;

public class StickSit : MonoBehaviour
{
    public DynamicBattleMusic DBM;
    public Movement StickBoi;
    public GameObject StickBoiObj;

    public float MinDis;

    private bool CanSit;
    private bool PlayerSat;

    public AudioSource AudioP;
    public AudioClip SitSFX;

    public GameObject SitText;
    public Transform SitPos;

    private Coroutine healRoutine;


    public GameObject StickCamOG;
    public GameObject StickCamCamp;

    public AudioSource CampMusic;
    public AudioSource BGMusic;

    public GameObject UI;
    public GameObject SavingObj;
    public AudioSource PauseMenu;
    public PauseManager PM;


    public AudioClip UIPopup;


    private float BufferInput = 1;
    private float LastBuffer;
    void Update()
    {
        float distance = Vector2.Distance(transform.position, StickBoi.transform.position);

        // Enemies nearby ? no sitting
        if (DBM != null && DBM.Enemies.Count > 0)
        {
            CanSit = false;
            SitText.SetActive(false);
            return;
        }

        CanSit = distance <= MinDis;

        SitText.SetActive(CanSit && !StickBoi.isSitting);
        if(PM == null)
        {
            PM = FindAnyObjectByType < PauseManager>();
        }
        if (CanSit && Input.GetKeyDown(KeyCode.S) && !PauseManager.isPaused && Time.time >= LastBuffer + BufferInput)
        {
            LastBuffer = Time.time;
            if (!StickBoi.isSitting)
                Sit();
            else
                Leave();
        }


        

    }

    public void Sit()
    {
        if (!CanSit || StickBoi.isSitting) return;
        StickBoi.SaveProgress();
        PauseMenu.mute = true;
        SavingObj.gameObject.SetActive(true);
        // StopCoroutine(MusicSwitchSmooth(false));
        BGMusic.gameObject.SetActive(false);
        CampMusic.gameObject.SetActive(true);

        UI.gameObject.SetActive(false);
        StickCamOG.gameObject.SetActive(false);
        StickCamCamp.gameObject.SetActive(true);
        StickBoi.StickAnim.CrossFade("StickSit",0,0);
        AudioP.PlayOneShot(SitSFX);

        StickBoi.isSitting = true;
        StickBoi.rb.velocity = Vector2.zero;
        StickBoi.rb.simulated = false;
        StickBoi.enabled = false;

        PlayerSat = true;
        StartCoroutine(MovePlayerToSeat());
    }


    private IEnumerator MusicSwitchSmooth(bool v)
    {
        if (v)
        {
            BGMusic.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(1.3f);
            CampMusic.gameObject.SetActive(true);

        }
        else
        {
            BGMusic.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(1.3f);
            CampMusic.gameObject.SetActive(false);
        }
    }

    public void Leave()
    {
        UI.gameObject.SetActive(true);
        PauseMenu.mute = false;

        BGMusic.gameObject.SetActive(true);
        CampMusic.gameObject.SetActive(false);
        
        PlayerSat = false;
       // StopCoroutine(MusicSwitchSmooth(true));
       // StartCoroutine(MusicSwitchSmooth(false));
        if (healRoutine != null)
            StopCoroutine(healRoutine);
        StickCamOG.gameObject.SetActive(true);
        StickCamCamp.gameObject.SetActive(false);
        StickBoi.rb.simulated = true;
        StickBoi.isSitting = false;
        StickBoi.enabled = true;
        StickBoi.StickAnim.CrossFade("Idle", 0, 0);
        StickBoi.AudioPlayer.PlayOneShot(StickBoi.LandSFX);

    }

    private IEnumerator MovePlayerToSeat()
    {
        while (Vector2.Distance(StickBoiObj.transform.position, SitPos.position) > 0.05f)
        {
            StickBoiObj.transform.position =
                Vector2.MoveTowards(
                    StickBoiObj.transform.position,
                    SitPos.position,
                    6f * Time.deltaTime
                );

            yield return null;
        }

        healRoutine = StartCoroutine(HealPlayer());
    }

    private IEnumerator HealPlayer()
    {
        StickBoi.StickHP.AudioP.PlayOneShot(StickBoi.StickHP.HealSFX);

        while (StickBoi.isSitting && StickBoi.StickHP.PlayerHP < 100f)
        {
            StickBoi.StickHP.PlayerHP += 20f * Time.deltaTime;
            StickBoi.StickHP.PlayerHP = Mathf.Min(StickBoi.StickHP.PlayerHP, 100f);
            yield return null;
        }
    }
}
