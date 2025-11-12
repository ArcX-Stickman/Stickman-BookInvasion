using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichAnimaion_In_Volume : MonoBehaviour
{

    public string Anim_Name;
    public bool isTrigger;
    public bool isBool;
    public bool isCrossfade;
    public Animator animator;
    public string Gtag = "";

    private void Update()
    {
        //Gtag = animator.gameObject.tag;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Gtag))
        {
            Switcharoo();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Gtag) && isBool)
        {
            animator.SetBool(Anim_Name, false);

        }
    }

    public void Switcharoo()
    {
        Debug.Log("Switcharooo");
        if (isBool)
        {
            Debug.Log("Booled");
            animator.SetBool(Anim_Name,true);
        }
        else if (isCrossfade)
        {
            animator.CrossFade(Anim_Name, 0, 0);
        }
        else if (isTrigger)
        {
            animator.SetTrigger(Anim_Name);
        }

    }
}
