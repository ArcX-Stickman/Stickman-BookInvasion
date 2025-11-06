using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuro_InRange : MonoBehaviour
{


    public bool InRange;

    // Start is called before the first frame update
    void Start()
    {
        InRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRange = true;
        }
    }


}
