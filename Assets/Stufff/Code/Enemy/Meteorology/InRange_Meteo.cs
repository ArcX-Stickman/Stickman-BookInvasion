using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRange_Meteo : MonoBehaviour
{
    public Meteorology Meteorology;

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
            Debug.Log("player touch");
            Meteorology.InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Meteorology.InRange = false;
        }
    }
}
