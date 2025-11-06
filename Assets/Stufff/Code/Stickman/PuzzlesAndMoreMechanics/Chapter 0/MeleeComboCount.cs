using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComboCount : MonoBehaviour
{

    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Five;
    public GameObject Six;
    public Movement PlayerManager;
    public int Count;
    // Update is called once per frame
    void Update()
    {
        // this is gotta be the most inneffeciant code i've ever wrote but i dont care ;)
        Count = PlayerManager.CurrentPunches;
        if(Count == 0)
        {
            One.gameObject.SetActive(false);
            Two.gameObject.SetActive(false);
            Three.gameObject.SetActive(false);
            Four.gameObject.SetActive(false);
            Five.gameObject.SetActive(false);
            Six.gameObject.SetActive(false);
        }
        else if (Count == 1)
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(false);
            Three.gameObject.SetActive(false);
            Four.gameObject.SetActive(false);
            Five.gameObject.SetActive(false);
            Six.gameObject.SetActive(false);
        }
        else if (Count == 2)
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(true);
            Three.gameObject.SetActive(false);
            Four.gameObject.SetActive(false);
            Five.gameObject.SetActive(false);
            Six.gameObject.SetActive(false);
        }
        else if (Count == 3)
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(true);
            Three.gameObject.SetActive(true);
            Four.gameObject.SetActive(false);
            Five.gameObject.SetActive(false);
            Six.gameObject.SetActive(false);
        }
        else if (Count == 4)
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(true);
            Three.gameObject.SetActive(true);
            Four.gameObject.SetActive(true);
            Five.gameObject.SetActive(false);
            Six.gameObject.SetActive(false);
        }
        else if (Count == 5)
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(true);
            Three.gameObject.SetActive(true);
            Four.gameObject.SetActive(true);
            Five.gameObject.SetActive(true);
            Six.gameObject.SetActive(false);
        }
        else
        {
            One.gameObject.SetActive(true);
            Two.gameObject.SetActive(true);
            Three.gameObject.SetActive(true);
            Four.gameObject.SetActive(true);
            Five.gameObject.SetActive(true);
            Six.gameObject.SetActive(true);
        }
    }
}
