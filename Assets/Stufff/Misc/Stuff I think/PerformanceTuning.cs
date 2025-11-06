using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceTuning : MonoBehaviour
{

    public GameObject scientist;
    public bool Scientist;
    public GameObject AcidRain;
    public bool Acid;


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
            if (Scientist)
            {
                scientist.GetComponent<Scientist>().enabled = true;
            }

            
            
        }

        if (collision.CompareTag("Culling"))
        {
            if (Acid)
            {
                Debug.Log("YeaH IT WORK");
                AcidRain.GetComponent<AcidRain>().enabled = true;
            }
        }
        
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Scientist)
        {
            if (!collision.CompareTag("Player"))
            {
                scientist.GetComponent<Scientist>().enabled = false;
            }
        }


        if (!collision.CompareTag("Culling"))
        {
            if (Acid)
            {
                AcidRain.GetComponent<AcidRain>().enabled = false;
            }
        }

    }
}
