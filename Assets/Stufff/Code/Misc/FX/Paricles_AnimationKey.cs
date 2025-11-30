using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paricles_AnimationKey : MonoBehaviour
{
    public Transform FX_Pos;
    public GameObject Particles;


    public void GetDatSigmaFX()
    {
        ObjectPooler.SpawnObj(Particles.gameObject,FX_Pos.position,Quaternion.identity,ObjectPooler.PoolType.Partcles);

    }
}
