using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVolume : MonoBehaviour
{
    public bool PlayerInVolume;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInVolume = true;
        }
    }
}
