using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TutorialKeyInput : MonoBehaviour
{
    public string Animname;
    public GameObject GameObject;
    private Animator animator;
    public KeyCode InputS;
    public string Txt;
    private PauseManager Pause;
    public bool FreezeTime;

    public bool SloMo;

    private bool Clicked;
    // Update is called once per frame
    void Start()
    {
        Clicked = false;
    }
    void Update()
    {
        if (Pause == null)
        {
            Pause = FindAnyObjectByType<PauseManager>();
            if (Pause == null) return;
        }

        TimeScaleBased();

        if (Input.GetKeyDown(InputS))
        {
            Clicked = true;
            Pause.NoHUD = false;
            animator.CrossFade(Animname, 0, 0);
            if (FreezeTime || SloMo)
                Pause.Resume();
        }
}

    

    public void TimeScaleBased()
    {
        if (gameObject.activeSelf && !Clicked)
        {
            if (FreezeTime)
            {
                Pause.NoHUD = true;
                Pause.Pause();
            }
            else if (SloMo)
            {           
                Time.timeScale = 0.4f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }

    }
}
