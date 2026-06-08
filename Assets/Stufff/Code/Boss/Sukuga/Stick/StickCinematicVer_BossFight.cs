using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class StickCinematicVer_BossFight : MonoBehaviour
{

    public Rigidbody2D rb;
    public float Speed;
    public float JumpForce;
    public bool Grounded;
    public LayerMask WhatsGround;
    public Animator StickAnim;
    
    public GameObject Enemy;
    public float MinDisEnemy;
    public float DisEnemy;

    public float MeleeCD;
    public float LastMelee;
    public AudioClip[] PunchWhooshes;
    public AudioClip[] PunchMiss;
    public AudioClip[] PunchHit;
    private bool inPunchVoidAnim;
    public LayerMask WhatsTheBookLayer;
    public float RayDistance;
    public AudioSource PunchAudioP;
    public bool Attack;
    



    public bool PunchA;
    public bool KickA;


    void Start()
    {
        
    }

    void Update()
    {
        RunImportantCrap();
        InputManager();
    }


    public void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckIfGrounded())
        {
            Jump();
        }

        if (Input.GetButton("Fire2") && Time.time >= MeleeCD +LastMelee)
        {
            LastMelee = Time.time;
            Punch();
        }
    }



    private bool CheckIfGrounded()
    {
        float extraHeight = 1.3f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight, WhatsGround);
        Debug.DrawRay(transform.position, Vector2.down * (GetComponent<BoxCollider2D>().bounds.extents.y + extraHeight), Color.red);
        return hit.collider != null;
    }

    public void RunImportantCrap()
    {
        Move();
        CheckIfGrounded();
    }


    public int punchIndex;
    public void Punch()
    {
        // put stuff for prediticing enemy

        if (Grounded)
        {
            PunchAnim();
        }
     
       
    }

    public void Move()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        StickAnim.SetBool("Move", horizontalInput != 0);
        rb.velocity = new Vector2(horizontalInput * Speed, rb.velocity.y);

    }

    public void Jump()
    {
        float horizontalMomentum = rb.velocity.x;
        rb.velocity = new Vector2(horizontalMomentum, JumpForce);


    }


    public void PunchAnim()
    {
        if (inPunchVoidAnim) return;

        bool KickAnim = (StickAnim.GetCurrentAnimatorStateInfo(0).IsName("Kick"));
        bool DontMakeMoreIndexxGrr = false;

        inPunchVoidAnim = true;


        if (EnemyClose())
        {
            PunchWhoosh_NonMiss_SFX();
        }
        else
        {
            PunchMiss_SFX();
        }


        if (Grounded && !KickAnim)
        {
            switch (punchIndex)
            {
                case 0:
                    StickAnim.CrossFade("Punch", 0, 0);
                    PunchA = true;
                    break;

                case 1:
                    StickAnim.CrossFade("Punch2", 0, 0);
                    PunchA = true;
                    break;

                case 2:
                    StickAnim.CrossFade("Kick", 0.2f, 0);
                    KickA = true;
                    break;
                default:
                    StickAnim.CrossFade("Punch", 0, 0);
                    PunchA = true;
                    break;
            }
        }


        if (!DontMakeMoreIndexxGrr)
        {
            DontMakeMoreIndexxGrr = true;

            punchIndex += 1;

        }

        if (punchIndex > 2)
            punchIndex = -1;
        inPunchVoidAnim = false;
        return;
    }


    private void PunchMiss_SFX()
    {
        AudioClip rnd = PunchWhooshes[UnityEngine.Random.Range(0, PunchMiss.Length)];
        float pitch = UnityEngine.Random.Range(1, 1.3f);
        PunchAudioP.pitch = pitch;
        StartCoroutine(PunchAnimWhoosh(rnd));


    }

    private void PunchWhoosh_NonMiss_SFX()
    {
        AudioClip rnd = PunchWhooshes[UnityEngine.Random.Range(0, PunchWhooshes.Length)];
        float pitch = UnityEngine.Random.Range(1, 1.3f);
        PunchAudioP.pitch = pitch;
        StartCoroutine(PunchAnimWhoosh(rnd));

    }

    public IEnumerator PunchAnimWhoosh(AudioClip rnd)
    {
        while (!PunchAnimKeyFrame)
        {
            yield return null;  
        }

        PunchAudioP.PlayOneShot(rnd);
        PunchAnimKeyFrame = false;
        yield break;

    }
    private bool PunchAnimKeyFrame;

    public void Animationkey_PunchSound()
    {
        PunchAnimKeyFrame = true;
    }


    private bool EnemyClose()  // since im kinda buns at this coding thing --- NOTE TO FUTURE SELF:  When adding min dis in Unity editor make sure it matches the enmey min dis for Punches or anything
    {
        DisEnemy = Vector2.Distance(transform.position, Enemy.gameObject.transform.position);

        if(DisEnemy <= MinDisEnemy)
        {
            return true;
        }

        return false;
    }

    public void AttackBool_ANIMKey(int T)
    {
        bool temp = false;
        if(T > 0)
        {
            temp = true;
        }
        Attack = temp;
        if (!temp)
        {
            PunchA = false;
            KickA = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }
}
