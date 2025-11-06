using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAdd : MonoBehaviour
{

    public GameObject Border;



    // Start is called before the first frame update
    void Start()
    {
        Border.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Border.gameObject.SetActive(true);
        }
    }
}
