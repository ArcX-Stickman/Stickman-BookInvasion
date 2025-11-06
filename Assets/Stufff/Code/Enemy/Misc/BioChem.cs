using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BioChem : MonoBehaviour
{
    public Animator BioChem_Anim;
    public GameObject KeyBoard;
    public TextMeshProUGUI KeyBoardKey;
    public AudioSource AudioP;
    public AudioClip Punch;
    public AudioClip MissSFX;
    public float Distance;
    public float MinDis;
    public Transform StickPOS;
    public Health StickHP;
    public Movement StickManager;
    public float DMG;
    public float Speed;
    public float LastPunch;
    public float PunchCD;
    public int HP;
    private bool HeDroppedItBROOO;
    public int DodgeCount;
    public HitMarker HitMarker;
    public TimeManager TimeManager;
    public GameObject Spill;
    public CameraScript CameraScript;
    public float MinDis4Event;
    private float OGCam;
    public float ZoomSize;
    // Start is called before the first frame update
    public void Start()
    {
        Spill.gameObject.SetActive(false);
        //OnceOnce = false;
        OGCam = CameraScript.CamSize;
        Spill.gameObject.SetActive(false);
        BioChem_Anim.enabled = true;
        BioChem_Anim.SetBool("Yap", true);
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector2.Distance(transform.position, StickPOS.position);
        if(Distance <= MinDis)
        {
            DropThat();

        }

        if (HeDroppedItBROOO && !Killed)
        {
            Yo_Ai_Do_SUM();
        }
       
    }

    public void DUDE_WHY_U_MAKE_ME_Spilled()
    {
    }
    public void Yo_Ai_Do_SUM()
    {
        if (!Killed)
        {
            StickManager.MaxSpeed = 0;
            StickManager.Speed = 0;
            if (HP <= 0)
            {
                BioChem_Anim.SetTrigger("Dead");

            }

            if (Time.time >= LastPunch + PunchCD)
            {
                BioChem_Anim.SetTrigger("Melee");
                LastPunch = Time.time;
            }

            if (KeyBoard.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StickManager.StickAnim.SetTrigger("Dodge");
                    StickManager.BackFlip = true;

                }
            }

            if (Distance <= MinDis4Event)
            {
                StickManager.Cart.gameObject.SetActive(false);
                StickManager.StickAnim.SetBool("Cart", false);
                StickManager.inSlipperyState = true;
                BioChem_Anim.SetTrigger("Drop");
                StickManager.rb.velocity = Vector2.zero;
                StickHP.MegaCharge = 0;



            }
            if (Distance > MinDis)
            {
                MoveTowardsStickBoi();
            }
        }
        

        
        if(DodgeCount >= 5 && !AlrDED)
        {
            Killed = true;
            AlrDED = true;
            StickManager.inSlipperyState = false;
            HitMarker.HitMarkerSFX();
            StickManager.MaxSpeed = 60;
            StickManager.Speed = 60;
            StickManager.Jump();
            StickManager.StickAnim.CrossFade("StickPunchInAir", 0, 0);
            StickManager.StickAnim.SetBool("Slip", false);
            BioChem_Anim.SetTrigger("Dead");

        }
    }
    private bool Killed;
    private bool AlrDED;
    public bool OnceOnce;
    public void DropThat()
    {
        if (!OnceOnce)  
        {
            CameraScript.CamSize = ZoomSize;
            TimeManager.SloMO();
            OnceOnce = true;
            Animator cartAnim = StickManager.Cart.GetComponent<Animator>();
            cartAnim.SetTrigger("Break");
            //StickManager.Cart.gameObject.SetActive(false);
            BioChem_Anim.SetTrigger("Drop");
            StickManager.StickAnim.SetBool("Slip", true);
            StickManager.inSlipperyState = true;
            HeDroppedItBROOO = true;
            StickManager.StickAnim.SetBool("Cart", false);
            BioChem_Anim.SetBool("Slippin", true);
            BioChem_Anim.SetBool("Yap", false);
            StickHP.MegaCharge = 0;
        }
       



    }

    public void Bye()
    {
        StickManager.HeCanMove = true;
        StickManager.ResetAllAnimationStates();
        StickManager.StickHP.PlayerHP += 40;
        StickManager.inSlipperyState = false;
        StickManager.StartCoroutine(StickManager.BackFlipMove());
        CameraScript.CamSize = OGCam;
        HitMarker.HitMarkerSFX();
        Destroy(gameObject);
       
    }
    public void MoveTowardsStickBoi()
    {
        if (Spill.gameObject.activeSelf)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickPOS.position);

            Vector2 targetPosition = new Vector2(StickPOS.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
        }  
    }
    public void EventFrame()
    {
        StickManager.BackFlip = false;
        KeyBoard.gameObject.SetActive(true);
        KeyBoardKey.text = "Q";
       

    }

    private bool forMelee;
    public void MeleeATK()
    {
        KeyBoard.gameObject.SetActive(false);

        if (!StickManager.BackFlip)
        {
            StickManager.StickAnim.CrossFade("Hurt",0,0);
            AudioP.PlayOneShot(Punch);
            StickHP.PlayerHP -= DMG;
        }
        else
        {
            //StickHP.MegaCharge += 4;
            DodgeCount += 1;
            TimeManager.FreezeTime(0.01f);
            AudioP.PlayOneShot(MissSFX);
        }
    }

    //min distance : 12.58497
    //Dis for Cart destroyer: 15.87686
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cart"))
        {
            DropThat();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cart"))
        {
            DropThat();
        }
    }
}
