using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle_Code : MonoBehaviour
{

    public GameObject ExplosionFX;
    public Health StickHP;
    public Transform MisslePOS;
    public AudioSource AudioP;
    public AudioClip SFX;
    public Movement StickMove;
    public bool TouchedPlayer;
    public bool CanExplode;
    public bool isHelicopter;
    public Transform HelicopterPOS;
    public float Speed;
    private Vector2 StickPOSLastSeen;
    public bool CanGo;
    public float DistanceToStick;
    public float MinDis;
    public bool GoHeli;
    private bool AlrSloMo;
    public float DistanceToHeli;
    public float MinDisHeli;
    public GameObject FireHeli;
    public Animator ExamAnim;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        DistanceToStick = Vector2.Distance(transform.position,StickHP.gameObject.transform.position);
        if (isHelicopter)
        {
            DistanceToHeli = Vector2.Distance(transform.position, HelicopterPOS.position);
        }
        if (CanExplode)
        {
            Boom();
        }
        if(CanGo)
        {
            Go_ToStick();
        }
        if (GoHeli)
        {
            Go_To_Chopper();
        }
    }


    public void Go_To_Chopper()
    {
        if (!AlrSloMo)
        {
            StickMove.TimeManager.SloMO();
            AlrSloMo = true;
        }
        Speed = 200;
        float angle = Mathf.Atan2(transform.position.y - HelicopterPOS.position.y, transform.position.x - HelicopterPOS.position.x);
        float x = Mathf.Cos(angle) * Speed;
        float y = Mathf.Sin(angle) * Speed;
        Vector3 pos = new Vector3(x, y,0) * Time.deltaTime;
        transform.position -= pos;
        if(DistanceToHeli <= MinDis)
        {
            StickMove.CameraScript.StartCoroutine(StickMove.CameraScript.ShakeCamGroundFeeling(140.5f, 1f));
            Debug.Log("Missle Go BOOM");
            ExamAnim.SetBool("Sniper", false);
            ExamAnim.SetBool("Scared",true);
            ExamAnim.CrossFade("Exam_Tension", 0, 0);
            FireHeli.gameObject.SetActive(true);
            GameObject boom = Instantiate(ExplosionFX, HelicopterPOS.position, Quaternion.identity);
            AudioP.PlayOneShot(SFX);
            Destroy(gameObject);
        }
    }


    public void Calc_Stick()
    {
        StickPOSLastSeen = new Vector2(StickMove.transform.position.x, StickMove.transform.position.y);
    }
    public void Go_ToStick()
    {
        Calc_Stick();
        if(DistanceToStick > MinDis)
        {
            float angle = Mathf.Atan2(transform.position.y - StickPOSLastSeen.y, transform.position.x - StickPOSLastSeen.x);
            float x = Mathf.Cos(angle) * Speed;
            float y = Mathf.Sin(angle) * Speed;
            Vector3 pos = new Vector3(x, y) * Time.deltaTime;
            transform.position -= pos;

        }
        else
        {
            GoHeli = true;
            StickMove.StickAnim.CrossFade("Counter",0,0);
            CanGo = false;
            Go_To_Chopper();

        }


    }

    public void Boom()
    {
        if (!isHelicopter)
        {
            AudioP.PlayOneShot(SFX);
            ExplosionFX.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if ( isHelicopter)
            {
                StickMove.StickAnim.SetTrigger("Catch");
                
            }
            if (!TouchedPlayer && !StickMove.isDashing && !StickMove.isSliding && !isHelicopter)
            {
                Debug.Log(" ladies and gents WeGotEm");
                TouchedPlayer = true;
                StickHP.PlayerHP = 0;
                StickMove.CameraScript.StartCoroutine(StickMove.CameraScript.ShakeCamGroundFeeling(80.5f, 1.2f));

            }

            else
            {
                if (!TouchedPlayer)
                {
                    TouchedPlayer = true;
                    StickHP.PlayerHP += 6.8f;
                    StickHP.MegaCharge += 4.6f;
                }
                
            }
            
        }

        if (collision.CompareTag("Book"))
        {
            if (isHelicopter)
            {
                GameObject boom = Instantiate(ExplosionFX, HelicopterPOS.position, Quaternion.identity);
                AudioP.PlayOneShot(SFX);
                //Destroy(gameObject);
                StickMove.CameraScript.StartCoroutine(StickMove.CameraScript.ShakeCamGroundFeeling(45.5f, 1f));


            }
        }
        
    }
}
