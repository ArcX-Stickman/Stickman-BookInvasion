using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCuller_PlayerDis : MonoBehaviour
{

    private float tick;
    public float MaxDis;
    private Animator animator;
    public bool Vec3;
    public Transform ReferencePoint;
    public float Dis_refer;
    private float lasttick;
    
    void Update()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }


        Dis_refer = Distance();
        if(Distance() >= MaxDis && Time.time >= lasttick + tick)
        {
            lasttick = Time.time;
            animator.enabled = false;
            this.enabled = false ;
        }

    }


    public float Distance()
    {
        float temp = 0;

        if (Vec3)
        {
            Vector3 pos1 = transform.position;
            Vector3 pos2 = ReferencePoint.position;

            pos1.z = 0;
            pos2.z = 0;

            temp = Vector3.Distance(pos1, pos2);
        }
        else
        {
            temp = Vector2.Distance(transform.position, ReferencePoint.position);
        }

        return temp;
    }

}
