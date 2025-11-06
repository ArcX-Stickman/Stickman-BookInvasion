using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionWithEffect : MonoBehaviour
{
    public string Tag;
    public GameObject FX;
    public AudioSource AudioP;
    public AudioClip SFX;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            GameObject FXX = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
            SFXPlay();
            Destroy(gameObject);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            GameObject FXX = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.GamObjects);
            SFXPlay();
            Destroy(gameObject);

        }
    }


    public void SFXPlay()
    {
        if(AudioP != null && SFX != null )
        {

            AudioP.PlayOneShot(SFX);
        } 
    }

}
