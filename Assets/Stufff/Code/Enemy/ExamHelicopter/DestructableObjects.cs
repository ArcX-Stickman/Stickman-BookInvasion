using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjects : MonoBehaviour
{
    public GameObject FX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ObjectOfDestruction"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ObjectOfDestruction"))
        {
            Destroy(gameObject);
        }
    }
}
