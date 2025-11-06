using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisionWithTag : MonoBehaviour
{

    public string Tag;
    public AudioSource AudioP;
    public AudioClip SFX;
    public GameObject FX;
    public Color ColorFX;
    public bool wantDelayForFX;
    public float SizeParticle;
    public float AmountParticles;
    public Movement Stick;
    private bool Alrdid;
    public bool LittleShake;
    public bool isPooledObject;
    public bool NoShake;

    private void Update()
    {
        if(Stick == null)
        {
            Stick = FindAnyObjectByType<Movement>();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag) && !wantDelayForFX)
        {
            SFXPlay();
            if (!isPooledObject)
            {
                Destroy(gameObject);

            }
            else
            {
                ObjectPooler.ReturnObjectToPool(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag) && !wantDelayForFX)
        {
            SFXPlay();
        }
    }


    public void SFXPlay()
    {
        if(AudioP != null && SFX != null)
        {
            AudioP.PlayOneShot(SFX);
        }
        if(FX != null)
        {
            if (!wantDelayForFX)
            {
                if (!NoShake)
                {
                    if (!LittleShake)
                    {
                        Stick.TimeManager.FreezeTime(0.05f);
                        Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(55.5f, 1f));
                    }
                    else
                    {
                        Stick.CameraScript.StartCoroutine(Stick.CameraScript.ShakeCamGroundFeeling(5.5f, 0.1f));

                    }
                }
               
                if(FX != null)
                {
                    GameObject FX_Spawn = ObjectPooler.SpawnObj(FX, transform.position, Quaternion.identity, ObjectPooler.PoolType.Partcles);
                    ParticleSystem particleSystem = FX_Spawn.GetComponent<ParticleSystem>();
                    var main = particleSystem.main;
                    main.startColor = ColorFX;
                    var emission = particleSystem.emission;
                    emission.rateOverTime = AmountParticles;
                    main.startSize = SizeParticle;
                }

                if (!isPooledObject)
                {
                    Destroy(gameObject);

                }
                else
                {
                    ObjectPooler.ReturnObjectToPool(gameObject);
                }


            }



        }
    }

   
}
