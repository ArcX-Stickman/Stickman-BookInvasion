using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTender : MonoBehaviour
{

    public float Speed;
    public float DisDes;
    public float DisPlayer;
    public float MinDisDes;
    public float MindisPlayerStopThing;
    public float MindisplayerMove;
    public Transform Destination;
    public Transform Stick;
    public SpriteRenderer sp;
    public Sprite Cooked;
    private Animator animator;
    public AudioSource AudioP;
    public AudioClip AudioClip;
    private bool SFXPLayed;
    void Update()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();

        }
        DisDes = Vector2.Distance(transform.position, Destination.position);
        DisPlayer = Vector2.Distance(transform.position, Stick.position);
        if(DisPlayer <= MindisplayerMove)
        {
            if(DisDes >= MinDisDes)
            {
                transform.position = Vector2.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);

            }

            if(DisPlayer <= MindisPlayerStopThing)
            {
                if (!SFXPLayed)
                {
                    SFXPLayed = true;
                    AudioP.PlayOneShot(AudioClip);
                }
                Speed = 0;
                animator.enabled = false;
                sp.sprite = Cooked;
            }
        }
    }
}
