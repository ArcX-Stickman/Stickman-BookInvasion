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

public class Active_Deactive_With_Input : MonoBehaviour
{
    public KeyCode InputS;
    public GameObject Obj;

    public bool Activate;
    public bool Deactivate;

    public bool AnimationBased;

    public Animator animator;

    // Use this only for crossfading nothin else
    public string animname;
    private bool AlrDid;

    void Update()
    {
        if(Obj == null || AlrDid) return;
        if(!AlrDid)
        {
            AlrDid = true;
            if (UnityEngine.Input.GetKeyDown(InputS))
            {
            if(!AnimationBased)
            {
                if (Activate)
                {
                    Obj.gameObject.SetActive(true);
                }
                if (Deactivate)
                {
                    Obj.gameObject.SetActive(false);
                }
            }
            else
            {
                //animator = Obj.GetComponent<Animator>();
                animator.CrossFade(animname, 0, 0, 0);

            }
            
        }
        }
        
    }
}
