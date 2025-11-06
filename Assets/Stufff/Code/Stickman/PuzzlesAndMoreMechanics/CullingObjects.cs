using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingObjects : MonoBehaviour
{
    public GameObject ObjectToCull;
    public bool WantToCullIfTouch;


    // Start is called before the first frame update
    void Start()
    {
        ObjectToCull.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && WantToCullIfTouch)
        {
            ObjectToCull.gameObject.SetActive(true);
        }
        
    }
}
