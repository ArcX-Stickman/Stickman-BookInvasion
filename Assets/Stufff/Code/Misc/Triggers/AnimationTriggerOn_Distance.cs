using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class AnimationTriggerOn_Distance : MonoBehaviour
{

    public string AninmationName;
    public float Distance;
    public float MinDis;
    public Movement Stick;
    public Animator Animator;
    private bool AlrDid;
    public Transform Overide;
    public Transform Overide_Refernce_BasicallyWhatObjIsTracking;
    // Update is called once per frame
    void Update()
    {
        if(Overide == null && Overide_Refernce_BasicallyWhatObjIsTracking == null )
        {
            Distance = Vector2.Distance(transform.position, Stick.transform.position);

        }
        else if(Overide != null) 
        {
            Distance = Vector2.Distance(Overide.position, Stick.transform.position);

        }
        else
        {
            Distance = Vector2.Distance(transform.position, Overide_Refernce_BasicallyWhatObjIsTracking.transform.position);
        }
        if (Distance <= MinDis && !AlrDid)
        {
            AlrDid = true;
            Animator.CrossFade(AninmationName, 0, 0);
        }
    }
}
