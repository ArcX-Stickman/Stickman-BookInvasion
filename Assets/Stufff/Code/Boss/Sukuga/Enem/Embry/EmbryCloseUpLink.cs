using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EmbryCloseUpLink : MonoBehaviour
{

    public StickCloseUpSuk StickCloseUpSuk;
    public AudioSource AudioP;
    public AudioClip SFX;
    public EmbrySukugaCut EmbrySukugaCut;
    public int HP;
    [SerializeField]private float IFram = 0.08f;
    private float IFramLast;
    public float CamShake;
    public CameraSukugaCuts CameraSukugaCuts;
    public float Hitstop;
    public GameObject PlayerDamageFX;
    public GameObject EnemyDamageFX;
    public Transform EnemyDMG_FX_POS;
    public Transform PLayerDMG_FX_;
    public AudioClip Miss;

    public float Zoom;
    private float OG_CAMSIZE;

    public Transform Targ;


    public Animator Anim;

    // AI
    public bool FirstPhaseAI;
    public int Punches;


    //Movement stuff
    public float TimePlayerFarEnemy;
    public float MinDisMove;
    public float DisMove;
    public float MoveSpeed;
    public AudioSource FootSteps;

    // Start is called before the first frame update
    void Start()
    {
        HP = EmbrySukugaCut.HP;
        OG_CAMSIZE = CameraSukugaCuts.virtualCamera.m_Lens.FieldOfView;
    }

    private void OnEnable()
    {
        HP = EmbrySukugaCut.HP;
        OG_CAMSIZE = CameraSukugaCuts.virtualCamera.m_Lens.FieldOfView;
    }


    // Update is called once per frame
    void Update()
    {
        EmbrySukugaCut.HP = HP;

        if(StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy && StickCloseUpSuk.Attack)
        {
            PunchSFX();
        }

        if(FirstPhaseAI)
        {
            FirstPhase();

            if(StickCloseUpSuk.isMovingSum)
            {

                StartCoroutine(CameraSukugaCuts.SwitchTargets(transform));
            }
        }

       

        DisMove = Vector2.Distance(transform.position, StickCloseUpSuk.transform.position);

        if(DisMove > MinDisMove)
        {
            TimePlayerFarEnemy += Time.deltaTime;
        }
        else
        {
            TimePlayerFarEnemy = 0;
        }



        if(TimePlayerFarEnemy >= MaxTimeAFK)
        {
            MoveTowardsStick();
        }
        else
        {
            Anim.SetBool("Move", false);

        }

    }
    public float MaxTimeAFK;
    public void MoveTowardsStick()
    {
        if(DisMove > MinDisMove)
        {
            Vector3 FakeStick = new Vector3(StickCloseUpSuk.transform.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, FakeStick, MoveSpeed * Time.deltaTime);
            Anim.SetBool("Move", true);
        }
        else
        {
            Anim.SetBool("Move", false);

        }

    }

    public float QuickDrawCD;
    private float LastQuickDraw;

    public float PuntCD;
    private float LastPunt;
    public void FirstPhase()
    {
        if(QuickDrawCD+ LastQuickDraw <= Time.time)
        {
            Anim.ResetTrigger("Hurt");

            Anim.SetTrigger("QuickDraw");
            LastQuickDraw = Time.time;
        }
        if(Punches >= 3 && StickCloseUpSuk.Attack && StickCloseUpSuk.MinDisEnemy >= StickCloseUpSuk.DisEnemy)
        {
            StartCoroutine(PuntCor());
            LastPunt = Time.time;
        }
    }
    private bool PuntCorRunning;
    private IEnumerator PuntCor()
    {
        // ts line ->             LastQuickDraw = Time.time;     i added it to not interfere with punt i dont want this as combo attack


        LastQuickDraw = Time.time;

        if (PuntCorRunning)
        {
            yield break;
        }
        PuntCorRunning =true;
        while (StickCloseUpSuk.Attack)
        {
            yield return null;
        }
        LastQuickDraw = Time.time;
        Anim.ResetTrigger("Hurt");

        Anim.SetTrigger("Punt");
        LastPunt = Time.time;
        PuntCorRunning = false;
        yield break;

    }


    // public Transform HandPosStick;
    public float VelocityShakeCam;
    public void PunchSFX()
    {
        if (!StickCloseUpSuk.IFrame && StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy && Time.time >= IFram + IFramLast)
        {

            IFramLast = Time.time;
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);

            Punches += 1;
            AudioClip rnd = StickCloseUpSuk.PunchHit[UnityEngine.Random.Range(0, StickCloseUpSuk.PunchHit.Length)];
            AudioP.PlayOneShot(rnd);



            if (StickCloseUpSuk.Attack)
            {
                HP -= 1;
                Anim.SetTrigger("Hurt");
                GameObject FX = Instantiate(EnemyDamageFX, EnemyDMG_FX_POS.position, Quaternion.identity);


                StartCoroutine(CameraSukugaCuts.Zoom(45, true, GapForCamZoom * 1.2f, GetComponent<CinemachineImpulseSource>(), CamShake));

                
                ShakeCam(CamShake);

            }

            // Vector2 trans = new Vector2((EmbryAim.position.x + StickAim.transform.position.x / 2), (EmbryAim.position.y + StickAim.transform.position.y / 2));

            // GameObject FX = Instantiate(RadFX.gameObject, EmbryAim.position, Quaternion.identity);

        }

    }

    public AnimationClip[] AnimCLipsHurtStick;

    public void StickHurtAnimPLay(int i)
    {
        if(StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy && !StickCloseUpSuk.InBackflip && StickCloseUpSuk.Grounded)
        {
            GameObject FX = Instantiate(PlayerDamageFX, PLayerDMG_FX_.transform.position, Quaternion.identity);

            Debug.Log("Playing" + AnimCLipsHurtStick[i].name);
            StickCloseUpSuk.StickAnim.CrossFade(AnimCLipsHurtStick[i].name, 0, 0);

        }

        return;
    }

    
    public float GapForCamZoom;
    public void Attack(float DMG)
    {
        if((StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy && !StickCloseUpSuk.InBackflip  && StickCloseUpSuk.Grounded))
        {
            AudioP.PlayOneShot(SFX);
            StickCloseUpSuk.Health -= DMG;
            ShakeCam(CamShake * 1.4f, true);
            CameraSukugaCuts cameraSukugaCuts = CameraSukugaCuts;
            if (cameraSukugaCuts != null)
            {
               /// cameraSukugaCuts.ZoomAnim(Zoom);
                StartCoroutine(CameraSukugaCuts.Zoom(Zoom,true, GapForCamZoom, StickCloseUpSuk.transform, true));

            }

        }
        else if(StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy + 6 && !StickCloseUpSuk.InBackflip && !StickCloseUpSuk.Grounded && StickCloseUpSuk.StickAnim.GetBool("Jump"))
        {
            // if player is jumping make it cool 
            AudioP.PlayOneShot(Miss);
            ShakeCam(CamShake * 2.3f, true);

            CameraSukugaCuts cameraSukugaCuts = CameraSukugaCuts;
            if (cameraSukugaCuts != null)
            {
                /// cameraSukugaCuts.ZoomAnim(Zoom);
                StartCoroutine(CameraSukugaCuts.Zoom(Zoom, true, GapForCamZoom, StickCloseUpSuk.transform, true));

            }

        }
    }

    public void Attack_HURT(string name)
    {

        // make sure its liteeraly same frame as dmg frame
        if ((StickCloseUpSuk.DisEnemy <= StickCloseUpSuk.MinDisEnemy && !StickCloseUpSuk.InBackflip))
        {
            StickCloseUpSuk.StickAnim.CrossFade(name, 0, 0);

        }
    }

    public void AttackWhoosh()
    {
        AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);


        AudioClip rnd = StickCloseUpSuk.PunchWhooshes[UnityEngine.Random.Range(0, StickCloseUpSuk.PunchWhooshes.Length)];
        AudioP.PlayOneShot(rnd);
    }

    public void ShakeCam()
    {
       // CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(CamShake);
        //CameraSukugaCuts CSC = FindAnyObjectByType<CameraSukugaCuts>();
       // CameraSukugaCuts.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), CamShake);
    }
    public void ShakeCam(float s)
    {
        // CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
       
        GetComponent<CinemachineImpulseSource>().GenerateImpulse(s);
    }


    public void ShakeCam(float s, bool hitsto)
    {
        // CinemachineImpulseManager.Instance.IgnoreTimeScale = true;
        if (Hitstop > 0)
        {
            TimeScale_MOD Mod = FindAnyObjectByType<TimeScale_MOD>();

            if (Mod != null)
            {
                Mod.Stop(Hitstop, 0.1f);
            }
        }

        GetComponent<CinemachineImpulseSource>().GenerateImpulse(s);
    }


  


    public CameraSukugaCuts Cam;
    public bool Flip;
    public void DiretionCamShake(float HoriShake)
    {
        CameraSukugaCuts CS = Cam;
        StartCoroutine(CS.ShakeCamDirection(HoriShake, 0.2f, transform, Flip));
    }
}
