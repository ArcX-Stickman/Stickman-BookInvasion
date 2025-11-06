using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCartKillable : MonoBehaviour
{

    public float MinDis;
    public float Distance;
    public Transform StickPOS;
    public Animator EnemAnim;
    public Transform FX_POS;
    public GameObject FX;
    public HitMarker HitMarker;
    public float TickCD;
    public float LastTick;
    public float speed;
    public bool CanMove;
    public bool HasAI;
    public Transform PositionMAX;
    //Declare
    public bool isSpecialAstro;
    public bool isSpecialPhysics;
    public bool isSpecialMeteorology;
    public float MinMinDIs;
    private bool isFleeing;
    public bool isScientist;
    private void Start()
    {
        EnemAnim = GetComponent<Animator>();
        CanMove = true;
        isFleeing = false;

    }
    void Update()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (isSpecialAstro)
        {
            if (!isFleeing)
            {
                if (distanceToPlayer <= MinMinDIs)
                {
                    isFleeing = true;
                    StickPOS = PositionMAX;
                    EnemAnim.SetTrigger("AYO");
                    Debug.Log("YOOO CHILLL!!!");
                }
                else if (distanceToPlayer <= MinDis)
                {
                    StickPOS = playerTransform; 
                    MoveToPlayer();
                }
            }

            if (isFleeing)
            {
                MoveAway_Astro(); 
            }
        }

        if (isSpecialMeteorology)
        {
            if (!isFleeing)
            {
                if (distanceToPlayer <= MinMinDIs)
                {
                    isFleeing = true;
                    EnemAnim.SetTrigger("uh");
                    Debug.Log("YOOO CHILLL!!!");
                    speed = 0;

                }
                else if (distanceToPlayer <= MinDis) 
                {
                    MoveToPlayer();
                }
            }

            if (isFleeing)
            {
                EnemAnim.SetTrigger("uh");
                speed = 0;
            }

        }

        if (isSpecialPhysics)
        {
            if (distanceToPlayer <= MinMinDIs)
            {
                isFleeing = true;
                EnemAnim.SetTrigger("Well_I_Tried");
                Debug.Log("i tried bruh");
                speed = 0;

            }
            else if (distanceToPlayer <= MinDis)
            {
                speed = 0;
            }
            if (isFleeing)
            {
                EnemAnim.SetTrigger("Well_I_Tried");
                Debug.Log("i tried bruh");
                speed = 0;
            }
        }

        if (isScientist)
        {
            if (distanceToPlayer <= MinMinDIs)
            {
                isFleeing = true;
                EnemAnim.SetTrigger("YO");
                Debug.Log("AYO LOOK");
                speed = 0;

            }
            else if (distanceToPlayer <= MinDis)
            {
                speed = 0;
            }
            if (isFleeing)
            {
                EnemAnim.SetTrigger("YO");
                Debug.Log("AYOOOOOO");
                speed = 0;
            }
        }

        Distance = Vector2.Distance(transform.position, StickPOS.position);

        if (TickCD + LastTick <= Time.time)
        {
            LastTick = Time.time;

            if (Distance <= MinDis && !HasAI)
            {
                EnemAnim.enabled = true;
                EnemAnim.SetTrigger("Bruh");
            }
            else
            {
                if (!HasAI)
                {
                    EnemAnim.enabled = false;
                }
            }
        }
    }



    public void MoveToPlayer()
    {
        Vector2 targetPosition = new Vector2(StickPOS.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void MoveAway_Astro()
    {
        speed = 80;
        Vector2 targetPosition = new Vector2(PositionMAX.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cart"))
        {
            if (isScientist)
            {
                GameObject pLayer = GameObject.FindGameObjectWithTag("Player").gameObject;
                Movement PlayerScript = pLayer.GetComponent<Movement>();    
                PlayerScript.HasKilledYappers_Sciests_WithACart = true;
            }
            GameObject FX1 = Instantiate(FX,transform.position,Quaternion.identity);
            HitMarker.HitMarkerSFX();
            Destroy(gameObject);
        }
    }
}
