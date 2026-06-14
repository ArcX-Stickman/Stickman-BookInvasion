using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EmbrySukugaCut : MonoBehaviour
{

    public StickCinematicVer_BossFight StickManager;
    public Animator EnemyAnim;
    public AudioClip PunchImpact;
    public AudioClip Stagger;
    public AudioSource PunchAudioP;

    public float DistanceFromPlayer;
    public float MinDisPlayer;
    public Transform ImpactFX;
    public GameObject ImpactFX_OBJ;

    public int HP;
    public Transform P1;
    public Transform P2;

    public Transform AimPoint;
    public GameObject Tendril;
    public float Speed;

    public float CamShake;

    public float MeleeCD;
    public float lastMelee;

    public float StaggerMeter;

    public float DMG_MELEE;



    public float AutoPunchCD;
    public float LastAutoPunch;

    public bool ParryPunch;
    private float SCALEX;

    public EmbryTop Top;

    public Rigidbody2D rb;

    [SerializeField]private bool CanMove = true;
    
    public Transform StickPOS;
    private void Start()
    {
        SCALEX = transform.localScale.x;
        EnemyAnim.CrossFade("EmbryFarPunchHeavy", 0, 0);
        LastSFX = Time.time;
        PunchAudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
        PunchAudioP.PlayOneShot(PunchImpact);


        AudioClip rnd = StickManager.PunchHit[UnityEngine.Random.Range(0, StickManager.PunchHit.Length)];

        PunchAudioP.PlayOneShot(rnd);

        CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
        CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake);

        GameObject FX = Instantiate(ImpactFX_OBJ.gameObject, ImpactFX.transform.position, Quaternion.identity);

    }
    public void AnimShake(float sha)
    {
        CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
        CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), sha);
    }
    // Update is called once per frame
    void Update()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position, StickManager.transform.position);

        RunImportant();   
    }

    
    public void MoveTowardsPlayer()
    {
       
        Vector3 PlayerPos = new Vector3(StickPOS.transform.position.x, transform.position.y,transform.position.z); // change number later
        float distanceFromPlayer = Vector2.Distance(transform.position, StickManager.transform.position);
        DistanceFromPlayer = distanceFromPlayer;

        if (HP <= 0) return;


        if (distanceFromPlayer > MinDisPlayer)
        {
            EnemyAnim.SetBool("Run", true);

            transform.position = Vector3.MoveTowards(transform.position, PlayerPos, Speed * Time.deltaTime);
        }
        else
        {
            EnemyAnim.SetBool("Run", false);

        }

        Vector2 targetPosition = new Vector2(StickManager.transform.position.x, transform.position.y);
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(SCALEX, 1, 1);

        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-SCALEX, 1, 1);
        }

    }


    public void RunImportant()
    {
        if (!OnceSM)
        {
            // Animations:
            AnimationManager();


            //movement
            if (CanMove)
            {
                MoveTowardsPlayer();

            }
            else if (CanMove == false && EnemyAnim.GetBool("Run"))
            {

                EnemyAnim.SetBool("Run", false);
            }

            // Managers
            TopManager();
        }
       
        StaggerManager();
    }

    private bool OnceSM;

    public Sprite[] StickFreezeSprites;

    public void StaggerManager()
    {
        if (StaggerMeter > 0) return;

        if (OnceSM) return;
       
        OnceSM = true;

        StickManager.StickAnim.enabled = false;

        SpriteRenderer StickR = StickManager.gameObject.GetComponentInChildren<SpriteRenderer>();
        StickR.sortingOrder = gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder + 10;
        switch (StickManager.punchIndex)
        {

            case 0:
                StickR.sprite = StickFreezeSprites[0];
                break;

            case 1:
                StickR.sprite = StickFreezeSprites[1];

                break;

            case 2:
                StickR.sprite = StickFreezeSprites[2];
                break;
            default:
                StickR.sprite = StickFreezeSprites[0];
                break;
        }

        PunchAudioP.PlayOneShot(Stagger);

        EnemyAnim.CrossFade("FarStagger", 0, 0);


        CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
        CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake * 2);

        GameObject FX = Instantiate(ImpactFX_OBJ.gameObject, transform.position, Quaternion.identity);
        StartCoroutine(ZoomCamIn());
        StartCoroutine(SlowlySlide());
       return;


    }
    [SerializeField] private float timerSlide;

    private bool Slid;

    public IEnumerator SlowlySlide()
    {
        if(Slid) yield break;
        Slid = true;
        float horizontalMomentum = 0.5f;
    
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector2(transform.localScale.x * ((horizontalMomentum)), 0);

        BoxCollider2D my = gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D Stick = StickManager.gameObject.GetComponent<BoxCollider2D>();
        float flipTime = 0.7f;
        float elapsedTime = 0f;
        
        while (flipTime > elapsedTime)
        {
            Physics2D.IgnoreCollision(my, Stick, true);

            timerSlide = elapsedTime;

            if (transform.localScale.x >= 0)
            {
                new Vector2(transform.localScale.x * ((horizontalMomentum)), 0);
            }
            else
            {
                new Vector2(transform.localScale.x * ((horizontalMomentum)), 0);
            }

            elapsedTime += Time.deltaTime;

            yield return null;

        }
        Physics2D.IgnoreCollision(my, Stick, false);

        rb.bodyType = RigidbodyType2D.Kinematic;


    }

    public IEnumerator ZoomCamIn()
    {
        float Max = 46.82f;
        float MinusThing = 67;


        CinemachineVirtualCamera CVM = FindAnyObjectByType<CinemachineVirtualCamera>();

        while (CVM.m_Lens.FieldOfView > Max)
        {
            CVM.m_Lens.FieldOfView -= MinusThing * Time.unscaledDeltaTime;
            yield return null;
        }

        yield break;

    }

    public int TimesPlayerJumpOver = 0;

    public void TopManager()
    {
        if(TimesPlayerJumpOver > 2 && Top.PlayerOnTop)
        {
            TimesPlayerJumpOver = 0;
            EnemyAnim.CrossFade("EmbryFlySwapImmediate",0,0);
        }
    }


    private bool AlrPausedMove;

    private IEnumerator PauseMove(float t)
    {
        if (AlrPausedMove) yield break;
        CanMove = false; 
        AlrPausedMove = true;
        yield return new WaitForSecondsRealtime(t);

        CanMove = true;
        AlrPausedMove = false;

        yield break;
    }


    private float LastSFX;
    private float SFXCD = 0.2f;
    [SerializeField] private bool Jump;
    public void AnimationManager()
    {
        if(HP > 0 && StaggerMeter > 0)
        {
            if (StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
            {



                if (StickManager.PunchA)
                {
                    
                    EnemyAnim.CrossFade("Punch", 0.001f, 0);

                }
                else if (StickManager.KickA)
                {
                    
                    EnemyAnim.CrossFade("Kick", 0, 0);
                }


            }
           
            
        }
        else if(!Jump && HP <= 0)
        {
            Jump = true;
            StartCoroutine(JumpBackNThrow());
            
        }


        if (HP > 0 && StaggerMeter > 0 && Time.time >= LastAutoPunch + AutoPunchCD && !ParryPunch && DistanceFromPlayer <= MinDisPlayer)
        {
            LastAutoPunch = Time.time;
            EnemyAnim.SetTrigger("Punch");
        }



    }
    [SerializeField] private float JumpSpeed;
    private bool StartedJBNT;


    public void StaggerMeterReduce()
    {
        if(StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
        {
            StaggerMeter -= 1;
        }
        return;
    }

    private IEnumerator JumpBackNThrow()
    {
        if (StartedJBNT) yield break;
        StartedJBNT = true;
        EnemyAnim.CrossFade("EmbryFarJump", 0, 0);

        float DisP1 = Vector2.Distance(transform.position, P1.position);
        float minDis = 2.5f;
        while(DisP1 > minDis)
        {
            DisP1 = Vector2.Distance(transform.position, P1.position);
            transform.position = Vector2.MoveTowards(transform.position, P1.position, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        float DisP2 = Vector2.Distance(transform.position, P2.position);

        while (DisP2 > minDis)
        {
            DisP2 = Vector2.Distance(transform.position, P2.position);
            transform.position = Vector2.MoveTowards(transform.position, P2.position, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        EnemyAnim.SetTrigger("Reach");
        yield break;

    }

    public void AnimationSFX_Punch()
    {
        if (StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
        {

       
            if (Time.time > LastSFX + SFXCD)
            {
                LastSFX = Time.time;
                PunchAudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
                PunchAudioP.PlayOneShot(PunchImpact);
               
                
                AudioClip rnd = StickManager.PunchHit[UnityEngine.Random.Range(0, StickManager.PunchHit.Length)];

                PunchAudioP.PlayOneShot(rnd);

                CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
                CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake);

                GameObject FX = Instantiate(ImpactFX_OBJ.gameObject,ImpactFX.transform.position,Quaternion.identity);

                HP -= 1;
            }
        }
        else if(DistanceFromPlayer <= MinDisPlayer)
        {
            if (Time.time > LastSFX + SFXCD)
            {
                LastSFX = Time.time;
                PunchAudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
                PunchAudioP.PlayOneShot(PunchImpact);
                AudioClip rnd = null;
                if (!StickManager.InBackflip && DistanceFromPlayer <= MinDisPlayer)
                {
                    rnd = StickManager.PunchHit[UnityEngine.Random.Range(0, StickManager.PunchHit.Length)];
                    CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
                    CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake);

                    GameObject FX = Instantiate(ImpactFX_OBJ.gameObject, StickManager.transform.position, Quaternion.identity);

                    StickManager.Health -= DMG_MELEE;
                }
                



                


                PunchAudioP.PlayOneShot(rnd);


            }
        }
           
    }


    public void WhooshPunchSFX()
    {
        AudioClip rnd = StickManager.PunchWhooshes[UnityEngine.Random.Range(0, StickManager.PunchWhooshes.Length)];
        PunchAudioP.PlayOneShot(rnd);
        return;

    }

    public AnimationClip[] StickHurtanimations;
    public void StickHurtAnim_MELEE(int index)
    {
        string SixSevenSigma = StickHurtanimations[index].name;

        if (DistanceFromPlayer <= MinDisPlayer)
        {
            StickManager.StickAnim.CrossFade(SixSevenSigma, 0, 0);
        }
    }

    public void ThrowTendril()
    {
        GameObject TendrilS = Instantiate(Tendril.gameObject,AimPoint.transform.position,Quaternion.identity);
        StartCoroutine(MoveTendril(TendrilS)); 
    }

    private IEnumerator MoveTendril(GameObject obj)
    {
        float distance = Vector3.Distance(obj.transform.position, StickManager.transform.position);

        QTE_Main QTE = gameObject.GetComponent<QTE_Main>();

        QTE.Duration = float.MaxValue;


        QTE.StartCoroutine(QTE.QTE());

        while(distance >= 2.1f)
        {
            distance = Vector3.Distance(obj.transform.position, StickManager.transform.position);
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, StickManager.transform.position, 40 * Time.deltaTime);
            yield return null;
        }
        QTE.Duration = QTE.CurrentDuration; // this jusr stops the QTE forcefully


        QTE.finished = true;


        if (QTE.WinQTE)
        {
            Debug.Log("QTE Sucess");
        }
        else
        {
            Debug.Log("QTE fail");

        }

        yield break;
    }

    public void ParryPunchON()
    {
        ParryPunch = true;
        return;
    }

    public void ParryPunchOFF()
    {
        ParryPunch = false;
        return;
    }


    public void SmackPlayer()
    {
        if(DistanceFromPlayer <= MinDisPlayer)
        {
            StartCoroutine(PauseMove(2));
            AudioClip rnd = StickManager.PunchHit[UnityEngine.Random.Range(0, StickManager.PunchHit.Length)];

            PunchAudioP.PlayOneShot(rnd);
            CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
            CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake);
            StickManager.Health -= DMG_MELEE;
            StickManager.StartCoroutine(StickManager.BackflipMofified(20, 4));
            StickManager.StickAnim.CrossFade("JumpHurt", 0, 0);
        }

        return;
        
    }
}
