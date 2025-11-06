using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaming : MonoBehaviour
{

    public float speed;
    public Transform currentPOS;
    public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentPOS.position, step);

       

        if (Vector2.Distance(transform.position, currentPOS.position) < 1f)
        {
           // currentPOS = currentPOS == R_Point.transform ? L_Point.transform : R_Point.transform;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RightBorder"))
        {
            transform.localScale = new Vector3(-1, 1, 1);

            rb.velocity = new Vector2(speed, 0);
        }
        if (collision.gameObject.CompareTag("LeftBorder"))
        {
            transform.localScale = new Vector3(1, 1, 1);

            rb.velocity = new Vector2(-speed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RightBorder"))
        {
           // currentPOS = 
        }
        if (collision.gameObject.CompareTag("LeftBorder"))
        {
           // currentPOS = 
        }
    }
}
