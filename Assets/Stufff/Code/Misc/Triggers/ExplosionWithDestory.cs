using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionWithDestory : MonoBehaviour
{
    [Header("Yo Arc This Works Like A trigger/activate type of script")]
    public GameObject Explosion;
    public AudioSource AudioP;
    public AudioClip BoomSFX;
    public GameObject Obj_ToDestory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ObjectPooler.SpawnObj(Explosion,Obj_ToDestory.transform.position,Quaternion.identity);
            AudioP.PlayOneShot(BoomSFX);
            this.enabled = false;
        }
    }
}
