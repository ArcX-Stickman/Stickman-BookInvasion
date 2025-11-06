using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qthing : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(gameObject);
        }
    }
}
