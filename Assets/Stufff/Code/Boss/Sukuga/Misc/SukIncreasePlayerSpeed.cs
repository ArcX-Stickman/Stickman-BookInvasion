using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SukIncreasePlayerSpeed : MonoBehaviour
{
    public StickCinematicVer_BossFight Stick;
    public float ThisSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Stick.Speed = ThisSpeed;
            Stick.OGSpeed = ThisSpeed;
            this.enabled = false;
        }
    }
}
