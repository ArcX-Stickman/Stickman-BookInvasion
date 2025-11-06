using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownership : MonoBehaviour
{

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        playerID = Random.Range(0, 1000); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
