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
    public Movement Stickboi;

    public bool DirectionMustMatchTarget;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Stickboi == null)
        {
            Stickboi = FindAnyObjectByType<Movement>();
        }
        if (collision.CompareTag("Bullet") && !isForced)
        {
            HitMarkerSFX();
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);
            StartCoroutine(Stickboi.CameraScript.ShakeCamGroundFeeling(20, 0.2f));

        }
        if (collision.CompareTag("ArcBullet") && !isForced)
        {
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

            HitMarkerSFX();
        }
        if (collision.CompareTag("StickMelee") && !isForced)
        {
            GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);
            StartCoroutine(Stickboi.CameraScript.ShakeCamGroundFeeling(UnityEngine.Random.Range(30.0f,35.0f), 0.2f));
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
            if (DirectionMustMatchTarget)
            {
                // Vector3 scale = new Vector3(transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);
                // effect.transform.localScale = scale;
                // foreach(Transform child in effect.transform)
                // {
                //     child.transform.localScale = scale;
                // }
                    effect.transform.SetParent(transform);

            }
        }
    }


    public void HitMarkerSFX_Particles()
    {
        AudioClip play = SFX[UnityEngine.Random.Range(0, SFX.Length)];
        AudioP.PlayOneShot(play);

        GameObject effect = ObjectPooler.SpawnObj(FX, FXPOS.position, Quaternion.identity, ObjectPooler.PoolingType = ObjectPooler.PoolType.Partcles);

    }


}
