using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    public AudioSource AudioP;
    public AudioClip[] SFX;
    //public AudioClip Hit2;
    //public AudioClip Hit3;
    //public Array list;
    public bool V2;
    public Transform FXPOS;
    public GameObject FX;
    public bool isForced;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !isForced)
        {
            HitMarkerSFX();
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

        } 
        if (collision.CompareTag("ArcBullet") && !isForced)
        {
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

            HitMarkerSFX();
        }
        if (collision.CompareTag("StickMelee") && !isForced)
        {
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

            HitMarkerSFX();
        }


        
    }

    public void HitMarkerSFX()
    {
        AudioClip play = SFX[UnityEngine.Random.Range(0, SFX.Length)];
        AudioP.PlayOneShot(play);

        if (isForced)
        {
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity,ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

        }
    }


    public void HitMarkerSFX_Particles()
    {
        AudioClip play = SFX[UnityEngine.Random.Range(0, SFX.Length)];
        AudioP.PlayOneShot(play);

        GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

    }


}
