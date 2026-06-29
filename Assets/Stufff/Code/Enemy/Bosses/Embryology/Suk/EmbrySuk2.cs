using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EmbrySuk2 : MonoBehaviour
{
    [SerializeField] private float VunerableWindow;
    public StickCinematicVer_BossFight Stick;
    public int Health;

    public float LastMelee;
    public float MeleeCD;
    public bool OutOfATK;


    public bool Vunerable;
    public Animator EnemyAnim;


    public float DisEnemy;
    public float MinDis;



    public float DashSpeed;
    public float LeapSpeed;
    public float JumpSpeed;
    public float FallDown;


    public Transform PointR;
    public Transform PointL;
    public Transform PointUP;



    public GameObject MotionBlurR;
    public GameObject MotionBlurL;

    public Rigidbody2D rb;



    public AudioSource AudioP;
    public AudioClip ClapReverb;

    public AudioClip ThrowSFX;
    public AudioClip FallSFX;
    public AudioClip JumpSFX;
    public AudioClip Dash;
    public AudioClip RunSFX;
    public AudioClip Stomp;
    public AudioClip StompAftermath;

    public float CamShake;

    public GameObject Shockwave;
    public AudioClip StaggerSFX;


    public float WaitTimeCombo;
    public float WaitTimeComboMin;

    public GameObject Cutscene;
    public GameObject CutSprite;
    public GameObject CutFin;
    public Transform MidPoint;
    public GameObject Game;
    public CinemachineVirtualCamera VirtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NotVun());

        OutOfATK = true;
        Vunerable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health > 0)
        {
            DisEnemy = Vector2.Distance(transform.position, Stick.transform.position);
            VunerableManager();
            TimeCur = Time.time;
        }
        else if(Health <= 0 && Time.timeScale == 1)
        {
            MidPointTransform midPointTransform =  FindAnyObjectByType<MidPointTransform>();
            midPointTransform.enabled = false; // now position is frozen yessir
            LowManager();
        }
      


    }

    private bool ReadytoSwap;
    private bool Operation67;
    public void LowManager()
    {

        if(!ReadytoSwap &&!Operation67)
        {
            Operation67= true;
            StartCoroutine(Op67());
        }

        if(ReadytoSwap)
        {
            if(Stick.transform.localScale.x <= 0)
            {
                SpriteRenderer SR =CutSprite.GetComponent<SpriteRenderer>();
                
                SpriteRenderer SR2 = CutFin.GetComponent<SpriteRenderer>();

                SR.flipX = true;
                SR2.flipX = true;

            }
            CutSprite.transform.position = MidPoint.position;

            GameObject[] FX = GameObject.FindGameObjectsWithTag("SuperB");
            foreach (GameObject fx in FX)
            {
                Destroy(fx);
            }

            VirtualCamera.gameObject.SetActive(false);
            Cutscene.gameObject.SetActive(true);
            Game.gameObject.SetActive(false);
        }
       
       
    }


    private IEnumerator Op67()
    {
        yield return new WaitForSecondsRealtime(0.6f);
        ReadytoSwap = true;
        yield break;
    }

    private bool Leaped;
    private bool Jump;
    private bool ALrinCorNotVun;
    public IEnumerator NotVun()
    {
        if(Health <= 0) yield break;
        if (Vunerable || ALrinCorNotVun) yield break;
        yield return new WaitForSecondsRealtime(0.7f);

        ALrinCorNotVun = true;
        Stagger = 4;

        Transform prev = null;
        if (!Leaped)
        {
            AudioP.PlayOneShot(JumpSFX);
            Leaped = true;

            EnemyAnim.CrossFade("EmbryLeapSUK2",0,0);

            if (transform.localScale.x > 0)
            {
                StartCoroutine(MoveToPOS(PointR, LeapSpeed));
                prev = PointR;
            }
            else
            {
                StartCoroutine(MoveToPOS(PointL, LeapSpeed));
                prev = PointL;

            }

        }
        while (!MoveCorDone)
        {
            yield return null; // wait fo da leaop to done
        }
        MoveCorDone = false;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        GameObject MB = null;
        yield return new WaitForSecondsRealtime(0.3f);

        EnemyAnim.CrossFade("EmbryThrowSUK2", 0, 0);
        yield return new WaitForSecondsRealtime(0.2f + WaitTimeCombo);


        if (!Jump)
        {
            AudioP.PlayOneShot(Dash);

            sr.enabled = false;

            if (prev == PointR)
            {
                MotionBlurR.gameObject.SetActive(true);
                MB = MotionBlurR;
            }
            else
            {
                MotionBlurL.gameObject.SetActive(true);
                MB = MotionBlurL;

            }

            Jump = true;
            transform.position = PointUP.position;
        }

        // motin blur finished
        while (MB.activeSelf)
        {
            yield return null;
        }
        sr.enabled = true;

        MoveCorDone = false;

        // throw

        EnemyAnim.CrossFade("EmbryAirIDLESUK2", 0, 0);
        float timer = 0;
        while (timer < WaitTimeComboMin)
        {
            timer += Time.deltaTime;


            LookAtPlayerAngleBased();


            yield return null;
        }

        AudioP.PlayOneShot(ThrowSFX);

        EnemyAnim.SetTrigger("Throw");


        yield return new WaitForSecondsRealtime(0.5f);


        AudioP.PlayOneShot(FallSFX);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (prev == PointR)
        {
            transform.localScale = new Vector3(1, 1, 1);

            StartCoroutine(MoveToPOS(PointL, FallDown, Stomp));

        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);


            StartCoroutine(MoveToPOS(PointR, FallDown,Stomp ));

        }
        EnemyAnim.SetBool("Fall", true);


        ALrinCorNotVun = false;

        while (!MoveCorDone)
        {
            yield return null;
        }

        MoveCorDone = false;
        EnemyAnim.SetBool("Fall", false);

        // boi landed

        Vector3 vector3 = new Vector3(transform.position.x, -18.9f,0);
        GameObject Shock = Instantiate(Shockwave, vector3, Quaternion.identity);
        DestroyOrHide_AnimationKey destroyOrHide_AnimationKey = Shock.GetComponent<DestroyOrHide_AnimationKey>();
        destroyOrHide_AnimationKey.isOG = false;
        ShockwaveDirecCtrl SDC = Shock.GetComponent<ShockwaveDirecCtrl>();
        SDC.Targ = prev;



        yield return new WaitForSecondsRealtime(WaitTimeComboMin);

        StartCoroutine(MoveToPOS(Stick.transform, LeapSpeed,true));
        AudioP.PlayOneShot(RunSFX);
        Vector2 targetPosition = new Vector2(Stick.transform.position.x, transform.position.y);
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        EnemyAnim.CrossFade("EmbryDash", 0, 0);

        while (!MoveCorDone)
        {
            yield return null;
        }

        EnemyAnim.CrossFade("PunchDMG", 0, 0);
        LandedTime = Time.time;

        Vunerable = true;


        Jump = false;
        Leaped = false;

        yield return new WaitForSecondsRealtime(WaitTimeComboMin);

        if(VunerableWindow > 1.5f)
        {
            VunerableWindow -= 0.25f;

        }
        if (WaitTimeComboMin > 0.3f)
        {
            WaitTimeComboMin -= 0.3f;

        }

        if (WaitTimeCombo > 0.5f)
        {
            WaitTimeCombo -= 0.2f;


        }
        else
        {
            WaitTimeCombo = 0.5f;
        }

        yield break;

    }

    private float LandedTime;
    private float TimeCur;
    public int Stagger;
    public void AnimShake(float sha)
    {
        CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
        CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), sha);
    }
    public void VunerableManager()
    {
        if (!Vunerable || ALrinCorNotVun) return;

        if (Vunerable)
        {
            Vector2 targetPosition = new Vector2(Stick.transform.position.x, transform.position.y);
            if (targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);

            }
            else if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

        }


        float timeBeen = Time.time - LandedTime;

        if (timeBeen >= VunerableWindow)
        {
            Vunerable = false;
            StartCoroutine(NotVun());
            Jump = false;
            Leaped = false;
        }


        if (Stick.Attack && DisEnemy <= MinDis && Time.time >= LastMelee + MeleeCD && OutOfATK)
        {
            LandedTime += 0.25f; // buffer time for player 
            LastMelee = Time.time;
            if (Stick.PunchA)
            {

                EnemyAnim.CrossFade("Punch", 0.001f, 0);

            }
            else if (Stick.KickA)
            {

                EnemyAnim.CrossFade("Kick", 0, 0);
            }
          
            if(Stagger - 1 == 0 || Stagger == 1)
            {
                TSM.SloMO(0.2f, 0.8f);

            }


        }

       


        if (Stagger <= 0)
        {
            AnimShake(CamShake* 3);

            TSM.Stop(0.1f);
            Vunerable = false;
            StartCoroutine(NotVun());
            Jump = false;
            Leaped = false;
            AudioP.PlayOneShot(StaggerSFX);
            Health -= 1;
        }
    }

    public TimeScale_MOD TSM;

    private bool MoveCorDone;
    public IEnumerator MoveToPOS(Transform pos, float speed)
    {
       
        float minDIS = 2.5f;
        float Dis = Vector2.Distance(transform.position, pos.position);
        while (Dis > minDIS)
        {
            Dis = Vector2.Distance(transform.position, pos.position);
            transform.position = Vector2.MoveTowards(transform.position, pos.position, speed * Time.deltaTime);
            yield return null;
        }
        MoveCorDone = true;
        yield break;
    }

    public IEnumerator MoveToPOS(Transform pos, float speed,AudioClip SFX)
    {

        float minDIS = 2.5f;
        float Dis = Vector2.Distance(transform.position, pos.position);
        while (Dis > minDIS)
        {
            Dis = Vector2.Distance(transform.position, pos.position);
            transform.position = Vector2.MoveTowards(transform.position, pos.position, speed * Time.deltaTime);
            yield return null;
        }
        MoveCorDone = true;
        AudioP.PlayOneShot(SFX);

        if(SFX == Stomp)
        {
            AnimShake(CamShake);
            GameObject FX = Instantiate(LandFX.gameObject, LandPoint.position, Quaternion.identity);


        }

        yield break;
    }


    public Transform LandPoint;
    public GameObject RadFX;
    public GameObject LandFX;


    public Transform StickAim;
    public Transform EmbryAim;
    public void PunchSFX()
    {
        if (DisEnemy <= MinDis )
        {

            AnimShake(CamShake);
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);


            AudioClip rnd = Stick.PunchHit[UnityEngine.Random.Range(0, Stick.PunchHit.Length)];
            AudioP.PlayOneShot(rnd);

            AudioP.PlayOneShot(ClapReverb);


            if (Stick.Attack)
            {
                Stagger -= 1;
            }

           // Vector2 trans = new Vector2((EmbryAim.position.x + StickAim.transform.position.x / 2), (EmbryAim.position.y + StickAim.transform.position.y / 2));

            GameObject FX = Instantiate(RadFX.gameObject, EmbryAim.position, Quaternion.identity);

        }
        
    }

    public IEnumerator MoveToPOS(Transform pos, float speed, bool moveOnlyX)
    {
        if (pos == null) yield break;

        Vector3 targetPosition = pos.position;

        if (moveOnlyX)
        {
            targetPosition = new Vector3(pos.position.x, transform.position.y, 0f);
        }

        float minDIS = 2.5f;
        float distance = Vector2.Distance(transform.position, targetPosition);
        float timer = 0;
        while (distance > minDIS)
        {
            timer += Time.deltaTime;
            if (timer > 2.5) yield break;
            distance = Vector2.Distance(transform.position, targetPosition);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            yield return null;
        }

        MoveCorDone = true;
    }



    public void OutATK()
    {
        OutOfATK = true;
    }

    public void inAtk()
    {
        OutOfATK = false;
    }

    public GameObject Ranged_Pro;
    public Transform AimPoint_Ranged;
    public void ThrowAttack()
    {
        Transform EndPos = null;
        if(transform.localScale.x > 0f)
        {
            EndPos = PointL;
        }
        else
        {
            EndPos = PointR;

        }


        // crap i learned in math
        // y = a(x-h)^2 + k

        /// x = something i guess
        /// h = this is not gonna be given
        ///  k = player y position
        ///  y = the desinty

        GameObject Projectile = Instantiate(Ranged_Pro, AimPoint_Ranged.position, Quaternion.identity);


        Vector2 PlayerPos = new Vector2(Stick.transform.position.x, Stick.transform.position.y);
        StartCoroutine(ThrowAttackPredicatable(EndPos, PlayerPos, Projectile));
        return;

    }


    public void ThrowAttackPLayerTargeted()
    {
      


        // crap i learned in math
        // y = a(x-h)^2 + k

        /// x = something i guess
        /// h = this is not gonna be given
        ///  k = player y position
        ///  y = the desinty

        GameObject Projectile = Instantiate(Ranged_Pro, AimPoint_Ranged.position, Quaternion.identity);


        Vector2 PlayerPos = new Vector2(Stick.transform.position.x, Stick.transform.position.y);
        StartCoroutine(PlayerThrowTargetedZygote(PlayerPos,Projectile));
        return;

    }


    private IEnumerator PlayerThrowTargetedZygote(Vector2 Player,GameObject projectile)
    {
        float mindis = 0.2f;
        float timer = 0;
        float dis = 0;
        dis = Vector2.Distance(projectile.transform.position, Player);

        float speed = RangedProSpeed / 2;


        while (timer < 2 && mindis <= dis)
        {
            dis = Vector2.Distance(projectile.transform.position, Player);
            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, Player, speed * Time.deltaTime);
            yield return null;

        }

        yield break;
    }


    public float RangedProSpeed;
    private IEnumerator ThrowAttackPredicatable(Transform Point, Vector2 PlayerPos, GameObject Ranged)
    {
        float timer = 0;
        float MinDis = 1.4f;
        float MidPoint_X = (transform.position.x + Stick.transform.position.x) / 2;

        Vector2 Vertex = new Vector2(MidPoint_X, PlayerPos.y + 7);

        float dis = Vector2.Distance(Ranged.transform.position, Vertex);



        // math crap
        float a = 0;
        float h = 0;
        float k = 0;
        float x = 0;
        float y = 0;


        Debug.Log("Vertex Point: " + Vertex);


        while(dis > MinDis && timer < 1.5f)
        {

            timer += Time.deltaTime;
            dis = Vector2.Distance(Ranged.transform.position, Vertex);

            Debug.Log("Vertex Dis: " + dis);

            Ranged.transform.position = Vector2.MoveTowards(Ranged.transform.position, Vertex, RangedProSpeed * Time.deltaTime);


            ///dis = Vector2.Distance(Ranged.transform.position, Point.position);
            yield return null;

            /// y = a * (Mathf.Pow((x - h), 2)) + k;

        }

        float timer2 = 0;
        dis = Vector2.Distance(Ranged.transform.position, PlayerPos);
        MinDis = 0.5f;
        while (dis > MinDis && timer2 < 1.5f)
        {
            Debug.Log("EndPoint Dis: " + dis);

            timer2 += Time.deltaTime;

            if(PlayerPos.y > Vertex.y)
            {
                PlayerPos.y = Ranged.transform.position.y;
            }



            dis = Vector2.Distance(Ranged.transform.position, PlayerPos);


            Ranged.transform.position = Vector2.MoveTowards(Ranged.transform.position, PlayerPos, RangedProSpeed * Time.deltaTime);


            ///dis = Vector2.Distance(Ranged.transform.position, Point.position);

            /// y = a * (Mathf.Pow((x - h), 2)) + k;
            /// 
            yield return null;

        }
        Debug.Log("YOOOO it finished ");


        yield break;
    }


    public float LookAtSpeed;
    public Quadrant Mid;
    public Quadrant Left;
    public Quadrant Right;
    private float tar;
    public void LookAtPlayerAngleBased()
    {
        if (Mid.Here)
        {
            transform.localScale = Vector3.one;
            tar = 77.23f;
        }
        else if (Left.Here)
        {
            transform.localScale = Vector3.one;
            tar = 11.74f;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            tar = -22.027f;
        }

        float z = transform.eulerAngles.z;
        z = Mathf.MoveTowardsAngle(z, tar, LookAtSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, z);
    }


    private void ChangeRotation(float tar)
    {
        float z = transform.rotation.z;
        z = Mathf.MoveTowards(z, tar, LookAtSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    
    public void DamagePlayer()
    {
        if (Stick.Attack) return;
        if (DisEnemy <= MinDis)
        {




            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);


            AudioClip rnd = Stick.PunchHit[UnityEngine.Random.Range(0, Stick.PunchHit.Length)];
            AudioP.PlayOneShot(rnd);

            AudioP.PlayOneShot(ClapReverb);


          

            // Vector2 trans = new Vector2((EmbryAim.position.x + StickAim.transform.position.x / 2), (EmbryAim.position.y + StickAim.transform.position.y / 2));

            GameObject FX = Instantiate(RadFX.gameObject, Stick.transform.position, Quaternion.identity);
            Stick.Health -= 10;
            AnimShake(CamShake * 1.5f);


        }
    }
}
