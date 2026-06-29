using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToNextScene : MonoBehaviour
{
    public static ObjectToNextScene instance;

     private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
