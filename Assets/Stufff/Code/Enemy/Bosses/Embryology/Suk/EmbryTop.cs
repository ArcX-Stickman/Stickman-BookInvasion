using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class EmbryTop : MonoBehaviour
{
    public Animator Enem;

    public bool PlayerOnTop;

    public EmbrySukugaCut ESC;

    private float LastTouchPlayer = 0;
    private float TouchCD = 0.5f;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enem.SetTrigger("FlyDetected");
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time >= LastTouchPlayer + TouchCD)
        {
            LastTouchPlayer = Time.time;
            ESC.TimesPlayerJumpOver += 1;
            PlayerOnTop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerOnTop = false;
        }
    }
}
