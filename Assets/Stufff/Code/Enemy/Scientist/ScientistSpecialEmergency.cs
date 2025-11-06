using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistSpecialEmergency : MonoBehaviour
{
    public Transform Button;
    public Transform StickPOS;
    public Animator Animator;
    public float Speed;
    public float Distance;
    public float MinDisForEvent;
    public float DistanceFromEmergency;
    public float MinDisForEmergency;
    public Animator ButtonAnim;
    public bool StartedEvent;
    public GameObject Alarm;
    public bool Pushed;
    private void Start()
    {
        ButtonAnim = Button.GetComponent<Animator>();
    }
    void Update()
    {
        CalcDis();
        if(Distance <= MinDisForEvent)
        {
            StartedEvent = true;
        }
        if (StartedEvent)
        {
            EventManager();
        }
    }

    public void CalcDis()
    {
        Distance = Vector2.Distance(transform.position,StickPOS.transform.position);
        DistanceFromEmergency = Vector2.Distance(transform.position, Button.position);
    }

    public void EventManager()
    {
        if(DistanceFromEmergency >= MinDisForEmergency)
        {
            ButtonAnim.enabled = false;
            transform.position =  Vector2.MoveTowards(transform.position,Button.position, Speed * Time.deltaTime);
            Animator.SetBool("Run", true);
        }
        else
        {
            Pushed = true;
            Alarm.gameObject.SetActive(true);
            ButtonAnim.enabled = true;
            Animator.SetBool("Run", false);
            Animator.SetTrigger("Yo");
        }
    }
}
