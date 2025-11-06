using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HUD_Destoyer : MonoBehaviour
{


    public GameObject Dependent;
    public bool DependsOnSomthing;
    public GameObject UI;
    public AudioClip UISFX;
    public AudioSource AudioP;
    public bool WantASFX;
    public bool Yo_You_Want;


    private void Start()
    {
        if (Yo_You_Want)
        {
            UI.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if(DependsOnSomthing && Dependent == null)
        {
            gameObject.SetActive(false);

        }
    }

    public void GET_OUT()
    {
        gameObject.SetActive(false);

    }
    public void NeedParentToHide()
    {
        if (WantASFX)
        {
            AudioP.PlayOneShot(UISFX);

        }
        UI.gameObject.SetActive(false);
    }
}
