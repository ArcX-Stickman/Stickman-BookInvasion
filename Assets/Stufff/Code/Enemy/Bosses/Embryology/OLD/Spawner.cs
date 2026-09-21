using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject Obj;
    public float CD;
    private float LastCD;
    public AudioSource AudioP;
    public AudioClip AudioClip;
    void Update()
    {
        if(Time.time >= CD + LastCD )
        {
            LastCD = Time.time;
            if(AudioP != null )
            {
                AudioP.PlayOneShot(AudioP.clip);
                AudioP.pitch = UnityEngine.Random.Range(0.5f, 1.4f);
            }
            SpawnIT();
        }
    }

    public void SpawnIT()
    {
        GameObject MinionC = Instantiate(Obj, transform.position, Quaternion.identity);
    }
}
