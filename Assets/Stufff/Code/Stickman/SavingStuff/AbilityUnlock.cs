using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    [Header("Yo This Only Works For Invisible Box Collider Or Animation Key Nothing Else Gang")]

    public Movement StickBoi;

    public bool isBackflip;
    public bool isSlide;
    public bool isDash;
    public GameObject UI;

    public Animator TextUI;
    public Animator UIAnim;
    [Header("SFX")]
    public AudioSource AudioP;
    public AudioClip BackflipJingle;
    public AudioClip SlideJingle;
    public AudioClip DashJingle;

    public PauseManager PauseManager;
    
    private void OnEnable() 
    {
        Inputted = false;
        ResumedIt = false;
        TextUI.gameObject.SetActive(true);
        UIAnim.gameObject.SetActive(true);
    }

    void Start()
    {
         if(StickBoi == null)
        {
            StickBoi = FindAnyObjectByType<Movement>();
        }
    }

    private bool Inputted;
    private bool ResumedIt;
    void Update()
    {
       if(gameObject.activeSelf && !Inputted)
        {
            PauseManager.NoHUD = true;
            PauseManager.NoMusicOnPauseForced = true;
            PauseManager.Pause();

        }
        if(Inputted && !ResumedIt)
        {
            ResumedIt = true;
            PauseManager.NoHUD = false;
            PauseManager.NoMusicOnPauseForced = false;

            PauseManager.Resume();
            TextUI.CrossFade("TextOut(UI For Ability)",0,0);
            UIAnim.CrossFade("FadeOut(UI)",0,0);

        }
        if(UnityEngine.Input.GetKeyDown(KeyCode.Backspace))
        {
            Inputted = true;
        }
    }

    public void SaveThisBoi()
    {
        if(StickBoi.StickHP.PlayerHP > 0)
        {
             AlrDid = true;
            if(UI != null)
            {
                UI.gameObject.SetActive(true);
            }
            if(isBackflip)
            {
            
                StickBoi.UnlockedBackflip = true;
            }
            if(isSlide)
            {
                StickBoi.UnlockedSlide = true;
            }
            if(isDash)
            {   
                StickBoi.UnlockedDash = true;
            }
            StickBoi.SaveProgress();

        }
       
    }

    public void PlayDatJingle()
    {
        
        if(isBackflip)
        {
            AudioP.PlayOneShot(BackflipJingle);
        }
        if(isSlide)
        {
            AudioP.PlayOneShot(SlideJingle);
        }
        if(isDash)
        {
            AudioP.PlayOneShot(DashJingle);
        }

    }
    private bool AlrDid;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !AlrDid)
        {
        
            SaveThisBoi();
        }
    }
}
