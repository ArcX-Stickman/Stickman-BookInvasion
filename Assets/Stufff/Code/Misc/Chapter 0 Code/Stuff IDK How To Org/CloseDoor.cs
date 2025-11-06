using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{

    public bool StickmanIsHere;
    public GameObject Door;
    public TimeManager TimeManager;
    public bool SlomoTime;
    public Movement Movement;




    // Start is called before the first frame update
    void Start()
    {
        Door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StickmanIsHere)
        {
            if(Door != null)
            {
                Door.SetActive(true);

            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickmanIsHere = true;
            if (SlomoTime == false)
            {
                Movement.SlideCancelSpeed = 90;
                SlomoTime = true;
                TimeManager.SloMO();
            }
            
        }
    }






}
