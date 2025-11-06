using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZ_Controller : MonoBehaviour
{
    public Movement Movement;
    public bool WantZ;
    public bool NoZ;
    void Update()
    {
        if(Movement == null)
        {
            Movement = FindAnyObjectByType<Movement>();
            
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (WantZ)
            {
                Movement.rb.constraints = RigidbodyConstraints2D.None;
            }
            else
            {
                Movement.rb.rotation = 0;
                Movement.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            }

        }
    }
}
