using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveDirecCtrl : MonoBehaviour
{
    public float Speed;
    public AudioClip DestroySFX;
    private TrapsSukuga TrapsSukuga;
    private TrapsV2 TrapsNorm;
    public Transform Targ;

    private bool Sukuga;
    void Start()
    {

        TrapsNorm = GetComponent<TrapsV2>();
        TrapsSukuga = GetComponent<TrapsSukuga>();

        if(TrapsNorm == null || TrapsSukuga == null)
        {
            TrapsNorm = GetComponentInChildren<TrapsV2>();
            TrapsSukuga = GetComponentInChildren<TrapsSukuga>();

        }

        if (TrapsNorm == null && TrapsSukuga != null)
        {
            Sukuga = true;
        } 
    }

    void Update()
    {
        if (Targ == null)
        {
            return;
        }


        if (Sukuga)
        {
            SukugaManager();
        }
        else
        {
            NormManager();
        }
    }

    [SerializeField] private float MinDis;

    public void SukugaManager()
    {
        float Distance = Vector2.Distance(transform.position, Targ.position);
        if(Distance >= MinDis) //&& !TrapsSukuga.DMGed
        {
            Distance = Vector2.Distance(transform.position, Targ.position);
            transform.position = Vector2.MoveTowards(transform.position, Targ.position, Speed * Time.deltaTime);
        }

   
    }

    // litteraly the same thing but jsut for different arcitectures like codes or scrips
    // yea bruh ion know what im talking about im just wasting kilobytes of storage my bad :(

    public void NormManager()
    {
        float Distance = Vector2.Distance(transform.position, Targ.position);
        if (Distance >= MinDis) // && !TrapsNorm.triggered
        {
            Distance = Vector2.Distance(transform.position, Targ.position);
            transform.position = Vector2.MoveTowards(transform.position, Targ.position, Speed * Time.deltaTime);
        }
    }
}
