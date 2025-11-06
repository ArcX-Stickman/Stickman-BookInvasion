using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunLogic : MonoBehaviour
{
    public Transform StickBoi;
    public float TurnTime;
    Vector3 Velocity;
    public float RotationCD;
    private float LastCalledCD;
    private Vector3 RotatedRay;
    public Vector3 SourceVector = Vector3.right;
    public Transform SunRay;
    private float YStart;
    public float AngleTemp;
    public Transform Ecology;
    private bool FacingRight;
    public EnemyAI EnemyAI;
    public BoxCollider2D SunRayBXCD;
    public BoxCollider2D StickCollider;
    private float Delay = 5;
    private float LastDelay;
    private Movement StickMan;
    private bool PLayer_IFrame;
    private void Start()
    {
        StickMan = StickBoi.GetComponent<Movement>();
        YStart = transform.position.y;
    }
    private void Update()
    {
        if(StickMan.isDashing || StickMan.isSliding || StickMan.BackFlip)
        {
            PLayer_IFrame = true;

        }
        else
        {
            PLayer_IFrame = false;
        }

        if (SunRayBXCD.IsTouching(StickCollider) && Time.time >= Delay + LastDelay && !PLayer_IFrame)
        {
            Debug.Log("Dude I Am Trying");
            EnemyAI.SunRay_Ecology_Manager();
        }
    }
    public void RotateTowardStick()
    {
        FacingRight = Ecology.transform.localScale.x >= 1;

        transform.position = new Vector2(transform.position.x, YStart);

        Vector3 direction = StickBoi.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!FacingRight)
        {
            // The Enemy is Messing this up so do sum ARJUN!!!!!!
            angle += 180f;
        }
        // Quaternions pmo like why did it take me 5 hrs to do this ;_;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        AngleTemp = angle;
    }

    
}
