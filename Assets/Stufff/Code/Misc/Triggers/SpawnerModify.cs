using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerModify : MonoBehaviour
{
    public float CD;
    public float SpawnTime;
    [Header("TYPE: RANDOM ORDER SHOOT TIME BY TIME")]
    public bool R_orderTYPE;
    public GameObject[] Spawners;


    
    void Update()
    {
        if (R_orderTYPE)
        {
            R_Order();
        }   
    }

    public void R_Order()
    {
        if(Time.time > SpawnTime + CD)
        {
            SpawnTime = Time.time;
            GameObject Spawn = Spawners[UnityEngine.Random.Range(0, Spawners.Length)];

            Spawner Curr = Spawn.GetComponent<Spawner>();

            Curr.SpawnIT();
        }
    }
}
