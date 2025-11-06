using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySpawner : MonoBehaviour
{
    public float Distance;
    public float MinDIS;
    public float MaxDIS;
    public GameObject MemoryObj;
    public float LastSpawn;
    public float SpawnCD;
    public Transform StickPOS;

    private float tick = 0.3f;
    private float lastTick;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lastTick + tick)
        {
            // limit sqaure root math on CPU as this is only on one section that doesnt rlly last 
            // helpful for the people on low end pc's mostly
            Distance = Vector2.Distance(transform.position,StickPOS.position);
            lastTick = Time.time;
        }
        if(Time.time >= LastSpawn + SpawnCD && Distance <= MaxDIS && Distance <= MinDIS)
        {
            GameObject spawnMemory = ObjectPooler.SpawnObj(MemoryObj,transform.position,Quaternion.identity,ObjectPooler.PoolType.GamObjects);
            LastSpawn = Time.time;
            TurnOffRigidBodyIfPlayerFar rbManager = spawnMemory.GetComponent<TurnOffRigidBodyIfPlayerFar>();
            rbManager.isOg = false;
            MemoryOBJ memory = spawnMemory.GetComponent<MemoryOBJ>();
            memory.AlrPicked = false;
            Trap memory_Deactivater = memory.GetComponent<Trap>();
            memory_Deactivater.AlrDid = false;
        }
    }
}
