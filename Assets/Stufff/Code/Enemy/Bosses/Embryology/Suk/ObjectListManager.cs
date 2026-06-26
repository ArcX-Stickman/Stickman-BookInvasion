using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectListManager : MonoBehaviour
{
    public GameObject[] List;

    void Start()
    {
        Movement m = FindAnyObjectByType<Movement>();
        if (m.StickHP.Testing)
        {
            foreach (GameObject G in List)
            {
                Debug.Log(gameObject.name + " 's List : " +  G.name + " Index: "+ G);
            }
        }
        
    }

  
}
