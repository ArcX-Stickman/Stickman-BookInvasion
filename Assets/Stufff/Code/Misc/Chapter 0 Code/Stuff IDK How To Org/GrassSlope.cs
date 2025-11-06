using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSlope : MonoBehaviour
{
    public Movement Stick;
    public BoxCollider2D Volume;




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
        if (collision.CompareTag("Player"))
        {
            Stick.SlideVolume = true;
            Stick.MaxSpeed = 30;
            StartCoroutine(Time());

            
        }
    }


    private IEnumerator Time()
    {

        Debug.Log("Time WHOO");
        yield return new WaitForSeconds(0.9f);

        Volume.isTrigger = false;
    }


    
}
