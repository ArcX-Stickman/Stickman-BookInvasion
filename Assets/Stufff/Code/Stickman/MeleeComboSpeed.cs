using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComboSpeed : MonoBehaviour
{
    public bool Book;
    public Movement Stickmanager;
    public float FixerCD;
    private float LastFixerCD;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Book"))
        {
            if(Time.time >= LastFixerCD + FixerCD)
            {
                Debug.LogWarning("Hit!");

                Stickmanager.ComboCount += 0.3f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Book"))
        {
            if (Time.time >= LastFixerCD + FixerCD)
            {
                Debug.LogWarning("Hit!");
                Stickmanager.ComboCount += 0.3f;
            }
        }
    }

    
}
