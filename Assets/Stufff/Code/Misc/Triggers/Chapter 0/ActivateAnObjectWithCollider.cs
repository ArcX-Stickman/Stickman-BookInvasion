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
    
    [Header("Optional Object")]

    public GameObject OptionalExtraObject;

    public bool OptionalObj_is_TutorialBased;
    public bool Opt_Activate;

    public string AnimTrans;

    void Start()
    {
        if (!StartActive)
        {
            TheObject.SetActive(false);

        }
    }

    void Update()
    {
        if (TheObject == null) return;
        if(Input.GetKeyDown(KeyCodeE) && OptionalObj_is_TutorialBased)
        {

            Animator anim = OptionalExtraObject.GetComponent<Animator>();
            if (anim != null)
            {
                if (!Opt_Activate)
                {
                    // The animtion name can change anytime so remeber to change this if needed
                    anim.CrossFade(AnimTrans, 0, 0);
                }
            }
            else
            {
                 if (Opt_Activate)
                {
                    OptionalExtraObject.gameObject.SetActive(true);
                }
                else
                {
                    OptionalExtraObject.gameObject.SetActive(false);

                }
            }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Alrdid)
        {
            if (WantMuteAudio)
            {
                AudioSource audioSource = TheObject.GetComponent<AudioSource>();
                audioSource.mute = true;
            }
            if(OptionalExtraObject != null)
            {
                OptionalExtraObject.gameObject.SetActive(true);
            }
            if (Wantonce)
            {
                Alrdid = true;
            }
            if(isTut && !WantDeactivate)
            {
                TextMeshProUGUI txt = TheObject.gameObject.GetComponent<TextMeshProUGUI>();
                txt.text = txtt;
                TutorialKeyInput keyCode = TheObject.gameObject.GetComponent<TutorialKeyInput>();
                keyCode.InputS = KeyCodeE;
            }
            if (WantDeactivate)
            {
                TheObject.SetActive(false);

            }
            else
            {
                if (wantAlrDid && !AlrDid)
                {
                    TheObject.SetActive(true);

                    AlrDid = true;
                }
                else if (!wantAlrDid)
                {
                    TheObject.SetActive(true);

                }
            }
            
        }
    }
}
