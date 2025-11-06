using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCulling : MonoBehaviour
{
    private Light2D light2;
    public float MaxDis;
    public float Distance;
    public Transform StickPOS;
    private float tick = 1f;
    private float LastTick;
    // Update is called once per frame
    private void Start()
    {
        light2 = GetComponent<Light2D>();
    }
    void Update()
    {
        if(StickPOS == null)
        {
            Movement stick = FindObjectOfType<Movement>();
            StickPOS = stick.transform;
        }
        if(light2 == null)
        {
            light2 = GetComponent<Light2D>();

        }
        if (Time.time >= tick + LastTick)
        {
            LastTick = Time.time;
            Distance = Vector2.Distance(transform.position,StickPOS.position);
        }
        if(Distance > MaxDis)
        {
            light2.enabled = false;
        }
        else
        {
            light2.enabled = true;
        }
    }
}
