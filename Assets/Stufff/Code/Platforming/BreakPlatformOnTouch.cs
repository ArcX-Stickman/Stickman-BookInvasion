using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatformOnTouch : MonoBehaviour
{
    public float MaxTime;
    public Animator Animator;
    public string name;
    public float trans;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollisionStuff();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollisionStuff();
        }
    }

    public void CollisionStuff()
    {
        if(Animator != null)
        {
            Animator.CrossFade(name, trans, 0);
            StartCoroutine(WaitNDes());

        }
        else
        {
            StartCoroutine(WaitNDes());
        }
    }





    private IEnumerator WaitNDes()
    {
        yield return new WaitForSeconds(MaxTime);
        DestoryThing();
        yield break;
    }

    public void DestoryThing()
    {
        Destroy(gameObject);
    }
}
