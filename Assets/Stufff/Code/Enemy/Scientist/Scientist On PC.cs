using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistOnPC : MonoBehaviour
{
    public float DistanceFromPlayer;
    public float DistanceThatTriggersEnemyShock;
    public float DistanceForKeyBoard;
    public Transform StickPOS;
    public Animator ScientistAnim;
    public AudioSource AudioP;
    public AudioClip Dead;
    public Transform FX_POS;
    public GameObject FX;
    private bool AlrPlaySound;
    public AudioClip Scared;
    public AudioSource Keyboard;
    void Start()
    {
        Keyboard.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        Calc_Distance();
        if (DistanceFromPlayer <= DistanceThatTriggersEnemyShock)
        {
            ScientistAnim.SetTrigger("Shock");
        }
        if (AlrPlaySound)
        {
            Keyboard.enabled = false;
        }

 
    }

    public void Calc_Distance()
    {
        DistanceFromPlayer = Vector2.Distance(transform.position, StickPOS.position);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            HandleDeath();


        }
        if (collision.gameObject.CompareTag("StickMelee"))
        {
            HandleDeath();


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && DistanceFromPlayer <= DistanceThatTriggersEnemyShock)
        {
            HandleDeath();
        }
        if (collision.gameObject.CompareTag("StickMelee"))
        {
            HandleDeath();
        }
    }

    public void ScaredSFX()
    {
        if (!AlrPlaySound)
        {
            AlrPlaySound = true;
            AudioP.PlayOneShot(Scared);
        }
    }
    public void HandleDeath()
    {
        ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
        Destroy(gameObject);

    }
}
