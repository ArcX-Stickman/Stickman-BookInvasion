using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObject_CheckPointBased : MonoBehaviour
{
    public GameObject Obj;
    public Movement Stick;
    public int Checkpoint;
    public bool Status_TurnOnIfActive_TurnOffIfDeactivate;
    void Update()
    {
        if(Stick.Checkpoint == Checkpoint)
        {
            Obj.gameObject.SetActive(Status_TurnOnIfActive_TurnOffIfDeactivate);
            this.enabled = false;
        }
    }
}
