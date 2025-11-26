using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTE_Builder : MonoBehaviour
{
    public Movement StickScript;
    public Health StickHP;
    public bool PlayerGetsOneShot;
    //Only works when PlayerGetsOneShot is False
    public float DMG;
    public KeyCode keyCode;
    private bool FinishedQTE;

    public bool SloMo;
    public float DelayOrDuration;

    private bool Started;

    public TutorialKeyInput TutorialKeyInput;

    public bool NeedToDisablePLayer;

    private StickMoveGlitch StickMoveGlitch;

    public String txt;

    public bool PlayerBackflip;
    public bool Dash;

    public bool Sliding;

    public GameObject PopUp;

    public bool WantPopUpAtEnd;

    

    void Update()
    {
        StickMoveGlitch = StickScript.gameObject.GetComponent<StickMoveGlitch>();
        if (gameObject.activeSelf)
        {
            if(!Started)
            {
                Started = true;
                StartCoroutine(QTE());
            }

            if(Input.GetKeyDown(keyCode))
            {
                StickScript.inQTE = false;
                FinishedQTE = true;
                StopCoroutine(QTE());
                if(NeedToDisablePLayer)
                {
                    if(PlayerBackflip)
                    {
                        StartCoroutine(StickScript.BackFlipMove());
                    }
                    if(Dash)
                    {
                        StartCoroutine(StickScript.Dash());
                    }
                    if(Sliding)
                    {
                        StartCoroutine(StickScript.Slide());
                    }
                    StickScript.enabled = true;
                    StickMoveGlitch.enabled = true;

                }
                this.enabled = false;

            }
        }
    }

    public IEnumerator QTE()
    {
        StickScript.inQTE = true;
        TutorialKeyInput.InputS = keyCode;
        TutorialKeyInput.gameObject.SetActive(true);
        TextMeshProUGUI textMeshProUGUI = TutorialKeyInput.GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = txt;
        if (SloMo)
        {
            StickScript.TimeManager.SloMO();
        }
         if(NeedToDisablePLayer)
        {
            StickScript.ResetAllAnimationStates();
            //StickScript.rb.velocity = new Vector2(0,StickScript.rb.velocity.y);
            StickScript.enabled = false;
            StickMoveGlitch.enabled = false;
        }
        yield return new WaitForSeconds(DelayOrDuration);
        if(NeedToDisablePLayer)
        {
            StickScript.enabled = true;
            StickMoveGlitch.enabled = true;
        }
        if(PlayerGetsOneShot && !FinishedQTE)
        {
            StickHP.PlayerHP = 0;
        }
        else
        {
            StickHP.PlayerHP -= DMG;
        }
        StickScript.inQTE = false;
        yield break;
    }


}
