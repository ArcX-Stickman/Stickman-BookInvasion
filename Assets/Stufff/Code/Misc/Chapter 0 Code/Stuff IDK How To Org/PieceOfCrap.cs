using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOfCrap : MonoBehaviour
{


    public bool CanBe;
    public GameObject Crap;

    

    // Update is called once per frame
    void Update()
    {
        if (!CanBe)
        {
            Crap.gameObject.SetActive(false);
        }
        else
        {
            Crap.gameObject.SetActive(true);
        }
    }
}
