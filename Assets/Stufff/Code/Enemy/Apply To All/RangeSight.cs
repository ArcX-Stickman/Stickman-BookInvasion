using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSight : MonoBehaviour
{

    public bool StickHere;
    public bool YeahThisThing;
    public bool YoStay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StickHere && YeahThisThing)
        {
            YoStay = true;
        }

        if (YoStay)
        {
            StickHere = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickHere = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            if (!YoStay && !YeahThisThing)
            {
                StickHere = false;

            }
            
        }
    }

}
