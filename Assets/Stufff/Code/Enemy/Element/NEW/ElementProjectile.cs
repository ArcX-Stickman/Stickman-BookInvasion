using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementProjectile : MonoBehaviour
{
    public float X_AXIS;
    public float Y_AXIS;
    public AudioSource AudioP;
    public AudioClip HIT_SFX;
    public bool isOG;
    public Transform StickPOS;
    public float Speed;
    public bool Hit;
    public Element_Patrol ElementReciver;
    private SpriteRenderer spR;
    private bool ALR_PLAYED_SFX;
    public GameObject FX;
    private void Start()
    {
        spR = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!isOG)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            float angle = Mathf.Atan2(transform.position.y - StickPOS.transform.position.y,
                transform.position.x - StickPOS.transform.position.x);
            X_AXIS = Mathf.Cos(angle) * Speed;
            Y_AXIS = Mathf.Sin(angle) * Speed;
            Vector3 Position = new Vector3(X_AXIS, Y_AXIS,0f) * Time.deltaTime;
            transform.position -= Position;
        }
        else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           ElementReciver.HIT = true;
            spR.sprite = null;
            if (!ALR_PLAYED_SFX)
            {
                ALR_PLAYED_SFX = true;
                AudioP.PlayOneShot(HIT_SFX);
            }
            ObjectPooler.SpawnObj(FX, StickPOS.position, Quaternion.identity,ObjectPooler.PoolType.Partcles);
        }
    }
}
