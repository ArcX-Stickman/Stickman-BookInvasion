using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDespawn : MonoBehaviour
{
    private bool AlrStarted;
    public ParticleSystem ParticleSystem;

    // Update is called once per frame
    void Update()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
        if (gameObject.name.Contains("(Clone)") && !AlrStarted)
        {
            AlrStarted = true;
            StartCoroutine(Despawn());
        }

    }

    IEnumerator Despawn()
    {
        yield return new WaitForSecondsRealtime(ParticleSystem.main.duration + 0.4f);
        ObjectPooler.ReturnObjectToPool(gameObject);
    }
}
