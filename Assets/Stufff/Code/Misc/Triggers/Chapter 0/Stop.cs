using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop : MonoBehaviour
{


    public GameObject StickCam;
    
    public void GoAwayCam()
    {
        Destroy(gameObject);
        StickCam.gameObject.SetActive(true);
    }
}
