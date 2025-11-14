using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAudioNorm_TimeManager : MonoBehaviour
{

    public TimeManager TM;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TM.ForceMusicSame = true;
            TM.SlowChase.pitch = 1;
            this.enabled = false;
        }
    }
}
