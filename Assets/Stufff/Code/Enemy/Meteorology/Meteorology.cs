using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Meteorology : MonoBehaviour
{

    public Health PlayerHealth;
    public Movement PlayerManager;
    public float speed;
    public float MinDIS;
    public Transform MeteoPOS;
    public float ThunderCD;
    private float LastATK;
    public bool TouchingPlayer;
    public Transform StickmanPOS;
    public Animator MeteoAnim;
    public bool CanGo;
    public float DMG;
    public float HP;
    public GameObject PagesFX;
    public Transform FXPoint;
    public AudioSource AudioP;
    public AudioClip ThunderFX;
    public bool InRange;
    public DodgeStreaks StreakManager;
    public Rigidbody2D rb;
    public float MaxSpeed;
    public AudioClip Miss;
    public bool FacingRight;
    public LabBreakIn LabBreakIn;
    public bool Brokein;
    public bool NeedLabDependencies;
    public GameObject Genetics;
    public bool InTutorial;
    public GameObject Directions_TO_Backflip;
    public AudioSource PopUp_SFX;
    public AudioClip PopUp;
    public GameObject TutorialHUD2;
    // Make it rain
    public GameObject Cloud;
    public Animator HUDAnim;
    public float AcidRainCD;
    public Transform cloudOffset;
    public float LastAcidRain;
    public TextMeshProUGUI TipText;


    // Start is called before the first frame update
    void Start()
    {
        speed = MaxSpeed;

        if (Directions_TO_Backflip != null)
        {
            Directions_TO_Backflip.gameObject.SetActive(false);

        }


    }

    // Update is called once per frame
    void Update()
    {
        AcidRain();


        if (CanGo)
        {
            PlayerSpotted();
        }

        if (NeedLabDependencies && Genetics == null)
        {
            Brokein = true;
        }

        if (HP <= 0)
        {

            if (InTutorial)
            {

                if (!DeadPopUp && InTutorial)
                {
                    PopUp_SFX.PlayOneShot(PopUp);
                    DeadPopUp = true;

                }


                Directions_TO_Backflip.gameObject.SetActive(true);
               
               
            }
            
            MeteoAnim.SetTrigger("Dead");
        }
    }


    private bool DeadPopUp;

    public void Death()
    {
        GameObject effect = Instantiate(PagesFX, FXPoint.position, Quaternion.identity);
        Destroy(gameObject);
        PlayerHealth.PlayerHP += 5;
        PlayerHealth.MegaCharge += 4;
    }


    private int StunCount;
    private float LastStun;
    private float StunCD = 2.2f;
    public void Stun()
    {
        if (Time.time >= LastStun + StunCD && StunCount < 3)
        {
            MeteoAnim.SetTrigger("Hurt");
            StunCount += 1;

            if (StunCount == 3)
            {
                LastStun = Time.time;
            }
        }



    }
    public void PlayerSpotted()
    {
        if (InRange)
        {
            if (Time.time >= LastATK + ThunderCD)
            {
                ThunderATK();
                LastATK = Time.time;
            }
        }

        if (PlayerHealth.PlayerHP > 0 && speed > 1)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, StickmanPOS.position);

            if (distanceToPlayer <= MinDIS)
            {
                TouchingPlayer = true;
                MeteoAnim.SetBool("Run", false);
            }
            else
            {
                TouchingPlayer = false;
                MeteoAnim.SetBool("Run", true);

                Vector2 targetPosition = new Vector2(StickmanPOS.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // Flip based on movement direction
                // Flip based on movement direction
                if (targetPosition.x < transform.position.x)
                {
                    FacingRight = true;
                    transform.localScale = new Vector3(1, 1, 1);
                    
                }
                else if (targetPosition.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    FacingRight = false;
                }

            }
        }
    }

    public void Accelerate()
    {
        speed += 0.2f;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            PlayerHealth.PlayerHP += 0.2f;
            GameObject effect = Instantiate(PagesFX, FXPoint.position, Quaternion.identity);
            HP -= 0.7f;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("ArcBullet"))
        {
            PlayerHealth.PlayerHP += 0.5f;
            GameObject effect = Instantiate(PagesFX, FXPoint.position, Quaternion.identity);
            HP -= 2f;
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            
            rb.velocity = Vector2.zero;
        }
        if (collision.CompareTag("StickMelee"))
        {
            PlayerHealth.PlayerHP += 0.1f;
            GameObject effect = Instantiate(PagesFX, FXPoint.position, Quaternion.identity);
            HP -= 0.3f;



            Stun();



        }
    }



    private void AcidRain()
    {
        if (!NeedLabDependencies)
        {
            if (Time.time >= AcidRainCD + LastAcidRain)
            {

                if (FacingRight)
                {
                    cloudOffset.position = new Vector3(StickmanPOS.position.x - 30, cloudOffset.position.y, cloudOffset.position.z);

                }
                else if (!FacingRight)
                {
                    cloudOffset.position = new Vector3(StickmanPOS.position.x + 30, cloudOffset.position.y, cloudOffset.position.z);
                }


                Vector2 spawnPosition = cloudOffset.position;


                GameObject cloud = Instantiate(Cloud, spawnPosition, Quaternion.identity);


                Destroy(cloud, 5f);


                LastAcidRain = Time.time;
            }
        }
        else if (NeedLabDependencies && Brokein)
        {
            if (Time.time >= AcidRainCD + LastAcidRain)
            {

                if (FacingRight)
                {
                    cloudOffset.position = new Vector3(StickmanPOS.position.x - 30, cloudOffset.position.y, cloudOffset.position.z);

                }
                else if (!FacingRight)
                {
                    cloudOffset.position = new Vector3(StickmanPOS.position.x + 30, cloudOffset.position.y, cloudOffset.position.z);
                }


                Vector2 spawnPosition = cloudOffset.position;


                GameObject cloud = Instantiate(Cloud, spawnPosition, Quaternion.identity);


                Destroy(cloud, 5f);


                LastAcidRain = Time.time;
            }
        }



       
    }






    public void ThunderATK()
    {
        MeteoAnim.SetTrigger("ATK");

    }

    public void StopMovement()
    {
        speed = 0;
    }

    public void GiveBackSpeed()
    {
        speed = MaxSpeed;
    }


    private bool PlayedSound;
    public void ThunderATKFunc()
    {
        if (InRange && PlayerManager.Grounded)
        {
            if (InTutorial && Directions_TO_Backflip != null)
            {
                if (!PlayedSound && InTutorial)
                {
                    PopUp_SFX.PlayOneShot(PopUp);
                    PlayedSound = true;

                }
                TipText.text = "Press [Q] To Evade Attacks";
                Directions_TO_Backflip.gameObject.SetActive(true);
                StartCoroutine(WaitForHUD());
            }


            PlayerManager.StickAnim.SetTrigger("Hurt");
            AudioP.PlayOneShot(ThunderFX);
            PlayerHealth.PlayerHP -= DMG;
            PlayerManager.MaxSpeed -= UnityEngine.Random.Range(0, 15); 
        }
        else
        {

            //if (InTutorial && Directions_TO_Backflip != null)
            //{
            //    Directions_TO_Backflip.gameObject.SetActive(false);
            //}

            StreakManager.StreakThing();
            AudioP.PlayOneShot(Miss);

            if (PlayerManager.BackFlip)
            {
                StartCoroutine(FreezeFX ());    
            }


        }
    }


    public IEnumerator FreezeFX()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        yield return new WaitForSeconds(0.0007f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

    }


    public IEnumerator WaitForHUD()
    {
        yield return new WaitForSecondsRealtime(3);
        HUDAnim.SetTrigger("GoBack");
    }
}
