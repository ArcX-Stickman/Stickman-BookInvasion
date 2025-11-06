using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    public GameObject Fox;
    public float CD;
    private float LastSpawn;
    public bool CanSpawn;
    void Update()
    {

        if (Time.time >= LastSpawn + CD && CanSpawn)
        {
            GameObject FoxClone = Instantiate(Fox,transform.position,Quaternion.identity);
            Fox fox = FoxClone.GetComponent<Fox>();
            fox.isOG = false;
            LastSpawn = Time.time;
        }
    }
}
