using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCollision : MonoBehaviour
{
    public Health StickHP;
    public GameObject Explosion;
    public AudioSource AudioP;
    public AudioClip Boom;
    public Movement StickMove;
    public AudioClip Miss;
    public bool isDodgable;
    private BoxCollider2D Bxcd2d;

    private void Update()
    {
        Bxcd2d = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!StickMove.isDashing)
            {
                ObjectPooler.SpawnObj(Explosion, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                AudioP.PlayOneShot(Boom);
                StickHP.PlayerHP = 0;
                Destroy(gameObject);
            }
            else if(StickMove.isDashing && isDodgable)
            {
                Physics2D.IgnoreCollision(Bxcd2d, StickMove.StickCollider,true);
                AudioP.PlayOneShot(Miss);
                StickMove.TimeManager.SloMO();

            }
            
        }

        if (collision.gameObject.CompareTag("Spec"))
        {

            ObjectPooler.SpawnObj(Explosion, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
            AudioP.PlayOneShot(Boom);
            Destroy(gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!StickMove.isDashing)
            {
                StickMove.CameraScript.StartCoroutine(StickMove.CameraScript.ShakeCamGroundFeeling(30.7f, 0.2f));

                ObjectPooler.SpawnObj(Explosion, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                AudioP.PlayOneShot(Boom);
                StickHP.PlayerHP = 0;
                Destroy(gameObject);
            }
            else if (StickMove.isDashing && isDodgable)
            {
                Physics2D.IgnoreCollision(Bxcd2d, StickMove.StickCollider, true);
                AudioP.PlayOneShot(Miss);
                StickMove.TimeManager.SloMO();

            }

        }

        if (collision.gameObject.CompareTag("Spec"))
        {
            StickMove.CameraScript.StartCoroutine(StickMove.CameraScript.ShakeCamGroundFeeling(9.7f, 0.5f));
            ObjectPooler.SpawnObj(Explosion, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
            AudioP.PlayOneShot(Boom);
            Destroy(gameObject);

        }
    }

}
