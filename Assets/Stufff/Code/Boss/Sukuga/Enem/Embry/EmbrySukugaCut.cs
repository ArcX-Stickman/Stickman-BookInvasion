using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmbrySukugaCut : MonoBehaviour
{

    public StickCinematicVer_BossFight StickManager;
    public Animator EnemyAnim;
    public AudioClip PunchImpact;
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


    // Update is called once per frame
    void Update()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position,StickManager.gameObject.transform.position);


        RunImportant();   
    }


    public void RunImportant()
    {
        // Animations:
        AnimationManager();
    }

    private float LastSFX;
    private float SFXCD = 0.2f;
    [SerializeField] private bool Jump;
    public void AnimationManager()
    {
        if(HP > 0)
        {
            if (StickManager.Attack && DistanceFromPlayer <= MinDisPlayer)
            {

                if (StickManager.PunchA)
                {
                    EnemyAnim.CrossFade("Punch", 0, 0);

                }
                else if (StickManager.KickA)
                {
                    EnemyAnim.CrossFade("Kick", 0, 0);
                }


            }
        }
        else if(!Jump)
        {
            Jump = true;
            StartCoroutine(JumpBackNThrow());
            
        }
       

    }
    [SerializeField] private float JumpSpeed;
    private bool StartedJBNT;
    private IEnumerator JumpBackNThrow()
    {
        if (StartedJBNT) yield break;
        StartedJBNT = true;
        EnemyAnim.CrossFade("EmbryFarJump", 0, 0);

        float DisP1 = Vector3.Distance(transform.position, P1.position);
        float minDis = 2.5f;
        while(DisP1 > minDis)
        {
            DisP1 = Vector3.Distance(transform.position, P1.position);
            transform.position = Vector3.MoveTowards(transform.position, P1.position, JumpSpeed * Time.deltaTime);
            yield return null;
        }

        float DisP2 = Vector3.Distance(transform.position, P2.position);

        while (DisP2 > minDis)
        {
            DisP2 = Vector3.Distance(transform.position, P2.position);
            transform.position = Vector3.MoveTowards(transform.position, P2.position, JumpSpeed * Time.deltaTime);
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
                CSC.CameraShake(gameObject.GetComponent<CinemachineImpulseSource>(), 1);

                GameObject FX = Instantiate(ImpactFX_OBJ.gameObject,ImpactFX.transform.position,Quaternion.identity);

                HP -= 1;
            }




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
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, StickManager.transform.position, 70 * Time.deltaTime);
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


}
