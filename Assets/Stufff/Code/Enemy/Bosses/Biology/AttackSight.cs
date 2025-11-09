using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSight : MonoBehaviour
{

    public bool StickmanIsHERE;
    public bool Force;
    public bool V2;
    public Movement Stickman;

    // Start is called before the first frame update
    void Start()
    {
        StickmanIsHERE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Force)
        {
            StickmanIsHERE = true;
        }
        if(Stickman == null)
        {
            Stickman = FindAnyObjectByType<Movement>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickmanIsHERE = true;
            Stickman.TouchingATKSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !Force)
        {
            StickmanIsHERE = false;
            Stickman.TouchingATKSight = false;

        }

    }

}
