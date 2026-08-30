using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStickSukuga : MonoBehaviour
{

    public Transform FinalDes;
    public float MinDis;
    public StickCinematicVer_BossFight StickSUK;
    public float speed;
    public bool MoveWithRB;

    private Rigidbody rb3D;
    public bool MustJump;
    public bool MustJumpInTime;
    public Animator Animator;
    public string Animation;
    public AudioClip SFX;
    public AudioSource AudioP;
    private bool AlrPlayedSFX;
    private bool PlayedAnimation;

    public float JumpTime;
    public float Boost = 0.2f;

    public bool VolumeBased;

    private void Start()
    {
        rb3D = StickSUK.GetComponent<Rigidbody>();
    }
    [SerializeField]private BoxCollider2D ThisC;
    [SerializeField] private BoxCollider2D StickC;
    // why i did serialze field and private you might ask well I DONT KNOW YOHOHOHOHOHOHOHOOOOOOOOO
    private void Update()
    {
        if (!VolumeBased)
            return;
        
        bool IsJumpingNow = StickSUK.Jumping;

        if (ThisC.IsTouching(StickC))
        {
            Debug.Log("TOUCHING VOLUME");

            if (MustJump)
            {
                if (MustJumpInTime)
                {
                    if (Time.time >= StickSUK.LastJump + Boost - JumpTime)
                    {
                        if (StickSUK.Jumping)
                        {
                            VolumeCrapBased();
                        }
                    }
                }
                else
                {
                    if (StickSUK.Jumping)
                    {
                        VolumeCrapBased();
                    }
                }
            }
            else
            {
                VolumeCrapBased();
            }
        }
    }

    private bool VBCOMPLE;
    public void VolumeCrapBased()
    {
        if (VBCOMPLE) return;
        VBCOMPLE = true;
        if (Animator != null && !PlayedAnimation)
        {
            Animator.CrossFade(Animation, 0, 0);
            PlayedAnimation = true;
        }
        if (SFX != null && AudioP != null && !AlrPlayedSFX)
        {
            AudioP.PlayOneShot(SFX);
            AlrPlayedSFX = true;
        }

        VolumeBased = false;
    }

    private void FixedUpdate()
    {
        if (VolumeBased) return;
        Vector2 V2 = new Vector2(FinalDes.position.x, FinalDes.position.y);

        if (Distance() > MinDis)
        {
            // Disable physics while cinematic movement happens
            if (StickSUK.rb != null)
            {
                StickSUK.rb.simulated = false;
            }

            if (rb3D != null)
            {
                rb3D.isKinematic = true;
            }


            StickSUK.StickAnim.SetBool("Jump", true);

            if (!MoveWithRB)
            {
                StickSUK.transform.position = Vector3.MoveTowards(
               StickSUK.transform.position,
               FinalDes.position,
               speed * Time.deltaTime
           );
            }
            else
            {

                Vector3 next = Vector3.MoveTowards(
                 StickSUK.transform.position,
                 FinalDes.position,
                 speed * Time.deltaTime);
                StickSUK.rb.MovePosition(next);

            }



            StickSUK.enabled = false;
        }
        else
        {
            // Re-enable physics
            if (StickSUK.rb != null)
            {
                StickSUK.rb.simulated = true;
            }

            if (rb3D != null)
            {
                rb3D.isKinematic = false;
            }


            StickSUK.enabled = true;
            this.enabled = false;
        }
    }

    // it don work so i got rid of it
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
       
    //    if ((collision.CompareTag("Player")))
    //    {
    //        if (VolumeBased)
    //        {
    //            Debug.Log("Ontrigger Collsion");
    //            if (MustJump && StickSUK.Jumping)
    //            {
    //                VolumeCrapBased();
    //            }

               
    //        }
    //    }
        
    //}


    public float Distance()
    {
        return Vector3.Distance(
            StickSUK.transform.position,
            FinalDes.position
        );
    }
}