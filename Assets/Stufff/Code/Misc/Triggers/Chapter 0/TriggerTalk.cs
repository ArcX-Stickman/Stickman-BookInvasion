using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerTalk : MonoBehaviour
{
    public bool AlrDid;
    public Scientist scientist;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!AlrDid)
            {
                AlrDid = true;
                scientist.IsYapping = true;
            }
        }
    }
}
