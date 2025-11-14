    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnON_BG : MonoBehaviour
{

    public GameObject BG;

    // Start is called before the first frame update
    void Start()
    {
        BG.gameObject.SetActive(true);   
    }

   
}
