using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemistry : MonoBehaviour
{
    public RangeSight RangeSight;
    public Animator Chem_Anim;
    public AudioSource AudioP;
    public AudioClip SFX;
    public GameObject NeuroScience_Flash;
    public TimeManager TimeManager;
    public Health StickHP;
    public RangeSight RPGSight;
    public Movement StickManager;
    public float HealthBeforeAction;
    public Missle_Code Missle_Code;
    public GameObject SlideHUD;
    public Movement PlayerIG;
    public bool isShooting;
    public bool NotSecondTime;
    public GameObject Missle_Follow_Type;
    // Update is called once per frame
    void Update()
    {
        
        if(StickManager == null)
        {
            StickManager = StickHP.gameObject.GetComponent<Movement>();
        }
        if (RangeSight.StickHere && !StickHP.Testing )
        {
            if (StickManager.isSliding)
            {
                StickManager.SlideCancelSpeed = 300;

            }
            isShooting = true;
            Chem_Anim.SetTrigger("RPG");
        }
    }

    public void NotSecondBoi()
    {
        if (NotSecondTime)
        {
            Missle_Follow_Type.gameObject.SetActive(true);
        }
    }


    public void MissleCall()
    {
        if ((Missle_Code != null))
        {
            Missle_Code.gameObject.SetActive(true);
            Missle_Code.Calc_Stick();
            Missle_Code.CanGo = true;
        }

 
    }
    public void PlaySoundFX()
    {
        AudioP.PlayOneShot(SFX);
    }

    public void ActivateHUD()
    {
        SlideHUD.gameObject.SetActive(true);
    }
    public void DeactivateHUD()
    {
        SlideHUD.gameObject.SetActive(false);
    }

    public void SloMo()
    {
        NeuroScience_Flash.gameObject.SetActive(false);
        TimeManager.SloMO();
    }

    public void NeuroFlashRemove()
    {
        NeuroScience_Flash.gameObject.SetActive(false);
    }
}
