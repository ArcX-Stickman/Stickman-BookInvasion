using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    public GameObject RainDrop;
    public Transform[] PossibleRain;
    public float CD;
    public float LastRain;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= CD + LastRain)
        {
            LastRain = Time.time;
            GameObject RainDrops = ObjectPooler.SpawnObj(RainDrop, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);

        }
    }

   
}
