using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Controls : MonoBehaviour
{

    public Movement Stick;
    public GameObject WASD;


    void Update()
    {
        if(Stick.Checkpoint == 0)
        {
            WASD.gameObject.SetActive(true);
        }
        else
        {
            WASD.gameObject.SetActive(false);

        }
    }
}
