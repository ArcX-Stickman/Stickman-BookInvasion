using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeObject : MonoBehaviour
{

    public GameObject Original;
    
    void Update()
    {
        if (Original.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
