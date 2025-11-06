using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Neuroscience : MonoBehaviour
{

    public float speed;
    public Transform NeuroPOS;
    public Movement PlayerManager;
    public Health PlayerHealth;
    public Transform StickmanPOS;
    public float MinDIS;
    public bool TouchingPlayer;
    public Animator NeuroAnim;
    public Neuro_InRange RangeManager;
    public GameObject LowTaperFade;
    public float NorSpeed;
    public GameObject Flash;
    private bool CanDisplayFX = false;
    public bool PlayerJumped;
    public GameObject Dark;
    public float Health;
    public float DMG;
    public AttackSight AttackSight;
    public GameObject Dead;
    public TimeManager TimeManager;
    public bool StealthKillable;
    public GameObject UIStealth;
    public Animator MainCamAnim;
    public HitMarker HitMarker;
    public bool WantMusic;
    public AudioSource BattleTheme;
    public AudioSource Hypothesis;
    public AudioClip Jingle;
    private bool hasPlayedStealthKill = false;
    public AudioSource AudioP;

    // Attack CD
    public float ClapCD;
    private float LastClap;



   

    // Start is called before the first frame update
    void Start()
    {
        LowTaperFade.gameObject.SetActive(false);
        Flash.gameObject.SetActive(false);
        speed = NorSpeed;
        LastClap = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (StealthKillable)
        {
            float distance = Vector2.Distance(transform.position, StickmanPOS.position);
            if(distance <= 30 && PlayerManager.InStealth)
            {
                UIStealth.gameObject.SetActive(true);
            }
            else
            {
                UIStealth.gameObject.SetActive(false);

            }
        }


        if (AttackSight.StickmanIsHERE)
        {
            PlayerSpotted();
        }


        if (PlayerManager.InStealthKill && !hasPlayedStealthKill)
        {
            NeuroAnim.SetTrigger("StealthKill");
            hasPlayedStealthKill = true;
        }

        if (!PlayerManager.InStealthKill)
        {
            hasPlayedStealthKill = false; 
        }


        if (PlayerManager.FirstClear)
        {
            Health = 0;
        }


        if (PlayerJumped)
        {
            PlayerSpotted();
        }


        if(Health <= 0)
        {
            NeuroAnim.SetTrigger("Dead");
            //LowTaperFade.gameObject.SetActive(false);
        }
    }

    private bool ALrPlayedJingle;
    public void Death()
    {
        if (WantMusic && !ALrPlayedJingle)
        {
            ALrPlayedJingle = true;
            
            AudioP.PlayOneShot(Jingle);
            BattleTheme.gameObject.SetActive(false);
            Hypothesis.gameObject.SetActive(true);
        }

        PlayerHealth.PlayerHP += 1;
        PlayerHealth.MegaCharge += 4;
        GameObject effect = Instantiate(Dead, NeuroPOS.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private bool AlrPlayedMusic;

    public void PlayerSpotted()
    {

        if (WantMusic && !PlayerManager.InStealthKill && !PlayerManager.InStealth)
        {
            if(!BattleTheme.gameObject.activeSelf && Health > 0)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }



            if (!AlrPlayedMusic)
            {
                AlrPlayedMusic = true;
                Hypothesis.gameObject.SetActive(false);
                BattleTheme.gameObject.SetActive(true);
            }
        }


        if (RangeManager.InRange)
        {
            Clap();
        }




        if (PlayerHealth.PlayerHP > 0 && speed > 1)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);


            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;
                NeuroAnim.SetBool("Run", false);
            }
            else
            {
                TouchingPlayer = false;
                NeuroAnim.SetBool("Run", true);

                Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


                if (targetPosition.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (targetPosition.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }


            
        }
    }





    public void Clap()
    {
        if(Time.time >= LastClap + ClapCD && AttackSight.StickmanIsHERE)
        {
            NeuroAnim.SetTrigger("Clap");
            LastClap = Time.time;

        }

        
    }



    



    public void SpeedNegative()
    {
        speed = 0;
    }


    public void RevertSpeed()
    {
        speed = NorSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerHealth.PlayerHP += 0.2f;
            GameObject effect = Instantiate(Dead, NeuroPOS.position, Quaternion.identity);
            Health -= 0.7f;
        }
        if(collision.CompareTag("ArcBullet"))
        {
            PlayerHealth.PlayerHP += 0.5f;
            GameObject effect = Instantiate(Dead, NeuroPOS.position, Quaternion.identity);
            Health -= 2f;
        }
        if (collision.CompareTag("StickMelee"))
        {
            PlayerHealth.PlayerHP += 0.1f;
            GameObject effect = Instantiate(Dead, NeuroPOS.position, Quaternion.identity);
            Health -= 0.3f;
        }
       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.InStealthKill)
            {
                HitMarker.HitMarkerSFX();
                MainCamAnim.SetBool("Zoom", false);
                NeuroAnim.SetTrigger("Dead");
                //PlayerManager.StickAnim.SetTrigger("StealthPunch");
                PlayerManager.InStealthKill = false;
                PlayerHealth.PlayerHP += 0.1f;
                GameObject effect = Instantiate(Dead, NeuroPOS.position, Quaternion.identity);
                //Destroy(gameObject);
            }

        }
    }



    public void Accelerate()
    {
        speed += 0.1f;

    }


    public void BrainFX()
    {
        PlayerManager.StickAnim.SetTrigger("Hurt");
        PlayerHealth.PlayerHP -= DMG;
        Flash.gameObject.SetActive(true);
        //LowTaperFade.gameObject.SetActive(true);
        StartCoroutine(ItsStilMassive());
    }


    
    public IEnumerator ItsStilMassive()
    {
        yield return new WaitForSeconds(3);
        //Dark.gameObject.SetActive(true);
        //LowTaperFade.gameObject.SetActive(false);
    }




}
