using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grog_Anim_Switch : MonoBehaviour
{

    public Animator animator;
    public float MinDis;
    public float Dis;
    public Movement Stick;


    void Update()
    {
        Dis = Vector2.Distance(transform.position,Stick.transform.position);    
        if(Dis <= MinDis)
        {
            animator.CrossFade("2", 0.2f, 0);

        }
        else
        {
            animator.CrossFade("1", 1.2f, 0);

        }
    }
}
