using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHandler : MonoBehaviour
{

    public Movement Stick;



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
        if (collision.CompareTag("Slide"))
        {
            Stick.SlideVolume = true;
            Debug.Log("uh");

        }

        if (collision.CompareTag("SlideEnd"))
        {
            Stick.SlideVolumeEnd = false;
        }



    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Slide"))
        {
            Stick.SlideVolume = false;

        }
    }



}
