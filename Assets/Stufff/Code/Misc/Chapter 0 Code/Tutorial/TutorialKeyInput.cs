using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
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
    // Update is called once per frame
    void Update()
    {

        animator = GameObject.GetComponent<Animator>();
        if (Input.GetKeyDown(InputS))
        {
            //TextMeshProUGUI txt = GameObject.GetComponent<TextMeshProUGUI>();
            //txt.text = Txt;
            animator.CrossFade(Animname, 0, 0);
        }

    }
}
