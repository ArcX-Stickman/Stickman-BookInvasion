using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightVision : MonoBehaviour
{
    public bool StickmanOnRight;
    public LeftVison LeftV;




    // Start is called before the first frame update
    void Start()
    {
        StickmanOnRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(LeftV.StickmanOnLeft == true)
        {
            StickmanOnRight = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickmanOnRight = true;
            LeftV.StickmanOnLeft = false;
        }
    }
}
