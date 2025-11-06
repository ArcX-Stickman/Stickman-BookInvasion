using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Exam : MonoBehaviour
{

    public Sniper_Code SniperManager;
    public AudioSource AudioP;
    public AudioClip Sound;
    public AudioClip ExamPunchSwoop;
    public AudioClip ExamPunch;
    public AudioClip Missed;
    // Heli Ladder Fight Stuff
    public Transform LadderPos;
    public Transform CineBD;
    public SpriteRenderer ExamTitle;
    public Animator ExamAnim;
    public float KickCD;
    public float LastKick;
    public Helicopter Helicopter;
    public float X_ValueLocal = 0;
    public bool isPunch;
    public LadderHeli LadderHeli;
    public float Health;
    public float HealthNeededForPhase2;
    public float HealthNeededForPhase3;
    public bool CinematicFightOver;
    public Health StickHP;
    public Movement StickManager;
    public float DMG_VALUE;
    public AudioClip ATK_ALERT;
    private bool NeedTOStartCorotineASAP = false;
    private bool WassupBoi;
    public GameObject PhysicsBook;
    public bool MustPara;
    public Transform EndPoint;
    public GameObject ExamParachute;
    public bool CaughtPara;

    void Start()
    {
        
    }

    void Update()
    {
        if (!CaughtPara)
        {
            ExamParachute.SetActive(false);

        }
        ExamParachute.transform.position = transform.position;
        AnimManager();
        if (Helicopter.StickManager.InHeliCineBeatdown || CinematicFightOver)
        {
            TransformUpdating();
            Combat();


        }
        if(Health <= HealthNeededForPhase2 && NeedTOStartCorotineASAP && !ReachedLadderDes_FinishCorotine)
        {
            StartCoroutine(FinalPhaseSetUp());
            NeedTOStartCorotineASAP = false;

        }
        if(Health <= HealthNeededForPhase2)
        {
            ExamAnim.SetTrigger("Transition");


        }
        if(PhysicsBook != null && Health <= HealthNeededForPhase3)
        {
            PhysicsBook.gameObject.SetActive(true);
        }
        else
        {
            PhysicsBook.gameObject.SetActive(false);

        }


        if (X_ValueLocal == 0)
        {
            X_ValueLocal = transform.localScale.x;
        }
    }
    public void NeedASAPANIM()
    {
        NeedTOStartCorotineASAP = true;
    }
    public void TransformUpdating()
    {
        if (CinematicFightOver)
        {
            
            transform.position = LadderPos.position;
            transform.rotation = LadderPos.rotation;
        }
        else if (Helicopter.StickManager.InHeliCineBeatdown && Health > HealthNeededForPhase2 && !CaughtPara)
        {
            transform.position = CineBD.position;

        }
        else if(Helicopter.StickManager.InHeliCineBeatdown && Health <= HealthNeededForPhase3 && CaughtPara)
        {
            ExamAnim.SetTrigger("Para");
            transform.position = Vector2.MoveTowards(transform.position, EndPoint.position, 15 * Time.deltaTime);
        }
        transform.localScale = new Vector3(X_ValueLocal * -1, transform.localScale.y, transform.localScale.z);
    }

    public void AnimationKey_ActivateFire()
    {
        Helicopter.Fire.gameObject.SetActive(true);
    }
    public void Combat()
    {
        if(Time.time >= KickCD + LastKick && Helicopter.StickManager.OnLadder || Time.time >= KickCD + LastKick && Helicopter.StickManager.InHeliCineBeatdown)
        {
            LastKick = Time.time;
            ExamAnim.SetTrigger("Kick");
        }

    }
    public void AnimManager()
    {
        if (Helicopter.Fire.activeSelf)
        {
            ExamAnim.SetBool("Scared", true);

        }
        if (Helicopter.StickManager.OnLadder && !CinematicFightOver)
        {
            ExamAnim.SetBool("Scared", true);
        }
        if (Helicopter.StickManager.InHeliCineBeatdown && !CinematicFightOver)
        {
            ExamAnim.SetBool("BD", true);
        }
        //if (Helicopter.Fire.activeSelf)
        //{
        //    ExamAnim.SetBool("Climb", true);
        //}
        //if (Helicopter.StickManager.OnLadder)
        //{
        //    ExamAnim.SetBool("Ladder", true);
        //}
    }

    private bool ReachedLadderDes_FinishCorotine;
    private bool CalledStickSetUpFinaleLadder;
    private bool FinishedSetupGud2Fight;
    public IEnumerator FinalPhaseSetUp()
    {
        StickManager.NeverDoCineBeatDownAgain = true;
        NeedTOStartCorotineASAP = false;
        StickManager.rb.bodyType = RigidbodyType2D.Dynamic;
        StickManager.InHeliCineBeatdown = false;
        StickManager.HeliManager();
        StickManager.OnLadder = true;
        StickManager.isFallingHeli = false;
        StickManager.InHeliCineBeatdown = false;
        StickManager.transform.position = Vector2.MoveTowards(StickManager.transform.position, StickManager.UpLadder.position, 40 * Time.deltaTime);
        float Distance = Mathf.Abs(transform.position.y - LadderPos.position.y);
        while (Distance >= 12f)
        {
            StickManager.OnLadder = true;
            StickManager.isFallingHeli = false;
            StickManager.InHeliCineBeatdown = false;
            StickManager.HeliManager();
            Debug.Log("Exam DIS " + Distance);
            StickManager.transform.position = Vector2.MoveTowards(StickManager.transform.position, StickManager.UpLadder.position, 40 * Time.deltaTime);
            Distance = Mathf.Abs(transform.position.y - LadderPos.position.y);
            transform.position = Vector2.MoveTowards(transform.position, LadderPos.position, 17 * Time.deltaTime);
            yield return null;
        }

        ExamAnim.SetBool("Ladder",true);
        ReachedLadderDes_FinishCorotine = true;
        CinematicFightOver = true;
        Debug.Log("Ended FInal Phase SetUp");
        yield break;
    }

    public void Destory()
    {
        ExamParachute.SetActive(true);
        Destroy(gameObject);
    }

    public void SFX()
    {
        AudioP.PlayOneShot(Sound);
    }

    public void SnipeCall()
    {
        SniperManager.ShootABullet();
    }

    public void PunchBoolTrue()
    {
        isPunch = true;
    }

    public void PunchBoolFalse()
    {
        isPunch = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("StickMelee") && StickManager.UpLadderWant || collision.CompareTag("StickMelee") && StickManager.InHeliCineBeatdown)
        {
            StickManager.CameraScript.StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(35.5f, 0.2f));
            LastKick += 0.2f;
            StickHP.PlayerHP += 1.5f;
            HitMarker hm = GetComponent<HitMarker>();
            hm.HitMarkerSFX();
            ExamAnim.SetTrigger("Hurt");
            Health -= 1;
            Helicopter.StickManager.CameraScript.StartCoroutine(Helicopter.StickManager.CameraScript.ShakeCam(2.5f, 0.15f));

        }
    }
    public void PunchSwoopSound()
    {
        AudioP.PlayOneShot(ExamPunchSwoop);
    }

    public void Alert()
    {
        AudioP.PlayOneShot(ATK_ALERT);
    }
    public void PunchAnim()
    {
        if (StickManager.InHeliCineBeatdown)
        {
            if (StickManager.AuraSigmaRizzyDodge)
            {
                AudioP.PlayOneShot(Missed);
                StickManager.TimeManager.FreezeTime(1f);

            }
            else
            {
                StickManager.StickAnim.SetTrigger("Hurt");
                AudioP.PlayOneShot(ExamPunch);
                StickHP.PlayerHP -= DMG_VALUE;
                StickManager.CameraScript.StartCoroutine(StickManager.CameraScript.ShakeCamGroundFeeling(80.5f, 0.5f));
                //Helicopter.StickManager.CameraScript.StartCoroutine(Helicopter.StickManager.CameraScript.ShakeCam(2.5f, 0.15f));
            }
        }
       
    }
}
