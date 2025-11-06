using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    public float Duration;
    private bool Touched;
    private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Movement playerChecker = collision.gameObject.GetComponent<Movement>();
            
            if (!Touched )
            {
                Touched = true; 
                StartCoroutine(FallCourtine());
            }
        }
    }

    public IEnumerator FallCourtine()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        yield return new WaitForSecondsRealtime(Duration);
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject,3);
    }

}
