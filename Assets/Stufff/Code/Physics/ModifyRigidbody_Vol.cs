using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyRigidbody_Vol : MonoBehaviour
{
    public Rigidbody2D Rigidbody2D;
    public float gravity;
    public bool isZero;

   

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gravity > 0 || gravity == 0 && isZero)
            {
                Rigidbody2D.gravityScale = gravity;
            }
        }
    }
}
