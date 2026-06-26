using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Obj;
    public float CD;
    private float LastCD;

    void Update()
    {
        if(Time.time >= CD + LastCD )
        {
            LastCD = Time.time;
            GameObject MinionC = Instantiate(Obj, transform.position, Quaternion.identity);
        }
    }
}
