using Cinemachine;
using System.Collections;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class EmbrySuk3 : MonoBehaviour
{
    [SerializeField] private float VunerableWindow = 2f;
    [SerializeField] private int DefaultStagger = 4;
    public StickCinematicVer_BossFight Stick;
    public int Health;
    public bool Vunerable;

    public float LastMelee;
    public float MeleeCD;
    public bool OutOfATK;
    public float DisEnemy;
    public float MinDis;
    public int Stagger;

    public Animator EnemyAnim;
    public AudioSource AudioP;
    public AudioClip StaggerSFX;
    public float CamShake;
    public TimeScale_MOD TSM;
    public float DelayMin;

    private float OpenedTime;
    public float StuckTime;

    public AudioClip ClashSFX;
    public AudioClip SlashSFX;
    public AudioClip SwordHit;

    public Transform PointR;
    public Transform PointL;
    public Transform PointUP;


    public AudioClip WhooshFall;
    public AudioClip JumpSFX;
    public AudioClip DashSFX;
    public AudioClip FallingSFX;
    public AudioClip LandSFX;
    public AudioClip ShockwaveHit;
    public AudioClip RangedATK_SFX;

    public GameObject LandFX;
    public Transform FXLAND_POS;

    public GameObject Shockwave;
    public Transform ShockwavePOS_R;
    public Transform ShockwavePOS_L;

    public Transform AimPoint_Ranged_MELEE;
    public GameObject RangedMelee;

    public GameObject SparkFX;
    public GameObject LandEffect;
    public Transform LandFX_POS;


    private void Start()
    {
        OpenedTime = Time.time;
        OutOfATK = true;
        Vunerable = true;
        ResetStagger();
    }

   
    private void Update()
    {

      
        if (Health <= 0 || Stick == null)
        {
            return;
        }

        DisEnemy = Vector2.Distance(transform.position, Stick.transform.position);
        VunerableManager();

        if(Vunerable == false)
        {
            ComboManager();
        }
    }



    private bool HasJumped;
    private bool Jumping;
    private bool InAir;
    public void ComboManager()
    {
        if (!HasJumped && !Jumping)
        {
            Jumping = true;
            StartCoroutine(Jump());
        }
        if(HasJumped && !InAir && !Leaping)
        {
            Leaping = true;
            StartCoroutine(LeapAir());
        }
        if(HasJumped && InAir && !RangedSlashin && !RangedSlash)
        {
            StartCoroutine(RangedSlashAttack());
            RangedSlashin = true;
        }
    }

    public float JumpSpeed;
    public float FallSpeed;
    private bool Leaping;
    private bool RangedSlash;
    private bool RangedSlashin;
    public GameObject MotionLinesR;
    public GameObject MotionLinesL;
    public int Stab;
    private IEnumerator LeapAir()
    {

        Leaping = true;
        GameObject lines= null;
        if(CurrentPos == PointL)
        {
            lines = MotionLinesL;
            MotionLinesL.gameObject.SetActive(true);
        }
        else
        {
            lines = MotionLinesR;
            MotionLinesR.gameObject.SetActive(true);
        }
        SpriteRenderer SR = GetComponentInChildren<SpriteRenderer>();
        SR.enabled = false;
        transform.position = new Vector2(transform.position.x, PointUP.position.y);
        Vector2 StickPosPast = new Vector2(Stick.transform.position.x, -16.5f);

        AudioP.PlayOneShot(DashSFX);

        while (lines.activeSelf)
        {
            yield return null;
        }

        EnemyAnim.CrossFade("EmbrySUK3_AIR_IDLE", 0, 0);
        SR.enabled = true;



        yield return new WaitForSecondsRealtime(DelayMin + 0.7f);
        EnemyAnim.CrossFade("EmbrySUK3AirDash", 0, 0);
        AudioP.PlayOneShot(FallingSFX);

        float minDis = 3f;

        float dis = Vector2.Distance(transform.position,StickPosPast);

        while(dis > minDis)
        {
            dis = Vector2.Distance(transform.position, StickPosPast);
            transform.position = Vector2.MoveTowards(transform.position, StickPosPast, FallSpeed * Time.deltaTime);
            yield return null;

        }
        AnimShake(CamShake * 2.5f);
        AudioP.PlayOneShot(LandSFX);
        GameObject LandFXt = Instantiate(LandFX, LandFX_POS.transform.position, Quaternion.identity);
        // yooo he's stuuuuckkkk
        EnemyAnim.CrossFade("EmbrySUK3_STUCK", 0, 0);
        yield return new WaitForSecondsRealtime(StuckTime);
        

        EnemyAnim.CrossFade("EmbrySUK3_OUT", 1, 0);


        yield return new WaitForSecondsRealtime(2.3f); // this is a gucci buffer time and gives a break and lets the plaeyr sees whats next
        Debug.Log("LeapEnded");


        Leaping = false;
        InAir = true;

        yield break;
    }


    private IEnumerator RangedSlashAttack()
    {
        Debug.Log("Starting RangedSlashAttack");
        RangedSlashin = true;
        EnemyAnim.CrossFade("EmbrySUK3_JUMP", 0f, 0);
    
        Transform TCurrentPos = null;
        if (transform.localScale.x > 0)
        {
            // facing left
            TCurrentPos = PointR;

        }
        else
        {
            TCurrentPos = PointL;
        }

        float dis = Vector2.Distance(transform.position, TCurrentPos.position);
        float minDis = 2;
        while (dis > minDis)
        {
            dis = Vector2.Distance(transform.position, TCurrentPos.position);
            transform.position = Vector2.MoveTowards(transform.position, TCurrentPos.position, (JumpSpeed *1.2f) * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.1f);

        EnemyAnim.CrossFade("Embry_SUK3_RANGED_SLASH", 0, 0);

        while (RangedSlashes < MaxRangedSlashes)
        {
            yield return null;
        }
        EnemyAnim.SetBool("RUN", true);

        RangedSlashes = 0;
        // bruh is chasing the player like a maniac oh nooo

        float DisP = Vector2.Distance(transform.position, Stick.transform.position);


        Stab = 0;

        while(Stab < 2)
        {
            FacePlayer();

            EnemyAnim.SetBool("RUN", true);
            // face left
            if(transform.localScale.x > 0)
            {
                Transform pos = PointL;
                float disPo = Vector2.Distance(transform.position,pos.position);
                while(disPo > 3)
                {
                    if(Stab >= 2)
                    {
                        break;
                    }
                    disPo = Vector2.Distance(transform.position, pos.position);
                   

                    if(DisEnemy <= MinDis)
                    {
                        EnemyAnim.CrossFade("EmbrySwordCombo3", 0, 0);
                        
                        FacePlayer();
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, pos.position, Speed * Time.deltaTime);
                    }

                    yield return null;
                }
                Stab += 1;

                FacePlayer();
            }
            else
            {
                Transform pos = PointR;
                float disPo = Vector2.Distance(transform.position, pos.position);
                while (disPo > 3)
                {
                    if (Stab >= 2)
                    {
                         break;
                    }
                    disPo = Vector2.Distance(transform.position, pos.position);

                    if (DisEnemy <= MinDis)
                    {
                        EnemyAnim.CrossFade("EmbrySwordCombo3", 0, 0);
                        FacePlayer();
                      
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, pos.position, Speed * Time.deltaTime);

                    }

                    yield return null;
                }
                Stab += 1;
                FacePlayer();
            }

            yield return null;
        }
        Vector2 Ptemp_TEMP = new Vector2(Stick.transform.position.x, transform.position.y);

        float DisE_2 = Vector2.Distance(transform.position, Ptemp_TEMP);
        while(DisE_2 > MinDis)
        {
            Vector2 Ptemp = new Vector2(Stick.transform.position.x,transform.position.y);

            DisE_2 = Vector2.Distance(transform.position, Stick.transform.position);
            FacePlayer();
            transform.position = Vector2.MoveTowards(transform.position, Ptemp, Speed * Time.deltaTime);
            yield return null;

        }

        Stagger = 4;
        OpenedTime = Time.time;
        EnemyAnim.SetBool("RUN", false);

        Vunerable = true;
        InAir = false;
        HasJumped = false;
        RangedSlash = false;
        RangedSlashin = false;
       
        if(VunerableWindow > 0.3f)
        {
            VunerableWindow -= 0.2f;

        }
        else
        {
            VunerableWindow = 0.2f;
        }
        if (DelayMin > 0.1f)
        {
            DelayMin -= 0.5f;
        }
        else
        {
            DelayMin = 0.1f;
        }
        if(StuckTime > 0.3f)
        {
            StuckTime -= 0.3f;

        }
        yield return new WaitForSecondsRealtime(0.3f);

        yield break;

    }


    public float Speed;

    [SerializeField] private int RangedSlashes;
    [SerializeField] private int MaxRangedSlashes;

    public void IncrementRangedSlash()
    {
        RangedSlashes += 1;
    }

    public Transform CurrentPos;
    private IEnumerator Jump()
    {
        yield return new WaitForSecondsRealtime(1.2f);
        AudioP.PlayOneShot(JumpSFX);

        EnemyAnim.CrossFade("EmbrySUK3_JUMP", 0f, 0);

        Jumping = true;
        CurrentPos = null;
        if (transform.localScale.x > 0)
        {
            // facing left
            CurrentPos = PointR;

        }
        else
        {
            CurrentPos = PointL;
        }

        float dis = Vector2.Distance(transform.position, CurrentPos.position);
        float minDis = 2;
        while(dis > minDis)
        {
            dis = Vector2.Distance(transform.position, CurrentPos.position);
            transform.position = Vector2.MoveTowards(transform.position, CurrentPos.position, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        EnemyAnim.CrossFade("IDLE", 0, 0);
        yield return new WaitForSecondsRealtime(DelayMin);
        

        HasJumped = true;
        Jumping = false;
        yield break;
    }


    public void OpenVunerableWindow()
    {
        OpenedTime = Time.time;
        Vunerable = true;
        ResetStagger();
    }

    public void CloseVunerableWindow()
    {
        Vunerable = false;
    }


    private int PunchIndex = 0;
    public void VunerableManager()
    {
        if (!Vunerable || Stick == null)
        {
            return;
        }

        FacePlayer();

        float timeBeen = Time.time - OpenedTime;
        if (timeBeen >= VunerableWindow)
        {
            CloseVunerableWindow();
            return;
        }

        if (DisEnemy <= MinDis && Time.time >= LastMelee + MeleeCD && OutOfATK)
        {
            OpenedTime += 0.15f;
            LastMelee = Time.time;
            PunchIndex += 1;
            switch (PunchIndex)
            {
                case 0:
                    EnemyAnim.CrossFade("EmbrySwordCombo", 0.1f, 0);
                    break;
                case 1:
                    EnemyAnim.CrossFade("EmbrySwordCombo2", 0.1f, 0);
                    break;
                case 2:
                    EnemyAnim.CrossFade("EmbrySwordCombo3", 0.1f, 0);
                    break;
                default:
                    EnemyAnim.CrossFade("EmbrySwordCombo", 0.1f, 0);
                    break;
            }

            if (Stagger <= 1 && TSM != null)
            {
                TSM.SloMO(0.2f, 0.8f);
            }


            if(PunchIndex >= 3)
            {
                PunchIndex = 0;
            }
        }

        if (Stagger <= 0)
        {
            AnimShake(CamShake * 3);

            if (TSM != null)
            {
                TSM.Stop(0.1f);
            }

            if (AudioP != null && StaggerSFX != null)
            {
                AudioP.PlayOneShot(StaggerSFX);
            }

            Health -= 1;
            CloseVunerableWindow();
            ResetStagger();
        }
    }


    public Transform EmbryAimFX;
    public Transform StickAimFX;
    public void ClashManager()
    {



        if (EnemyAnim.GetBool("RUN"))
        {
            Stab += 1;
        }

        if (Stick.Attack && DisEnemy <= MinDis) 
        {
            float x = (EmbryAimFX.position.x + StickAimFX.position.x) / 2;

            Vector2 Mid = new Vector2(x, EmbryAimFX.position.y);

            GameObject SparksT = Instantiate(SparkFX, Mid, Quaternion.identity);

            AudioP.PlayOneShot(ClashSFX);
            AnimShake(CamShake);
            Stagger -= 1;
        }
        else if(DisEnemy <= MinDis && !Stick.Attack)
        {
            GameObject SparksT = Instantiate(SparkFX, Stick.transform.position, Quaternion.identity);
            AudioP.PlayOneShot(SwordHit);
            AnimShake(CamShake);
            Stick.Health -= 25;
        }

    }

    private void FacePlayer()
    {
        float playerX = Stick.transform.position.x;

        if (playerX < transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else if (playerX > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void ResetStagger()
    {
        Stagger = Mathf.Max(1, DefaultStagger);
    }

    public void AnimShake(float sha)
    {
        CameraSukugaCuts cameraCuts = FindAnyObjectByType<CameraSukugaCuts>();
        CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();

        if (cameraCuts == null || impulseSource == null)
        {
            return;
        }

        cameraCuts.CameraShake(impulseSource, sha);
    }


    public void ShockWaveSpawn()
    {
        AnimShake(CamShake);
        GameObject ShockR = Instantiate(Shockwave, ShockwavePOS_R.transform.position, Quaternion.identity);
        GameObject ShockL = Instantiate(Shockwave, ShockwavePOS_L.transform.position, Quaternion.identity);

        ShockwaveDirecCtrl Sr = ShockR.GetComponent<ShockwaveDirecCtrl>();
        ShockwaveDirecCtrl Sl = ShockL.GetComponent<ShockwaveDirecCtrl>();

        Sr.Targ = PointR;
        Sl.Targ = PointL;


    }

    public AudioClip Pefection;
    public void PerfectParry()
    {
        if(Stick.Attack && DisEnemy <= MinDis && Stick.LastMelee == Time.time - 0.3f)
        {
            Debug.Log("Perfect Parry");
            TSM.SloMO(0.2f, 0.5f);
            AudioP.PlayOneShot(Pefection);

        }
            
    }
    public void SpawnRangedSlash()
    {
        GameObject Slash = Instantiate(RangedMelee, AimPoint_Ranged_MELEE.position, Quaternion.identity);
        ShockwaveDirecCtrl Shock = Slash.GetComponent<ShockwaveDirecCtrl>();

        Transform pos = null;
        if(transform.localScale.x > 0)
        {
            Slash.transform.localScale = new Vector3 (-Slash.transform.localScale.x, Slash.transform.localScale.y, Slash.transform.localScale.z);
            pos = PointL;
        }
        else
        {
            pos = PointR;
        }
        Shock.IgnoreY = true;
        Shock.Targ = pos;
    }


    public void OutOFATK()
    {
        OutOfATK = true;
    }

    public void InAttack()
    {
        OutOfATK = false;
    }
}
