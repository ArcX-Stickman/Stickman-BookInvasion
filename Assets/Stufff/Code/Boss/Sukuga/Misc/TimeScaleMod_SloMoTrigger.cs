using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleMod_SloMoTrigger : MonoBehaviour
{

    public TimeScale_MOD TimeScale_MOD;
    public float Time;
    public float Scale;
    public bool AlrDid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") &&!AlrDid)
        {
            TimeScale_MOD.SloMO(Time,Scale);
            AlrDid = true;
        }
    }
}
