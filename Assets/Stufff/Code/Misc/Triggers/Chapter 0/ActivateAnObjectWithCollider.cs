using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivateAnObjectWithCollider : MonoBehaviour
{
    [Header("Main Object")]
    public GameObject TheObject;
    public GameObject Enemies;
    public bool wantAlrDid;
    public bool AlrDid;
    public bool WantDeactivate;
    public bool isTut;
    public string txtt;
    public KeyCode KeyCodeE;
    public bool Wantonce;
    public bool Alrdid;
    public bool StartActive;
    public bool WantMuteAudio;
    public bool TurnOnAudio;

    [Header("Optional Object")]
    public GameObject OptionalExtraObject;
    public bool OptionalObj_is_TutorialBased;
    public bool Opt_Activate;
    public string AnimTrans;

    [Header("Timer Based Settings")]
    public bool WantDelayForActivation;
    public float Delay;

    private bool TimerStart;


    [Header("Extra Settings")]

    public bool StickDisabledWhenObjActivate;



    void Start()
    {
        if (!StartActive && TheObject != null)
            TheObject.SetActive(false);
    }

    void Update()
    {
        if (TheObject == null) return;

        if (OptionalObj_is_TutorialBased && Input.GetKeyDown(KeyCodeE))
            HandleOptionalObject();
    }


    private void HandleOptionalObject()
    {
        if (OptionalExtraObject == null) 
            return;

        Animator anim = OptionalExtraObject.GetComponent<Animator>();

        if (anim != null)
        {
            if (!Opt_Activate)
                anim.CrossFade(AnimTrans, 0, 0);
        }
        else
        {
            OptionalExtraObject.SetActive(Opt_Activate);
        }
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (Alrdid) return;

        if (Wantonce)
            Alrdid = true;


        if (StickDisabledWhenObjActivate)
        {
            StartCoroutine(Stick_Requested_Disable());
        }

        if (WantMuteAudio && TheObject != null)
        {
            AudioSource audioSource = TheObject.GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.mute = true;

            audioSource.enabled = false;
        }
        if (TurnOnAudio && TheObject != null)
        {
            AudioSource audioSource = TheObject.GetComponent<AudioSource>();
            if (audioSource != null)
                audioSource.mute = false;
            audioSource.enabled = true;
          
        }

        if (OptionalExtraObject != null)
            OptionalExtraObject.SetActive(true);

        if (isTut && !WantDeactivate && TheObject != null)
        {
            TextMeshProUGUI txt = TheObject.GetComponent<TextMeshProUGUI>();
            if (txt != null)
                txt.text = txtt;

            TutorialKeyInput keyCode = TheObject.GetComponent<TutorialKeyInput>();
            if (keyCode != null)
                keyCode.InputS = KeyCodeE;
        }

        if (WantDeactivate)
        {
            if (TheObject != null)
                TheObject.SetActive(false);
            return;
        }

        if (WantDelayForActivation)
        {
            if (!TimerStart)
                StartCoroutine(TimerBased());
            return;
        }

        if (wantAlrDid)
        {
            if (!AlrDid && TheObject != null)
            {
                TheObject.SetActive(true);
                AlrDid = true;
            }
        }
        else
        {
            if (TheObject != null)
                TheObject.SetActive(true);
        }
    }


    public IEnumerator TimerBased()
    {
        TimerStart = true;
        yield return new WaitForSecondsRealtime(Delay);

        if (TheObject != null)
            TheObject.SetActive(true);

        TimerStart = false;
    }


    public IEnumerator Stick_Requested_Disable()
    {
        Movement Stick = FindAnyObjectByType<Movement>();

        while (TheObject != null)
        {
            Stick.StopFootsteps();
            Stick.ResetAllAnimationStates();
            Stick.rb.velocity = Vector2.zero;
            Stick.enabled = false;
            yield return null;
        }

        Stick.enabled = true;
        Stick.rb.simulated = true;


        yield break;
    }
}
