using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftVison : MonoBehaviour
{

    public bool StickmanOnLeft;
    public RightVision RightV;




    // Start is called before the first frame update
    void Start()
    {
        StickmanOnLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RightV.StickmanOnRight == true)
        {
            StickmanOnLeft = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickmanOnLeft = true;
            RightV.StickmanOnRight = false;
        }
    }

}
