using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsShot_HeliChaseQTE : MonoBehaviour
{
    public GameObject PhysicsPara;
    public GameObject PhysicsFall;
    public AudioSource AudioP;
    public AudioClip FallSFX;
    public AudioClip HurtSFX;
    private BoxCollider2D PhysicsParaBXCD;
    public StickEnterTree ImportantTreeReference;
    public Movement Stickman;
    public GameObject UI_PopUp;
    public MoveTowardsPosition ExamManager;
    private float OGSpeed;
    public Animator Exam;
    public ExamFlee ExamFlee;
    // Start is called before the first frame update
    void Start()
    {
       OGSpeed = ExamManager.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(PhysicsPara != null)
        {
            PhysicsParaBXCD = PhysicsPara.GetComponent<BoxCollider2D>();

        }
   
        if (ImportantTreeReference.StickInTree && PhysicsPara != null && !ExamFlee.Alr_Did)
        {
            Stickman.TimeManager.ForceMusicSame = false;
            ExamManager.Speed = 35;
            ExamManager.SecondDisNow = true;
            Exam.SetTrigger("Mad");
            Stickman.StickHP.MegaCharge = 100;
            UI_PopUp.gameObject.SetActive(true);
            Stickman.TimeManager.SloMO();
            PhysicsParaBXCD.enabled = true;
        }
        else
        {
            Exam.CrossFade("ExamPara_Happy", 0, 0);
            ExamManager.Speed = ExamManager.Speed;
            if(PhysicsParaBXCD != null)
            {
                PhysicsParaBXCD.enabled = false;

            }
            UI_PopUp.gameObject.SetActive(false);
        }
    }
}
