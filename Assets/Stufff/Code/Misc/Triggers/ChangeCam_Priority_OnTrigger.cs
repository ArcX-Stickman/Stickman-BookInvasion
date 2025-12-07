using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCam_Priority_OnTrigger : MonoBehaviour
{

    public Cinemachine.CinemachineVirtualCamera Cam1;
    public Cinemachine.CinemachineVirtualCamera Cam2;
    public int Cam1Num;
    public int Cam2Num;
    private bool AlrDid;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Cam1.Priority = Cam1Num;
            Cam2.Priority = Cam2Num;
            this.enabled = true;

        }
    }
}
