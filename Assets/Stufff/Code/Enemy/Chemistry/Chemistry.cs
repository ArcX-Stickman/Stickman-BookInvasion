using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Movement PlayerIG;
    public bool isShooting;
    public bool NotSecondTime;
    public GameObject Missle_Follow_Type;
    public string QTE_Text;
    public TextMeshProUGUI QTE_HUD;
    private TutorialKeyInput TutorialKeyInput;
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
            MoveTowardsPosition MTP = Missle_Follow_Type.GetComponent<MoveTowardsPosition>();
            MTP.Chem = true;
            MTP.Chemistry = (this);
            QTE_HUD.gameObject.SetActive(true);
            QTE_HUD.text = QTE_Text;
            TutorialKeyInput = QTE_HUD.GetComponent<TutorialKeyInput>();
            if(TutorialKeyInput != null)
            {
                TutorialKeyInput.InputS = KeyCode.C;
            }
            
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
