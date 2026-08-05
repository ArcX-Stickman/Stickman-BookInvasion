using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Animation_BasedOnCollision : MonoBehaviour
{
    public string CrossFadeAnimation;
    public string CompareTag_Tag;
    public Animator anim;
    public bool OnlyOnce;

    private void OnEnable()
    {
        if(anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleDatCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision2)
    {
        HandleDatCollision(collision2.collider);
    }

    public void HandleDatCollision(Collider2D collision) 
    {
        if (collision.CompareTag(CompareTag_Tag))
        {
            anim.CrossFade(CrossFadeAnimation, 0, 0);
            if(OnlyOnce)
            {
                this.enabled = false;
            }
            this.enabled = false;
        }
    
    }

    public void ForcedAnim()
    {
        anim.CrossFade(CrossFadeAnimation, 0, 0);
        if (OnlyOnce)
        {
            this.enabled = false;
        }
        this.enabled = false;
    }

}
