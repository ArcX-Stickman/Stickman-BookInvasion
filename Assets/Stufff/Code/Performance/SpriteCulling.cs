using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpriteCulling : MonoBehaviour
{
    public float MaxDis;
    public float Distance;
    public Transform StickPOS;
    public SpriteRenderer sR;
    private float tick = 1f;
    private float LastTick;
    void Update()
    {
        if (StickPOS == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            StickPOS = stick.transform;
        }
        if (sR == null)
        {
            sR = GetComponent<SpriteRenderer>();

        }
        if (Time.time >= tick + LastTick)
        {
            LastTick = Time.time;
            Distance = Vector2.Distance(transform.position, StickPOS.position);
        }
        if (Distance > MaxDis)
        {
            sR.enabled = false;
        }
        else
        {
            sR.enabled = true;
        }
    }
}
