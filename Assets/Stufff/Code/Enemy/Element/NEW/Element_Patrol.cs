using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Patrol : MonoBehaviour
{
    public float TurnCD;
    public float LastTurn;
    public Movement StickManager;
    public AttackSight AttackSight;
    public Animator Animator;
    private float X;
    private bool ALRDID;
    public GameObject Projectile;
    public Transform AimPoint;
    public float SecondsDelay;
    public bool HIT;
    // Start is called before the first frame update
    void Start()
    {
        X = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(TurnCD + LastTurn <= Time.time)
        {
            TurnAround();
            LastTurn = Time.time;
        }

        if (AttackSight.StickmanIsHERE && !ALRDID && !StickManager.InStealth)
        {
            ALRDID = true;
            Animator.SetTrigger("Attack");
            TurnCD = 4798645; //big ol number
        }
        if (AttackSight.StickmanIsHERE && !StickManager.InStealth)
        {
            StickManager.HeCanMove = false;
            StickManager.MaxSpeed = 0;
            if (StickManager.Grounded)
            {
                StickManager.StickAnim.Rebind();
                StickManager.StickAnim.enabled = false;
                StickManager.rb.bodyType = RigidbodyType2D.Static;

            }
        }
        if (HIT)
        {
            StickManager.StickAnim.enabled = true;
            StickManager.StickHP.PlayerHP = 0;
        }
    }
   


    public void TurnAround()
    {
        if(StickManager.StickHP.PlayerHP > 0)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-X, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(X, transform.localScale.y, transform.localScale.z);

            }
        }
       
    }


    public void ThrowAttack()
    {
        GameObject projectile = Instantiate(Projectile,AimPoint.transform.position,Quaternion.identity);
        float angle = Mathf.Atan2(transform.position.y - StickManager.transform.position.y, 
            transform.position.x - StickManager.transform.position.x);

        ElementProjectile elementProjec = projectile.GetComponent<ElementProjectile>();
        elementProjec.isOG = false;
        float speed = elementProjec.Speed;
        elementProjec.ElementReciver = (this);
        float x = Mathf.Cos(angle) * speed;
        float y = Mathf.Sin(angle) * speed;
        projectile.transform.position += new Vector3(x,y,0f) * Time.deltaTime;

    }
}
