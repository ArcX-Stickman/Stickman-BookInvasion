using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Embry_Attach : MonoBehaviour
{
    public bool SpecialEventOver_1st;
    public BookAI EnemyAI;
    public float moveSpeed;
    public AudioClip MegaPunchStick;
    public Animator Embry;
    public GameObject BossTheme;
    public GameObject CineCam1;

    public AudioClip QTEButtonPOP;
    public AudioClip ButtonClick;
    public AudioClip Fail;
    public AudioClip AlertLaunchKick;
    public AudioClip JetWhoosh;
    public AudioClip Impact;
    public GameObject FX_Dirt;
 

    public AudioSource AudioP;


    public Animator Embry_QTE_1_AVOID_ZY;
    public Animation Embry_QTE_1_AVOID_ZY_STARTUP;


    public GameObject[] QTEs;



    public Slider slider;

    public Movement movement;




    public float LastLaunchKick;
    public float LauchCD;
    public float Y_Axis_Launch;



    public GameObject Tendril;
    public AudioClip SpawnTen;
    public AudioClip TenRip;
    public AudioClip ThrowSFX;

    public float LastTen;
    public float TenCD;
    public Transform AimPoint;


    // Start is called before the first frame update
    void Start()
    {
        BossTheme.gameObject.SetActive(false);

        EnemyAI = GetComponent<BookAI>();
    }
    private bool OP34243242467676;
    // Update is called once per frame
    void Update()
    {

        if (movement.StickHP.Testing && !OP34243242467676)
        {
            OP34243242467676 = true;
            LauchCD /= 2;
        }


        if(!SpecialEventOver_1st)
        {
            EnemyAI.enabled = false;
        }
        else
        {
            movement.MaxSpeed = 100;
            movement.Speed = 100;
        }

        if(movement.isInPunchAttach || movement.PunchBool && EnemyAI.DistanceFromPlayer <= EnemyAI.MinMeleeDistance + 5)
        {
            if(LastLaunchKick <= 0 && SpecialEventOver_1st)
            {
                LastLaunchKick = Time.time;
                StartCoroutine(LaunchKick());
            }
        }
        else if(SpecialEventOver_1st && LastLaunchKick > 0 && Time.time >= LastLaunchKick + LauchCD)
        {
            LastLaunchKick = Time.time;
            StartCoroutine(LaunchKick());
        }

        if(Time.time >= LastTen + TenCD && SpecialEventOver_1st && LastLaunchKick > 0)
        {
            LastTen = Time.time;
            AudioP.PlayOneShot(SpawnTen);
            EnemyAI.EnemyAnim.CrossFade("EmbryTendril_Ranged", 0.3f, 0);
        }


    }



    private Projectiles lastthrownProjectile;
    public void MiniRangedProjectile()
    {
        AudioP.PlayOneShot(ThrowSFX);
        GameObject Projectile = Instantiate(Tendril, AimPoint.position, Quaternion.identity);
        lastthrownProjectile = Projectile.GetComponent<Projectiles>();

        Rigidbody2D rb = Projectile.GetComponent<Rigidbody2D>();
       // rb.AddTorque(200);



        Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        Projectile.GetComponent<Projectiles>().SetDirection(shootDirection);
        Projectile.GetComponent<Projectiles>().Accelerating = true;
        Projectile.GetComponent<Projectiles>().isOG = false;

        Projectile.transform.localScale = new Vector2(Projectile.transform.localScale.x * shootDirection.x, Projectile.transform.localScale.y);


    }
    //





    private bool Start1stEvent;


    public bool AnimatiorBoolLaunchKickReady;
    public bool AnimatiorBoolJumpReady;
    public IEnumerator LaunchKick()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        transform.Translate(Vector3.right * 40 * Time.deltaTime);

        yield return new WaitForSecondsRealtime(0.2f);
        EnemyAI.EnemyAnim.CrossFade("EmbrySpinJumpKick", 0.2f, 0);
        Vector2 FakeStickPos = new Vector2(transform.position.x, movement.transform.position.y);

        EnemyAI.enabled = false;
        Rigidbody2D rb = EnemyAI.gameObject.GetComponent<Rigidbody2D>();
        rb.simulated = false;
        // turn off any crap that messes the launch up

        Vector2 FakePos = new Vector2(FakeStickPos.x, Y_Axis_Launch);

        //float FakeDIS = Mathf.Abs(transform.position.y - FakePos.y);

        while (!AnimatiorBoolJumpReady)
        {
            yield return null;  
        }
        float speed = 150;

       
        while (Vector2.Distance(transform.position, FakePos) > 0.1f)
        {
            //FakeDIS = Mathf.Abs(transform.position.y - FakePos.y);
            transform.position = Vector2.MoveTowards(transform.position, FakePos, speed * Time.deltaTime);

           

            yield return null;
        }

        while (!AnimatiorBoolLaunchKickReady )
        {
            yield return null;
        }

        Vector2 PlayerLastLocation = movement.gameObject.transform.position;
        AudioP.PlayOneShot(AlertLaunchKick);
       // yield return new WaitForSecondsRealtime(0.f); // bufffer time for the player to gettt outta there

        float FakePlayerDIS = Mathf.Abs(transform.position.y - movement.transform.position.y);


        float fallSpeed = 500;
        float gravity = 200f;
        AudioP.PlayOneShot(JetWhoosh);
        float maxtimer = 1.3f;
        float timerSlamdown = 0;
        bool broken = false;

        while (Vector2.Distance(transform.position, FakeStickPos) > 0.5f)
        {
            timerSlamdown += Time.deltaTime;

            EnemyAI.EnemyAnim.SetTrigger("LOOP_SJK");
            if(transform.position.y <= FakeStickPos.y)
            {
                 break;
            }
            fallSpeed += gravity * Time.deltaTime;
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;


            if (timerSlamdown >= maxtimer)
            {
                broken = true;
                break;
            }
            yield return null;
        }
        AudioP.PlayOneShot(Impact);
       

        rb.simulated = true;

        rb.velocity = Vector2.zero;
        EnemyAI.enabled = true;
        EnemyAI.EnemyAnim.ResetTrigger("LOOP_SJK");



        //prevent ground clipping

       



        if (TouchingPlayer)
        {
            StartCoroutine(movement.PlayerKnockBack(5, 0, 0.5f));
            EnemyAI.DamagePlayer(25);
            
        }
        EnemyAI.EnemyAnim.CrossFade("EmbryIDLE", 0,0);
        movement.CameraScript.StartCoroutine(movement.CameraScript.ShakeCamGroundFeeling(80, 0.4f));
        while (!EnemyAI.CheckIfGrounded())
        {
            yield return null;
        }
        EnemyAI.SpawnGroundDecal(new Vector2(FakeStickPos.x,transform.position.y - 5));
        GameObject FX = Instantiate(FX_Dirt.gameObject, new Vector2(FakeStickPos.x, transform.position.y - 5), Quaternion.identity);

        float checker = 0;
        float maxcheck = 1.6f;

        while(checker < maxcheck)
        {
            if (transform.position.y < movement.transform.position.y - 8  && movement.Grounded|| transform.position.y > movement.transform.position.y + 2 && movement.Grounded)
            {
                // this is the worst case scenario if this ahappens a lot make it better
                transform.position = new Vector2(transform.position.x, movement.transform.position.y);
            }
            checker += Time.deltaTime;
            yield return null;
        }


        yield break;

    }



    private bool TouchingPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee"))
        {
            if (!SpecialEventOver_1st &&!Start1stEvent)
            {
                Start1stEvent = true;
                StartCoroutine(Special1stEvent());
                BossTheme.gameObject.SetActive(true);
            }
        }

        if (collision.CompareTag("Player"))
        {
            if (!movement.BackFlip)
            {
                TouchingPlayer = true;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TouchingPlayer = false;
        }
    }

    public IEnumerator Special1stEvent()
    {

        float ShakeCam = 60;
        Embry.CrossFade("Sliding",0,0);
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        float t = 0;
        float maxtime = 0;
        if (movement.StickHP.Testing)
        {
            maxtime = 2;
        }
        else
        {
            maxtime = 3.5f;
        }
        while(t < maxtime)
        {
            ShakeCam -= 2 * Time.unscaledDeltaTime;
            movement.CameraScript.StartCoroutine(movement.CameraScript.ShakeCamGroundFeeling(ShakeCam, 0.1f));
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            t += Time.deltaTime;

            if(t >= maxtime - 1.4f)
            {
                SpecialEventOver_1st = true;
                EnemyAI.enabled = true;

                Embry.SetTrigger("SlideEnd");
            }
            yield return null;
        }

        /// make the player QTE doging all the zygotes



        EnemyAI.enabled = true;
        float MaxSpeed_Enem = EnemyAI.MaxSpeed;
        while (!QTE_AV_ZY)
        {
            EnemyAI.MaxSpeed = 0;
            yield return null;
        }
        EnemyAI.MaxSpeed = MaxSpeed_Enem;

        yield break;
    }


    public void SwitchCam_1()
    {
        CineCam1.gameObject.SetActive(true);
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        brain.m_DefaultBlend.m_Time = 0f;
        TimeManager tm = FindAnyObjectByType<TimeManager>();
        tm.enabled = false;
        Time.timeScale = 0;
       Time.fixedDeltaTime = 0;
        StartCoroutine(QTE_1_AVOID_ZY());
        

        
    }

    public void SwitchOG_Cam()
    {
        movement.enabled = true;
        EnemyAI.enabled = true;

        if (CineCam1 != null)
        {
            CineCam1.gameObject.SetActive(false);

        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

    private bool QTE_AV_ZY = false ;
    public IEnumerator QTE_1_AVOID_ZY()
    {
        slider.gameObject.SetActive(true);
        slider.maxValue = 100;

        slider.value = slider.maxValue;

        KeyCode[] qteSequence = { KeyCode.Q, KeyCode.Q, KeyCode.D, KeyCode.A };
        List<KeyCode> playerInput = new List<KeyCode>();

        for (int i = 0; i < QTEs.Length && i < qteSequence.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            AudioP.PlayOneShot(QTEButtonPOP);
            QTEs[i].gameObject.SetActive(true);
            Animator anim = QTEs[i].GetComponent<Animator>();

            anim.CrossFade("Keynorm", 0, 0);

            TextMeshProUGUI text = QTEs[i].GetComponentInChildren<TextMeshProUGUI>();
            text.gameObject.SetActive(true);
            text.text = qteSequence[i].ToString();
        }

        float timer = 0f;


        QTE_Attributes QA = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();

        int index = 0;
        float qteDuration;
        float qteTimer;
        while (!QA.Ready)
        {


          

            if (index < qteSequence.Length)
            {
                if (Input.GetKeyDown(qteSequence[index]))
                {
                    playerInput.Add(qteSequence[index]);
                    Animator anim = QTEs[index].GetComponent<Animator>();
                    TextMeshProUGUI text = QTEs[index].GetComponentInChildren<TextMeshProUGUI>();
                    text.gameObject.SetActive(false);
                    anim.CrossFade("KeyCrackFX",0,0);


                    AudioP.PlayOneShot(ButtonClick);

                    index++;
                }
                //  wrong input = fail immediately
                else if (Input.anyKeyDown)
                {
                    KeyCode pressed =
                        Input.GetKeyDown(KeyCode.Q) ? KeyCode.Q :
                        Input.GetKeyDown(KeyCode.A) ? KeyCode.A :
                        Input.GetKeyDown(KeyCode.D) ? KeyCode.D : KeyCode.None;

                    if (pressed != KeyCode.None && pressed != qteSequence[index])
                    {
                        AudioP.PlayOneShot(Fail);
                        // instant fail
                        break;
                    }
                }
            }
            slider.value -= (1 * Time.unscaledDeltaTime) * 100;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.2f); // buffer
        // finished QTE
        slider.gameObject.SetActive(false);
        bool success = index == qteSequence.Length;

        if (success)
        {
            Embry_QTE_1_AVOID_ZY.CrossFade("EMBRY_PASS_CH0_P1_DOGING", 0.2f, 0);
        }
        else
        {
            Embry_QTE_1_AVOID_ZY.CrossFade("EMBRY_FAIL_CH0_P1_DOGING", 0.4f, 0);

        }

        foreach (GameObject G in QTEs)
        {
            G.SetActive(false);
        }


        // misc
        for (int i = 0; i < playerInput.Count; i++)
        {
            Debug.Log("Player Input List: " + playerInput[i]);
        }

        QTE_Attributes QTE_A = Embry_QTE_1_AVOID_ZY.gameObject.GetComponent<QTE_Attributes>();
        while (!QTE_A.HasFinished)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.6f);
        SwitchOG_Cam();
        QTE_AV_ZY = true;
        yield break;
    }
    private bool QTE_Right(KeyCode[] OG, List<KeyCode> Player)
    {
        if (Player.Count < OG.Length)
            return false;

        for (int c = 0; c < OG.Length; c++)
        {
            if (Player[c] != OG[c])
                return false;
        }

        return true;
    }




    public void AnimationKeys_Enable_EnemyAI()
    {
        EnemyAI.enabled = true;
    }


    public void AnimationKeys_Disable_EnemyAI()
    {
        EnemyAI.enabled = false;
    }
    

    public void PlayRip()
    {
        AudioP.PlayOneShot(TenRip);
    }
}
