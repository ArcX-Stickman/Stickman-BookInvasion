using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbrySuk2 : MonoBehaviour
{
    public StickCinematicVer_BossFight Stick;
    public float Health;

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
        DisEnemy = Vector2.Distance(transform.position, Stick.transform.position);
        VunerableManager();



    }

    private bool Leaped;
    private bool Jump;
    private bool ALrinCorNotVun;
    public IEnumerator NotVun()
    {
        if (Vunerable || ALrinCorNotVun) yield break;
        ALrinCorNotVun = true;
        Stagger = 4;

        Transform prev = null;
        if (!Leaped)
        {
            Leaped = true;

            EnemyAnim.SetBool("Leap", true);

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
        EnemyAnim.SetBool("Leap", false);
        MoveCorDone = false;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        GameObject MB = null;
        yield return new WaitForSecondsRealtime(0.3f);

        EnemyAnim.CrossFade("EmbryThrowSUK2", 0, 0);
        yield return new WaitForSecondsRealtime(1);


        if (!Jump)
        {
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

        yield return new WaitForSecondsRealtime(1.4f);


        EnemyAnim.SetTrigger("Throw");

        yield return new WaitForSecondsRealtime(1.4f);



        if (prev == PointR)
        {
            StartCoroutine(MoveToPOS(PointL, FallDown));

        }
        else
        {
            StartCoroutine(MoveToPOS(PointR, FallDown));

        }
        EnemyAnim.SetBool("Fall", true);


        ALrinCorNotVun = false;
        Vunerable = true;

        while (!MoveCorDone)
        {
            yield return null;
        }

        MoveCorDone = false;
        EnemyAnim.SetBool("Fall", false);


        if (prev == PointR)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;

        }


        yield return new WaitForSecondsRealtime(1.2f);

        StartCoroutine(MoveToPOS(Stick.transform, LeapSpeed));
        EnemyAnim.CrossFade("EmbryDash", 0, 0);

        while (!MoveCorDone)
        {
            yield return null;
        }

        EnemyAnim.CrossFade("EmbryIDLESUK2", 0, 0);



        Jump = false;
        Leaped = false;

        yield return new WaitForSecondsRealtime(0.1f);

        yield break;

    }


    public int Stagger;

    public void VunerableManager()
    {
        if (!Vunerable) return;

        if (Stick.Attack && DisEnemy <= MinDis && Time.time >= LastMelee + MeleeCD && OutOfATK)
        {
            LastMelee = Time.time;
            if (Stick.PunchA)
            {

                EnemyAnim.CrossFade("Punch", 0.001f, 0);

            }
            else if (Stick.KickA)
            {

                EnemyAnim.CrossFade("Kick", 0, 0);
            }
            Stagger -= 1;
            AudioP.pitch = UnityEngine.Random.Range(0.9f, 1.3f);


            AudioClip rnd = Stick.PunchHit[UnityEngine.Random.Range(0, Stick.PunchHit.Length)];
            AudioP.PlayOneShot(rnd);

            AudioP.PlayOneShot(ClapReverb);

        }

        if (Stagger <= 0)
        {
            Vunerable = false;
            StartCoroutine(NotVun());

        }
    }

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



    public void OutATK()
    {
        OutOfATK = true;
    }

    public void inAtk()
    {
        OutOfATK = false;
    }
}
