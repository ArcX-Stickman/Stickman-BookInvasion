using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabBreakIn : MonoBehaviour
{
    public GameObject NormalMusic;
    public GameObject SlideMusic;
    public GameObject BreakInMusic;
    public GameObject LabBG;
    public Animator BGAnim;
    public Animator LabFade;
    public GameObject StuffInLab;
    public GameObject CamAnimDis;
    public Movement Stickman;
    public OneShot GeneticsBook;
    public TimeManager TimeManager;
    public AudioClip BassDrop;
    //public GameObject AlertUI;


    public GameObject GeneticsBookIntro;
    public Animator GenticIntroAnim;

    // public GameObject BreakWindowSFX;

    public SpriteRenderer LabRender;
    public Sprite BrokenLab;
    public AudioSource AudioP;
    public AudioClip WindowBreak;

    public GameObject WindowFX;
    public Transform WindowPos;
    public GameObject Hypothesis;

    public bool BreakIn;
    private bool AlrBrokeIn;

    public GameObject LightinLab;

    public BG_Music BG_Music;

    // Start is called before the first frame update
    void Start()
    {
        //AlertUI.SetActive(false);
        BreakIn = false;
        GenticIntroAnim = GeneticsBookIntro.GetComponent<Animator>();
        SlideMusic.gameObject.SetActive(false);
        LabBG.gameObject.SetActive(false);
        StuffInLab.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Stickman.Checkpoint == Stickman.ExamHeliID_Checkpoint && !AlrBrokeIn)
        {
            BG_Music.LowHP_BG.gameObject.SetActive(false);
            BreakInMusic.gameObject.SetActive(true);
            LightinLab.gameObject.SetActive(true);
            Stickman.BreakIn = true;
            AlrBrokeIn = true;
            Stickman.InCinematic = true;
            LabBG.gameObject.SetActive(true);
            StuffInLab.gameObject.SetActive(true);
            Hypothesis.gameObject.SetActive(false);
            


        }
    }

   

    private void RestoreNormalSpeed()
    {
        Stickman.GetComponent<Animator>().speed = 1f;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale;
        AudioP.pitch = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet") && !AlrBrokeIn)
        {
            LightinLab.gameObject.SetActive(true);
            AlrBrokeIn = true;
            Stickman.InCinematic = true;
            //AlertUI.SetActive(true);
            AudioP.PlayOneShot(BassDrop);
            TimeManager.SloMO();
            
            Stickman.BreakIn = true;
            BreakIn = true;

            if(Hypothesis != null)
            {
                Hypothesis.gameObject.SetActive(false);

            }
            Stickman.StickHP.MegaCharge = 30;
            Stickman.StickHP.changerate = 0.09f;
            GenticIntroAnim = GeneticsBookIntro.GetComponent<Animator>();
            Animator Anim = CamAnimDis.GetComponent<Animator>();
            //GenticIntroAnim.SetTrigger("Turn");
            Anim.SetBool("InLab", true);
            Instantiate(WindowFX, WindowPos.position, Quaternion.identity);
            StartCoroutine(Stickman.CameraScript.ShakeCamGroundFeeling(120, 0.3f));
            LabBG.gameObject.SetActive(true);
            AudioP.PlayOneShot(WindowBreak);
            SlideMusic.gameObject.SetActive(false);
            LabRender.sprite = BrokenLab;
            LabFade.SetTrigger("Fade");
            BreakInMusic.gameObject.SetActive(true);
            StuffInLab.gameObject.SetActive(true);
        }
        if (collision.CompareTag("Bullet"))
        {
            ObjectPooler.ReturnObjectToPool(collision.gameObject);
        }


    }
}

